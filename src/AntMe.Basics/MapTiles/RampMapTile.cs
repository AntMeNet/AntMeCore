using AntMe.Basics.MapProperties;
using System;
using System.Collections.Generic;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Represents a Ramp through a Cliff.
    /// </summary>
    public class RampMapTile : MapTile, IUpdateableMapTile
    {
        //
        // Default Position with Orientation East
        //
        // #    #
        // #    #
        // +    +
        // +    +
        //

        /// <summary>
        /// Default Constrcutor.
        /// </summary>
        public RampMapTile(SimulationContext context) : base(context)
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
                    case Compass.East: return null;
                    case Compass.South: return HeightLevel;
                    case Compass.West: return null;
                    case Compass.North: return (byte)(HeightLevel + 1);
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
                    case Compass.East: return (byte)(HeightLevel + 1);
                    case Compass.South: return null;
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
                    case Compass.South: return (byte)(HeightLevel + 1);
                    case Compass.West: return null;
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
                    case Compass.West: return (byte)(HeightLevel + 1);
                    case Compass.North: return null;
                    default: throw new NotSupportedException("Wrong Orientation");
                }
            }
        }

        /// <summary>
        /// Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public override float GetHeight(Vector2 position)
        {
            position = new Vector2(
                Math.Max(0, Math.Min(Map.CELLSIZE, position.X)),
                Math.Max(0, Math.Min(Map.CELLSIZE, position.Y)));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets called in every Round to update Items in this Map Tile.
        /// </summary>
        /// <param name="round">Current Round</param>
        /// <param name="items">Included Items</param>
        public void Update(int round, IEnumerable<Item> items)
        {
            // TODO: Just to test the update mechanism: Items slide down the ramp slowly
            throw new NotImplementedException();
        }
    }
}
