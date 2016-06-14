using System;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Map Tile for the convex Part of the Cliff.
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
        /// Default Constrcutor.
        /// </summary>
        public ConvexCliffMapTile(SimulationContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns the Level to enter on the East Side.
        /// </summary>
        public override byte? EnterLevelEast
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return HeightLevel;
                    case Compass.South: return HeightLevel;
                    case Compass.West: return null;
                    case Compass.North: return null;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        public override byte? EnterLevelSouth
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return null;
                    case Compass.South: return HeightLevel;
                    case Compass.West: return HeightLevel;
                    case Compass.North: return null;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        public override byte? EnterLevelWest
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return null;
                    case Compass.South: return null;
                    case Compass.West: return HeightLevel;
                    case Compass.North: return HeightLevel;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        public override byte? EnterLevelNorth
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return HeightLevel;
                    case Compass.South: return null;
                    case Compass.West: return null;
                    case Compass.North: return HeightLevel;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }
    }
}
