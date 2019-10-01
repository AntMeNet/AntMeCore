using System;
using System.Collections.Generic;
using System.Linq;
using AntMe.Basics.Items;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    ///     Faction for Ants.
    /// </summary>
    public sealed class AntFaction : Faction
    {
        private readonly AntFactionState _state = new AntFactionState();
        private readonly Dictionary<int, AnthillItem> antHills = new Dictionary<int, AnthillItem>();
        private readonly string[] names;
        private int _antRespawnDelay;
        private AnthillItem primaryHill;
        private int totalAntCount;

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="factoryType">Type of the Player Factory</param>
        /// <param name="level">Reference to the Level</param>
        public AntFaction(SimulationContext context, Type factoryType, Level level)
            : base(context, factoryType, level)
        {
            // TODO: Check Factory Type?

            // Load all possible Ant Names
            names = AntGeneratorFiles.femaleNames.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        ///     Initialize a Faction.
        /// </summary>
        protected override void OnInit()
        {
            Level.Engine.OnRemoveItem += Level_RemovedItem;

            // Generate the first group of Anthills (Initial Anthills)
            var hillCount = Settings.GetInt<AntFaction>("InitialAnthillCount").Value;
            hillCount = Math.Min(hillCount, Settings.GetInt<AntFaction>("TotalAnthillCount").Value);
            hillCount = Math.Min(hillCount, Settings.GetInt<AntFaction>("ConcurrentAnthillCount").Value);
            for (var i = 0; i < hillCount; i++)
                if (i == 0)
                    // Generate Primary Hill
                    primaryHill = CreateAntHill(Home);
                else
                    // TODO: Random Positions (on i > 1)
                    CreateAntHill(Home);

            // Generate the first group of Ants (Initial Ants)
            var antCount = Settings.GetInt<AntFaction>("InitialAntCount").Value;
            antCount = Math.Min(antCount, Settings.GetInt<AntFaction>("TotalAntCount").Value);
            antCount = Math.Min(antCount, Settings.GetInt<AntFaction>("ConcurrentAntCount").Value);
            for (var i = 0; i < antCount; i++)
                if (primaryHill != null)
                    CreateAnt(primaryHill);
        }

        private void Level_RemovedItem(Item item)
        {
            if (Units.ContainsKey(item))
                Units.Remove(item);
        }

        protected override void OnUpdate(int round)
        {
            // Generate new Ants
            // - check for Repawn Delay
            // - check for maximum Count of concurrent Ants (Settings: ConcurrentAntCount)
            // - check for maximum Ants per Simulation (Settings: TotalAntCount)
            if (_antRespawnDelay-- <= 0 &&
                Units.Count < Settings.GetInt<AntFaction>("ConcurrentAntCount").Value &&
                totalAntCount < Settings.GetInt<AntFaction>("TotalAntCount").Value)
                CreateAnt(primaryHill);
        }

        protected override void OnUpdated(int round)
        {
        }

        /// <summary>
        ///     Generates a new Anthill.
        /// </summary>
        /// <param name="position">Position</param>
        private AnthillItem CreateAntHill(Vector2 position)
        {
            var hill = new AnthillItem(Context, this, position);
            Level.Engine.InsertItem(hill);
            antHills.Add(hill.Id, hill);
            return hill;
        }

        /// <summary>
        ///     Generates a new Ant at the position of the given Anthill.
        /// </summary>
        private AntItem CreateAnt(AnthillItem anthill)
        {
            // Find Direction
            var direction = Angle.FromDegree(Random.Next(0, 360));
            var rim = Vector2.FromAngle(direction) * (anthill.Radius + AntItem.AntRadius);
            var position = anthill.Position.ToVector2XY() + rim;

            // Type anfragen
            var antType = (Factory.Interop as AntFactoryInterop).RequestCreateMember();
            if (antType == null)
                // Spieler will offensichtlich keine Ameise erstellen
                return null;

            // Prüfen, ob es sich um den richtigen Typen handelt
            if (!antType.IsSubclassOf(typeof(AntUnit)))
                throw new ArgumentException("Given Type is not an Ant Unit");

            // Get Attributes
            var attributes = GetAttributes(antType);

            // Namen erzeugen
            var name = names[Random.Next(0, names.Length - 1)];

            // AntItem erstellen
            var antItem = new AntItem(Context, attributes, this, position, direction, name);
            var antUnit = (AntUnit) Activator.CreateInstance(antType);

            CreateUnit(antUnit, antItem);

            Level.Engine.InsertItem(antItem);
            totalAntCount++;

            // TODO: Kosten

            // Stats
            _antRespawnDelay = Settings.GetInt<AntFaction>("AntRespawnDelay").Value;

            return antItem;
        }

        /// <summary>
        ///     Wird von einer Ameise aufgerufen, um den nächstgelegenen
        ///     Ameisenbau zu finden.
        /// </summary>
        /// <param name="item">Aufrufende Ameise</param>
        /// <returns>Instanz des nächstgelegenen Ameisenbaus</returns>
        public AnthillInfo GetClosestAnthill(Item item)
        {
            // TODO: Check for a smarter Way without Info Object
            return antHills.Values.Select(anthill => anthill.GetItemInfo() as AnthillInfo).OrderBy(i => i.Distance)
                .FirstOrDefault();
        }
    }
}