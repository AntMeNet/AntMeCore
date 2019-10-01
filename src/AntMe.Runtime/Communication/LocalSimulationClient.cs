using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AntMe.Runtime.EventLog;

namespace AntMe.Runtime.Communication
{
    /// <summary>
    ///     Basisklasse für alle lokal stattfindenen Simulationen.
    /// </summary>
    internal abstract class LocalSimulationClient : ISimulationClient
    {
        protected string[] extensionPaths;
        private readonly PlayerInfo[] players;
        protected ITypeResolver resolver;
        private readonly Slot[] slots;
        private readonly List<UserProfile> users;
        private readonly Stopwatch watch = new Stopwatch();

        public LocalSimulationClient(string[] extensionPaths, ITypeResolver resolver)
        {
            this.resolver = resolver;
            this.extensionPaths = extensionPaths;

            // User erstellen
            Master = new UserProfile {Id = ClientId, Username = "local"};
            users = new List<UserProfile>();
            users.Add(Master);

            // Slots erstellen
            slots = new Slot[AntMe.Level.MAX_SLOTS];
            players = new PlayerInfo[AntMe.Level.MAX_SLOTS];
            for (byte i = 0; i < AntMe.Level.MAX_SLOTS; i++)
                slots[i] = new Slot {Id = i, ColorKey = (PlayerColor) i};

            // Defaults
            Rate = AntMe.Level.FRAMES_PER_SECOND;
            EventLog = Log.CreateLog(true);
        }

        public void Dispose()
        {
            if (ServerState != SimulationState.Stopped)
                StopSimulation();
        }

        #region Common Properties

        /// <summary>
        ///     Gibt immer true zurück, da es sich hier um eine lokale Simulation handelt.
        /// </summary>
        public bool IsMaster => true;

        /// <summary>
        ///     Liefert eine Referenz auf das Userprofil des Masters.
        /// </summary>
        public UserProfile Master { get; }

        /// <summary>
        ///     Gibt den Master zurück, da es sich um eine lokale Simulation handelt.
        /// </summary>
        public UserProfile Me => Master;

        /// <summary>
        ///     Gibt das aktuell eingestellte Level oder null zurück.
        /// </summary>
        public LevelInfo Level { get; private set; }

        /// <summary>
        ///     Liefert eine Liste der aktuell teilnehmenden User. Diese enthält nur den Master.
        /// </summary>
        public ReadOnlyCollection<UserProfile> Users => users.AsReadOnly();

        /// <summary>
        ///     Gibt immer 1 zurück, da es sich hier um eine lokale Simulation handelt.
        /// </summary>
        public int ClientId { get; } = 1;

        /// <summary>
        ///     Da es sich um eine lokale Simulation handelt, kommt hier immer true zurück.
        /// </summary>
        public bool IsOpen => true;

        /// <summary>
        ///     Da es sich um eine lokale Simulation handelt, kommt hier immer true zurück.
        /// </summary>
        public bool IsReady => true;

        /// <summary>
        ///     Da es sich um eine lokale Simulation handelt, kommt hier immer 1 zurück.
        /// </summary>
        public byte Protocol => 1;

        /// <summary>
        ///     Gibt die Frame-Rate in Frames pro Sekunde zurück.
        /// </summary>
        public byte Rate { get; private set; }

        /// <summary>
        ///     Gibt das EventLog dieser Simulation zurück.
        /// </summary>
        public Log EventLog { get; }

        /// <summary>
        ///     Liefert die liste der aktuell verfügbaren Slots zurück.
        /// </summary>
        public ReadOnlyCollection<Slot> Slots
        {
            get
            {
                if (Level != null)
                    slots.Where(i => i.Id < Level.LevelDescription.MaxPlayerCount)
                        .ToList().AsReadOnly();
                return new List<Slot>().AsReadOnly();
            }
        }

        #endregion

        #region Connection related

        /// <summary>
        ///     Hat in diesem Kontext keine Funktion, da es sich um eine
        ///     lokale Simulation handelt.
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        ///     Hat in diesem Kontext keine Funktion, da es sich um eine
        ///     lokale Simulation handelt.
        /// </summary>
        /// <param name="username">Benutzername</param>
        public void Open(string username)
        {
            Open();

            ChangeUsername(username);
        }

        /// <summary>
        ///     Verursacht einen Aufruf des OnError-Events, hat aber sonst keine
        ///     Auswirkungen, da es sich um eine lokale Simulation handelt.
        /// </summary>
        /// <param name="ex"></param>
        public void CloseByError(Exception ex)
        {
            OnError?.Invoke(this, ex.Message);
        }

