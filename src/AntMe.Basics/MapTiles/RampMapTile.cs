using AntMe.Basics.MapProperties;
using System;
using System.Collections.Generic;

namespace AntMe.Basics.MapTiles
{
    public class RampMapTile : MapTile, IUpdateableMapTile
    {
        public RampMapTile(byte heightLevel, bool canEnter) : base(heightLevel, canEnter)
        {
        }

        public Compass Orientation { get; set; }

        public override float GetHeight(Vector2 position)
        {
            position = new Vector2(
                Math.Max(0, Math.Min(Map.CELLSIZE, position.X)),
                Math.Max(0, Math.Min(Map.CELLSIZE, position.Y)));

            throw new NotImplementedException();
        }

        public void Update(int round, IEnumerable<Item> items)
        {
            // TODO: Just to test the update mechanism: Items slide down the ramp slowly
            throw new NotImplementedException();
        }
    }
}
