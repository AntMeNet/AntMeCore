using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Runtime
{
    public partial class TypeMapper
    {
        #region Level Properties

        private class LevelPropertiesTypeMap : StateInfoTypeMap<Func<Level, LevelProperty, LevelStateProperty>, Action>
        {
            public Func<Level, LevelProperty> CreatePropertyDelegate { get; set; }
        }

        private List<LevelPropertiesTypeMap> levelProperties = new List<LevelPropertiesTypeMap>();

        public IEnumerable<IStateInfoTypeMapperEntry> LevelProperties
        {
            get
            {
                return levelProperties;
            }
        }

        public void RegisterLevelProperty<P>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null)
            where P : LevelProperty
        {
            RegisterLevelProperty<P, LevelStateProperty>(extensionPack, name, false, createPropertyDelegate, null);
        }

        public void RegisterLevelProperty<P, S>(IExtensionPack extensionPack, string name, Func<Level, LevelProperty> createPropertyDelegate = null, Func<Level, LevelProperty, LevelStateProperty> createStatePropertyDelegate = null)
            where P : LevelProperty
            where S : LevelStateProperty
        {
            RegisterLevelProperty<P, S>(extensionPack, name, true, createPropertyDelegate, createStatePropertyDelegate);
        }

        private void RegisterLevelProperty<P, S>(IExtensionPack extensionPack, string name, bool stateSet, Func<Level, LevelProperty> createPropertyDelegate = null, Func<Level, LevelProperty, LevelStateProperty> createStatePropertyDelegate = null)
        {
            Type stateType = null;
            if (stateSet)
                stateType = typeof(S);

            levelProperties.Add(new LevelPropertiesTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(P),
                StateType = stateType,
                InfoType = null,
                CreatePropertyDelegate = createPropertyDelegate,
                CreateStateDelegate = createStatePropertyDelegate,
                CreateInfoDelegate = null,
            });
        }

        #endregion

        #region Level Extender

        private class LevelExtenderTypeMap : ExtenderTypeMap<Action<Level>> { }

        private List<LevelExtenderTypeMap> levelExtender = new List<LevelExtenderTypeMap>();

        public IEnumerable<IRankedTypeMapperEntry> LevelExtender
        {
            get
            {
                return levelExtender;
            }
        }

        public void RegisterLevelExtender<L>(IExtensionPack extensionPack, string name, int rank, Action<Level> extenderDelegate)
            where L : Level
        {
            // TODO: Check Stuff
            levelExtender.Add(new LevelExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(L),
                Rank = rank,
                ExtenderDelegate = extenderDelegate
            });
        }

        #endregion

        #region Level Resolver

        /// <summary>
        /// Resolves all Extensions and Stuff for a new Level.
        /// </summary>
        /// <param name="level">Level</param>
        public void ResolveLevel(Level level)
        {
            // Level Properties
            foreach (var item in levelProperties)
            {
                LevelProperty property = null;
                if (item.CreatePropertyDelegate != null)
                    property = item.CreatePropertyDelegate(level);
                else
                {
                    property = Activator.CreateInstance(item.Type, level) as LevelProperty;
                    if (property == null)
                        throw new Exception("Not able to create a Level Property");
                }

                if (property != null)
                    level.AddProperty(property);
            }

            // TODO: Level-Vererbung auflösen
            // Level Extender
            foreach (var extender in levelExtender.Where(e => e.Type == level.GetType()).OrderBy(e => e.Rank))
            {
                extender.ExtenderDelegate(level);
            }
        }

        /// <summary>
        /// Generates a State for the given Level.
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns>New State</returns>
        public LevelState CreateLevelState(Level level)
        {
            if (level == null)
                throw new ArgumentNullException("level");

            LevelState state = new LevelState();

            // State Properties auffüllen
            foreach (var property in level.Properties)
            {
                var map = levelProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (map == null)
                    throw new NotSupportedException("Property is not registered.");

                LevelStateProperty prop = null;
                if (map.CreateStateDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = map.CreateStateDelegate(level, property);

                    if (prop != null)
                    {
                        if (prop.GetType() != map.StateType)
                        {
                            // TODO: Trace
                            throw new NotSupportedException("Delegate returned a wrong Property Type");
                        }

                        state.AddProperty(prop);
                    }
                }
                else if (map.StateType != null)
                {
                    // Option 2: Dynamische Erzeugung
                    prop = Activator.CreateInstance(map.StateType, level, property) as LevelStateProperty;
                    if (prop == null)
                    {
                        // TODO: Trace
                        throw new Exception("Could not create State Property");
                    }
                    state.AddProperty(prop);
                }
            }

            return state;
        }

        #endregion
    }
}
