
using AntMe.Basics.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Faction for Ants.
    /// </summary>
    public sealed class AntFaction : Faction
    {
        private AnthillItem primaryHill = null;
        private readonly Dictionary<int, AnthillItem> antHills = new Dictionary<int, AnthillItem>();
        private readonly string[] names;

        private readonly AntFactionState _state = new AntFactionState();
        private int _antRespawnDelay;
        private int totalAntCount = 0;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="factoryType">Type of the Player Factory</param>
        /// <param name="level">Reference to the Level</param>
        public AntFaction(SimulationContext context, Type factoryType, Level level)
            : base(context, factoryType, level)
        {
            // TODO: Check Factory Type?

            // Load all possible Ant Names
            names = AntGeneratorFiles.femaleNames.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Initialize a Faction.
        /// </summary>
        protected override void OnInit()
        {
            Level.Engine.OnRemoveItem += Level_RemovedItem;

            // Generate the first group of Anthills (Initial Anthills)
            int hillCount = Settings.GetInt<AntFaction>("InitialAnthillCount").Value;
            hillCount = Math.Min(hillCount, Settings.GetInt<AntFaction>("TotalAnthillCount").Value);
            hillCount = Math.Min(hillCount, Settings.GetInt<AntFaction>("ConcurrentAnthillCount").Value);
            for (int i = 0; i < hillCount; i++)
            {
                
                if (i == 0)
                {
                    // Generate Primary Hill
                    primaryHill = CreateAntHill(Home);
                }
                else
                {
                    // TODO: Random Positions (on i > 1)
                    CreateAntHill(Home);
                }
            }

            // Generate the first group of Ants (Initial Ants)
            int antCount = Settings.GetInt<AntFaction>("InitialAntCount").Value;
            antCount = Math.Min(antCount, Settings.GetInt<AntFaction>("TotalAntCount").Value);
            antCount = Math.Min(antCount, Settings.GetInt<AntFaction>("ConcurrentAntCount").Value);
            for (var i = 0; i < antCount; i++)
            {
                if (primaryHill != null)
                    CreateAnt(primaryHill);
            }
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
            {
                CreateAnt(primaryHill);
            }
        }

        protected override void OnUpdated(int round) { }

        /// <summary>
        /// Generates a new Anthill.
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
        /// Generates a new Ant at the position of the given Anthill.
        /// </summary>
        private AntItem CreateAnt(AnthillItem anthill)
        {
            // Find Direction
            Angle direction = Angle.FromDegree(Random.Next(0, 360));
            Vector2 rim = Vector2.FromAngle(direction) * (anthill.Radius + AntItem.AntRadius);
            Vector2 position = anthill.Position.ToVector2XY() + rim;

            // Type anfragen
            Type antType = (Factory.Interop as AntFactoryInterop).RequestCreateMember();
            if (antType == null)
            {
                // Spieler will offensichtlich keine Ameise erstellen
                return null;
            }

            // Prüfen, ob es sich um den richtigen Typen handelt
            if (!antType.IsSubclassOf(typeof(AntUnit)))
                throw new ArgumentException("Given Type is not a primordial Ant");

            // Auf Kasten prüfen
            var caste = new PrimordialCasteAttribute();
            var castes = antType.GetCustomAttributes(typeof(CasteAttribute), true);
            if (castes.Length > 0 && castes[0] is CasteAttribute)
            {
                var attribute = castes[0] as CasteAttribute;

                // Caste Mapping ermitteln
                Type casteType = attribute.GetType();
                object[] mappings = casteType.GetCustomAttributes(typeof(CasteAttributeMappingAttribute), false);
                if (mappings.Length != 1 || !(mappings[0] is CasteAttributeMappingAttribute))
                    throw new ArgumentException("The used Caste-Attribute has no Mapping");

                // Mapping versuchen
                try
                {
                    var mapping = mappings[0] as CasteAttributeMappingAttribute;
                    var tempCaste = new PrimordialCasteAttribute
                    {
                        Name = (string)casteType.GetProperty(mapping.NameProperty).GetValue(attribute, null),
                        Attack = (int)casteType.GetProperty(mapping.AttackProperty).GetValue(attribute, null),
                        Attention = (int)casteType.GetProperty(mapping.AttentionProperty).GetValue(attribute, null),
                        Defense = (int)casteType.GetProperty(mapping.DefenseProperty).GetValue(attribute, null),
                        Speed = (int)casteType.GetProperty(mapping.SpeedProperty).GetValue(attribute, null),
                        Strength = (int)casteType.GetProperty(mapping.StrengthProperty).GetValue(attribute, null)
                    };

                    // Prüfung
                    tempCaste.Check();

                    caste = tempCaste;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("The mapping of the used Caste-Attribute failed", ex);
                }
            }

            // Namen erzeugen
            string name = names[Random.Next(0, names.Length - 1)];

            // AntItem erstellen
            AntItem antItem = new AntItem(Context, this, position, direction, name);
            AntUnit antUnit = (AntUnit)Activator.CreateInstance(antType);

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
            return antHills.Values.
                Select(anthill => anthill.GetItemInfo(item) as AnthillInfo).
                OrderBy(i => i.Distance).
                FirstOrDefault();
        }
    }
}