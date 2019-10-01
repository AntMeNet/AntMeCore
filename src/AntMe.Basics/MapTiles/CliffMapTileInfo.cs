namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Base Class for all Cliff based Map Tile Infos.
    /// </summary>
    public abstract class CliffMapTileInfo : MapTileInfo
    {
        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="tile">Reference to the Tile</param>
        /// <param name="observer">Observing Item</param>
        public CliffMapTileInfo(CliffMapTile tile, Item observer) : base(tile, observer)
        {
        }
    }
}