using System;
using System.Collections.Generic;
using System.Linq;

namespace AntMe.Runtime
{
    public partial class TypeMapper
    {
        #region Map Properties

        private class MapPropertiesTypeMap : StateInfoTypeMap<Func<Map, MapProperty, MapStateProperty>, Action>
        {
            public Func<Map, MapProperty> CreatePropertyDelegate { get; set; }
        }

        private List<MapPropertiesTypeMap> mapProperties = new List<MapPropertiesTypeMap>();

        /// <summary>
        /// Registers additional Map Properties.
        /// </summary>
        /// <typeparam name="T">Type of Map Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        public void RegisterMapProperty<T>(IExtensionPack extensionPack, string name,
            Func<Map, T> createPropertyDelegate = null)
            where T : MapProperty
        {
            RegisterMapProperty<T, MapStateProperty>(extensionPack, name, false, createPropertyDelegate, null);
        }

        /// <summary>
        /// Registers additional Map Properties.
        /// </summary>
        /// <typeparam name="T">Type of Map Property</typeparam>
        /// <typeparam name="S">Type of State for the Map Property</typeparam>
        /// <param name="extensionPack">Reference to the Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="createPropertyDelegate">Optional Delegate to create the Property</param>
        /// <param name="createStateDelegate">Optional Delegate to create State Property</param>
        public void RegisterMapProperty<T, S>(IExtensionPack extensionPack, string name,
            Func<Map, T> createPropertyDelegate = null,
            Func<Map, MapProperty, S> createStateDelegate = null)
            where T : MapProperty
            where S : MapStateProperty
        {
            RegisterMapProperty<T, S>(extensionPack, name, true, createPropertyDelegate, createStateDelegate);
        }

