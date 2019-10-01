namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Concave Map Tile Info.
    /// </summary>
    public class ConcaveCliffMapTileInfo : CliffMapTileInfo
    {
        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="tile">Reference to the Tile</param>
        /// <param name="observer">Observing Item</param>
        public ConcaveCliffMapTileInfo(ConcaveCliffMapTile tile, Item observer) : base(tile, observer)
        {
        }
    }
}