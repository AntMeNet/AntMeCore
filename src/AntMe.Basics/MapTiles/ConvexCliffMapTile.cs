using System;
using System.Collections.Generic;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Map Tile for the convex Part of the Cliff.
    /// </summary>
    public class ConvexCliffMapTile : CliffMapTile
    {
        //
        // Default Position with Orientation East
        //
        //    #+
        //    #+
        // ####+
        // +++++
        //

        /// <summary>
        ///     Default Constrcutor.
        /// </summary>
        public ConvexCliffMapTile(SimulationContext context) : base(context)
        {
        }

        /// <summary>
        ///     Returns the Level to enter on the East Side.
        /// </summary>
        protected override byte? GetConnectionLevelEast()
        {
            return HeightLevel;
        }

        /// <summary>
        ///     Returns the Level to enter on the South Side.
        /// </summary>
        protected override byte? GetConnectionLevelSouth()
        {
            return HeightLevel;
        }

        protected override bool OnValidateNorthSide(MapTile tile, IList<Exception> exceptions)
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