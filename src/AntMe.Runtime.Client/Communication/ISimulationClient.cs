using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AntMe.Runtime.Communication;
using AntMe.Runtime.EventLog;

namespace AntMe.Runtime.Client.Communication
{
    /// <summary>
    /// Interface für Verbindungen zu einem Simulation Server.
    /// </summary>
    public interface ISimulationClient : ISimulationService, IDisposable
    {
        /// <summary>
        /// Öffnet den Endpoint
        /// </summary>
        Task Open();

        /// <summary>
        /// Öffnet die Verbindung zum Server.
        /// </summary>
        /// <param name="username">Initialer Username</param>
        Task Open(string username);

        /// <summary>
        /// Schließt die Verbindung aufgrund einer Exception.
        /// </summary>
        /// <param name="ex">Error</param>
        Task CloseByError(Exception ex);

        /// <summary>
        /// Schließt die Verbindung zum Server, falls offen.
        /// </summary>
        Task Close();

        /// <summary>
        /// ID der Verbindung - gesetzt vom Server.
        /// </summary>
        int ClientId { get; }

        /// <summary>
        /// Gibt an, ob die Verbindung offen ist.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// GIbt an, ob Header und ID übertragen wurde und die Verbindung 
        /// bereit zum Empfang ist.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Gibt die Versionsnummer des Übertragungsprotokolls an.
        /// </summary>
        byte Protocol { get; }

        /// <summary>
        /// Gibt an, ob der aktuelle Client der Spiel Master ist.
        /// </summary>
        bool IsMaster { get; }

        /// <summary>
        /// Gibt das aktuell geladene Level an.
        /// </summary>
        LevelInfo Level { get; }

        /// <summary>
        /// EventLog für diese Simulation.
        /// </summary>
        Log EventLog { get; }

        /// <summary>
        /// Eine Liste der aktuell verbundenen User.
        /// </summary>
        ReadOnlyCollection<UserProfile> Users { get; }

        /// <summary>
        /// Eine Liste der Level-Slots.
        /// </summary>
        ReadOnlyCollection<Slot> Slots { get; }

        /// <summary>
        /// GIbt das User-Profil des Masters zurück oder null, falls es noch keinen gibt.
        /// </summary>
        UserProfile Master { get; }

        /// <summary>
        /// Gibt das eigene User-Profil zurück.
        /// </summary>
        UserProfile Me { get; }

        /// <summary>
        /// Status des verbundenen Servers.
        /// </summary>
        SimulationState ServerState { get; }

        /// <summary>
        /// Snapshot des aktuellen Simulationsstands oder null, falls die Simulation nicht läuft.
        /// </summary>
        LevelState CurrentState { get; }

        /// <summary>
        /// Gibt die Framerate (Frames pro Sekunde) an.
        /// </summary>
        byte Rate { get; }

        event EventHandler<UserProfile> OnMasterChanged;

        event EventHandler OnUserlistChanged;

        event EventHandler<UserProfile> OnUserAdded;

        event EventHandler<int> OnUserDropped;

        event EventHandler<UserProfile> OnUsernameChanged;

        event Action<ISimulationClient, UserProfile, string> OnMessageReceived;

        event EventHandler<LevelInfo> OnLevelChanged;

        event EventHandler OnPlayerReset;

        event EventHandler<byte> OnPlayerChanged;

        event Action<ISimulationClient, SimulationState, byte> OnSimulationChanged;

        event EventHandler<LevelState> OnSimulationState;

        event EventHandler<Exception> OnError;

        event EventHandler OnClose;

    }
}