        /// <summary>
        ///     Hat in diesem Kontext keine Funktion, da es sich um eine
        ///     lokale Simulation handelt.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        ///     Wird in diesem Kontext nicht geworfen.
        /// </summary>
        public event CloseClientDelegate OnClose;

        /// <summary>
        ///     Informiert über einen Fehler in der Simulation.
        /// </summary>
        public event ErrorClientDelegate OnError;

        #endregion

        #region User related

        /// <summary>
        ///     Hat hier keine Funktion, da es sich um eine lokale Simulation handelt und der Master bereits feststeht.
        /// </summary>
        /// <param name="message">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool AquireMaster()
        {
            return true;
        }

        /// <summary>
        ///     Hat hier keine Funktion, da es sich um eine lokale Simulation handelt und der Master bereits feststeht.
        /// </summary>
        /// <param name="message">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool FreeMaster()
        {
            return true;
        }

        /// <summary>
        ///     Hat hier keine Funktion, da es sich um eine lokale Simulation handelt und Mastername feststeht.
        /// </summary>
        /// <param name="message">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool ChangeUsername(string name)
        {
            return true;
        }

        /// <summary>
        ///     Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event SimulationClientDelegate<UserProfile> OnMasterChanged;

        /// <summary>
        ///     Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event SimulationClientDelegate OnUserlistChanged;

        /// <summary>
        ///     Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event SimulationClientDelegate<UserProfile> OnUserAdded;

        /// <summary>
        ///     Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event SimulationClientDelegate<int> OnUserDropped;

        /// <summary>
        ///     Wird in diesem Kontext nicht genutzt.
        /// </summary>
        public event SimulationClientDelegate<UserProfile> OnUsernameChanged;

        #endregion

        #region Chat related

        /// <summary>
        ///     Sendet eine Nachricht an sich selbst, da es sich hier um eine lokale Simulation handelt.
        /// </summary>
        /// <param name="message">Nachricht</param>
        public void SendMessage(string message)
        {
            OnMessageReceived?.Invoke(this, Master, message);
        }

        public event SimulationClientDelegate<UserProfile, string> OnMessageReceived;

        #endregion

        #region Settings related

        /// <summary>
        ///     Legt das Level für diese Simulation fest. Darf nur im gestoppten Modus genutzt werden.
        /// </summary>
        /// <param name="level">Level Infos mit AssemblyFile und TypeName</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool UploadLevel(TypeInfo level)
        {
            // Sicher stellen, dass der Modus stimmt.
            if (ServerState != SimulationState.Stopped) throw new Exception("Simulation already started");

            if (level == null)
            {
                // Level leeren
                Level = null;
                ResetSlots();

                // Event werfen
                OnLevelChanged?.Invoke(this, null);

                return true;
            }

            // Prüfen, ob eine Datei angehängt wurde
            if (level.AssemblyFile == null) throw new Exception("There is no Assembly File");

            // Prüfen, ob ein Typ angegeben wurde
            if (string.IsNullOrEmpty(level.TypeName)) throw new Exception("There is no Level Type");

            // Level analysieren
            var info = ExtensionLoader.SecureFindLevel(extensionPaths, level.AssemblyFile, level.TypeName);
            if (info != null)
            {
                info.Type.AssemblyFile = level.AssemblyFile;
                Level = info;
                ResetSlots();
            }

            // Event werfen
            OnLevelChanged?.Invoke(this, Level);

            return true;
        }

        /// <summary>
        ///     Legt die zu verwendende KI für Slot 0 fest.
        /// </summary>
        /// <param name="player">KI</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool UploadPlayer(TypeInfo player)
        {
            return UploadMaster(0, player);
        }

        /// <summary>
        ///     Legt die zu verwendende KI für einen beliebigen Slot fest.
        /// </summary>
        /// <param name="slot">Slot (0...7)</param>
        /// <param name="player">KI</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool UploadMaster(byte slot, TypeInfo player)
        {
            // Sicher stellen, dass der Modus stimmt.
            if (ServerState != SimulationState.Stopped) throw new Exception("Simulation already started");

            // Prüfen, ob überhaupt schon ein Level feststeht.
            if (Level == null) throw new Exception("There is no Level set");

            // Slot Grenzbereiche prüfen
            if (slot < 0 || slot >= Level.LevelDescription.MaxPlayerCount)
                throw new Exception("Slot Value is out of List");

            if (player == null)
            {
                // Player entfernen
                players[slot] = null;

                // Event werfen
                OnPlayerChanged?.Invoke(this, slot);

                return true;
            }

            // Prüfen, ob eine Datei angehängt wurde
            if (player.AssemblyFile == null) throw new Exception("There is no Assembly File");

            // Prüfen, ob ein Typ angegeben wurde
            if (string.IsNullOrEmpty(player.TypeName)) throw new Exception("There is no Player Type");

            // Level analysieren
            var info = ExtensionLoader.SecureFindPlayer(extensionPaths, player.AssemblyFile, player.TypeName);
            if (info != null)
            {
                info.Type.AssemblyFile = player.AssemblyFile;
                players[slot] = info;
            }

            // Event werfen
            OnPlayerChanged?.Invoke(this, slot);

            return true;
        }

