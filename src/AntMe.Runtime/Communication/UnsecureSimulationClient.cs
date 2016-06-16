using System;
using System.Linq;
using System.Reflection;

namespace AntMe.Runtime.Communication
{
    internal sealed class UnsecureSimulationClient : LocalSimulationClient
    {
        Level simulation = null;

        public UnsecureSimulationClient(string[] extensionPaths, ITypeResolver resolver) : base(extensionPaths, resolver) { }

        public Level LevelInstance { get { return simulation; } }

        protected override void InitSimulation(int seed, LevelInfo level, PlayerInfo[] players, Slot[] slots)
        {
            Assembly levelAssembly = Assembly.Load(level.Type.AssemblyFile);
            Type levelType = levelAssembly.GetType(level.Type.TypeName);

            SimulationContext context = ExtensionLoader.CreateSimulationContext();

            simulation = Activator.CreateInstance(levelType, context) as Level;

            // Player erzeugen
            LevelSlot[] levelSlots = new LevelSlot[AntMe.Level.MAX_SLOTS];
            for (int i = 0; i < AntMe.Level.MAX_SLOTS; i++)
            {
                // Skipp, falls nicht vorhanden
                if (players[i] == null)
                    continue;

                Assembly playerAssembly = Assembly.Load(players[i].Type.AssemblyFile);
                Type factoryType = playerAssembly.GetType(players[i].Type.TypeName);

                // Identify Name
                var playerAttributes = factoryType.GetCustomAttributes(typeof(FactoryAttribute), true);
                if (playerAttributes.Length != 1)
                    throw new Exception("Player does not have the right number of Player Attributes");

                FactoryAttribute playerAttribute = playerAttributes[0] as FactoryAttribute;

                // Find the right Mapping
                var mappingAttributes = playerAttribute.GetType().
                    GetCustomAttributes(typeof(FactoryAttributeMappingAttribute), false);
                if (mappingAttributes.Length != 1)
                    throw new Exception("Player Attribute has no valid Property Mapping Attribute");

                FactoryAttributeMappingAttribute mappingAttribute = mappingAttributes[0] as FactoryAttributeMappingAttribute;

                // Werte auslesen
                string name = playerAttribute.GetType().GetProperty(mappingAttribute.NameProperty).
                    GetValue(playerAttribute, null) as string;

                levelSlots[i] = new LevelSlot()
                {
                    FactoryType = factoryType,
                    Name = name,
                    Color = slots[i].ColorKey,
                    Team = slots[i].Team
                };
            }

            // Level initialisieren
            simulation.Init(seed, levelSlots);
        }

        protected override LevelState UpdateSimulation()
        {
            return simulation.NextState();
        }

        protected override void FinalizeSimulation()
        {
            simulation = null;
        }
    }
}
