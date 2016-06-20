using System;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Map Tile for the concave Part of the Cliff.
    /// </summary>
    public class ConcaveCliffMapTile : CliffMapTile
    {
        //
        // Default Position with Orientation East
        //
        // #####
        // #++++
        // #+
        // #+
        //

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ConcaveCliffMapTile(SimulationContext context) : base(context) { }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        protected override byte? GetConnectionLevelWest()
        {
            return (byte)(HeightLevel + 1);
        }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        protected override byte? GetConnectionLevelNorth()
        {
            return (byte)(HeightLevel + 1);
        }
    }
}
