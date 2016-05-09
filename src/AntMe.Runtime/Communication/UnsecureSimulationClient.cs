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
            simulation = Activator.CreateInstance(levelType, ExtensionLoader.DefaultTypeResolver) as Level;

            // Player erzeugen
            Faction[] levelFactions = new Faction[8];
            for (int i = 0; i < 8; i++)
            {
                // Skipp, falls nicht vorhanden
                if (players[i] == null)
                    continue;

                // TODO: Use Extension Loader for Name-Stuff

                Assembly playerAssembly = Assembly.Load(players[i].Type.AssemblyFile);
                Type playerType = playerAssembly.GetType(players[i].Type.TypeName);

                // Identify Name
                var playerAttributes = playerType.GetCustomAttributes(typeof(FactoryAttribute), true);
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

                // Identify Faction
                levelFactions[i] = resolver.CreateFaction(playerType, name, slots[i].ColorKey);

                // Falls Faction nicht gefunden werden konnte
                if (levelFactions[i] == null)
                    throw new Exception(string.Format("Cound not identify Faction for player {0}.",
                        players[i].Type.TypeName));

                // TODO: Load Settings from somewhere

            }

            // Level initialisieren
            simulation.Init(ExtensionLoader.DefaultTypeResolver, resolver.GetGlobalSettings(), seed, levelFactions.ToArray());
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
