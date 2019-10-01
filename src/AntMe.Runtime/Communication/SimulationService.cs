using System;
using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false
    )]
    internal sealed class SimulationService : ISimulationService, IDisposable
    {
        private readonly ISimulationCallback callback;

        private readonly int id;
        private readonly SimulationServer server;

        public SimulationService()
        {
            callback = OperationContext.Current.GetCallbackChannel<ISimulationCallback>();
            id = SimulationServer.Register(this, callback, out server);
        }

        public void Dispose()
        {
            SimulationServer.Unregister(this);
        }

        [OperationBehavior(
            ReleaseInstanceMode = ReleaseInstanceMode.BeforeCall
        )]
        public int Hello(string username)
        {
            try
            {
                server.ChangeUsername(this, username);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }

            return id;
        }

        [OperationBehavior(
            ReleaseInstanceMode = ReleaseInstanceMode.AfterCall
        )]
        public void Goodbye()
        {
        }

        public void AquireMaster()
        {
            try
            {
                server.AquireMaster(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void FreeMaster()
        {
            try
            {
                server.FreeMaster(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void ChangeUsername(string name)
        {
            try
            {
                server.ChangeUsername(this, name);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                server.SendMessage(this, message);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void UploadLevel(TypeInfo level)
        {
            try
            {
                server.UploadLevel(this, level);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void UploadPlayer(TypeInfo player)
        {
            try
            {
                server.UploadPlayer(this, player);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void UploadMaster(byte slot, TypeInfo player)
        {
            try
            {
                server.UploadMaster(this, slot, player);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void SetPlayerState(byte slot, PlayerColor color, byte team, bool ready)
        {
            try
            {
                server.SetPlayerState(this, slot, color, team, ready);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void UnsetPlayerState()
        {
            try
            {
                server.UnsetPlayerState(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void SetMasterState(byte slot, PlayerColor color, byte team, bool ready)
        {
            try
            {
                server.SetMasterState(this, slot, color, team, ready);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void StartSimulation()
        {
            try
            {
                server.StartSimulation(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void PauseSimulation()
        {
            try
            {
                server.PauseSimulation(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void ResumeSimulation()
        {
            try
            {
                server.ResumeSimulation(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void StopSimulation()
        {
            try
            {
                server.StopSimulation(this);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }

        public void PitchSimulation(byte frames)
        {
            try
            {
                server.PitchSimulation(this, frames);
            }
            catch (Exception ex)
            {
                throw new FaultException<AntMeFault>(new AntMeFault(), ex.Message);
            }
        }
    }
}