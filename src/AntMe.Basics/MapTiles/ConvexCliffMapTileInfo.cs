namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Map Tile Info for the Convex Cliff.
    /// </summary>
    public class ConvexCliffMapTileInfo : CliffMapTileInfo
    {
        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="tile">Reference to the Tile</param>
        /// <param name="observer">Observing Item</param>
        public ConvexCliffMapTileInfo(ConvexCliffMapTile tile, Item observer) : base(tile, observer)
        {
        }
    }
}