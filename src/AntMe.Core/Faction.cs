using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    /// Base Class to all Factions.
    /// </summary>
    public abstract class Faction : PropertyList<FactionProperty>
    {
        /// <summary>
        /// Type of the Players Factory Class.
        /// </summary>
        private Type factoryType;

        private readonly Dictionary<Item, FactionInfo> factionInfos = new Dictionary<Item, FactionInfo>();
        private FactionState state;

        /// <summary>
        /// Standard-Konstruktor für Factions.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="factoryType"></param>
        /// <param name="level">Reference to the Level</param>
        protected Faction(SimulationContext context, Type factoryType, Level level)
        {
            Context = context;
            Units = new Dictionary<Item, UnitGroup>();
            Level = level;
            this.factoryType = factoryType;

            Context.Resolver.ResolveFaction(this);
        }

        /// <summary>
        /// Slot Index.
        /// </summary>
        public byte SlotIndex { get; private set; }

        /// <summary>
        /// Team Index.
        /// </summary>
        public byte TeamIndex { get; private set; }

        /// <summary>
        /// Player Name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Player Color.
        /// </summary>
        public PlayerColor PlayerColor { get; private set; }

        /// <summary>
        ///     Gibt eine Referenz aufs aktuelle Level zurück.
        /// </summary>
        public Level Level { get; private set; }

        /// <summary>
        ///     Gibt den Startpunkt der Fraktion zurück.
        /// </summary>
        public Vector2 Home { get; private set; }

        /// <summary>
        /// Gibt den aktuellen Punktestand dieser Faction an.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Factory/Slot-Spezifische Kopie der Settings.
        /// </summary>
        public Settings Settings { get { return Context.Settings; } }

        /// <summary>
        /// Current Simulation Context for this Faction.
        /// </summary>
        public SimulationContext Context { get; private set; }

        /// <summary>
        ///     Der Zufallszahlengenerator für diese Fraktion. Bitte immer
        ///     verwenden, um eine deterministrische Simulation zu
        ///     gewährleisten.
        /// </summary>
        public Random Random { get { return Context.Random; } }

        /// <summary>
        /// Group of Factory Interop-Instances.
        /// </summary>
        public FactoryGroup Factory { get; private set; }

        /// <summary>
        /// List of all Groups for Unit Interop-Instances.
        /// </summary>
        public Dictionary<Item, UnitGroup> Units { get; private set; }

        /// <summary>
        ///     Methode wird vom Level zur Initialisierung der Fraktion aufgerufen.
        ///     Ideal zur Initialisierung von Listen und Caches.
        /// </summary>
        public void Init(byte slotIndex, byte teamIndex, string name, PlayerColor color, Random random, Vector2 home)
        {
            SlotIndex = slotIndex;
            TeamIndex = teamIndex;
            Name = name;
            PlayerColor = color;
            Home = home;

            // Creates the Simulation Context for this Faction
            Context = new SimulationContext(Context.Resolver, Context.Settings, random);

            // Factory für Ameisen erzeugen
            Factory = new FactoryGroup()
            {
                Factory = (FactionFactory)Activator.CreateInstance(factoryType),
                Interop = Context.Resolver.CreateFactoryInterop(this)
            };
            Factory.Factory.Init(Factory.Interop);

            // TODO: Types checken!

            OnInit();
        }

        protected UnitGroup CreateUnit(FactionUnit unit, FactionItem item)
        {
            UnitInterop unitInterop = Context.Resolver.CreateUnitInterop(this, item) as UnitInterop;
            unit.Init(unitInterop);

            var group = new UnitGroup()
            {
                Item = item,
                Interop = unitInterop,
                Unit = unit
            };

            Units.Add(item, group);

            return group;
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
            Factory.Interop.InternalUpdate(round);

            // Unit Interop Updates
            foreach (var unit in Units.Values)
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
                result = Context.Resolver.CreateFactionInfo(this, observer);
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
                state = Context.Resolver.CreateFactionState(this);
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
            state.SlotIndex = SlotIndex;
            state.PlayerColor = PlayerColor;
            state.StartPoint = Home;
            state.Points = Points;
        }

        /// <summary>
        /// Class to hold the group of relating Interop- and Factory-Instances.
        /// </summary>
        public sealed class FactoryGroup
        {
            /// <summary>
            /// Reference to the Interop Instance.
            /// </summary>
            public FactoryInterop Interop { get; set; }

            /// <summary>
            /// Reference to the Factory Instance.
            /// </summary>
            public FactionFactory Factory { get; set; }
        }

        /// <summary>
        /// Class to hold the group of relating Interop-, Item- and Unit-Instances.
        /// </summary>
        public sealed class UnitGroup
        {
            /// <summary>
            /// Reference to the Interop Instance.
            /// </summary>
            public UnitInterop Interop { get; set; }

            /// <summary>
            /// Refernce to the Unit Instance.
            /// </summary>
            public FactionUnit Unit { get; set; }

            /// <summary>
            /// Reference to the Item Instance.
            /// </summary>
            public FactionItem Item { get; set; }
        }
    }
}