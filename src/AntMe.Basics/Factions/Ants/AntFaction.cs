
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

            // Ameisenhügel erstellen und platzieren
            for (int i = 0; i < Settings.GetInt<AntFaction>("InitialAnthillCount").Value; i++)
            {
                // TODO: Random Positions (on i > 1)
                CreateAntHill(Home);
            }

            // Erste Gruppe Ameisen erstellen
            for (var i = 0; i < Settings.GetInt<AntFaction>("InitialAntCount").Value; i++)
                CreateAnt();
        }

        private void Level_RemovedItem(Item item)
        {
            if (Units.ContainsKey(item))
                Units.Remove(item);
        }

        protected override void OnUpdate(int round)
        {
            // Ameisen nachproduzieren
            // - die Zeitverzögerung für neue Ameisen muss abgelaufen sein
            // - die Anzahl gleichzeitiger Ameisen muss kleiner dem maximalwert sein
            // - die Anzahl ingesamt erstellter Ameisen muss kleiner als der maximalwert sein
            if (_antRespawnDelay-- <= 0 &&
                Units.Count < Settings.GetInt<AntFaction>("ConcurrentAntCount").Value &&
                totalAntCount < Settings.GetInt<AntFaction>("TotalAntCount").Value)
            {
                CreateAnt();
            }

            // Neuer Punktestand berechnen
            Points = 0;
            foreach (var anthill in antHills.Values)
            {
                //Points += anthill.AppleAmount;
                //Points += anthill.SugarAmount;
            }
        }

        private void CreateAntHill(Vector2 position)
        {
            var hill = new AnthillItem(Context, this, position);
            Level.Engine.InsertItem(hill);
            antHills.Add(hill.Id, hill);
        }

        private void CreateAnt()
        {
            // TODO: Create Member Methode muss mehr Informationen bekommen und 
            // auch bestimmen, in welchem Ameisenhügel die Ameise erscheint.
            float range = 10f;
            Vector2 position = Home +
                               new Vector2(range * (float)Random.NextDouble(), range * (float)Random.NextDouble()) -
                               new Vector2(5, 5);

            // Type anfragen
            Type antType = (Factory.Interop as AntFactoryInterop).RequestCreateMember();
            if (antType == null)
            {
                // Spieler will offensichtlich keine Ameise erstellen
                return;
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
            AntItem antItem = new AntItem(Context, this, position, Angle.FromDegree(Random.Next(0, 359)), name);
            AntUnit antUnit = (AntUnit)Activator.CreateInstance(antType);

            CreateUnit(antUnit, antItem);

            Level.Engine.InsertItem(antItem);
            totalAntCount++;

            // TODO: Kosten

            // Stats
            _antRespawnDelay = Settings.GetInt<AntFaction>("AntRespawnDelay").Value;
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