using System;

namespace AntMe.Basics.Factions
{
    /// <summary>
    /// Statistical Factory Interop Property to count and group Items by Type
    /// </summary>
    public sealed class ByTypeStatisticsInterop : StatisticsInterop<Type>
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        public ByTypeStatisticsInterop(Faction faction, FactoryInterop interop) : this(faction, interop, null) { }

        /// <summary>
        /// Specialized Constructor with an additional Filter Type Definition.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        /// <param name="filterType">Required Base Type</param>
        public ByTypeStatisticsInterop(Faction faction, FactoryInterop interop, Type filterType) : base(faction, interop, filterType)
        {

        }

        /// <summary>
        /// Callback to identify the group key.
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="group">Group Key</param>
        /// <returns>true if the Item is relevant for this Statistic</returns>
        protected override bool CategorizeItem(Item item, out Type group)
        {
            group = item.GetType();
            return true;
        }
    }
}
