using System;
using System.Collections.ObjectModel;
using AntMe.Runtime.EventLog;

namespace AntMe.Runtime.Communication
{
    /// <summary>
    ///     Interface für Verbindungen zu einem Simulation Server.
    /// </summary>
    public interface ISimulationClient : IDisposable
    {
        /// <summary>
        ///     ID der Verbindung - gesetzt vom Server.
        /// </summary>
        int ClientId { get; }

        /// <summary>
        ///     Gibt an, ob die Verbindung offen ist.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        ///     GIbt an, ob Header und ID übertragen wurde und die Verbindung
        ///     bereit zum Empfang ist.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        ///     Gibt die Versionsnummer des Übertragungsprotokolls an.
        /// </summary>
        byte Protocol { get; }

        /// <summary>
        ///     Gibt an, ob der aktuelle Client der Spiel Master ist.
        /// </summary>
        bool IsMaster { get; }

        /// <summary>
        ///     Gibt das aktuell geladene Level an.
        /// </summary>
        LevelInfo Level { get; }

        /// <summary>
        ///     EventLog für diese Simulation.
        /// </summary>
        Log EventLog { get; }

        /// <summary>
        ///     Eine Liste der aktuell verbundenen User.
        /// </summary>
        ReadOnlyCollection<UserProfile> Users { get; }

        /// <summary>
        ///     Eine Liste der Level-Slots.
        /// </summary>
        ReadOnlyCollection<Slot> Slots { get; }

        /// <summary>
        ///     GIbt das User-Profil des Masters zurück oder null, falls es noch keinen gibt.
        /// </summary>
        UserProfile Master { get; }

        /// <summary>
        ///     Gibt das eigene User-Profil zurück.
        /// </summary>
        UserProfile Me { get; }

        /// <summary>
        ///     Status des verbundenen Servers.
        /// </summary>
        SimulationState ServerState { get; }

        /// <summary>
        ///     Snapshot des aktuellen Simulationsstands oder null, falls die Simulation nicht läuft.
        /// </summary>
        LevelState CurrentState { get; }

        /// <summary>
        ///     Gibt die Framerate (Frames pro Sekunde) an.
        /// </summary>
        byte Rate { get; }

        /// <summary>
        ///     Öffnet den Endpoint
        /// </summary>
        void Open();

        /// <summary>
        ///     Schließt die Verbindung aufgrund einer Exception.
        /// </summary>
        /// <param name="ex">Error</param>
        void CloseByError(Exception ex);

        /// <summary>
        ///     Schließt die Verbindung zum Server, falls offen.
        /// </summary>
        void Close();

        /// <summary>
        ///     Benachrichtgung über das Schließen des Clients.
        /// </summary>
        event CloseClientDelegate OnClose;

        /// <summary>
        ///     Benachrichtigung über einen Fehler bei der Verbindung.
        /// </summary>
        event ErrorClientDelegate OnError;

        /// <summary>
        ///     Öffnet die Verbindung zum Server.
        /// </summary>
        /// <param name="username">Initialer Username</param>
        void Open(string username);

        /// <summary>
        ///     Wird geworfen, wenn der Master des Servers sich ändert.
        /// </summary>
        event SimulationClientDelegate<UserProfile> OnMasterChanged;

        /// <summary>
        ///     Wird geworfen, sobald sich die komplette Userlist des Servers ändert.
        ///     Wird gewöhnlich nur beim ersten Kontakt geliefert. Danach werden
        ///     User nur noch per Add und Remove signalisiert.
        /// </summary>
        event SimulationClientDelegate OnUserlistChanged;

        /// <summary>
        ///     Wird geworfen, wenn sich ein neuer User verbindet.
        /// </summary>
        event SimulationClientDelegate<UserProfile> OnUserAdded;

        /// <summary>
        ///     Wird geforfen, wenn ein verbundener User die Verbindung getrennt hat.
        /// </summary>
        event SimulationClientDelegate<int> OnUserDropped;

        /// <summary>
        ///     Wird geworfen, wenn ein verbundener User seinen Anzeigenamen
        ///     verändert hat.
        /// </summary>
        event SimulationClientDelegate<UserProfile> OnUsernameChanged;

        /// <summary>
        ///     Wird bei einer neuen Chat-Nachricht eines anderen Users geworfen.
        /// </summary>
        event SimulationClientDelegate<UserProfile, string> OnMessageReceived;

