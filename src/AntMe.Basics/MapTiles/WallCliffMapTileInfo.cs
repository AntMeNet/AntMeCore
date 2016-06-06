using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.MapTiles
{
    public class WallCliffMapTileInfo : CliffMapTileInfo
    {
        public WallCliffMapTileInfo(Level level, MapTile tile, Item observer) : base(level, tile, observer)
        {
        }
    }
}
