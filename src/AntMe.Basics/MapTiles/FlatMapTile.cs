namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Represents a flat Map Tile.
    /// </summary>
    public class FlatMapTile : MapTile
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public FlatMapTile(SimulationContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns the Level to enter on the East Side.
        /// </summary>
        protected override byte? GetConnectionLevelEast()
        {
            return HeightLevel;
        }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        protected override byte? GetConnectionLevelNorth()
        {
            return HeightLevel;
        }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        protected override byte? GetConnectionLevelSouth()
        {
            return HeightLevel;
        }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        protected override byte? GetConnectionLevelWest()
        {
            return HeightLevel;

        }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public override float GetHeight(Vector2 position)
        {
            return HeightLevel * Map.Levelheight;
        }
    }
}
