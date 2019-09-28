using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AntMe.Runtime.Client.Communication;
using AntMe.Runtime.Communication;
using AntMe.Runtime.EventLog;

namespace AntMe.Runtime.Simulation.Communication
{
    /// <summary>
    /// Basisklasse für alle lokal stattfindenen Simulationen.
    /// </summary>
    internal abstract class LocalSimulationClient : ISimulationClient
    {
        protected string[] extensionPaths;
        protected ITypeResolver resolver;

        private readonly int clientId = 1;
        private UserProfile master;
        private List<UserProfile> users;
        private LevelInfo level;
        private byte rate;
        private Slot[] slots;
        private PlayerInfo[] players;
        private Log log;
        private Stopwatch watch = new Stopwatch();

        public LocalSimulationClient(string[] extensionPaths, ITypeResolver resolver)
        {
            this.resolver = resolver;
            this.extensionPaths = extensionPaths;

            // User erstellen
            master = new UserProfile() { Id = clientId, Username = "local" };
            users = new List<UserProfile>();
            users.Add(master);

            // Slots erstellen
            slots = new Slot[AntMe.Level.MAX_SLOTS];
            players = new PlayerInfo[AntMe.Level.MAX_SLOTS];
            for (byte i = 0; i < AntMe.Level.MAX_SLOTS; i++)
                slots[i] = new Slot() { Id = i, ColorKey = (PlayerColor)i };

            // Defaults
            rate = AntMe.Level.FRAMES_PER_SECOND;
            log = Log.CreateLog(true);
        }

        #region Common Properties

        /// <summary>
        /// Gibt immer true zurück, da es sich hier um eine lokale Simulation handelt.
        /// </summary>
        public bool IsMaster { get { return true; } }

        /// <summary>
        /// Liefert eine Referenz auf das Userprofil des Masters.
        /// </summary>
        public UserProfile Master { get { return master; } }

        /// <summary>
        /// Gibt den Master zurück, da es sich um eine lokale Simulation handelt.
        /// </summary>
        public UserProfile Me { get { return master; } }

        /// <summary>
        /// Gibt das aktuell eingestellte Level oder null zurück.
        /// </summary>
        public LevelInfo Level { get { return level; } }

        /// <summary>
        /// Liefert eine Liste der aktuell teilnehmenden User. Diese enthält nur den Master.
        /// </summary>
        public ReadOnlyCollection<UserProfile> Users { get { return users.AsReadOnly(); } }

        /// <summary>
        /// Gibt immer 1 zurück, da es sich hier um eine lokale Simulation handelt.
        /// </summary>
        public int ClientId { get { return clientId; } }

        /// <summary>
        /// Da es sich um eine lokale Simulation handelt, kommt hier immer true zurück.
        /// </summary>
        public bool IsOpen { get { return true; } }

        /// <summary>
        /// Da es sich um eine lokale Simulation handelt, kommt hier immer true zurück.
        /// </summary>
        public bool IsReady { get { return true; } }

        /// <summary>
        /// Da es sich um eine lokale Simulation handelt, kommt hier immer 1 zurück.
        /// </summary>
        public byte Protocol { get { return 1; } }

        /// <summary>
        /// Gibt die Frame-Rate in Frames pro Sekunde zurück.
        /// </summary>
        public byte Rate { get { return rate; } }

        /// <summary>
        /// Gibt das EventLog dieser Simulation zurück.
        /// </summary>
        public Log EventLog { get { return log; } }

        /// <summary>
        /// Liefert die liste der aktuell verfügbaren Slots zurück.
        /// </summary>
        public ReadOnlyCollection<Slot> Slots
        {
            get
            {
                if (this.level != null)
                    slots.Where(i => i.Id < level.LevelDescription.MaxPlayerCount)
                        .ToList().AsReadOnly();
                return new List<Slot>().AsReadOnly();
            }
        }

        #endregion

        #region Connection related

        /// <summary>
        /// Hat in diesem Kontext keine Funktion, da es sich um eine 
        /// lokale Simulation handelt.
        /// </summary>
        public Task Open()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hat in diesem Kontext keine Funktion, da es sich um eine 
        /// lokale Simulation handelt.
        /// </summary>
        /// <param name="username">Benutzername</param>
        public async Task Open(string username)
        {
            await Open();
            await ChangeUsername(username);
        }

