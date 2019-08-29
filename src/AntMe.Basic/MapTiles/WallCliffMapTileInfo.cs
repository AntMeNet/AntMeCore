namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Map Tile Info for the Cliff Walls.
    /// </summary>
    public class WallCliffMapTileInfo : CliffMapTileInfo
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="tile">Reference to the Tile</param>
        /// <param name="observer">Observing Item</param>
        public WallCliffMapTileInfo(WallCliffMapTile tile, Item observer) : base(tile, observer)
        {
        }
    }
}