        /// <summary>
        ///     Legt die Slot-Parameter wie Farbe und Readystate für den angegebenen Slot fest.
        /// </summary>
        /// <param name="slot">Slot-Nummer</param>
        /// <param name="color">neue Farbe</param>
        /// <param name="ready">gibt an, ob der Slot vollständig ist</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool SetPlayerState(byte slot, PlayerColor color, byte team, bool ready)
        {
            return SetMasterState(slot, color, team, ready);
        }

        public bool UnsetPlayerState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Legt die Slot-Parameter wie Farbe und Readystate für den angegebenen Slot fest.
        /// </summary>
        /// <param name="slot">Slot-Nummer</param>
        /// <param name="color">neue Farbe</param>
        /// <param name="ready">gibt an, ob der Slot vollständig ist</param>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool SetMasterState(byte slot, PlayerColor color, byte team, bool ready)
        {
            // Sicher stellen, dass der Modus stimmt.
            if (ServerState != SimulationState.Stopped) throw new Exception("Simulation already started");

            // Prüfen, ob überhaupt schon ein Level feststeht.
            if (Level == null) throw new Exception("There is no Level set");

            // Slot Grenzbereiche prüfen
            if (slot < 0 || slot >= Level.LevelDescription.MaxPlayerCount)
                throw new Exception("Slot Value is out of List");

            // TODO: freie farben checken
            var oldColor = slots[slot].ColorKey;
            if (oldColor != color)
                for (byte i = 0; i < AntMe.Level.MAX_SLOTS; i++)
                    // Nachschauen, welcher Spieler die neue Farbe vorher hatte 
                    // und mit aktueller Farbe wechseln
                    if (slots[i].ColorKey == color)
                    {
                        slots[i].ColorKey = oldColor;

                        OnPlayerChanged?.Invoke(this, i);

                        break;
                    }

            // Parameter setzen
            slots[slot].ColorKey = color;
            slots[slot].Team = team;
            slots[slot].ReadyState = ready;

            OnPlayerChanged?.Invoke(this, slot);

            return true;
        }

        private void ResetSlots()
        {
            for (var i = 0; i < AntMe.Level.MAX_SLOTS; i++)
            {
                slots[i].ColorKey = (PlayerColor) i;
                slots[i].PlayerInfo = false;
                slots[i].Profile = null;
                slots[i].Team = (byte) (i + 1);
                slots[i].ReadyState = false;

                players[i] = null;
            }

            // Event werfen
            OnPlayerReset?.Invoke(this);
        }

        /// <summary>
        ///     Informiert über ein neues Level.
        /// </summary>
        public event SimulationClientDelegate<LevelInfo> OnLevelChanged;

        /// <summary>
        ///     Informiert über einen kompletten Reset der Player Slots.
        /// </summary>
        public event SimulationClientDelegate OnPlayerReset;

        /// <summary>
        ///     Informiert über eine Änderung des angegebenen Slots.
        /// </summary>
        public event SimulationClientDelegate<byte> OnPlayerChanged;

        #endregion

        #region Flow related

        /// <summary>
        ///     Gibt den aktuellen Status des Simulators zurück.
        /// </summary>
        public SimulationState ServerState { get; private set; } = SimulationState.Stopped;

        /// <summary>
        ///     Gibt den letzten Stand zurück oder null, falls noch nichts existiert.
        /// </summary>
        public LevelState CurrentState { get; private set; }

        /// <summary>
        ///     Startet die Simulation, sofern alle Daten vollständig sind.
        /// </summary>
        /// <param name="result">Eventuelle Fehlermeldung</param>
        /// <returns>Erfolgsmeldung</returns>
        public bool StartSimulation()
        {
            // Im Falle des Pause-Modes wird einfach fortgesetzt
            if (ServerState == SimulationState.Paused)
            {
                ResumeSimulation();
                return true;
            }

            // Simulation läuft bereits
            if (ServerState == SimulationState.Running) return true;

            // Prüfen, ob ein Level vorhanden ist
            if (Level == null) throw new Exception("There is not Level set");

            // Player zählen
            var count = 0;
            for (var i = 0; i < AntMe.Level.MAX_SLOTS; i++)
                if (players[i] != null)
                {
                    // Prüfen, ob player bereit ist
                    if (!slots[i].ReadyState) throw new Exception("Player " + (i + 1) + " is not ready");

                    // Prüfen, ob Faction Filter aktiv ist
                    var filter = Level.FactionFilter.Where(f => f.SlotIndex == i);
                    if (filter.Count() > 0)
                        if (filter.Where(f => f.Type.TypeName == players[i].FactionType).Count() == 0)
                            throw new Exception("Player " + i + " does not match the Faction filter");

                    count++;
                }

            // prüfen, ob genügend Player vorhanden sind
            if (count < Level.LevelDescription.MinPlayerCount) throw new Exception("Not enougth player");

            if (count > Level.LevelDescription.MaxPlayerCount) throw new Exception("Too many player");

            // State ändern
            ServerState = SimulationState.Running;
            OnSimulationChanged?.Invoke(this, ServerState, Rate);

            // Thread erzeugen
            thread = new Thread(SimulationLoop);
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();

            return true;
        }

