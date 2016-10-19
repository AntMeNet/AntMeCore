﻿using AntMe.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AntMe.Runtime
{
    internal sealed class SecureHost : MarshalByRefObject
    {
        private SimulationContext _context;
        private LevelStateByteSerializer _serializer;
        private Level _level;

        public void Setup(string[] extensionPaths, Setup settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings", "Settings is null");

            if (settings.Level == null)
                throw new ArgumentNullException("settings", "Leve is null");

            if (settings.Player.Length != Level.MAX_SLOTS)
                throw new ArgumentException(string.Format("Player-Array must have a length of {0}", Level.MAX_SLOTS));

            if (settings.Colors.Length != Level.MAX_SLOTS)
                throw new ArgumentException(string.Format("Player-Array must have a length of {0}", Level.MAX_SLOTS));

            // Eindeutigkeit der Farben prüfen
            var colors = new List<PlayerColor>();
            for (int i = 0; i < Level.MAX_SLOTS; i++)
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

            // Create Serializer
            _context = ExtensionLoader.CreateSimulationContext();
            _serializer = new LevelStateByteSerializer(_context);

            // TODO: this is for debug
            AppDomain.CurrentDomain.AssemblyLoad += (x, y) => { };
            AppDomain.CurrentDomain.AssemblyResolve += (x, y) => null;
            AppDomain.CurrentDomain.ResourceResolve += (x, y) => null;
            AppDomain.CurrentDomain.TypeResolve += (x, y) => null;

            // Level erzeugen
            Assembly levelAssembly = Assembly.Load(settings.Level.AssemblyFile);
            Type levelType = levelAssembly.GetType(settings.Level.TypeName, true);
            Level lvl = Activator.CreateInstance(levelType, _context) as Level;
            _level = lvl;

            // Player erzeugen
            LevelSlot[] levelSlots = new LevelSlot[Level.MAX_SLOTS];
            for (int i = 0; i < Level.MAX_SLOTS; i++)
            {
                // Skipp, falls nicht vorhanden
                if (settings.Player[i] == null)
                    continue;

                Assembly playerAssembly = Assembly.Load(settings.Player[i].AssemblyFile);
                Type factoryType = playerAssembly.GetType(settings.Player[i].TypeName);

                // Identify Name
                object[] playerAttributes = factoryType.GetCustomAttributes(typeof(FactoryAttribute), true);
                if (playerAttributes.Length != 1)
                    throw new Exception("Player does not have the right number of Player Attributes");

                var playerAttribute = playerAttributes[0] as FactoryAttribute;

                // Find the right Mapping
                object[] mappingAttributes = playerAttribute.GetType().
                    GetCustomAttributes(typeof(FactoryAttributeMappingAttribute), false);
                if (mappingAttributes.Length != 1)
                    throw new Exception("Player Attribute has no valid Property Mapping Attribute");

                var mappingAttribute = mappingAttributes[0] as FactoryAttributeMappingAttribute;

                // Werte auslesen
                var name = playerAttribute.GetType().GetProperty(mappingAttribute.NameProperty).
                    GetValue(playerAttribute, null) as string;

                levelSlots[i] = new LevelSlot()
                {
                    FactoryType = factoryType,
                    Name = name,
                    Color = settings.Colors[i],
                    Team = settings.Teams[i]
                };
            }

            // Level initialisieren
            _level.Init(settings.Seed, levelSlots);
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
            return _level?.LastException;
        }
    }
}