        /// <summary>
        /// Verursacht einen Aufruf des OnError-Events, hat aber sonst keine 
        /// Auswirkungen, da es sich um eine lokale Simulation handelt.
        /// </summary>
        /// <param name="ex"></param>
        public Task CloseByError(Exception ex)
        {
            OnError?.Invoke(this, ex);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hat in diesem Kontext keine Funktion, da es sich um eine 
        /// lokale Simulation handelt.
        /// </summary>
        public Task Close()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wird in diesem Kontext nicht geworfen.
        /// </summary>
        public event EventHandler OnClose;

        /// <summary>
        /// Informiert über einen Fehler in der Simulation.
        /// </summary>
        public event EventHandler<Exception> OnError;

        /// <summary>
        /// Hat in diesem Kontext keine Funktion, da es sich um eine 
        /// lokale Simulation handelt.
        /// </summary>
        public Task<int> Hello(string username)
        {
            return Task.FromResult(-1);
        }

        /// <summary>
        /// Hat in diesem Kontext keine Funktion, da es sich um eine 
        /// lokale Simulation handelt.
        /// </summary>
        public Task Goodbye()
        {
            return Task.CompletedTask;
        }

        #endregion

        #region User related

        /// <summary>
        /// Hat hier keine Funktion, da es sich um eine lokale Simulation handelt und der Master bereits feststeht.
        /// </summary>
        /// <param name="message">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task AquireMaster()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hat hier keine Funktion, da es sich um eine lokale Simulation handelt und der Master bereits feststeht.
        /// </summary>
        /// <param name="message">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task FreeMaster()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hat hier keine Funktion, da es sich um eine lokale Simulation handelt und Mastername feststeht.
        /// </summary>
        /// <param name="message">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task ChangeUsername(string name)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event EventHandler<UserProfile> OnMasterChanged;

        /// <summary>
        /// Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event EventHandler OnUserlistChanged;

        /// <summary>
        /// Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event EventHandler<UserProfile> OnUserAdded;

        /// <summary>
        /// Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event EventHandler<int> OnUserDropped;

        /// <summary>
        /// Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event EventHandler<UserProfile> OnUsernameChanged;

        #endregion

        #region Chat related

        /// <summary>
        /// Sendet eine Nachricht an sich selbst, da es sich hier um eine lokale Simulation handelt.
        /// </summary>
        /// <param name="message">Nachricht</param>
        public Task SendMessage(string message)
        {
            OnMessageReceived?.Invoke(this, master, message);
            return Task.CompletedTask;
        }

        public event Action<ISimulationClient, UserProfile, string> OnMessageReceived;

        #endregion

        #region Settings related

        /// <summary>
        /// Legt das Level für diese Simulation fest. Darf nur im gestoppten Modus genutzt werden.
        /// </summary>
        /// <param name="level">Level Infos mit AssemblyFile und TypeName</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task UploadLevel(TypeInfo level)
        {
            // Sicher stellen, dass der Modus stimmt.
            if (ServerState != SimulationState.Stopped)
            {
                throw new Exception("Simulation already started");
            }

            if (level == null)
            {
                // Level leeren
                this.level = null;
                ResetSlots();

                // Event werfen
                OnLevelChanged?.Invoke(this, null);

                return Task.CompletedTask;
            }
            else
            {
                // Prüfen, ob eine Datei angehängt wurde
                if (level.AssemblyFile == null)
                {
                    throw new Exception("There is no Assembly File");
                }

                // Prüfen, ob ein Typ angegeben wurde
                if (string.IsNullOrEmpty(level.TypeName))
                {
                    throw new Exception("There is no Level Type");
                }

                // Level analysieren
                LevelInfo info = ExtensionLoader.SecureFindLevel(extensionPaths, level.AssemblyFile, level.TypeName);
                if (info != null)
                {
                    info.Type.AssemblyFile = level.AssemblyFile;
                    this.level = info;
                    ResetSlots();
                }

                // Event werfen
                OnLevelChanged?.Invoke(this, this.level);

                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Legt die zu verwendende KI für Slot 0 fest.
        /// </summary>
        /// <param name="player">KI</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task UploadPlayer(TypeInfo player)
        {
            return UploadMaster(0, player);
        }

        /// <summary>
        /// Legt die zu verwendende KI für einen beliebigen Slot fest.
        /// </summary>
        /// <param name="slot">Slot (0...7)</param>
        /// <param name="player">KI</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task UploadMaster(byte slot, TypeInfo player)
        {
            // Sicher stellen, dass der Modus stimmt.
            if (ServerState != SimulationState.Stopped)
            {
                throw new Exception("Simulation already started");
            }

            // Prüfen, ob überhaupt schon ein Level feststeht.
            if (this.level == null)
            {
                throw new Exception("There is no Level set");
            }

            // Slot Grenzbereiche prüfen
            if (slot < 0 || slot >= this.level.LevelDescription.MaxPlayerCount)
            {
                throw new Exception("Slot Value is out of List");
            }

            if (player == null)
            {
                // Player entfernen
                players[slot] = null;

                // Event werfen
                OnPlayerChanged?.Invoke(this, slot);

                return Task.CompletedTask;
            }
            else
            {
                // Prüfen, ob eine Datei angehängt wurde
                if (player.AssemblyFile == null)
                {
                    throw new Exception("There is no Assembly File");
                }

                // Prüfen, ob ein Typ angegeben wurde
                if (string.IsNullOrEmpty(player.TypeName))
                {
                    throw new Exception("There is no Player Type");
                }

                // Level analysieren
                PlayerInfo info = ExtensionLoader.SecureFindPlayer(extensionPaths, player.AssemblyFile, player.TypeName);
                if (info != null)
                {
                    info.Type.AssemblyFile = player.AssemblyFile;
                    players[slot] = info;
                }

                // Event werfen
                OnPlayerChanged?.Invoke(this, slot);

                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Legt die Slot-Parameter wie Farbe und Readystate für den angegebenen Slot fest.
        /// </summary>
        /// <param name="slot">Slot-Nummer</param>
        /// <param name="color">neue Farbe</param>
        /// <param name="ready">gibt an, ob der Slot vollständig ist</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task SetPlayerState(byte slot, PlayerColor color, byte team, bool ready)
        {
            return SetMasterState(slot, color, team, ready);
        }

        public Task UnsetPlayerState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Legt die Slot-Parameter wie Farbe und Readystate für den angegebenen Slot fest.
        /// </summary>
        /// <param name="slot">Slot-Nummer</param>
        /// <param name="color">neue Farbe</param>
        /// <param name="ready">gibt an, ob der Slot vollständig ist</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task SetMasterState(byte slot, PlayerColor color, byte team, bool ready)
        {
            // Sicher stellen, dass der Modus stimmt.
            if (ServerState != SimulationState.Stopped)
            {
                throw new Exception("Simulation already started");
            }

            // Prüfen, ob überhaupt schon ein Level feststeht.
            if (this.level == null)
            {
                throw new Exception("There is no Level set");
            }

            // Slot Grenzbereiche prüfen
            if (slot < 0 || slot >= this.level.LevelDescription.MaxPlayerCount)
            {
                throw new Exception("Slot Value is out of List");
            }

            // TODO: freie farben checken
            PlayerColor oldColor = slots[slot].ColorKey;
            if (oldColor != color)
            {
                for (byte i = 0; i < AntMe.Level.MAX_SLOTS; i++)
                {
                    // Nachschauen, welcher Spieler die neue Farbe vorher hatte 
                    // und mit aktueller Farbe wechseln
                    if (slots[i].ColorKey == color)
                    {
                        slots[i].ColorKey = oldColor;

                        OnPlayerChanged?.Invoke(this, i);

                        break;
                    }
                }

            }

            // Parameter setzen
            slots[slot].ColorKey = color;
            slots[slot].Team = team;
            slots[slot].ReadyState = ready;

            OnPlayerChanged?.Invoke(this, slot);

            return Task.CompletedTask;
        }

        private void ResetSlots()
        {
            for (int i = 0; i < AntMe.Level.MAX_SLOTS; i++)
            {
                slots[i].ColorKey = (PlayerColor)i;
                slots[i].PlayerInfo = false;
                slots[i].Profile = null;
                slots[i].Team = (byte)(i + 1);
                slots[i].ReadyState = false;

                players[i] = null;
            }

            // Event werfen
            OnPlayerReset?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Informiert über ein neues Level.
        /// </summary>
        public event EventHandler<LevelInfo> OnLevelChanged;

        /// <summary>
        /// Informiert über einen kompletten Reset der Player Slots.
        /// </summary>
        public event EventHandler OnPlayerReset;

        /// <summary>
        /// Informiert über eine Änderung des angegebenen Slots.
        /// </summary>
        public event EventHandler<byte> OnPlayerChanged;

        #endregion

        #region Flow related

        /// <summary>
        /// Gibt den aktuellen Status des Simulators zurück.
        /// </summary>
        public SimulationState ServerState { get { return state; } }

        /// <summary>
        /// Gibt den letzten Stand zurück oder null, falls noch nichts existiert.
        /// </summary>
        public LevelState CurrentState { get { return currentState; } }

        /// <summary>
        /// Startet die Simulation, sofern alle Daten vollständig sind.
        /// </summary>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public Task StartSimulation()
        {
            // Im Falle des Pause-Modes wird einfach fortgesetzt
            if (state == SimulationState.Paused)
            {
                ResumeSimulation();
                return Task.CompletedTask;
            }

            // Simulation läuft bereits
            if (state == SimulationState.Running)
            {
                return Task.CompletedTask;
            }

            // Prüfen, ob ein Level vorhanden ist
            if (level == null)
            {
                throw new Exception("There is not Level set");
            }

            // Player zählen
            int count = 0;
            for (int i = 0; i < AntMe.Level.MAX_SLOTS; i++)
            {
                if (players[i] != null)
                {
                    // Prüfen, ob player bereit ist
                    if (!slots[i].ReadyState)
                    {
                        throw new Exception("Player " + (i + 1) + " is not ready");
                    }

                    // Prüfen, ob Faction Filter aktiv ist
                    var filter = level.FactionFilter.Where(f => f.SlotIndex == i);
                    if (filter.Count() > 0)
                    {
                        if (filter.Where(f => f.Type.TypeName == players[i].FactionType).Count() == 0)
                        {
                            throw new Exception("Player " + i + " does not match the Faction filter");
                        }
                    }

                    count++;
                }
            }

            // prüfen, ob genügend Player vorhanden sind
            if (count < level.LevelDescription.MinPlayerCount)
            {
                throw new Exception("Not enougth player");
            }

            if (count > level.LevelDescription.MaxPlayerCount)
            {
                throw new Exception("Too many player");
            }

            // State ändern
            state = SimulationState.Running;
            OnSimulationChanged?.Invoke(this, state, rate);

            // Thread erzeugen
            thread = new Thread(SimulationLoop);
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Pausiert eine laufende Anwendung oder tut nichts, falls gestoppt.
        /// </summary>
        public Task PauseSimulation()
        {
            // Nicht reagieren, falls die Simulation nicht läuft.
            if (state == SimulationState.Running)
            {
                state = SimulationState.Paused;
                OnSimulationChanged?.Invoke(this, state, rate);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Setzt eine pausierte Simulation wieder fort.
        /// </summary>
        public Task ResumeSimulation()
        {
            // Nur reagieren, falls der Pause-Mode aktiv ist
            if (state == SimulationState.Paused)
            {
                // state melden und setzen
                state = SimulationState.Running;
                OnSimulationChanged?.Invoke(this, state, rate);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stoppt eine laufende Anwendung.
        /// </summary>
        public Task StopSimulation()
        {
            // Nicht reagieren, falls ohnehin eine Simulation läuft.
            if (state != SimulationState.Stopped)
            {
                // State ändern und kurz auf den Thread warten.
                state = SimulationState.Stopped;
                if (thread.Join(2000))
                    thread.Abort();

                OnSimulationChanged?.Invoke(this, state, rate);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Left eine neue Framerate für die Simulation fest.
        /// </summary>
        /// <param name="frames">Neue Rate in Frames pro Sekunde</param>
        public Task PitchSimulation(byte frames)
        {
            rate = Math.Max((byte)1, frames);

            OnSimulationChanged?.Invoke(this, state, rate);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Informiert über die Änderung des Simulation States und der Framerate.
        /// </summary>
        public event Action<ISimulationClient, SimulationState, byte> OnSimulationChanged;

        /// <summary>
        /// Informiert über einen neuen Simulation State.
        /// </summary>
        public event EventHandler<LevelState> OnSimulationState;

        #endregion

        #region Simulation

        private Thread thread;
        private SimulationState state = SimulationState.Stopped;
        private LevelState currentState;

        /// <summary>
        /// Initialisierung der Simulation.
        /// </summary>
        /// <returns>Erfolgsmeldung</returns>
        protected abstract void InitSimulation(int seed, LevelInfo level, PlayerInfo[] players, Slot[] slots);

        /// <summary>
        /// Ermittlung des nächsten Simulations-States.
        /// </summary>
        /// <returns>Neuer Simulation State</returns>
        protected abstract LevelState UpdateSimulation();

        /// <summary>
        /// Abschließende Arbeiten bei Beenden der Simulation.
        /// </summary>
        protected abstract void FinalizeSimulation();

        private void SimulationLoop()
        {
            try
            {
                InitSimulation(0, level, players, slots);
            }
            catch (Exception ex)
            {
                // Event werfen
                OnError?.Invoke(this, ex);

                // Simulation stoppen
                StopSimulation();
            }

            while (state != SimulationState.Stopped)
            {
                // Pausemodus skippen
                if (state == SimulationState.Paused || rate <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                // Messung starten
                watch.Restart();

                // State erzeugen
                currentState = UpdateSimulation();

                // Das EventLog füttern
                log.Update(currentState);

                if (currentState.Mode > LevelMode.Running)
                {
                    state = SimulationState.Stopped;
                    continue;
                }

                OnSimulationState?.Invoke(this, currentState);

                // Wartezeit zwischen Frames
                while (state != SimulationState.Stopped &&
                    watch.ElapsedMilliseconds < (int)(1000 / rate))
                {
                    Thread.Sleep(1);
                }
            }

            FinalizeSimulation();
        }

        #endregion

        public void Dispose()
        {
            if (state != SimulationState.Stopped)
                StopSimulation();
        }
    }
}
