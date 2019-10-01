namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Base Class for all Cliff Based Map Tile States.
    /// </summary>
    public abstract class CliffMapTileState : MapTileState
    {
        /// <summary>
        ///     Reference to the related Cliff Tile.
        /// </summary>
        protected new readonly CliffMapTile MapTile;

        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public CliffMapTileState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="mapTile">Related Tile</param>
        public CliffMapTileState(CliffMapTile mapTile) : base(mapTile)
        {
            MapTile = mapTile;
        }
    }
}