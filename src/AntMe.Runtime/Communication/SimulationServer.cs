using AntMe.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;

namespace AntMe.Runtime.Communication
{
    /// <summary>
    /// Host for WCF based Remote Simulations.
    /// </summary>
    public sealed class SimulationServer
    {
        #region Constants

        /// <summary>
        /// Initial Framerate for the simulation.
        /// </summary>
        public const byte INITFRAMERATE = Level.FramesPerSecond;

        /// <summary>
        /// Default Name for Pipes.
        /// </summary>
        public const string DEFAULTPIPE = "AntMeService";

        /// <summary>
        /// Default Port for TCP Connections.
        /// </summary>
        public const int DEFAULTPORT = 599;

        /// <summary>
        /// Maximum Size for Levels.
        /// </summary>
        public const int MAXLEVELSIZE = 100000;

        /// <summary>
        /// Maximum Size for Player-Files.
        /// </summary>
        public const int MAXPLAYERSIZE = 100000;

        #endregion

        #region Static Server Methods

        /// <summary>
        /// Instance of Service Host.
        /// </summary>
        private static SimulationServer instance;

        /// <summary>
        /// Starts to listen for new connections.
        /// </summary>
        /// <param name = "extensionPaths" > List of pathes to search for Extensions</param>
        /// <param name="pipeName">name of NamedPipe or null, if disabled</param>
        /// <param name="port">TCP Port to listen for new connection or 0, if disabled</param>
        public static void Start(string[] extensionPaths, string pipeName, int port)
        {
            if (instance != null)
                throw new Exception("Server is already running");

            instance = new SimulationServer(extensionPaths, pipeName, port);
            try
            {
                instance.Open();
            }
            catch (Exception)
            {
                instance = null;
                throw;
            }
        }

        /// <summary>
        /// Stops running server.
        /// </summary>
        public static void Stop()
        {
            if (instance == null)
                throw new Exception("There is no running server");

            try
            {
                instance.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                instance = null;
            }
        }

        /// <summary>
        /// Returns current Server State.
        /// </summary>
        public static bool IsRunning { get { return instance != null; } }

        /// <summary>
        /// Registers a new Client at the server Instance.
        /// </summary>
        /// <param name="service">Reference to the service</param>
        /// <param name="callback">Reference to the callback</param>
        /// <param name="server">output Parameter for the server Reference</param>
        /// <returns>New Id for this client.</returns>
        internal static int Register(ISimulationService service, ISimulationCallback callback, out SimulationServer server)
        {
            if (instance == null)
                throw new Exception("Server is not running");

            server = instance;
            return instance.RegisterClient(service, callback);
        }

        /// <summary>
        /// Unregisters a client from the server.
        /// </summary>
        /// <param name="service"></param>
        internal static void Unregister(ISimulationService service)
        {
            instance.UnregisterClient(service);
        }

        #endregion

        #region Service Host Handling

        /// <summary>
        /// Pipe Name (NamedPipes)
        /// </summary>
        private string pipeName;

        /// <summary>
        /// Port Number (TCP)
        /// </summary>
        private int port;

        /// <summary>
        /// Service Host for incoming Connections.
        /// </summary>
        private ServiceHost host;

        /// <summary>
        /// List of pathes to search for Extensions.
        /// </summary>
        private string[] extensionPaths;

        private SimulationServer(string[] extensionPaths, string pipeName, int port)
        {
            this.pipeName = pipeName;
            this.port = port;
            this.extensionPaths = extensionPaths;
        }

        /// <summary>
        /// </summary>
        private void Open()
        {
            List<Uri> uris = new List<Uri>();
            if (!string.IsNullOrEmpty(pipeName))
                uris.Add(new Uri("net.pipe://localhost"));


            host = new ServiceHost(typeof(SimulationService), uris.ToArray());

            if (!string.IsNullOrEmpty(pipeName))
                host.AddServiceEndpoint(typeof(ISimulationService), new NetNamedPipeBinding(), pipeName);

            host.Description.Behaviors.Add(new ServiceThrottlingBehavior()
            {
                MaxConcurrentCalls = 500,
                MaxConcurrentInstances = 100,
                MaxConcurrentSessions = 200
            });

            host.Open();
        }

