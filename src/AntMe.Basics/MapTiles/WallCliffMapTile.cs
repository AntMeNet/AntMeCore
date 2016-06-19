using System;

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
        /// Returns the Level to enter on the East Side.
        /// </summary>
        public override byte? ConnectionLevelEast
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return null;
                    case Compass.South: return (byte)(HeightLevel + 1);
                    case Compass.West: return null;
                    case Compass.North: return HeightLevel;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Level to enter on the South Side.
        /// </summary>
        public override byte? ConnectionLevelSouth
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return HeightLevel;
                    case Compass.South: return null;
                    case Compass.West: return (byte)(HeightLevel + 1);
                    case Compass.North: return null;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Level to enter on the West Side.
        /// </summary>
        public override byte? ConnectionLevelWest
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return null;
                    case Compass.South: return HeightLevel;
                    case Compass.West: return null;
                    case Compass.North: return (byte)(HeightLevel + 1);
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Level to enter on the North Side.
        /// </summary>
        public override byte? ConnectionLevelNorth
        {
            get
            {
                switch (Orientation)
                {
                    case Compass.East: return (byte)(HeightLevel + 1);
                    case Compass.South: return null;
                    case Compass.West: return HeightLevel;
                    case Compass.North: return null;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }
    }
}
