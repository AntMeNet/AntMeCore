using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.MapProperties
{
    public class UpdateTileProperty : MapProperty
    {
        /// <summary>
        /// List of all updateable Map Tiles.
        /// </summary>
        private HashSet<IUpdateableMapTile> updateableMapTiles;

    }
}