        private void Close()
        {
            // TODO: Disconnect all clients
            if (host != null)
            {
                host.Close();
            }
        }

        #endregion

        #region Connection Management

        private Dictionary<ISimulationService, ClientInfo> clients = new Dictionary<ISimulationService, ClientInfo>();

        private int nextId = 0;

        private class ClientInfo
        {
            public ISimulationService ServiceInterface { get; set; }

            public LevelStateByteSerializer Serializer { get; set; }

            public ISimulationCallback CallbackInterface { get; set; }

            public UserProfile UserProfile { get; set; }

            public TypeInfo PlayerType { get; set; }

            public PlayerInfo PlayerInfo { get; set; }
        }

        private int RegisterClient(ISimulationService service, ISimulationCallback callback)
        {
            int id;
            ClientInfo info;

            lock (clients)
            {
                id = nextId++;
                info = new ClientInfo()
                {
                    ServiceInterface = service,
                    CallbackInterface = callback,
                    UserProfile = new UserProfile()
                    {
                        Id = id,
                        Username = "Client " + id
                    }
                };

                clients.Add(service, info);
            }

            // Send new User to the rest
            SendUserAdded(info.UserProfile);

            // Full Info Set to the new Client
            SendUserInit(info);

            return id;
        }

        private void UnregisterClient(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                // Remove from Game
                int removedSlot = -1;
                bool removedAsMaster = false;
                lock (simulationLock)
                {
                    // Search for Slots with that user
                    foreach (var slot in slots)
                    {
                        if (slot.Profile == client.UserProfile)
                        {
                            // Reset Slot
                            slot.PlayerInfo = false;
                            playerInfos[slot.Id] = null;
                            playerTypes[slot.Id] = null;
                            slot.Profile = null;
                            slot.ReadyState = false;

                            removedSlot = slot.Id;
                            break;
                        }
                    }

                    // Remove from Master
                    if (master == client.UserProfile)
                    {
                        master = null;
                        removedAsMaster = true;
                    }
                }

                // Inform others about Slot Change
                if (removedSlot > -1)
                    SendPlayerChanged(slots[removedSlot]);

                // Inform others about Master Change
                if (removedAsMaster)
                    SendMasterChanged(-1);

                // Remove Client Reference
                lock (clients)
                {
                    client.PlayerType = null;
                    clients.Remove(service);
                }

                // Send remove User to the rest
                SendUserDropped(client.UserProfile.Id);
            }
        }

        #endregion

        #region Chat Management

