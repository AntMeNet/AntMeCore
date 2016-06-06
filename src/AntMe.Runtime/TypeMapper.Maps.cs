using System;
using System.Collections.Generic;

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
        public void RegisterMapProperty<T>(IExtensionPack extensionPack, string name, Func<Level, T> createPropertyDelegate = null)
            where T : MapProperty
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers additional Map Properties.
        /// </summary>
        /// <typeparam name="T">Type of Map Property</typeparam>
        /// <typeparam name="S">Type of State for the Map Property</typeparam>
        /// <param name="extensionPack"></param>
        /// <param name="name"></param>
        /// <param name="createPropertyDelegate"></param>
        /// <param name="createStateDelegate"></param>
        public void RegisterMapProperty<T, S>(IExtensionPack extensionPack, string name, Func<Level, T> createPropertyDelegate = null, Func<Map, S> createStateDelegate = null)
            where T : MapProperty
            where S : MapStateProperty
        {
            throw new NotImplementedException();
        }

        private void RegisterMapProperty<T, S>(IExtensionPack extensionPack, string name, bool stateSet)
        {

        }

        /// <summary>
        /// List all Map Properties.
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> MapProperties { get { return mapProperties; } }

        #endregion

        #region Map Extender

        #endregion

        #region Map Tiles

        public void RegisterMapTile<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Item, S> createStateDelegate = null,
            Func<Item, Item, I> createInfoDelegate = null)
            where T : MapTile
            where S : MapTileState
            where I : MapTileInfo
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// List of all Map Tiles
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> MapTiles { get { throw new NotImplementedException(); } }

        #endregion

        #region Map Tile Properties

        public void RegisterMapTileProperty<T, S, I>(IExtensionPack extensionPack, string name,
            Func<Item, S> createStateDelegate = null,
            Func<Item, Item, I> createInfoDelegate = null)
            where T : MapTile
            where S : MapTileState
            where I : MapTileInfo
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// List of all Map Tile Properties.
        /// </summary>
        public IEnumerable<IStateInfoTypeMapperEntry> MapTileProperties { get { throw new NotImplementedException(); } }

        #endregion

        #region Map Tile Extender

        #endregion

        #region Map Resolver

        public void ResolveMap(Map map)
        {
            throw new NotImplementedException();
        }

        public MapState CreateMapState(Map map)
        {
            throw new NotImplementedException();
        }

        public MapTileInfo CreateMapTileInfo(MapTile tile, Item observer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
