using System;
using System.Collections.Generic;
using AntMe.Basics.MapProperties;

namespace AntMe.Basics.MapTiles
{
    /// <summary>
    ///     Lava Material.
    /// </summary>
    public class LavaMaterial : MapMaterial, IUpdateableMapTile
    {
        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        public LavaMaterial(SimulationContext context) : base(context, 0.1f)
        {
        }

        /// <summary>
        ///     Gets called in every Round to update Items in this Map Tile.
        /// </summary>
        /// <param name="round">Current Round</param>
        /// <param name="items">Included Items</param>
        public void Update(int round, IEnumerable<Item> items)
        {
            // TODO: Kill items (if Attackable) on the lava Field slowly.
            throw new NotImplementedException();
        }
    }
}