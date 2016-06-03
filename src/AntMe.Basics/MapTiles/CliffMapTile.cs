using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.MapTiles
{
    public abstract class CliffMapTile : MapTile
    {
        public CliffMapTile(byte heightLevel) : base(heightLevel, false)
        {
        }

        public Compass Orientation { get; set; }

        public override float GetHeight(Vector2 position)
        {
            return (HeightLevel + 1) * Map.LEVELHEIGHT;
        }
    }
}
