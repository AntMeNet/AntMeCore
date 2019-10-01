using System;
using System.Collections.Generic;
using AntMe.Basics.MapProperties;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Represents a Ramp through a Cliff.
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
        ///     Default Constrcutor.
        /// </summary>
        public RampMapTile(SimulationContext context) : base(context)
        {
        }

        /// <summary>
        ///     Gets called in every Round to update Items in this Map Tile.
        /// </summary>
        /// <param name="round">Current Round</param>
        /// <param name="items">Included Items</param>
        public void Update(int round, IEnumerable<Item> items)
        {
            // TODO: Just to test the update mechanism: Items slide down the ramp slowly
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns the Level to enter on the South Side.
        /// </summary>
        protected override byte? GetConnectionLevelSouth()
        {
            return HeightLevel;
        }

        /// <summary>
        ///     Returns the Level to enter on the North Side.
        /// </summary>
        protected override byte? GetConnectionLevelNorth()
        {
            return (byte) (HeightLevel + 1);
        }

        protected override bool OnValidateEastSide(MapTile tile, IList<Exception> exceptions)
        {
            if (!(tile is RampMapTile) && !(tile is RightRampToCliffMapTile))
            {
                exceptions.Add(new NotSupportedException("Map Tile must be a Ramp or Right Cliff Tile"));
                return false;
            }

            return true;
        }

        protected override bool OnValidateWestSide(MapTile tile, IList<Exception> exceptions)
        {
            if (!(tile is RampMapTile) && !(tile is LeftRampToCliffMapTile))
            {
                exceptions.Add(new NotSupportedException("Map Tile must be a Ramp or Left Cliff Tile"));
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Returns the Height at the given Position.
        /// </summary>
        /// <param name="position">relative Position</param>
        /// <returns>Map Height</returns>
        public override float GetHeight(Vector2 position)
        {
            // Clamp Position to the Cell Boarders
            position = new Vector2(
                Math.Max(0, Math.Min(Map.CELLSIZE, position.X)),
                Math.Max(0, Math.Min(Map.CELLSIZE, position.Y)));

            // Normalize to [0..1(
            position /= Map.CELLSIZE;

            // Calculation
            switch (Orientation)
            {
                case MapTileOrientation.NotRotated: return (1f - position.Y + HeightLevel) * Map.LEVELHEIGHT;
                case MapTileOrientation.RotBy90Degrees: return (position.Y + HeightLevel) * Map.LEVELHEIGHT;
                case MapTileOrientation.RotBy180Degrees: return (position.X + HeightLevel) * Map.LEVELHEIGHT;
                case MapTileOrientation.RotBy270Degrees: return (1f - position.X + HeightLevel) * Map.LEVELHEIGHT;
                default: throw new NotSupportedException("Unsupported Map Tile Orientation");
            }
        }
    }
}