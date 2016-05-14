using AntMe.Basics.Items;
using System;

namespace AntMe.Basics.Factions.Ants.Interop
{
    /// <summary>
    /// Statistical Factory Interop Property to count and group Items by Caste
    /// </summary>
    public sealed class ByCasteStatisticsInterop : StatisticsInterop<string>
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        public ByCasteStatisticsInterop(Faction faction, FactoryInterop interop) : this(faction, interop, null) { }

        /// <summary>
        /// Specialized Constructor with an additional Filter Type Definition.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        /// <param name="filterType">Required Base Type</param>
        public ByCasteStatisticsInterop(Faction faction, FactoryInterop interop, Type filterType) : base(faction, interop, filterType) { }

        /// <summary>
        /// Callback to identify the group key.
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="group">Group Key</param>
        /// <returns>true if the Item is relevant for this Statistic</returns>
        protected override bool CategorizeItem(Item item, out string group)
        {
            // Only Ant Items are relevant.
            AntItem antItem = item as AntItem;
            if (antItem != null)
            {
                // TODO: Fix this as soon Ants get Castes back
                group = string.Empty;
                return true;
            }

            group = string.Empty;
            return false;
        }
    }
}
