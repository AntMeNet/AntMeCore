using AntMe.Runtime.EventLog;
using AntMe.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    /// <summary>
    /// The WCF specific Version of the Simulation Client.
    /// </summary>
    internal sealed class WcfSimulationClient : ISimulationClient
    {
        private int masterId = -1;
        private List<UserProfile> users = new List<UserProfile>();
        private List<Slot> slots = new List<Slot>(AntMe.Level.MAX_SLOTS);
        private FrameToByteSerializer _deserializer;
        private SimulationState _serverState = SimulationState.Stopped;
        private Frame _currentState;
        private LevelInfo _level;
        private byte _rate = SimulationServer.INITFRAMERATE;

        private WcfSimulationCallback callback;
        private ISimulationService channel;
        private DuplexChannelFactory<ISimulationService> factory;

        private string[] extensionPaths;

        #region Construction

        /// <summary>
        /// Private Constructor
        /// </summary>
        /// <param name="callback">Instance of the Callback Class</param>
        internal WcfSimulationClient(string[] extensionPaths, WcfSimulationCallback callback)
        {
            ClientId = -1;
            IsOpen = false;
            IsReady = false;
            Protocol = 0;

            this.extensionPaths = extensionPaths;

            this.callback = callback;
            this.callback.OnLevelChanged += callback_OnLevelChanged;
            this.callback.OnMasterChanged += callback_OnMasterChanged;
            this.callback.OnMessageReceived += callback_OnMessageReceived;
            this.callback.OnPlayerChanged += callback_OnPlayerChanged;
            this.callback.OnPlayerReset += callback_OnPlayerReset;
            this.callback.OnSimulationChanged += callback_OnSimulationChanged;
            this.callback.OnSimulationState += callback_OnSimulationState;
            this.callback.OnUserAdded += callback_OnUserAdded;
            this.callback.OnUserDropped += callback_OnUserDropped;
            this.callback.OnUserlistChanged += callback_OnUserlistChanged;
            this.callback.OnUsernameChanged += callback_OnUsernameChanged;

            for (byte i = 0; i < AntMe.Level.MAX_SLOTS; i++)
                slots.Add(new Slot() { Id = i });
        }

        /// <summary>
        /// Creates the Connection to the Service Connector Factory & Channel.
        /// </summary>
        /// <param name="factory">Factory</param>
        /// <param name="channel">Channel</param>
        internal void Create(DuplexChannelFactory<ISimulationService> factory, ISimulationService channel)
        {
            this.channel = channel;
            this.factory = factory;
        }

        /// <summary>
        /// Returns the Client Id.
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        /// Returns if the Connection is open.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Returns if the Connection is ready for Communication.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Returns the Protocol Number.
        /// </summary>
        public byte Protocol { get; private set; }

        /// <summary>
        /// Opens the Connection to the Server.
        /// </summary>
        /// <param name="username">Username</param>
        public void Open(string username)
        {
            try
            {
                int id = channel.Hello(username);
                ClientId = id;
                IsOpen = true;
                IsReady = false;
                Protocol = 1;
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        /// <summary>
        /// Opens the Connection to the Server.
        /// </summary>
        public void Open()
        {
            string username = "Unknown";
            Open(username);
        }

        /// <summary>
        /// Close Connection by Error.
        /// </summary>
        /// <param name="ex"></param>
        public void CloseByError(Exception ex)
        {
            OnError?.Invoke(this, ex.Message);

            Close();
        }

        /// <summary>
        /// Close Connection.
        /// </summary>
        public void Close()
        {
            // Try to call the last "Goodbye"
            try
            {
                channel?.Goodbye();
            }
            catch (Exception) { }

            OnClose?.Invoke(this);
        }

        /// <summary>
        /// Event to inform about closing the connection.
        /// </summary>
        public event CloseClientDelegate OnClose;

        /// <summary>
        /// Event to inform about closing by Error.
        /// </summary>
        public event ErrorClientDelegate OnError;

        #endregion

        #region Connection

        /// <summary>
        /// Is the current Client the Server Master?
        /// </summary>
        public bool IsMaster
        {
            get { return masterId == ClientId; }
        }

        /// <summary>
        /// Returns the Current Level Information.
        /// </summary>
        public LevelInfo Level
        {
            get { return _level; }
        }

        public Log EventLog
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// List of Users.
        /// </summary>
        public ReadOnlyCollection<UserProfile> Users
        {
            get { return users.AsReadOnly(); }
        }

        /// <summary>
        /// List of Slots.
        /// </summary>
        public ReadOnlyCollection<Slot> Slots
        {
            get { return slots.AsReadOnly(); }
        }

        /// <summary>
        /// User Profile of the Server Master.
        /// </summary>
        public UserProfile Master
        {
            get { return users.SingleOrDefault(u => u.Id == masterId); }
        }

        /// <summary>
        /// Current User Profile.
        /// </summary>
        public UserProfile Me
        {
            get { return users.SingleOrDefault(u => u.Id == ClientId); }
        }

        /// <summary>
        /// Current Server State.
        /// </summary>
        public SimulationState ServerState
        {
            get { return _serverState; }
        }

        /// <summary>
        /// Latest Simulation State.
        /// </summary>
        public Frame CurrentState
        {
            get { return _currentState; }
        }

        /// <summary>
        /// Current Simulation Framerate.
        /// </summary>
        public byte Rate
        {
            get { return _rate; }
        }

        

        /// <summary>
        /// Change the Username
        /// </summary>
        /// <param name="name">New User</param>
        /// <returns>Success</returns>
        public bool ChangeUsername(string name)
        {
            try
            {
                channel.ChangeUsername(name);
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        /// <summary>
        /// Send Chat Message
        /// </summary>
        /// <param name="message">Message</param>
        public void SendMessage(string message)
        {
            try
            {
                channel.SendMessage(message);
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        #endregion

        #region Simulation Management

        /// <summary>
        /// Aqure the Master-Slot
        /// </summary>
        /// <returns>Success</returns>
        public bool AquireMaster()
        {
            try
            {
                channel.AquireMaster();
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        /// <summary>
        /// Free the Master Slot.
        /// </summary>
        /// <returns>Success</returns>
        public bool FreeMaster()
        {
            try
            {
                channel.FreeMaster();
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        public bool UploadLevel(TypeInfo level)
        {
            try
            {
                channel.UploadLevel(level);
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        public bool UploadPlayer(TypeInfo player)
        {
            try
            {
                channel.UploadPlayer(player);
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        public bool UploadMaster(byte slot, TypeInfo player)
        {
            try
            {
                channel.UploadMaster(slot, player);
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        public bool SetPlayerState(byte slot, PlayerColor color, byte team, bool ready)
        {
            try
            {
                channel.SetPlayerState(slot, color, team, ready);
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public bool UnsetPlayerState()
        {
            try
            {
                channel.UnsetPlayerState();
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public bool SetMasterState(byte slot, PlayerColor color, byte team, bool ready)
        {
            try
            {
                channel.SetMasterState(slot, color, team, ready);
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        #endregion

        #region Flow

        /// <summary>
        /// Start the Simulation.
        /// </summary>
        /// <returns>Success</returns>
        public bool StartSimulation()
        {
            try
            {
                channel.StartSimulation();
                return true;
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }

        }

        /// <summary>
        /// Pause the Simulation.
        /// </summary>
        public void PauseSimulation()
        {
            try
            {
                channel.PauseSimulation();
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        /// <summary>
        /// Resume the Simulation.
        /// </summary>
        public void ResumeSimulation()
        {
            try
            {
                channel.ResumeSimulation();
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        /// <summary>
        /// Stops the Simulation.
        /// </summary>
        public void StopSimulation()
        {
            try
            {
                channel.StopSimulation();
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        /// <summary>
        /// Change the Framerate of the Simulation.
        /// </summary>
        /// <param name="frames"></param>
        public void PitchSimulation(byte frames)
        {
            try
            {
                channel.PitchSimulation(frames);
            }
            catch (FaultException<AntMeFault> fault)
            {
                throw new ArgumentException(fault.Detail.Description);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        #endregion

        public event SimulationClientDelegate<UserProfile> OnMasterChanged;

        public event SimulationClientDelegate OnUserlistChanged;

        public event SimulationClientDelegate<UserProfile> OnUserAdded;

        public event SimulationClientDelegate<int> OnUserDropped;

        public event SimulationClientDelegate<UserProfile> OnUsernameChanged;

        public event SimulationClientDelegate<UserProfile, string> OnMessageReceived;

        public event SimulationClientDelegate<LevelInfo> OnLevelChanged;

        public event SimulationClientDelegate OnPlayerReset;

        public event SimulationClientDelegate<byte> OnPlayerChanged;

        public event SimulationClientDelegate<SimulationState, byte> OnSimulationChanged;

        public event SimulationClientDelegate<Frame> OnSimulationState;

        public void Dispose()
        {
            // TODO (Deserializer?)
        }

        #region Server Callbacks

        private void callback_OnUsernameChanged(UserProfile user)
        {
            lock (users)
            {
                var hit = users.SingleOrDefault(u => u.Id == user.Id);
                if (hit != null)
                {
                    hit.Username = user.Username;
                }
            }

            OnUsernameChanged?.Invoke(this, user);
        }

        private void callback_OnUserlistChanged(UserProfile[] parameter)
        {
            lock (users)
            {
                this.users.Clear();
                this.users.AddRange(parameter);
            }

            OnUserlistChanged?.Invoke(this);
        }

        private void callback_OnUserDropped(int id)
        {
            bool hit = false;
            lock (users)
            {
                var user = users.SingleOrDefault(u => u.Id == id);
                if (user != null)
                {
                    hit = true;
                    users.Remove(user);
                }
            }

            if (hit)
                OnUserDropped?.Invoke(this, id);
        }

        private void callback_OnUserAdded(UserProfile user)
        {
            bool hit = false;
            lock (users)
            {
                // Check, if exists
                if (!users.Any(u => u.Id == user.Id))
                {
                    hit = true;
                    users.Add(user);
                }
            }

            if (hit)
                OnUserAdded?.Invoke(this, user);
        }

        private void callback_OnSimulationState(byte[] parameter)
        {
            // Skip, as long there is no Deserializer
            if (_deserializer == null)
            {
                SimulationContext context = ExtensionLoader.CreateSimulationContext();
                _deserializer = new FrameToByteSerializer(context);
            }

            // Set latest Main State
            _currentState = _deserializer.Deserialize(parameter);

            OnSimulationState?.Invoke(this, _currentState);
        }

        private void callback_OnSimulationChanged(SimulationState state, byte framerate)
        {
            // Server State changed from running to stopped
            if (_serverState != SimulationState.Stopped && state == SimulationState.Stopped)
            {
                // Trash Deserializer
                _deserializer?.Dispose();
                _deserializer = null;
                
                // Remove last State
                _currentState = null;
            }

            //// Server State changed from stopped to running
            //if (_serverState == SimulationState.Stopped && state != SimulationState.Stopped)
            //{
            //    if (_deserializer == null)
            //    {
            //        _deserializer = new StateDeserializer();
            //    }
            //}

            // Set local Properties
            _serverState = state;
            _rate = framerate;

            // Drop Event
            OnSimulationChanged?.Invoke(this, state, framerate);
        }

        private void callback_OnPlayerReset(Slot[] parameter)
        {
            foreach (var slot in parameter)
            {
                if (slot.Id < 0 || slot.Id > 7)
                    continue;

                slots[slot.Id].ColorKey = slot.ColorKey;
                slots[slot.Id].PlayerInfo = slot.PlayerInfo;
                slots[slot.Id].Profile = slot.Profile;
                slots[slot.Id].ReadyState = slot.ReadyState;
                slots[slot.Id].Team = slot.Team;
            }

            OnPlayerReset?.Invoke(this);
        }

        private void callback_OnPlayerChanged(Slot slot)
        {
            var hit = slots.SingleOrDefault(s => s.Id == slot.Id);
            if (hit != null)
            {
                hit.ColorKey = slot.ColorKey;
                hit.PlayerInfo = slot.PlayerInfo;
                hit.Profile = slot.Profile;
                hit.ReadyState = slot.ReadyState;
                hit.Team = slot.Team;

                OnPlayerChanged?.Invoke(this, hit.Id);
            }
        }

        private void callback_OnMessageReceived(UserProfile sender, string message)
        {
            OnMessageReceived?.Invoke(this, sender, message);
        }

        private void callback_OnMasterChanged(int id)
        {
            masterId = id;

            OnMasterChanged?.Invoke(this, Master);
        }

        private void callback_OnLevelChanged(TypeInfo level)
        {
            LevelInfo levelInfo = null;
            if (level != null)
                levelInfo = ExtensionLoader.SecureFindLevel(extensionPaths, level);

            _level = levelInfo;

            OnLevelChanged?.Invoke(this, levelInfo);
        }

        #endregion
    }
}