        internal void ChangeUsername(ISimulationService service, string username)
        {
            // TODO: Check username

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                client.UserProfile.Username = username;

                // Tell others
                SendUsernameChanged(client.UserProfile);
            }
        }

        internal void SendMessage(ISimulationService service, string message)
        {
            // TODO: Validate?

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                // Tell others
                SendMessageReceived(client.UserProfile, message);
            }
        }

        #endregion

        #region Match Management

        private UserProfile master = null;

        private object simulationLock = new object();
        private TypeInfo levelType = null;
        private LevelInfo levelInfo = null;
        private TypeInfo[] playerTypes = new TypeInfo[AntMe.Level.MaxSlots];
        private PlayerInfo[] playerInfos = new PlayerInfo[AntMe.Level.MaxSlots];
        private Slot[] slots = new[] 
        { 
            new Slot() { Id = 0, ColorKey = (PlayerColor)0, PlayerInfo = false, Profile = null, ReadyState = false, Team = 0 },
            new Slot() { Id = 1, ColorKey = (PlayerColor)1, PlayerInfo = false, Profile = null, ReadyState = false, Team = 1 },
            new Slot() { Id = 2, ColorKey = (PlayerColor)2, PlayerInfo = false, Profile = null, ReadyState = false, Team = 2 },
            new Slot() { Id = 3, ColorKey = (PlayerColor)3, PlayerInfo = false, Profile = null, ReadyState = false, Team = 3 },
            new Slot() { Id = 4, ColorKey = (PlayerColor)4, PlayerInfo = false, Profile = null, ReadyState = false, Team = 4 },
            new Slot() { Id = 5, ColorKey = (PlayerColor)5, PlayerInfo = false, Profile = null, ReadyState = false, Team = 5 },
            new Slot() { Id = 6, ColorKey = (PlayerColor)6, PlayerInfo = false, Profile = null, ReadyState = false, Team = 6 },
            new Slot() { Id = 7, ColorKey = (PlayerColor)7, PlayerInfo = false, Profile = null, ReadyState = false, Team = 7 }
        };

        private SecureSimulation simulation = null;
        private byte frames = SimulationServer.INITFRAMERATE;
        private bool paused = false;

        /// <summary>
        /// Aquires Master State for the caller.
        /// </summary>
        /// <param name="service">Caller</param>
        internal void AquireMaster(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                lock (simulationLock)
                {
                    // Skip, if Client is still master
                    if (master == client.UserProfile)
                        return;

                    // Someone else is Master
                    if (master != null)
                        throw new InvalidOperationException("Client '" + master.Username + "' is already Master");

                    master = client.UserProfile;
                }

                // Inform the Clients
                SendMasterChanged(master.Id);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        /// <summary>
        /// Clears the Master State
        /// </summary>
        /// <param name="service"></param>
        internal void FreeMaster(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                lock (simulationLock)
                {
                    // Someone else is master
                    if (master != client.UserProfile)
                        throw new InvalidOperationException("You are not the Master");

                    master = null;
                }

                // Inform the clients
                SendMasterChanged(-1);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }

        }

        /// <summary>
        /// Uploads a new Level to the System.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="levelType">TypeInfo of the new level</param>
        internal void UploadLevel(ISimulationService service, TypeInfo levelType)
        {
            // Check File Size
            if (levelType != null && levelType.AssemblyFile.Length > SimulationServer.MAXLEVELSIZE)
                throw new ArgumentException("File is too large");

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                // Analyse Level
                LevelInfo levelInfo = null;
                if (levelType != null)
                    levelInfo = ExtensionLoader.SecureFindLevel(extensionPaths, levelType.AssemblyFile, levelType.TypeName);

                lock (simulationLock)
                {
                    // Check for Master
                    if (master != client.UserProfile)
                        throw new InvalidOperationException("You are not the master");

                    // Check for running Simualtions
                    if (simulation != null)
                        throw new InvalidOperationException("Simulation is already running");

                    // Replace Level
                    if (levelInfo == null)
                    {
                        // Reset Level
                        this.levelInfo = null;
                        this.levelType = null;
                    }
                    else
                    {
                        // Load New Level
                        this.levelInfo = levelInfo;
                        this.levelType = levelType;
                    }

                    // Reset Slots
                    for (byte i = 0; i < Level.MaxSlots; i++)
                    {
                        slots[i].Profile = null;
                        playerInfos[i] = null;
                        playerTypes[i] = null;
                        slots[i].ColorKey = (PlayerColor)i;
                        slots[i].PlayerInfo = false;
                        slots[i].ReadyState = false;
                        slots[i].Team = i;
                    }
                }

                // Inform everybody about the Level Change
                SendLevelChanged(levelType);

                // Inform everybody about the Slot Reset
                SendPlayerReset(slots);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        /// <summary>
        /// Uploads a Client related Player File.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="playerType">Player File</param>
        internal void UploadPlayer(ISimulationService service, TypeInfo playerType)
        {
            // Check File Size
            if (playerType != null && playerType.AssemblyFile.Length > MAXPLAYERSIZE)
                throw new ArgumentException("File is too large");

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                // Analyse File
                PlayerInfo playerInfo = null;
                if (playerType != null)
                    playerInfo = ExtensionLoader.SecureFindPlayer(extensionPaths, playerType.AssemblyFile, playerType.TypeName);

                // Save Player Infos
                if (playerInfo == null)
                {
                    client.PlayerType = null;
                    client.PlayerInfo = null;
                }
                else
                {
                    client.PlayerType = playerType;
                    client.PlayerInfo = playerInfo;
                }

                // Apply to Used Slot
                Slot affectedSlot = null;
                lock (simulationLock)
                {
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (slots[i].Profile == client.UserProfile)
                        {
                            affectedSlot = slots[i];

                            // Set new Parameter
                            if (playerInfo != null)
                            {
                                playerInfos[i] = playerInfo;
                                playerTypes[i] = playerType;
                                affectedSlot.PlayerInfo = true;
                            }
                            else
                            {
                                playerInfos[i] = null;
                                playerTypes[i] = null;
                                affectedSlot.PlayerInfo = false;
                            }

                            break;
                        }
                    }
                }

                // Inform others
                if (affectedSlot != null)
                    SendPlayerChanged(affectedSlot);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        /// <summary>
        /// Uploads a Player File into a Slot
        /// </summary>
        /// <param name="service"></param>
        /// <param name="slot">Target Slot</param>
        /// <param name="playerType">Player File</param>
        internal void UploadMaster(ISimulationService service, byte slot, TypeInfo playerType)
        {
            // Check Slot
            if (slot < 0 || slot > 7)
                throw new ArgumentOutOfRangeException("Slot must be between 0 and 7");

            // Check File Size
            if (playerType != null && playerType.AssemblyFile.Length > SimulationServer.MAXPLAYERSIZE)
                throw new ArgumentException("File is too large");

            // Check for an existing Level
            if (levelInfo == null)
                throw new InvalidOperationException("No level is set yet");

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                PlayerInfo playerInfo = null;
                if (playerType != null)
                    playerInfo = ExtensionLoader.SecureFindPlayer(extensionPaths, playerType.AssemblyFile, playerType.TypeName);

                lock (simulationLock)
                {
                    // Check for Master
                    if (master != client.UserProfile)
                        throw new InvalidOperationException("You are not the master");

                    // Set File
                    if (playerInfo == null)
                    {
                        // Remove Master Blocker
                        if (slots[slot].Profile == master)
                            slots[slot].Profile = null;

                        playerInfos[slot] = null;
                        playerTypes[slot] = null;
                        slots[slot].PlayerInfo = false;
                        slots[slot].ReadyState = false;
                    }
                    else
                    {
                        // TODO: Faction-Matching
                        slots[slot].Profile = master;
                        playerInfos[slot] = playerInfo;
                        playerTypes[slot] = playerType;
                        slots[slot].PlayerInfo = true;
                    }
                }

                // Inform everybody about the Slot Change
                SendPlayerChanged(slots[slot]);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        /// <summary>
        /// Sets the Player State for the given Slot
        /// </summary>
        /// <param name="service"></param>
        /// <param name="slot">Target Slot</param>
        /// <param name="color">Prefered Color</param>
        /// <param name="team">Team Number</param>
        /// <param name="ready">Ready Flag</param>
        internal void SetPlayerState(ISimulationService service, byte slot, PlayerColor color, byte team, bool ready)
        {
            // Check Slot Range
            if (slot < 0 || slot > 7)
                throw new ArgumentOutOfRangeException("Slot must be between 0 and 7");

            // Check Team Range
            if (team < 0 || team > 7)
                throw new ArgumentOutOfRangeException("Team must be between 0 and 7");

            // Check for an existing Level
            if (levelInfo == null)
                throw new InvalidOperationException("No level is set yet");

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                Slot oldSlot = null;
                lock (simulationLock)
                {
                    if (slots[slot].Profile != null && slots[slot].Profile != client.UserProfile)
                        throw new InvalidOperationException("You are not Owner of this Slot");

                    // Free old Slot
                    for (int i = 0; i < slots.Length; i++)
                    {
                        // Skip current slot
                        if (i == slot)
                            continue;

                        // Reset old slot
                        if (slots[i].Profile == client.UserProfile)
                        {
                            oldSlot = slots[i];
                            slots[i].PlayerInfo = false;
                            slots[i].Profile = null;
                            slots[i].ReadyState = false;
                            playerInfos[i] = null;
                            playerTypes[i] = null;
                            break;
                        }
                    }

                    // TODO: Color Matching

                    // Also transfer PlayerInfo
                    if (client.PlayerType != null)
                    {
                        // TODO: Filter Matching
                        playerInfos[slot] = client.PlayerInfo;
                        playerTypes[slot] = client.PlayerType;
                        slots[slot].PlayerInfo = true;
                    }
                    else
                    {
                        slots[slot].PlayerInfo = false;
                    }

                    slots[slot].Profile = client.UserProfile;
                    slots[slot].Team = team;
                    slots[slot].ReadyState = ready;
                }

                // Send Info about the current Slot
                SendPlayerChanged(slots[slot]);

                // Send Info about the old Slot
                if (oldSlot != null)
                    SendPlayerChanged(oldSlot);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        /// <summary>
        /// Resets the Slot of the calling user
        /// </summary>
        /// <param name="service"></param>
        internal void UnsetPlayerState(SimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                Slot slot = null;
                lock (simulationLock)
                {
                    // Find Slot
                    for (int i = 0; i < slots.Length; i++)
                    {
                        // Reset old slot
                        if (slots[i].Profile == client.UserProfile)
                        {
                            slot = slots[i];
                            slot.PlayerInfo = false;
                            slot.Profile = null;
                            slot.ReadyState = false;
                            break;
                        }
                    }
                }

                // Send Info about the current Slot
                if (slot != null)
                    SendPlayerChanged(slot);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        /// <summary>
        /// Sets the Master State for the given Slot
        /// </summary>
        /// <param name="service"></param>
        /// <param name="slot">Target Slot</param>
        /// <param name="color">Prefered Color</param>
        /// <param name="team">Team Number</param>
        /// <param name="ready">Ready Flag</param>
        internal void SetMasterState(ISimulationService service, byte slot, PlayerColor color, byte team, bool ready)
        {
            // Check Slot Range
            if (slot < 0 || slot > 7)
                throw new ArgumentOutOfRangeException("Slot must be between 0 and 7");

            // Check Team Range
            if (team < 0 || team > 7)
                throw new ArgumentOutOfRangeException("Team must be between 0 and 7");

            // Check for an existing Level
            if (levelInfo == null)
                throw new InvalidOperationException("No level is set yet");

            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                if (master != client.UserProfile)
                    throw new InvalidOperationException("You are not the master");

                lock (simulationLock)
                {
                    // Allow only active Slots to be set by the Master
                    if (slots[slot].Profile == null)
                        throw new InvalidOperationException("Slot is not active");

                    // TODO: Color Matching

                    slots[slot].Team = team;
                    slots[slot].ReadyState = ready;
                }

                // Inform others
                SendPlayerChanged(slots[slot]);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        #endregion

        #region Flow Management

        internal void StartSimulation(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                lock (simulationLock)
                {
                    if (master != client.UserProfile)
                        throw new InvalidOperationException("You are not the master");

                    // Check for running Simulations
                    if (simulation != null)
                        throw new InvalidOperationException("There is a running Simulation");

                    if (levelInfo == null)
                        throw new InvalidOperationException("There is no level set");

                    Setup settings = new Setup()
                    {
                        Level = levelType
                    };

                    SimulationContext context = ExtensionLoader.CreateSimulationContext();
                    Map map = MapSerializer.Deserialize(context, levelInfo.Map);

                    int count = 0;
                    for (int i = 0; i < Level.MaxSlots; i++)
                    {
                        // Check Player
                        if (slots[i].Profile != null)
                        {
                            count++;

                            // Start Positions
                            if (i > map.StartPoints.Length)
                                throw new InvalidOperationException("No Startpoint for Slot " + i + " on this map");

                            // Player File available
                            if (!slots[i].PlayerInfo)
                                throw new InvalidOperationException("Slot " + i + " has no Player File uploaded");

                            // Ready Flag enabled
                            if (!slots[i].ReadyState)
                                throw new InvalidOperationException("Slot " + i + " is not ready");

                            // Faction Filter
                            if (levelInfo.FactionFilter.Where(f => f.SlotIndex == i).Count() > 0)
                            {
                                var playerInfo = playerInfos[i];
                                // TODO: Faction Filter
                            }
                            settings.Colors[i] = slots[i].ColorKey;
                            settings.Teams[i] = slots[i].Team;
                            settings.Player[i] = playerTypes[i];
                        }
                    }

                    // Min Playercount check
                    if (count < levelInfo.LevelDescription.MinPlayerCount)
                        throw new InvalidOperationException("Not enought player for this Map");

                    // Max Playercount check
                    if (count > levelInfo.LevelDescription.MaxPlayerCount)
                        throw new InvalidOperationException("Too many player for this Map");

                    simulation = new SecureSimulation(extensionPaths);
                    simulation.Start(settings);

                    // Start Simulation Loop
                    simulationThread = new Thread(SimulationLoop);
                    simulationThread.IsBackground = true;
                    simulationThread.Priority = ThreadPriority.Lowest;
                    simulationThread.Start();
                }

                // Inform everbody
                SendSimulationChanged(frames);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        internal void PauseSimulation(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                if (master != client.UserProfile)
                    throw new InvalidOperationException("You are not the master");

                // Check for running Simulations
                if (simulation == null)
                    throw new InvalidOperationException("There is no running Simulation");

                paused = true;

                // Inform everybody
                SendSimulationChanged(this.frames);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        internal void ResumeSimulation(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                if (master != client.UserProfile)
                    throw new InvalidOperationException("You are not the master");

                // Check for running Simulations
                if (simulation == null)
                    throw new InvalidOperationException("There is no running Simulation");

                paused = false;

                // Inform everybody
                SendSimulationChanged(this.frames);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        internal void StopSimulation(ISimulationService service)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                if (master != client.UserProfile)
                    throw new InvalidOperationException("You are not the master");

                // Check for running Simulations
                if (simulation == null)
                    throw new InvalidOperationException("There is no running Simulation");

                // Stop Simulation
                running = false;
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        internal void PitchSimulation(ISimulationService service, byte frames)
        {
            ClientInfo client;
            if (clients.TryGetValue(service, out client))
            {
                if (master != client.UserProfile)
                    throw new InvalidOperationException("You are not the master");

                this.frames = frames;

                // Inform everybody
                SendSimulationChanged(this.frames);
            }
            else
            {
                throw new InvalidOperationException("Client not registered");
            }
        }

        #endregion

        #region Simulation Loop

        private Thread simulationThread = null;
        private bool running = false;

        private void SimulationLoop()
        {
            running = true;
            Stopwatch watch = new Stopwatch();
            watch.Restart();

            while (running && simulation != null && simulation.State == Runtime.SimulationState.Running)
            {
                // Delay
                Thread.Sleep(1);

                // Check for Framerate
                if (!paused && watch.ElapsedMilliseconds > (1000 / frames))
                {
                    watch.Restart();

                    // TODO: Handle potential Problems
                    var state = simulation.NextState();
                    SendSimulationState(state);
                }
            }

            // Dispose Simulation
            simulation?.Dispose();
            simulation = null;

            // Inform everybody
            SendSimulationChanged(this.frames);

            // Disconnect Thread
            simulationThread = null;
        }

        #endregion

        #region Callbacks

        private void SendMasterChanged(int id)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.MasterChanged(id));
            }
        }

        private void SendUserInit(ClientInfo receiver)
        {
            lock (clients)
            {
                UserProfile[] profiles = clients.Values.Select(c => c.UserProfile).ToArray();
                RunAsync(receiver, () =>
                {
                    // Send the Full list of Users
                    receiver.CallbackInterface.UserlistChanged(profiles);

                    // Send Master State
                    receiver.CallbackInterface.MasterChanged(master != null ? master.Id : -1);

                    // Send Level Update
                    receiver.CallbackInterface.LevelChanged(levelType);

                    // Send Slot Reset
                    receiver.CallbackInterface.PlayerReset(slots);

                    // Send Simulation State
                    SimulationState state = SimulationState.Stopped;
                    if (simulation != null)
                        state = paused ? SimulationState.Paused : SimulationState.Running;
                    receiver.CallbackInterface.SimulationChanged(state, frames);
                });
            }
        }

        private void SendUserAdded(UserProfile user)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                {
                    // Skipp own user
                    if (receiver.UserProfile == user) continue;
                    RunAsync(receiver, () => receiver.CallbackInterface.UserAdded(user));
                }
            }
        }

        private void SendUserDropped(int id)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.UserDropped(id));
            }
        }

        private void SendUsernameChanged(UserProfile user)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.UsernameChanged(user));
            }
        }

        private void SendMessageReceived(UserProfile sender, string message)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.MessageReceived(sender, message));
            }
        }

        private void SendLevelChanged(TypeInfo level)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.LevelChanged(level));
            }
        }

        private void SendPlayerReset(Slot[] slots)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.PlayerReset(slots));
            }
        }

        private void SendPlayerChanged(Slot slot)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () => receiver.CallbackInterface.PlayerChanged(slot));
            }
        }

        private void SendSimulationChanged(byte framerate)
        {
            lock (clients)
            {
                // Send Simulation State
                SimulationState state = SimulationState.Stopped;
                if (simulation != null)
                    state = paused ? SimulationState.Paused : SimulationState.Running;

                foreach (var receiver in clients.Values)
                    RunAsync(receiver, () =>
                    {
                        // Create Serializer on Simulation Startup
                        if (state != SimulationState.Stopped && receiver.Serializer == null)
                        {
                            SimulationContext context = ExtensionLoader.CreateSimulationContext();
                            receiver.Serializer = new LevelStateByteSerializer(context);
                        }

                        // Dispose Serializer on Simulation Shutdown
                        if (state == SimulationState.Stopped)
                        {
                            receiver.Serializer?.Dispose();
                            receiver.Serializer = null;
                        }

                        receiver.CallbackInterface.SimulationChanged(state, framerate);
                    });
            }
        }

        private void SendSimulationState(LevelState state)
        {
            lock (clients)
            {
                foreach (var receiver in clients.Values)
                {
                    // Create a new Serializer
                    if (receiver.Serializer == null)
                    {
                        SimulationContext context = ExtensionLoader.CreateSimulationContext();
                        receiver.Serializer = new LevelStateByteSerializer(context);
                    }

                    // Serialize
                    byte[] buffer =  receiver.Serializer.Serialize(state);
                    RunAsync(receiver, () => receiver.CallbackInterface.SimulationState(buffer));
                }
            }
        }

        private void RunAsync(ClientInfo client, Action action)
        {
            Task t = new Task(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    // TODO: Log somewhere
                    Unregister(client.ServiceInterface);
                }
            });
            t.Start();
        }

        #endregion
    }
}
