using AntMe.Basics.Factions.Ants;
using AntMe.Items.Basics;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Factions.Ants
{
    public sealed class AntFaction : Faction
    {
        private readonly Dictionary<int, AnthillItem> _antHills = new Dictionary<int, AnthillItem>();
        private readonly Type _colonyType;
        private readonly string[] _names;

        private readonly Random _random = new Random();
        private readonly AntFactionState _state = new AntFactionState();
        private int _antRespawnDelay;
        private int totalAntCount = 0;

        public AntFaction(ITypeResolver resolver, Settings settings, Type factoryType, Level level)
            : base(resolver, settings, factoryType, level)
        {
            // TODO: Check Factory Type?

            _names = AntGeneratorFiles.femaleNames.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected override void OnInit()
        {
            Level.Engine.OnRemoveItem += Level_RemovedItem;

            // Ameisenhügel erstellen und platzieren
            if (Settings.GetBool<AntFaction>("InitialAntHill") ?? false)
                CreateAntHill(Home);

            // Erste Gruppe Ameisen erstellen
            for (var i = 0; i < Settings.GetInt<AntFaction>("AntCount"); i++)
                CreateAnt();
        }

        private void Level_RemovedItem(Item item)
        {
            if (UnitInterops.ContainsKey(item.Id))
                UnitInterops.Remove(item.Id);
        }

        protected override void OnUpdate(int round)
        {
            // Ameisen nachproduzieren
            // - die Zeitverzögerung für neue Ameisen muss abgelaufen sein
            // - die Anzahl gleichzeitiger Ameisen muss kleiner dem maximalwert sein
            // - die Anzahl ingesamt erstellter Ameisen muss kleiner als der maximalwert sein
            if (_antRespawnDelay-- <= 0 &&
                UnitInterops.Count < Settings.GetInt<AntFaction>("AntMaxConcurrentCount") &&
                totalAntCount < Settings.GetInt<AntFaction>("AntMaxTotalCount"))
            {
                CreateAnt();
            }

            // Neuer Punktestand berechnen
            Points = 0;
            foreach (var anthill in _antHills.Values)
            {
                Points += anthill.AppleAmount;
                Points += anthill.SugarAmount;
            }
        }

        private void CreateAntHill(Vector2 position)
        {
            var hill = new AnthillItem(Resolver, this, position);
            Level.Engine.InsertItem(hill);
            _antHills.Add(hill.Id, hill);
        }

        private void CreateAnt()
        {
            // TODO: Create Member Methode muss mehr Informationen bekommen und 
            // auch bestimmen, in welchem Ameisenhügel die Ameise erscheint.
            float range = 10f;
            Vector2 position = Home +
                               new Vector2(range * (float)_random.NextDouble(), range * (float)_random.NextDouble()) -
                               new Vector2(5, 5);

            // Type anfragen
            Type antType = (FactoryInterop as AntFactoryInterop).RequestCreateMember();
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
            string name = _names[_random.Next(0, _names.Length - 1)];

            // AntItem erstellen
            AntUnit antUnit = (AntUnit)Activator.CreateInstance(antType);
            AntItem antItem = new AntItem(Resolver, position, Angle.FromDegree(_random.Next(0, 359)), this, name, caste);
            AntUnitInterop unitInterop = Resolver.CreateUnitInterop(this) as AntUnitInterop;
            antUnit.Init(unitInterop);

            Level.Engine.InsertItem(antItem);
            UnitInterops.Add(antItem.Id, new FactionUnitInteropGroup()
            {
                Item = antItem,
                Interop = unitInterop,
                Unit = antUnit
            });
            totalAntCount++;

            // TODO: Kosten


            // Stats
            _antRespawnDelay = Settings.GetInt<AntFaction>("AntRespawnDelay") ?? 0;
        }

        /// <summary>
        ///     Wird von einer Ameise aufgerufen, um den nächstgelegenen
        ///     Ameisenbau zu finden.
        /// </summary>
        /// <param name="item">Aufrufende Ameise</param>
        /// <returns>Instanz des nächstgelegenen Ameisenbaus</returns>
        public AnthillInfo GetClosestAnthill(AntItem item)
        {
            return _antHills.Values.
                Select(anthill => anthill.GetItemInfo(item) as AnthillInfo).
                OrderBy(i => i.Distance).
                FirstOrDefault();
        }
    }
}