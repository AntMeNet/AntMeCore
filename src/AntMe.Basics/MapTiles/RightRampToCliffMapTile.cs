using System;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Represents the part beween a Cliff and a Ramp (Right Side).
    /// </summary>
    public class RightRampToCliffMapTile : WallCliffMapTile
    {
        public RightRampToCliffMapTile(SimulationContext context) : base(context) { }

        protected override void OnValidateEastSide(MapTile tile)
        {
            if (!(tile is CliffMapTile))
                throw new NotSupportedException("Map Tile must be a Cliff Tile");
        }

        protected override void OnValidateWestSide(MapTile tile)
        {
            if (!(tile is RampMapTile))
                throw new NotSupportedException("Map Tile must be a Ramp Tile");
        }
    }
}
