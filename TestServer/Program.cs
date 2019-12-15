using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AntMe.Runtime.Simulation.Communication;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] extensionPaths = new string[] {
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions",
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions\\netstandard2.0",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions"
            };
            var uri = "http://localhost:80/AntMeServer/";

            Console.WriteLine("AntMe! Test Server");

            Console.WriteLine($"Start Server on: {uri}");

            try
            {
                SimulationServer.Start(extensionPaths, uri);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.WriteLine("press any key to close the app");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Server is Running...");
            Console.WriteLine("press any key to stop the server and close the app");
            Console.ReadLine();
            SimulationServer.Stop();
            Console.WriteLine("Goodbye...");

        }
    }
}
