using System.Collections.Generic;

namespace AntMe.Basics.MapProperties
{
    /// <summary>
    /// Interface for all Map Tiles that need updates.
    /// </summary>
    public interface IUpdateableMapTile
    {
        /// <summary>
        /// Gets called in every Round to update Items in this Map Tile.
        /// </summary>
        /// <param name="round">Current Round</param>
        /// <param name="items">Included Items</param>
        void Update(int round, IEnumerable<Item> items);
    }
}
