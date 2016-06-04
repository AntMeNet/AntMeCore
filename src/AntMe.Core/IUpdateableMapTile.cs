using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Interface for all Map Tiles that need updates.
    /// </summary>
    public interface IUpdateableMapTile
    {
        void Update(int round, IEnumerable<Item> items);
    }
}
