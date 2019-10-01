namespace AntMe
{
    /// <summary>
    ///     Base Class for all Map Tile Property Infos.
    /// </summary>
    public abstract class MapTileInfoProperty : InfoProperty
    {
        /// <summary>
        ///     Reference to the related Map Tile.
        /// </summary>
        protected readonly MapTile MapTile;

        /// <summary>
        ///     Reference to the related Property.
        /// </summary>
        protected new readonly MapTileProperty Property;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Related Map Tile</param>
        /// <param name="property">Related Property</param>
        /// <param name="observer">Observer</param>
        public MapTileInfoProperty(MapTile mapTile, MapTileProperty property, Item observer)
            : base(property, observer)
        {
            MapTile = mapTile;
            Property = property;
        }
    }
}