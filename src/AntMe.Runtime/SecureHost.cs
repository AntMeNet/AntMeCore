using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AntMe.Runtime
{
    internal sealed class SecureHost : MarshalByRefObject
    {
        private readonly StateSerializer _serializer;
        private Level _level;

        public SecureHost()
        {
            _serializer = new StateSerializer();
        }

        public void Setup(string[] extensionPaths, Setup settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings", "Settings is null");

            if (settings.Level == null)
                throw new ArgumentNullException("settings", "Leve is null");

            if (settings.Player.Length != 8)
                throw new ArgumentException("Player-Array must have a length of 8");

            if (settings.Colors.Length != 8)
                throw new ArgumentException("Player-Array must have a length of 8");

            // Eindeutigkeit der Farben prüfen
            var colors = new List<PlayerColor>();
            for (int i = 0; i < 8; i++)
            {
                if (settings.Colors[i] == PlayerColor.Undefined)
                    throw new ArgumentException("Undefined is not a valid color for slot " + i, "settings");

                if (colors.Contains(settings.Colors[i]))
                    throw new ArgumentException("Color " + settings.Colors[i] + " is assigned double", "settings");
                colors.Add(settings.Colors[i]);
            }
            colors.Clear();

            // Load Default Assemblies
            ExtensionLoader.LoadExtensions(extensionPaths, null, false);

            // TODO: this is for debug
            AppDomain.CurrentDomain.AssemblyLoad += (x, y) => { };
            AppDomain.CurrentDomain.AssemblyResolve += (x, y) => null;
            AppDomain.CurrentDomain.ResourceResolve += (x, y) => null;
            AppDomain.CurrentDomain.TypeResolve += (x, y) => null;

            // Level erzeugen
            Assembly levelAssembly = Assembly.Load(settings.Level.AssemblyFile);
            Type levelType = levelAssembly.GetType(settings.Level.TypeName, true);
            Level lvl = Activator.CreateInstance(levelType, ExtensionLoader.DefaultTypeResolver) as Level;
            _level = lvl;

            // Player erzeugen
            var levelFactions = new Faction[8];
            for (int i = 0; i < 8; i++)
            {
                // Skipp, falls nicht vorhanden
                if (settings.Player[i] == null)
                    continue;

                // TODO: Use Extension Loader for Name-Stuff

                Assembly playerAssembly = Assembly.Load(settings.Player[i].AssemblyFile);
                Type playerType = playerAssembly.GetType(settings.Player[i].TypeName);

                // Identify Name
                object[] playerAttributes = playerType.GetCustomAttributes(typeof (FactoryAttribute), true);
                if (playerAttributes.Length != 1)
                    throw new Exception("Player does not have the right number of Player Attributes");

                var playerAttribute = playerAttributes[0] as FactoryAttribute;

                // Find the right Mapping
                object[] mappingAttributes = playerAttribute.GetType().
                    GetCustomAttributes(typeof (FactoryAttributeMappingAttribute), false);
                if (mappingAttributes.Length != 1)
                    throw new Exception("Player Attribute has no valid Property Mapping Attribute");

                var mappingAttribute = mappingAttributes[0] as FactoryAttributeMappingAttribute;

                // Werte auslesen
                var name = playerAttribute.GetType().GetProperty(mappingAttribute.NameProperty).
                    GetValue(playerAttribute, null) as string;

                // Identify Faction
                levelFactions[i] = ExtensionLoader.DefaultTypeResolver.CreateFaction(playerType, name, settings.Colors[i]);

                // Falls Faction nicht gefunden werden konnte
                if (levelFactions[i] == null)
                    throw new Exception(string.Format("Cound not identify Faction for player {0}.",
                        settings.Player[i].TypeName));

                // TODO: Load Settings from somewhere
            }

            // Level initialisieren
            _level.Init(ExtensionLoader.DefaultTypeResolver, ExtensionLoader.DefaultTypeResolver.GetGlobalSettings(), 
                settings.Seed, levelFactions.ToArray());
        }

        public byte[] NextFrame()
        {
            if (_level == null)
                throw new NotSupportedException("Level not ready");
            if (_level.Mode == LevelMode.Uninit)
                throw new InvalidOperationException("Level is not initialized");
            if (_level.Mode != LevelMode.Running)
                throw new InvalidOperationException("Level is finished");

            LevelState state = _level.NextState();
            return _serializer.Serialize(state);
        }

        public Exception GetLastException()
        {
            if (_level != null)
                return _level.LastException;
            return null;
        }
    }
}