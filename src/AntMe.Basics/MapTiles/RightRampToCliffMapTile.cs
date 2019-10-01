using System;
using System.Collections.Generic;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Represents the part beween a Cliff and a Ramp (Right Side).
    /// </summary>
    public class RightRampToCliffMapTile : WallCliffMapTile
    {
        public RightRampToCliffMapTile(SimulationContext context) : base(context)
        {
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
            if (!(tile is RampMapTile))
            {
                exceptions.Add(new NotSupportedException("Map Tile must be a Ramp Tile"));
                return false;
            }

            return true;
        }
    }
}