namespace AntMe.Basics.MapTileProperties
{
    /// <summary>
    /// Property Info for walkable Tiles.
    /// </summary>
    public class WalkableTileInfoProperty : MapTileInfoProperty
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Related Map Tile</param>
        /// <param name="property">Related Property</param>
        /// <param name="observer">Observer</param>
        public WalkableTileInfoProperty(MapTile mapTile, WalkableTileProperty property, Item observer) 
            : base(mapTile, property, observer)
        {
        }
    }
}
