using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AntMe.Runtime.Client.Communication;
using AntMe.Runtime.Communication;
using AntMe.Runtime.EventLog;
using AntMe.Serialization;
using Microsoft.AspNet.SignalR.Client;

namespace AntMe.Runtime.Client
{
    /// <summary>
    /// The SignalR specific version of the simulation client.
    /// </summary>
    public sealed class SignalRSimulationClient : ISimulationClient
    {
        private readonly string[] _extensionPaths;

        private IHubProxy _hubProxy;
        private int _masterId = -1;
        private List<UserProfile> _users;
        private List<Slot> _slots;
        private LevelStateByteSerializer _deserializer;

        public SignalRSimulationClient(string[] extensionPaths)
        {
            _extensionPaths = extensionPaths;
            _users = new List<UserProfile>();
            _slots = new List<Slot>(AntMe.Level.MAX_SLOTS);
            SimulationContext context = ExtensionLoader.CreateSimulationContext();
            _deserializer = new LevelStateByteSerializer(context);

            ServerState = Runtime.SimulationState.Stopped;
            Rate = AntMe.Level.FRAMES_PER_SECOND;

        }

        public async Task Create(string uri)
        {
            var hubConnection = new HubConnection(uri);
            _hubProxy = hubConnection.CreateHubProxy("AntMeSimulationService");

            // register Callbacks
            _hubProxy.On<int>(nameof(ISimulationCallback.MasterChanged), MasterChanged);
            _hubProxy.On<UserProfile[]>(nameof(ISimulationCallback.UserlistChanged), UserlistChanged);
            _hubProxy.On<UserProfile>(nameof(ISimulationCallback.UserAdded), UserAdded);
            _hubProxy.On<int>(nameof(ISimulationCallback.UserDropped), UserDropped);
            _hubProxy.On<UserProfile>(nameof(ISimulationCallback.UsernameChanged), UsernameChanged);
            _hubProxy.On<UserProfile, string>(nameof(ISimulationCallback.MessageReceived), MessageReceived);
            _hubProxy.On<TypeInfo>(nameof(ISimulationCallback.LevelChanged), LevelChanged);
            _hubProxy.On<Slot[]>(nameof(ISimulationCallback.PlayerReset), PlayerReset);
            _hubProxy.On<Slot>(nameof(ISimulationCallback.PlayerChanged), PlayerChanged);
            _hubProxy.On<SimulationState, byte>(nameof(ISimulationCallback.SimulationChanged), SimulationChanged);
            _hubProxy.On<byte[]>(nameof(ISimulationCallback.SimulationState), SimulationState);

            await hubConnection.Start();
        }

        #region SimulationClient

        /// <summary>
        /// Opens the Connection to the Server.
        /// </summary>
        public Task Open()
        {
            return Open("Unkown");
        }

        /// <summary>
        /// Opens the Connection to the Server.
        /// </summary>
        /// <param name="username">Username</param>
        public async Task Open(string username)
        {
            try
            {
                int id = await Hello(username);
                ClientId = id;
                IsOpen = true;
                IsReady = false;
                Protocol = 1;
            }
            catch (Exception ex)
            {
                await CloseByError(ex);
                throw;
            }
        }

        /// <summary>
        /// Schließt die Verbindung aufgrund einer Exception.
        /// </summary>
        /// <param name="ex">Error</param>
        public Task CloseByError(Exception ex)
        {
            OnError?.Invoke(this, ex);
            return Close();
        }

        /// <summary>
        /// Schließt die Verbindung zum Server, falls offen.
        /// </summary>
        public async Task Close()
        {
            // Try to call the last "Goodbye"
            try
            {
                await Goodbye();
            }
            catch (Exception) { }

            OnClose?.Invoke(this, new EventArgs());
        }


        public void Dispose()
        {
            // TODO (Deserializer?)
        }

        #endregion

        #region Client Methods

        private void MasterChanged(int id)
        {
            _masterId = id;

            OnMasterChanged?.Invoke(this, Master);
        }