        /// <summary>
        ///     Pausiert eine laufende Anwendung oder tut nichts, falls gestoppt.
        /// </summary>
        public void PauseSimulation()
        {
            // Nicht reagieren, falls die Simulation nicht läuft.
            if (ServerState == SimulationState.Running)
            {
                ServerState = SimulationState.Paused;
                OnSimulationChanged?.Invoke(this, ServerState, Rate);
            }
        }

        /// <summary>
        ///     Setzt eine pausierte Simulation wieder fort.
        /// </summary>
        public void ResumeSimulation()
        {
            // Nur reagieren, falls der Pause-Mode aktiv ist
            if (ServerState != SimulationState.Paused)
                return;

            // state melden und setzen
            ServerState = SimulationState.Running;
            OnSimulationChanged?.Invoke(this, ServerState, Rate);
        }

        /// <summary>
        ///     Stoppt eine laufende Anwendung.
        /// </summary>
        public void StopSimulation()
        {
            // Nicht reagieren, falls ohnehin eine Simulation läuft.
            if (ServerState == SimulationState.Stopped)
                return;

            // State ändern und kurz auf den Thread warten.
            ServerState = SimulationState.Stopped;
            if (thread.Join(2000))
                thread.Abort();

            OnSimulationChanged?.Invoke(this, ServerState, Rate);
        }

        /// <summary>
        ///     Left eine neue Framerate für die Simulation fest.
        /// </summary>
        /// <param name="frames">Neue Rate in Frames pro Sekunde</param>
        public void PitchSimulation(byte frames)
        {
            Rate = Math.Max((byte) 1, frames);

            OnSimulationChanged?.Invoke(this, ServerState, Rate);
        }

        /// <summary>
        ///     Informiert über die Änderung des Simulation States und der Framerate.
        /// </summary>
        public event SimulationClientDelegate<SimulationState, byte> OnSimulationChanged;

        /// <summary>
        ///     Informiert über einen neuen Simulation State.
        /// </summary>
        public event SimulationClientDelegate<LevelState> OnSimulationState;

        #endregion

        #region Simulation

        private Thread thread;

        /// <summary>
        ///     Initialisierung der Simulation.
        /// </summary>
        /// <returns>Erfolgsmeldung</returns>
        protected abstract void InitSimulation(int seed, LevelInfo level, PlayerInfo[] players, Slot[] slots);

        /// <summary>
        ///     Ermittlung des nächsten Simulations-States.
        /// </summary>
        /// <returns>Neuer Simulation State</returns>
        protected abstract LevelState UpdateSimulation();

        /// <summary>
        ///     Abschließende Arbeiten bei Beenden der Simulation.
        /// </summary>
        protected abstract void FinalizeSimulation();

        private void SimulationLoop()
        {
            try
            {
                InitSimulation(0, Level, players, slots);
            }
            catch (Exception ex)
            {
                // Event werfen
                OnError?.Invoke(this, ex.Message);

                // Simulation stoppen
                StopSimulation();
            }

            while (ServerState != SimulationState.Stopped)
            {
                // Pausemodus skippen
                if (ServerState == SimulationState.Paused || Rate <= 0)
                {
                    Thread.Sleep(1);
                    continue;
                }

                // Messung starten
                watch.Restart();

                // State erzeugen
                CurrentState = UpdateSimulation();

                // Das EventLog füttern
                EventLog.Update(CurrentState);

                if (CurrentState.Mode > LevelMode.Running)
                {
                    ServerState = SimulationState.Stopped;
                    continue;
                }

                OnSimulationState?.Invoke(this, CurrentState);

                // Wartezeit zwischen Frames
                while (ServerState != SimulationState.Stopped &&
                       watch.ElapsedMilliseconds < 1000 / Rate)
                    Thread.Sleep(1);
            }

            FinalizeSimulation();
        }

        #endregion
    }
}