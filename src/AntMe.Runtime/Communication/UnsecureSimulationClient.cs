using System;
using System.Reflection;

namespace AntMe.Runtime.Communication
{
    internal sealed class UnsecureSimulationClient : LocalSimulationClient
    {
        public UnsecureSimulationClient(string[] extensionPaths, ITypeResolver resolver) : base(extensionPaths,
            resolver)
        {
        }

        public Level LevelInstance { get; private set; }

        protected override void InitSimulation(int seed, LevelInfo level, PlayerInfo[] players, Slot[] slots)
        {
            var levelAssembly = Assembly.Load(level.Type.AssemblyFile);
            var levelType = levelAssembly.GetType(level.Type.TypeName);

            var context = ExtensionLoader.CreateSimulationContext();

            LevelInstance = Activator.CreateInstance(levelType, context) as Level;

            // Player erzeugen
            var levelSlots = new LevelSlot[AntMe.Level.MAX_SLOTS];
            for (var i = 0; i < AntMe.Level.MAX_SLOTS; i++)
            {
                // Skipp, falls nicht vorhanden
                if (players[i] == null)
                    continue;

                var playerAssembly = Assembly.Load(players[i].Type.AssemblyFile);
                var factoryType = playerAssembly.GetType(players[i].Type.TypeName);

                // Identify Name
                var playerAttributes = factoryType.GetCustomAttributes(typeof(FactoryAttribute), true);
                if (playerAttributes.Length != 1)
                    throw new Exception("Player does not have the right number of Player Attributes");

                var playerAttribute = playerAttributes[0] as FactoryAttribute;

                // Find the right Mapping
                var mappingAttributes = playerAttribute.GetType()
                    .GetCustomAttributes(typeof(FactoryAttributeMappingAttribute), false);
                if (mappingAttributes.Length != 1)
                    throw new Exception("Player Attribute has no valid Property Mapping Attribute");

                var mappingAttribute = mappingAttributes[0] as FactoryAttributeMappingAttribute;

                // Werte auslesen
                var name = playerAttribute.GetType().GetProperty(mappingAttribute.NameProperty)
                    .GetValue(playerAttribute, null) as string;

                levelSlots[i] = new LevelSlot
                {
                    FactoryType = factoryType,
                    Name = name,
                    Color = slots[i].ColorKey,
                    Team = slots[i].Team
                };
            }

            // Level initialisieren
            LevelInstance.Init(seed, levelSlots);
        }

        protected override LevelState UpdateSimulation()
        {
            return LevelInstance.NextState();
        }

        protected override void FinalizeSimulation()
        {
            LevelInstance = null;
        }
    }
}