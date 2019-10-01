namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Map Tile State for the concave Cliff.
    /// </summary>
    public class ConcaveCliffMapTileState : CliffMapTileState
    {
        public ConcaveCliffMapTileState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Related Tile</param>
        public ConcaveCliffMapTileState(ConcaveCliffMapTile mapTile) : base(mapTile)
        {
        }
    }
}