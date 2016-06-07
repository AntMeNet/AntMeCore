namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Map Tile Info for Ramps.
    /// </summary>
    public class RampMapTileInfo : MapTileInfo
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Reference to the Level</param>
        /// <param name="tile">Reference to the Tile</param>
        /// <param name="observer">Observing Item</param>
        public RampMapTileInfo(Level level, MapTile tile, Item observer) : base(level, tile, observer)
        {
        }
    }
}
