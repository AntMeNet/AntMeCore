namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Map Tile Info for flat Tiles.
    /// </summary>
    public class FlatMapTileInfo : MapTileInfo
    {
        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="tile">Reference to the Tile</param>
        /// <param name="observer">Observing Item</param>
        public FlatMapTileInfo(MapTile tile, Item observer) : base(tile, observer)
        {
        }
    }
}