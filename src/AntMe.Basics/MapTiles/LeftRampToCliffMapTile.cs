using System;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    /// Represents the part beween a Cliff and a Ramp (Left Side).
    /// </summary>
    public class LeftRampToCliffMapTile : WallCliffMapTile
    {
        public LeftRampToCliffMapTile(SimulationContext context) : base(context) { }

        protected override void OnValidateEastSide(MapTile tile)
        {
            if (!(tile is RampMapTile))
                throw new NotSupportedException("Map Tile must be a Ramp Tile");
        }

        protected override void OnValidateWestSide(MapTile tile)
        {
            if (!(tile is CliffMapTile))
                throw new NotSupportedException("Map Tile must be a Cliff Tile");
        }
    }
}
