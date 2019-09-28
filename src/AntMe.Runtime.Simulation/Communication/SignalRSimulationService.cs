using System.Collections.Generic;
using System.Threading.Tasks;
using AntMe.Runtime.Communication;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AntMe.Runtime.Simulation.Communication
{
    [HubName("AntMeSimulationService")]
    public class SignalRSimulationService : Hub<ISimulationCallback>, ISimulationService
    {
        private static readonly IDictionary<string, SimulationServer> clientServerDicionary = new Dictionary<string, SimulationServer>();
        private static readonly IDictionary<string, int> clientIdDictionary = new Dictionary<string, int>();
        
        public override Task OnConnected()
        {
            // add new connection to the server
            int id = SimulationServer.Register(Context.ConnectionId, Clients.Caller, out var server);
            //add connection to the local dictionary, so the correct/releated server can be addressed.
            lock (clientServerDicionary)
            {
                clientServerDicionary.Add(Context.ConnectionId, server);
                clientIdDictionary.Add(Context.ConnectionId, id);
            }
            return base.OnConnected();
        }


        public override Task OnReconnected()
        {
            if (!clientServerDicionary.ContainsKey(Context.ConnectionId))
            {
                // ReAdd Client to server (should only be necessary if the Server tried to callback the client before the connection could be reestablished.)
                int id = SimulationServer.Register(Context.ConnectionId, Clients.Caller, out var server);
                lock (clientServerDicionary)
                {
                    clientServerDicionary.Add(Context.ConnectionId, server);
                    clientIdDictionary.Add(Context.ConnectionId, id);
                }
            }
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            SimulationServer.Unregister(Context.ConnectionId);
            lock (clientServerDicionary)
            {
                clientServerDicionary.Remove(Context.ConnectionId);
                clientIdDictionary.Remove(Context.ConnectionId);
            }
            return base.OnDisconnected(stopCalled);
        }

        public Task Goodbye()
        {
            return Task.CompletedTask;
        }

        public Task<int> Hello(string username)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.ChangeUsername(Context.ConnectionId, username);
            if (clientIdDictionary.TryGetValue(Context.ConnectionId, out var id))
                return Task.FromResult(id);
            return Task.FromResult(-1);
        }

        public Task AquireMaster()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.AquireMaster(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task ChangeUsername(string name)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.ChangeUsername(Context.ConnectionId, name);
            return Task.CompletedTask;
        }

        public Task FreeMaster()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.FreeMaster(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task PauseSimulation()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.PauseSimulation(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task PitchSimulation(byte frames)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.PitchSimulation(Context.ConnectionId, frames);
            return Task.CompletedTask;
        }

        public Task ResumeSimulation()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.ResumeSimulation(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task SendMessage(string message)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.SendMessage(Context.ConnectionId, message);
            return Task.CompletedTask;
        }

        public Task SetMasterState(byte slot, PlayerColor color, byte team, bool ready)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.SetMasterState(Context.ConnectionId, slot, color, team, ready);
            return Task.CompletedTask;
        }

        public Task SetPlayerState(byte slot, PlayerColor color, byte team, bool ready)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.SetPlayerState(Context.ConnectionId, slot, color, team, ready);
            return Task.CompletedTask;
        }

        public Task StartSimulation()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.StartSimulation(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task StopSimulation()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.StopSimulation(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task UnsetPlayerState()
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.UnsetPlayerState(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task UploadLevel(TypeInfo level)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.UploadLevel(Context.ConnectionId, level);
            return Task.CompletedTask;
        }

        public Task UploadMaster(byte slot, TypeInfo player)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.UploadMaster(Context.ConnectionId, slot, player);
            return Task.CompletedTask;
        }

        public Task UploadPlayer(TypeInfo player)
        {
            clientServerDicionary.TryGetValue(Context.ConnectionId, out var server);
            server?.UploadPlayer(Context.ConnectionId, player);
            return Task.CompletedTask;
        }
    }
}
