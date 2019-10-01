namespace AntMe
{
    /// <summary>
    ///     Base Class for all Map Tile Infos.
    /// </summary>
    public abstract class MapTileInfo : PropertyList<MapTileInfoProperty>
    {
        /// <summary>
        ///     Reference to the related Map Tile.
        /// </summary>
        protected readonly MapTile MapTile;

        /// <summary>
        ///     Reference to the related Observer.
        /// </summary>
        protected readonly Item Observer;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Map Tile</param>
        /// <param name="observer">Observer</param>
        public MapTileInfo(MapTile mapTile, Item observer)
        {
            MapTile = mapTile;
            Observer = observer;
        }
    }
}