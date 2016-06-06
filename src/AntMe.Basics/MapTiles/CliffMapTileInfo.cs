using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.MapTiles
{
    public abstract class CliffMapTileInfo : MapTileInfo
    {
        public CliffMapTileInfo(Level level, MapTile tile, Item observer) : base(level, tile, observer)
        {
        }
    }
}