        /// <summary>
        ///     Wird geworfen, wenn der Master ein neues Level hochgeladen hat.
        /// </summary>
        event SimulationClientDelegate<LevelInfo> OnLevelChanged;

        /// <summary>
        ///     Wird geworfen, wenn sich alle Level-Slots geändert haben. Das
        ///     passiert nur beim ersten Kontakt des Clients oder beim Upload eines
        ///     neuen Levels.
        /// </summary>
        event SimulationClientDelegate OnPlayerReset;

        /// <summary>
        ///     Signal, falls sich der Zustand eines einzelnen Level-Slots verändert.
        /// </summary>
        event SimulationClientDelegate<byte> OnPlayerChanged;

        /// <summary>
        ///     Signalisiert die Änderung des Simulationszustands und der Rate.
        /// </summary>
        event SimulationClientDelegate<SimulationState, byte> OnSimulationChanged;

        /// <summary>
        ///     Signalisiert die Ankunft eines neuen Simulationsframes.
        /// </summary>
        event SimulationClientDelegate<LevelState> OnSimulationState;

        #region Methoden

        #region Master Handling

        /// <summary>
        ///     Versucht den aktuellen Client in die Position des Masters zu versetzen.
        /// </summary>
        /// <returns></returns>
        bool AquireMaster();

        /// <summary>
        ///     Gibt den Master wieder frei
        /// </summary>
        /// <returns></returns>
        bool FreeMaster();

        #endregion

        #region User Handling

        /// <summary>
        ///     Ändert den Usernamen dieses Clients.
        /// </summary>
        /// <param name="name">Neuer Name</param>
        /// <returns>Erfolgsmeldung</returns>
        bool ChangeUsername(string name);

        #endregion

        #region Chat Handling

        /// <summary>
        ///     Sendet eine Chat-Nachricht an die anderen Clients des aktuell verbundenen Server.
        /// </summary>
        /// <param name="message">Nachricht</param>
        void SendMessage(string message);

        #endregion

        #region Settings Handling

        /// <summary>
        ///     Setzt das Level, das gespielt werden soll.
        /// </summary>
        /// <param name="level">Level</param>
        bool UploadLevel(TypeInfo level);

        /// <summary>
        ///     Lädt die KI des aktuellen Clients hoch.
        /// </summary>
        /// <param name="player"></param>
        bool UploadPlayer(TypeInfo player);

        /// <summary>
        ///     Erlaubt dem Master einen beliebigen Spieler-Slot zu überschreiben, falls dieser nicht schon von einem Spieler
        ///     belegt ist.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="player"></param>
        bool UploadMaster(byte slot, TypeInfo player);

        bool SetPlayerState(byte slot, PlayerColor color, byte team, bool ready);

        /// <summary>
        ///     Removes the Player from the given slot.
        /// </summary>
        /// <returns></returns>
        bool UnsetPlayerState();

        bool SetMasterState(byte slot, PlayerColor color, byte team, bool ready);

        #endregion

        #region Flow Handling

        /// <summary>
        ///     Startet die Simulation, sofern dieser Client der Master ist und alle Spieler bereit sind.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        bool StartSimulation();

        /// <summary>
        ///     Hält die laufende Simulation an, sofern sie läuft.
        /// </summary>
        void PauseSimulation();

        /// <summary>
        ///     Setzt eine angehaltene Simulation fort.
        /// </summary>
        void ResumeSimulation();

        /// <summary>
        ///     Hält eine laufende Simulation an.
        /// </summary>
        void StopSimulation();

        /// <summary>
        ///     Reguliert die Framerate der aktuellen Simulation.
        /// </summary>
        /// <param name="frames"></param>
        void PitchSimulation(byte frames);

        #endregion

        #endregion
    }

    public delegate void SimulationClientDelegate(ISimulationClient client);

    public delegate void SimulationClientDelegate<T>(ISimulationClient client, T parameter);

    public delegate void SimulationClientDelegate<T, U>(ISimulationClient client, T parameter1, U parameter2);

    public delegate void ErrorClientDelegate(ISimulationClient client, string message);

    public delegate void CloseClientDelegate(ISimulationClient client);

    public delegate void ClientDelegate<T>(ISimulationClient client, T parameter);
}