        /// <summary>
        /// Registers additional Map Properties.
        /// </summary>
        /// <typeparam name="T">Type of Map Property</typeparam>
        /// <typeparam name="S">Type of State for the Map Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="stateSet"></param>
        /// <param name="createPropertyDelegate"></param>
        /// <param name="createStateDelegate"></param>
        private void RegisterMapProperty<T, S>(IExtensionPack extensionPack, string name, bool stateSet,
            Func<Map, T> createPropertyDelegate = null,
            Func<Map, MapProperty, S> createStateDelegate = null)
            where T : MapProperty
            where S : MapStateProperty
        {
            ValidateDefaults(extensionPack, name);

            // Handle Property Type
            Type type = typeof(T);
            ValidateType<MapProperty>(type, new Type[] { typeof(SimulationContext), typeof(Map) }, false);

            // Registration Collision
            if (mapProperties.Any(p => p.Type == type))
                throw new NotSupportedException("Property is already registered");

            // Handle State Type
            Type stateType = null;
            if (stateSet)
            {
                stateType = typeof(S);
                ValidateType<MapStateProperty>(stateType, new Type[] { typeof(Map), typeof(T) }, true);
            }

            mapProperties.Add(new MapPropertiesTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                StateType = stateType,
                CreatePropertyDelegate = createPropertyDelegate,
                CreateStateDelegate = createStateDelegate,
            });
        }

        /// <summary>
        /// List all Map Properties.
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> MapProperties { get { return mapProperties; } }

        #endregion

        #region Map Extender

        private class MapExtenderTypeMap : ExtenderTypeMap<Action<Map>> { }

        private List<MapExtenderTypeMap> mapExtender = new List<MapExtenderTypeMap>();

        /// <summary>
        /// Registeres a Delegate to extend a Map.
        /// </summary>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="priority">Priority</param>
        /// <param name="extenderDelegate">Extender Delegate</param>
        public void RegisterMapExtender(IExtensionPack extensionPack, string name, Action<Map> extenderDelegate, int priority)
        {
            ValidateDefaults(extensionPack, name);

            // Kein Delegat angegeben
            if (extenderDelegate == null)
                throw new ArgumentNullException("extenderDelegate");

            mapExtender.Add(new MapExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(Map),
                Rank = priority,
                ExtenderDelegate = extenderDelegate
            });
        }

        /// <summary>
        /// List of all Map Extender.
        /// </summary>
        public IEnumerable<IRankedTypeMapperEntry> MapExtender { get { return mapExtender.OrderBy(g => g.Rank); } }

        #endregion

        #region Map Material

        private List<TypeMap> mapMaterials = new List<TypeMap>();

        /// <summary>
        /// Registers a new Material.
        /// </summary>
        /// <typeparam name="T">Material Type</typeparam>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Material Name</param>
        public void RegisterMapMaterial<T>(IExtensionPack extensionPack, string name)
            where T : MapMaterial
        {
            ValidateDefaults(extensionPack, name);

            Type t = typeof(T);
            ValidateType<MapMaterial>(t, new Type[] { typeof(SimulationContext) }, false);

            mapMaterials.Add(new TypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = t
            });
        }

        /// <summary>
        /// List of all available Materials.
        /// </summary>
        public IEnumerable<ITypeMapperEntry> MapMaterials { get { return mapMaterials; } }

        #endregion

        #region Map Tiles

        private class MapTileTypeMap : StateInfoTypeMap<Func<MapTile, MapTileState>, Func<MapTile, Item, MapTileInfo>> { }

        private List<MapTileTypeMap> mapTiles = new List<MapTileTypeMap>();

        /// <summary>
        /// Registeres a Map Tile.
        /// </summary>
        /// <typeparam name="T">Map Tile Type</typeparam>
        /// <typeparam name="S">Map Tile State Type</typeparam>
        /// <typeparam name="I">Map Tile Info Type</typeparam>
        /// <param name="extensionPack">Extension Pack</param>
        /// <param name="name">Name</param>
        /// <param name="createStateDelegate">Optional Create State Delegate</param>
        /// <param name="createInfoDelegate">Optional Create Info Delegate</param>
        public void RegisterMapTile<T, S, I>(IExtensionPack extensionPack, string name,
            Func<MapTile, S> createStateDelegate = null,
            Func<MapTile, Item, I> createInfoDelegate = null)
            where T : MapTile
            where S : MapTileState
            where I : MapTileInfo
        {
            ValidateDefaults(extensionPack, name);

            Type t = typeof(T);
            ValidateType<MapTile>(t, new Type[] { typeof(SimulationContext) }, false);

            Type stateType = typeof(S);
            ValidateType<MapTileState>(stateType, new Type[] { typeof(T) }, true);

            Type infoType = typeof(I);
            ValidateType<MapTileInfo>(infoType, new Type[] { typeof(T), typeof(Item) }, false);

            mapTiles.Add(new MapTileTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = t,
                StateType = stateType,
                InfoType = infoType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });
        }

        /// <summary>
        /// List of all Map Tiles
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> MapTiles { get { return mapTiles; } }

        #endregion

        #region Map Tile Properties

        private class MapTilePropertyTypeMap : StateInfoTypeMap<Func<MapTile, MapTileProperty, MapTileStateProperty>, Func<MapTile, MapTileProperty, Item, MapTileInfoProperty>> { }

        private List<MapTilePropertyTypeMap> mapTileProperties = new List<MapTilePropertyTypeMap>();

        public void RegisterMapTileProperty<T>(IExtensionPack extensionPack, string name)
            where T : MapTileProperty
        {
            RegisterMapTileProperty<T, MapTileStateProperty, MapTileInfoProperty>(extensionPack, name, false, false, null, null);
        }

        public void RegisterMapTilePropertyS<T, S>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, S> createStateDelegate = null)
            where T : MapTileProperty
            where S : MapTileStateProperty
        {
            RegisterMapTileProperty<T, S, MapTileInfoProperty>(extensionPack, name, true, false, createStateDelegate, null);
        }

        public void RegisterMapTilePropertyI<T, I>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, Item, I> createInfoDelegate = null)
            where T : MapTileProperty
            where I : MapTileInfoProperty
        {
            RegisterMapTileProperty<T, MapTileStateProperty, I>(extensionPack, name, false, true, null, createInfoDelegate);
        }

        public void RegisterMapTilePropertySI<T, S, I>(IExtensionPack extensionPack, string name,
            Func<MapTile, MapTileProperty, S> createStateDelegate = null,
            Func<MapTile, MapTileProperty, Item, I> createInfoDelegate = null)
            where T : MapTileProperty
            where S : MapTileStateProperty
            where I : MapTileInfoProperty
        {
            RegisterMapTileProperty<T, S, I>(extensionPack, name, true, true, createStateDelegate, createInfoDelegate);
        }

        private void RegisterMapTileProperty<T, S, I>(IExtensionPack extensionPack, string name,
            bool stateSet, bool infoSet,
            Func<MapTile, MapTileProperty, S> createStateDelegate = null,
            Func<MapTile, MapTileProperty, Item, I> createInfoDelegate = null)
            where T : MapTileProperty
            where S : MapTileStateProperty
            where I : MapTileInfoProperty
        {
            ValidateDefaults(extensionPack, name);

            // Handle Property Type
            Type type = typeof(T);
            ValidateType<MapTileProperty>(type, new Type[] { typeof(SimulationContext), typeof(MapTile) }, false);

            // Registration Collision
            if (mapTileProperties.Any(p => p.Type == type))
                throw new NotSupportedException("Property is already registered");


            // Handle State Type
            Type stateType = null;
            if (stateSet)
            {
                stateType = typeof(S);
                ValidateType<MapTileStateProperty>(stateType, new Type[] { typeof(MapTile), typeof(T) }, true);
            }

            // Handle Info Type
            Type infoType = null;
            if (infoSet)
            {
                infoType = typeof(I);
                ValidateType<MapTileInfoProperty>(infoType, new Type[] { typeof(MapTile), typeof(T), typeof(Item) });
            }

            mapTileProperties.Add(new MapTilePropertyTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                StateType = stateType,
                InfoType = infoType,
                CreateStateDelegate = createStateDelegate,
                CreateInfoDelegate = createInfoDelegate
            });
        }

        /// <summary>
        /// List of all Map Tile Properties.
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> MapTileProperties { get { return mapTileProperties; } }

        #endregion

        #region Map Tile Attachments

        private class MapTileAttachmentTypeMap : AttachmentTypeMap<Func<MapTile, MapTileProperty>> { }

        private List<MapTileAttachmentTypeMap> mapTileAttachments = new List<MapTileAttachmentTypeMap>();

        public void AttachMapTileProperty<I, P>(IExtensionPack extensionPack, string name, Func<MapTile, P> createPropertyDelegate = null)
            where I : MapTile
            where P : MapTileProperty
        {
            ValidateDefaults(extensionPack, name);

            if (!mapTileProperties.Any(c => c.Type == typeof(P)))
                throw new ArgumentException("Property is not registered");

            if (mapTileAttachments.Any(c => c.Type == typeof(I) && c.AttachmentType == typeof(P)))
            {
                // TODO: Tracer
                string msg = string.Format("Item Property Combination '{0}'/'{1}' is already reagistered", typeof(I).FullName, typeof(P).FullName);
                throw new NotSupportedException(msg);
            }

            mapTileAttachments.Add(new MapTileAttachmentTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(I),
                AttachmentType = typeof(P),
                CreateDelegate = createPropertyDelegate
            });
        }

        public IEnumerable<IAttachmentTypeMapperEntry> MapTileAttachments { get { return mapTileAttachments; } }

        #endregion

        #region Map Tile Extender

        private class MapTileExtenderTypeMap : ExtenderTypeMap<Action<MapTile>> { }

        private List<MapTileExtenderTypeMap> mapTileExtender = new List<MapTileExtenderTypeMap>();

        public void RegisterMapTileExtender<T>(IExtensionPack extensionPack, string name, Action<MapTile> extenderDelegate, int priority)
            where T : MapTile
        {
            // Kein Name angegeben
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            // Kein Delegat angegeben
            if (extenderDelegate == null)
                throw new ArgumentNullException("extenderDelegate");

            mapTileExtender.Add(new MapTileExtenderTypeMap()
            {
                ExtensionPack = extensionPack,
                Name = name,
                Type = typeof(T),
                Rank = priority,
                ExtenderDelegate = extenderDelegate
            });
        }

        public IEnumerable<IRankedTypeMapperEntry> MapTileExtender { get { return mapTileExtender; } }


        #endregion

        #region Map Resolver

        /// <summary>
        /// Resolves the given Map.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="map">Map</param>
        public void ResolveMap(SimulationContext context, Map map)
        {
            if (map == null)
                throw new ArgumentNullException("Map");

            // Attachements
            foreach (var mapProperty in mapProperties)
            {
                if (mapProperty.CreatePropertyDelegate != null)
                {
                    MapProperty property = mapProperty.CreatePropertyDelegate(map);
                    if (property != null)
                    {
                        if (property.GetType() != mapProperty.Type)
                        {
                            // TODO: Trace
                            throw new NotSupportedException("Delegate returned wrong Property Type");
                        }
                        map.AddProperty(property);
                    }
                }
                else
                {
                    map.AddProperty(Activator.CreateInstance(mapProperty.Type, context, map) as MapProperty);
                }
            }

            // Extender
            foreach (var extender in mapExtender.OrderBy(c => c.Rank))
            {
                extender.ExtenderDelegate(map);
            }
        }

        public void ResolveMapTile(SimulationContext context, MapTile tile)
        {
            if (tile == null)
                throw new ArgumentNullException("tile");

            if (!MapTiles.Any(c => c.Type == tile.GetType()))
                throw new NotSupportedException("Item is not registered.");

            // Type Order
            List<Type> types = new List<Type>();
            Type current = tile.GetType();
            types.Add(current);
            while (current != typeof(MapTile))
            {
                current = current.BaseType;
                types.Add(current);
            }
            Type[] itemTypes = types.ToArray();
            Array.Reverse(itemTypes);

            // Attachements
            foreach (var type in itemTypes)
            {
                foreach (var attachment in mapTileAttachments.Where(c => c.Type == type))
                {
                    // Skip if allready available
                    if (tile.Properties.Any(p => p.GetType() == attachment.AttachmentType))
                        continue;

                    if (attachment.CreateDelegate != null)
                    {
                        MapTileProperty property = attachment.CreateDelegate(tile);
                        if (property != null)
                        {
                            if (property.GetType() != attachment.AttachmentType)
                                throw new NotSupportedException("Delegate returned wrong Property Type");
                            tile.AddProperty(property);
                        }
                    }
                    else
                    {
                        tile.AddProperty(Activator.CreateInstance(attachment.AttachmentType, context, tile) as MapTileProperty);
                    }
                }
            }

            // Extender
            foreach (var extender in mapTileExtender.Where(c => itemTypes.Contains(c.Type)).OrderBy(c => c.Rank))
            {
                extender.ExtenderDelegate(tile);
            }
        }

        /// <summary>
        /// Generates a Map State for the given Map.
        /// </summary>
        /// <param name="map">Map</param>
        /// <returns>Map State</returns>
        public MapState CreateMapState(Map map)
        {
            if (map == null)
                throw new ArgumentNullException("Map");

            MapState state = new MapState(map);

            // State Properties auffüllen
            foreach (var property in map.Properties)
            {
                var mapping = mapProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (mapping == null)
                    throw new NotSupportedException("Property is not registered.");

                MapStateProperty prop = null;
                if (mapping.CreateStateDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = mapping.CreateStateDelegate(map, property);

                    if (prop != null)
                    {
                        if (prop.GetType() != mapping.StateType)
                        {
                            // TODO: Trace
                            throw new NotSupportedException("Delegate returned a wrong Property Type");
                        }

                        state.AddProperty(prop);
                    }
                }
                else if (mapping.StateType != null)
                {
                    // Option 2: Dynamische Erzeugung
                    prop = Activator.CreateInstance(mapping.StateType, map, property) as MapStateProperty;
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

        public MapTileState CreateMapTileState(MapTile tile)
        {
            if (tile == null)
                throw new ArgumentNullException("tile");

            // Identify State Type
            var tileMapping = mapTiles.FirstOrDefault(m => m.Type == tile.GetType());
            if (tileMapping == null)
                throw new ArgumentException("Tile is not registered.");

            MapTileState state;
            if (tileMapping.CreateStateDelegate != null)
            {
                // Option 1: There is a Create State Delegate registered.
                state = tileMapping.CreateStateDelegate(tile);
                if (state == null || state.GetType() != tileMapping.StateType)
                    throw new NotSupportedException("Delegate returns a wrong Type");
            }
            else
            {
                // Option 2: Create State by Type
                state = Activator.CreateInstance(tileMapping.StateType, tile) as MapTileState;
            }

            // Handle Material
            state.Material = tile.Material;

            // State Properties
            foreach (var property in tile.Properties)
            {
                var propertyMapping = mapTileProperties.FirstOrDefault(p => p.Type == property.GetType());
                if (propertyMapping == null)
                    throw new NotSupportedException("Property is not registered.");

                MapTileStateProperty prop = null;
                if (propertyMapping.CreateStateDelegate != null)
                {
                    // Option 1: Create Delegate
                    prop = propertyMapping.CreateStateDelegate(tile, property);
                    if (prop != null)
                    {
                        if (prop.GetType() != propertyMapping.StateType)
                        {
                            // TODO: Trace
                            throw new NotSupportedException("Delegate returned a wrong Property Type");
                        }

                        state.AddProperty(prop);
                    }
                }
                else if (propertyMapping.StateType != null)
                {
                    // Option 2: Dynamische Erzeugung
                    prop = Activator.CreateInstance(propertyMapping.StateType, tile, property) as MapTileStateProperty;
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

        public MapTileInfo CreateMapTileInfo(MapTile tile, Item observer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
