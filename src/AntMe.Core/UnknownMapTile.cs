namespace AntMe
{
    /// <summary>
    /// Placeholder Class for Unknown Map Tiles.
    /// </summary>
    public sealed class UnknownMapTile : MapTile
    {
        /// <summary>
        /// Name of the Map Tile.
        /// </summary>
        public string MapTileType { get; private set; }

        /// <summary>
        /// Returns the Payload for the given Map Tile.
        /// </summary>
        public byte[] Payload { get; private set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="mapTileType">Name of the original Map Tile Type</param>
        /// <param name="payload">Payload for the Deserializer</param>
        internal UnknownMapTile(SimulationContext context, string mapTileType, byte[] payload) : base(context)
        {
            MapTileType = mapTileType;
            Payload = payload;
        }

        public override byte? EnterLevelEast { get { return null; } }

        public override byte? EnterLevelNorth { get { return null; } }

        public override byte? EnterLevelSouth { get { return null; } }

        public override byte? EnterLevelWest { get { return null; } }

        public override float GetHeight(Vector2 position)
        {
            return 0f;
        }
    }
}