        private void UserlistChanged(UserProfile[] users)
        {
            lock (_users)
            {
                _users.Clear();
                _users.AddRange(users);
            }

            OnUserlistChanged?.Invoke(this, new EventArgs());
        }

        private void UserAdded(UserProfile user)
        {
            bool hit = false;
            lock (_users)
            {
                // Check, if exists
                if (_users.All(u => u.Id != user.Id))
                {
                    hit = true;
                    _users.Add(user);
                }
            }

            if (hit)
                OnUserAdded?.Invoke(this, user);
        }

        private void UserDropped(int id)
        {
            bool hit = false;
            lock (_users)
            {
                var user = _users.SingleOrDefault(u => u.Id == id);
                if (user != null)
                {
                    hit = true;
                    _users.Remove(user);
                }
            }

            if (hit)
                OnUserDropped?.Invoke(this, id);
        }

        private void UsernameChanged(UserProfile user)
        {
            lock (_users)
            {
                var hit = _users.SingleOrDefault(u => u.Id == user.Id);
                if (hit != null)
                {
                    hit.Username = user.Username;
                }
            }

            OnUsernameChanged?.Invoke(this, user);
        }

        private void MessageReceived(UserProfile sender, string message)
        {
            OnMessageReceived?.Invoke(this, sender, message);
        }

        private void LevelChanged(TypeInfo level)
        {
            LevelInfo levelInfo = null;
            if (level != null)
                levelInfo = ExtensionLoader.SecureFindLevel(_extensionPaths, level.AssemblyFile, level.TypeName);

            Level = levelInfo;

            OnLevelChanged?.Invoke(this, levelInfo);
        }

        private void PlayerReset(Slot[] parameter)
        {
            foreach (var slot in parameter)
            {
                if (slot.Id > 7)
                    continue;

                _slots[slot.Id].ColorKey = slot.ColorKey;
                _slots[slot.Id].PlayerInfo = slot.PlayerInfo;
                _slots[slot.Id].Profile = slot.Profile;
                _slots[slot.Id].ReadyState = slot.ReadyState;
                _slots[slot.Id].Team = slot.Team;
            }

            OnPlayerReset?.Invoke(this, new EventArgs());
        }

        private void PlayerChanged(Slot slot)
        {
            var hit = _slots.SingleOrDefault(s => s.Id == slot.Id);
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

        private void SimulationChanged(SimulationState state, byte framerate)
        {
            // Server State changed from running to stopped
            if (ServerState != Runtime.SimulationState.Stopped && state == Runtime.SimulationState.Stopped)
            {
                // Trash Deserializer
                _deserializer?.Dispose();
                _deserializer = null;

                // Remove last State
                CurrentState = null;
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
            ServerState = state;
            Rate = framerate;

            // Drop Event
            OnSimulationChanged?.Invoke(this, state, framerate);
        }

        private void SimulationState(byte[] state)
        {
            // Skip, as long there is no Deserializer
            if (_deserializer == null)
            {
                SimulationContext context = ExtensionLoader.CreateSimulationContext();
                _deserializer = new LevelStateByteSerializer(context);
            }

            // Set latest Main State
            CurrentState = _deserializer.Deserialize(state);

            OnSimulationState?.Invoke(this, CurrentState);
        }

        #endregion

        #region Server Methods

        public Task<int> Hello(string username)
        {
            return _hubProxy?.Invoke<int>(nameof(Hello), username);
        }

        public Task Goodbye()
        {
            return _hubProxy?.Invoke(nameof(Goodbye));
        }

        #region Master Handling


        public Task AquireMaster()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(AquireMaster));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task FreeMaster()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(FreeMaster));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        #endregion

        #region User Handling


        public Task ChangeUsername(string name)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(ChangeUsername), name);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        #endregion

        #region Chat Handling

