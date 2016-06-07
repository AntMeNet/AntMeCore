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
        public FlatMapTile() : base(true)
        {
        }

        /// <summary>
        /// Returns the Level to enter on the East Side.
        /// </summary>
        public override byte? EnterLevelEast
        {
            get
            {
                return HeightLevel;
            }
        }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        public override byte? EnterLevelNorth
        {
            get
            {
                return HeightLevel;
            }
        }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        public override byte? EnterLevelSouth
        {
            get
            {
                return HeightLevel;
            }
        }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        public override byte? EnterLevelWest
        {
            get
            {
                return HeightLevel;
            }
        }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public override float GetHeight(Vector2 position)
        {
            return HeightLevel * Map.LEVELHEIGHT;
        }
    }
}
