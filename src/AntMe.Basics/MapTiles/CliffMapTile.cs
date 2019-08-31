namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Base Class for all Cliff Map Tiles 
    /// </summary>
    public abstract class CliffMapTile : MapTile
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public CliffMapTile(SimulationContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public override float GetHeight(Vector2 position)
        {
            return (HeightLevel + 1) * Map.LEVELHEIGHT;
        }
    }
}
