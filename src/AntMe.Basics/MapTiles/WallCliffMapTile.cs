using System;
using System.Collections.Generic;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Reporesents a Wall in a Cliff
    /// </summary>
    public class WallCliffMapTile : CliffMapTile
    {
        //
        // Default Position with Orientation East
        //
        // ########
        // ++++++++
        // 

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public WallCliffMapTile(SimulationContext context) : base(context) { }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        protected override byte? GetConnectionLevelSouth()
        {
            return HeightLevel;
        }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        protected override byte? GetConnectionLevelNorth()
        {
            return (byte)(HeightLevel + 1);
        }

        protected override bool OnValidateEastSide(MapTile tile, IList<Exception> exceptions)
        {
            if (!(tile is CliffMapTile))
            {
                exceptions.Add(new NotSupportedException("Map Tile must be a Cliff Tile"));
                return false;
            }
            return true;
        }

        protected override bool OnValidateWestSide(MapTile tile, IList<Exception> exceptions)
        {
            if (!(tile is CliffMapTile))
            {
                exceptions.Add(new NotSupportedException("Map Tile must be a Cliff Tile"));
                return false;
            }
            return true;
        }
    }
}