        public Task SendMessage(string message)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(SendMessage), message);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        #endregion

        #region Settings Handling

        public Task UploadLevel(TypeInfo level)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(UploadLevel), level);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task UploadPlayer(TypeInfo player)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(UploadPlayer), player);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task UploadMaster(byte slot, TypeInfo player)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(UploadMaster), slot, player);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task SetPlayerState(byte slot, PlayerColor color, byte team, bool ready)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(SetPlayerState), slot, color, team, ready);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task UnsetPlayerState()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(UnsetPlayerState));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task SetMasterState(byte slot, PlayerColor color, byte team, bool ready)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(SetMasterState), slot, color, team, ready);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        #endregion

        #region Flow Handling

        public Task StartSimulation()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(StartSimulation));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task PauseSimulation()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(PauseSimulation));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task ResumeSimulation()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(ResumeSimulation));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task StopSimulation()
        {
            try
            {
                return _hubProxy?.Invoke(nameof(StopSimulation));
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        public Task PitchSimulation(byte frames)
        {
            try
            {
                return _hubProxy?.Invoke(nameof(PitchSimulation), frames);
            }
            catch (Exception ex)
            {
                CloseByError(ex);
                throw;
            }
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// ID der Verbindung - gesetzt vom Server.
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        /// Gibt an, ob die Verbindung offen ist.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// GIbt an, ob Header und ID übertragen wurde und die Verbindung 
        /// bereit zum Empfang ist.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Gibt die Versionsnummer des Übertragungsprotokolls an.
        /// </summary>
        public byte Protocol { get; private set; }

        /// <summary>
        /// Gibt an, ob der aktuelle Client der Spiel Master ist.
        /// </summary>
        public bool IsMaster => _masterId == ClientId;

        /// <summary>
        /// Gibt das aktuell geladene Level an.
        /// </summary>
        public LevelInfo Level { get; private set; }

        /// <summary>
        /// EventLog für diese Simulation.
        /// </summary>
        public Log EventLog => throw new NotImplementedException();

        /// <summary>
        /// Eine Liste der aktuell verbundenen User.
        /// </summary>
        public ReadOnlyCollection<UserProfile> Users => _users.AsReadOnly();

        /// <summary>
        /// Eine Liste der Level-Slots.
        /// </summary>
        public ReadOnlyCollection<Slot> Slots => _slots.AsReadOnly();

        /// <summary>
        /// GIbt das User-Profil des Masters zurück oder null, falls es noch keinen gibt.
        /// </summary>
        public UserProfile Master => _users.SingleOrDefault(u => u.Id == _masterId);

        /// <summary>
        /// Gibt das eigene User-Profil zurück.
        /// </summary>
        public UserProfile Me => _users.SingleOrDefault(u => u.Id == ClientId);

        /// <summary>
        /// Status des verbundenen Servers.
        /// </summary>
        public SimulationState ServerState { get; private set; }

        /// <summary>
        /// Snapshot des aktuellen Simulationsstands oder null, falls die Simulation nicht läuft.
        /// </summary>
        public LevelState CurrentState { get; private set; }

        /// <summary>
        /// Gibt die Framerate (Frames pro Sekunde) an.
        /// </summary>
        public byte Rate { get; private set; }

        #endregion

        #region Events

        public event EventHandler<UserProfile> OnMasterChanged;

        public event EventHandler OnUserlistChanged;

        public event EventHandler<UserProfile> OnUserAdded;

        public event EventHandler<int> OnUserDropped;

        public event EventHandler<UserProfile> OnUsernameChanged;

        public event Action<ISimulationClient, UserProfile, string> OnMessageReceived;

        public event EventHandler<LevelInfo> OnLevelChanged;

        public event EventHandler OnPlayerReset;

        public event EventHandler<byte> OnPlayerChanged;

        public event Action<ISimulationClient, SimulationState, byte> OnSimulationChanged;

        public event EventHandler<LevelState> OnSimulationState;

        public event EventHandler<Exception> OnError;

        public event EventHandler OnClose;

        #endregion
    }
}
