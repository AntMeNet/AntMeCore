using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Basisklasse für alle Factions.
    /// </summary>
    public abstract class Faction : PropertyList<FactionProperty>
    {
        private Type factoryType;

        /// <summary>
        /// Referenz auf den verwendeten Type Resolver.
        /// </summary>
        protected readonly ITypeResolver Resolver;

        private readonly Dictionary<Item, FactionInfo> factionInfos = new Dictionary<Item, FactionInfo>();
        private FactionState state;

        /// <summary>
        /// Standard-Konstruktor für Factions.
        /// </summary>
        /// <param name="resolver">Referenz auf den Resolver</param>
        /// <param name="factoryType"></param>
        /// <param name="name">Player Name</param>
        /// <param name="color">Player Farbe</param>
        protected Faction(ITypeResolver resolver, Type factoryType, string name, PlayerColor color)
        {
            // TODO: Factory Type check
            this.factoryType = factoryType;

            UnitInterops = new Dictionary<int, FactionUnitInteropGroup>();
            Resolver = resolver;
            PlayerColor = color;
            Name = name;

            resolver.ResolveFaction(this);
        }

        /// <summary>
        /// Gibt den Spieler Index innerhalb des Levels an.
        /// </summary>
        public byte PlayerIndex { get; set; }

        /// <summary>
        /// Gibt den Namen der Faction zurück.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gibt die Spielerfarbe zurück.
        /// </summary>
        public PlayerColor PlayerColor { get; set; }

        /// <summary>
        ///     Gibt eine Referenz aufs aktuelle Level zurück.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        ///     Gibt den Startpunkt der Fraktion zurück.
        /// </summary>
        public Vector2 Home { get; set; }

        /// <summary>
        /// Gibt den aktuellen Punktestand dieser Faction an.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Factory/Slot-Spezifische Kopie der Settings.
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        ///     Der Zufallszahlengenerator für diese Fraktion. Bitte immer
        ///     verwenden, um eine deterministrische Simulation zu
        ///     gewährleisten.
        /// </summary>
        public Random Random { get; set; }

        /// <summary>
        /// Instanz der Factory Klasse.
        /// </summary>
        public FactionFactory Factory { get; set; }

        /// <summary>
        /// Interop Instanz der Factory Klasse.
        /// </summary>
        public FactoryInterop FactoryInterop { get; set; }

        /// <summary>
        /// Liste der Unit/Interop/Item Bindung auf Basis der Item-Id.
        /// </summary>
        public Dictionary<int, FactionUnitInteropGroup> UnitInterops { get; private set; }

        /// <summary>
        ///     Methode wird vom Level zur Initialisierung der Fraktion aufgerufen.
        ///     Ideal zur Initialisierung von Listen und Caches.
        /// </summary>
        public void Init()
        {
            // Factory für Ameisen erzeugen
            FactoryInterop = Resolver.CreateFactoryInterop(this);
            Factory = (FactionFactory)Activator.CreateInstance(factoryType);
            Factory.Init(FactoryInterop);

            OnInit();
        }

        /// <summary>
        /// Wird beim Initialisieren der Faction aufgerufen und kann für Faction-Spezifische 
        /// Initialisierungen verwendet werden.
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        ///     Wird vom Level in jeder Simulationsrunde aufgerufen, damit die
        ///     Fraktion die eigene Logik anwenden kann.
        /// </summary>
        public void Update(int round)
        {
            // Faction Update
            OnUpdate(round);

            // Faction Interop Update
            FactoryInterop.InternalUpdate(round);

            // Unit Interop Updates
            foreach (var unit in UnitInterops.Values)
                unit.Interop.InternalUpdate(round);
        }

        /// <summary>
        /// Wird bei jedem Faction-Update aufgerufen und kann für Faction-spezifische 
        /// Aktivitäten pro Update genutzt werden.
        /// </summary>
        /// <param name="round">Aktueller Runden-Counter</param>
        protected abstract void OnUpdate(int round);

        /// <summary>
        /// Liefert die Faction Info zurück.
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public FactionInfo GetFactionInfo(Item observer)
        {
            FactionInfo result;
            if (!factionInfos.TryGetValue(observer, out result))
                result = Resolver.CreateFactionInfo(this, observer);
            return result;
        }

        /// <summary>
        ///     Wird vom Level zur Erstellung des Fraktionsstatus aufgerufen. Es wird
        ///     empfohlen zum Füllen die Prefill-Methode aufzurufen.
        /// </summary>
        /// <returns>Neue Instanz des Faction States</returns>
        public FactionState GetFactionState()
        {
            if (state == null)
                state = Resolver.CreateFactionState(this);
            return state;
        }

        /// <summary>
        ///     Füllt den übergebenen State mit den Informationen der Basis-Klasse
        ///     wie den Spieler Index und Typ-Informationen.
        /// </summary>
        /// <param name="state">Instanz des zu füllenden States</param>
        protected void PrefillState(FactionState state)
        {
            if (state == null)
                throw new ArgumentNullException();

            // TODO: Faction Name über TypeMapper ermitteln
            state.FactionName = GetType().Name;
            state.Name = Name;
            state.PlayerIndex = PlayerIndex;
            state.PlayerColor = PlayerColor;
            state.StartPoint = Home;
            state.Points = Points;
        }

        /// <summary>
        /// Interop Gruppierung von Unit, Interop und Item
        /// </summary>
        public sealed class FactionUnitInteropGroup
        {
            /// <summary>
            /// Referenz auf Interop
            /// </summary>
            public UnitInterop Interop { get; set; }

            /// <summary>
            /// Referenz auf die Unit Instanz
            /// </summary>
            public FactionUnit Unit { get; set; }

            /// <summary>
            /// Referenz auf das Item
            /// </summary>
            public FactionItem Item { get; set; }
        }
    }
}