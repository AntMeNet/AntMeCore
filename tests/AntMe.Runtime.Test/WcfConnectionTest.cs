using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime.Communication;
using System.Collections.Generic;
using System.Linq;
using AntMe.Simulation;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace AntMe.Runtime.Test
{
    [TestClass]
    public class WcfConnectionTest
    {
        [TestMethod]
        public void DefaultScenarioSameThreadTest()
        {
            SimulationServer.Start("AntMeTest", SimulationServer.DEFAULTPORT);

            byte[] file = File.ReadAllBytes(@".\AntMe.Levelpack.dll");
            LoaderInfo loader = AntMe.Runtime.ExtensionLoader.SecureAnalyseExtension(file, true, true);
            loader.Levels[0].Type.AssemblyFile = file;
            loader.Players[0].Type.AssemblyFile = file;
            int lastRound = -1;

            // Client 1
            ISimulationClient client1 = SimulationClient.CreateNamedPipe("AntMeTest");
            client1.OnLevelChanged += (c, l) => 
            {
                if (l != null)
                {
                    Task t = new Task(() =>
                    {
                        c.UploadPlayer(loader.Players[0].Type);
                        c.SetPlayerState(0, PlayerColor.Green, 0, true);
                    });
                    t.Start();
                }
            };
            client1.Open("Client 1");

            // Client 2
            ISimulationClient client2 = SimulationClient.CreateNamedPipe("AntMeTest");
            client2.OnPlayerChanged += (c, s) => 
            {
                var slot = c.Slots[s];
                if (slot.ReadyState && slot.PlayerInfo)
                {
                    Task t = new Task(() => { c.StartSimulation(); });
                    t.Start();
                }
            };
            client2.OnSimulationState += (c, s) => 
            {
                lastRound = s.Round;
            };
            client2.Open("Client 2");
            client2.AquireMaster();
            client2.UploadLevel(loader.Levels[0].Type);

            Thread.Sleep(10000);
            Assert.IsTrue(lastRound > 0);

            SimulationServer.Stop();
        }
    }
}
