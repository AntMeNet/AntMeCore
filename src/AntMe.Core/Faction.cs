using System;
using System.Collections.Generic;

namespace AntMe
{
    /// <summary>
    ///     Base Class to all Factions.
    /// </summary>
    public abstract class Faction : PropertyList<FactionProperty>
    {
        /// <summary>
        ///     Local Faction Info Cache.
        /// </summary>
        private readonly Dictionary<Item, FactionInfo> factionInfos = new Dictionary<Item, FactionInfo>();

        /// <summary>
        ///     Type of the Players Factory Class.
        /// </summary>
        private readonly Type factoryType;

        /// <summary>
        ///     Local Instance of the Faction State.
        /// </summary>
        private FactionState state;

        /// <summary>
        ///     Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="factoryType">Type of Factory Class</param>
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
        ///     Slot Index.
        /// </summary>
        public byte SlotIndex { get; private set; }

        /// <summary>
        ///     Team Index.
        /// </summary>
        public byte TeamIndex { get; private set; }

        /// <summary>
        ///     Player Name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Player Color.
        /// </summary>
        public PlayerColor PlayerColor { get; private set; }

        /// <summary>
        ///     Reference to the Level.
        /// </summary>
        public Level Level { get; }

        /// <summary>
        ///     Gets the default Start Point.
        /// </summary>
        public Vector2 Home { get; private set; }

        /// <summary>
        ///     Factory-specific Settings.
        /// </summary>
        public KeyValueStore Settings => Context.Settings;

        /// <summary>
        ///     Current Simulation Context for this Faction.
        /// </summary>
        public SimulationContext Context { get; private set; }

        /// <summary>
        ///     Gets the Randomizer for this Faction.
        /// </summary>
        public Random Random => Context.Random;

        /// <summary>
        ///     Group of Factory Interop-Instances.
        /// </summary>
        public FactoryGroup Factory { get; private set; }

        /// <summary>
        ///     List of all Groups for Unit Interop-Instances.
        /// </summary>
        public Dictionary<Item, UnitGroup> Units { get; }

        /// <summary>
        ///     Initializes the Faction.
        /// </summary>
        public void Init(byte slotIndex, byte teamIndex, string name, PlayerColor color, Random random, Vector2 home)
        {
            SlotIndex = slotIndex;
            TeamIndex = teamIndex;
            Name = name;
            PlayerColor = color;
            Home = home;

            // Creates the Simulation Context for this Faction
            Context = new SimulationContext(Context.Resolver, Context.Mapper, Context.Settings, random);

            // Init all Properties
            foreach (var property in Properties)
                property.Init();

            // Factory für Ameisen erzeugen
            Factory = new FactoryGroup
            {
                Factory = (FactionFactory) Activator.CreateInstance(factoryType),
                Interop = Context.Resolver.CreateFactoryInterop(this)
            };
            Factory.Factory.Init(Factory.Interop);

            // TODO: Types checken!

            OnInit();
        }

        /// <summary>
        ///     Scans the given Type for Unit Attributes.
        /// </summary>
        /// <param name="unitType">Type to scan</param>
        /// <returns>Collected Attributes</returns>
        protected UnitAttributeCollection GetAttributes(Type unitType)
        {
            // Try to find the Caste Name
            var categoryName = string.Empty;
            var unitCategoryNames = unitType.GetCustomAttributes(typeof(UnitGroupAttribute), true);
            if (unitCategoryNames.Length > 0)
                categoryName = (unitCategoryNames[0] as UnitGroupAttribute).Name;

            // Collect Attributes
            var attributesValues = new Dictionary<string, sbyte>();
            foreach (UnitAttribute unitAttribute in unitType.GetCustomAttributes(typeof(UnitAttribute), true))
            {
                // Check for Attributes with the same Key
                if (attributesValues.ContainsKey(unitAttribute.Key))
                    throw new NotSupportedException(string.Format("Attribute Key {0} from {1} is already in the list.",
                        unitAttribute.Key, unitType.FullName));

                // Check Min-Max-Values
                if (unitAttribute.Value < unitAttribute.MinValue)
                    throw new NotSupportedException(string.Format(
                        "Attribute {0} from {1} has a Value smaller than MinValue", unitAttribute.Key,
                        unitType.FullName));
                if (unitAttribute.Value > unitAttribute.MaxValue)
                    throw new NotSupportedException(string.Format(
                        "Attribute {0} from {1} has a Value greater than MaxValue", unitAttribute.Key,
                        unitType.FullName));

                // Add
                attributesValues.Add(unitAttribute.Key, unitAttribute.Value);
            }

            return new UnitAttributeCollection(categoryName, attributesValues);
        }

        /// <summary>
        ///     Generate a Unit/Item/Interop Combination based on the Unit and Item
        /// </summary>
        /// <param name="unit">Unit Instance</param>
        /// <param name="item">Item Instance</param>
        /// <returns></returns>
        protected UnitGroup CreateUnit(FactionUnit unit, FactionItem item)
        {
            // TODO: Check valid Types

            var unitInterop = Context.Resolver.CreateUnitInterop(this, item);
            unit.Init(unitInterop);

            var group = new UnitGroup
            {
                Item = item,
                Interop = unitInterop,
                Unit = unit
            };

            Units.Add(item, group);

            return group;
        }

        /// <summary>
        ///     Method will be called after Initializing the Faction.
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        ///     Updates the Faction and all the including Properties.
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
        ///     Gets a call before the Faction updates itself.
        /// </summary>
        /// <param name="round">Current Round</param>
        protected abstract void OnUpdate(int round);

        /// <summary>
        ///     Gets a call after Faction Update.
        /// </summary>
        /// <param name="round">Current Round</param>
        protected abstract void OnUpdated(int round);

        /// <summary>
        ///     Generates Faction Info.
        /// </summary>
        /// <param name="observer">Reference to the observing Item.</param>
        /// <returns></returns>
        public FactionInfo GetFactionInfo(Item observer)
        {
            FactionInfo result;
            if (!factionInfos.TryGetValue(observer, out result))
                result = Context.Resolver.CreateFactionInfo(this, observer);
            return result;
        }

        /// <summary>
        ///     Generates the Faction State.
        /// </summary>
        /// <returns>Updated Faction State</returns>
        public FactionState GetFactionState()
        {
            if (state == null)
                state = Context.Resolver.CreateFactionState(this);
            return state;
        }

        /// <summary>
        ///     Gets a call before <see cref="GetFactionState" /> returns the State Reference.
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
        }

        /// <summary>
        ///     Class to hold the group of relating Interop- and Factory-Instances.
        /// </summary>
        public sealed class FactoryGroup
        {
            /// <summary>
            ///     Reference to the Interop Instance.
            /// </summary>
            public FactoryInterop Interop { get; set; }

            /// <summary>
            ///     Reference to the Factory Instance.
            /// </summary>
            public FactionFactory Factory { get; set; }
        }

        /// <summary>
        ///     Class to hold the group of relating Interop-, Item- and Unit-Instances.
        /// </summary>
        public sealed class UnitGroup
        {
            /// <summary>
            ///     Reference to the Interop Instance.
            /// </summary>
            public UnitInterop Interop { get; set; }

            /// <summary>
            ///     Refernce to the Unit Instance.
            /// </summary>
            public FactionUnit Unit { get; set; }

            /// <summary>
            ///     Reference to the Item Instance.
            /// </summary>
            public FactionItem Item { get; set; }
        }
    }
}