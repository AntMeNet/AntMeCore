using System;

namespace AntMe.Basics.Factions
{
    /// <summary>
    /// Statistical Factory Interop Property to count the total Numbers of Items.
    /// </summary>
    public sealed class TotalStatisticsInterop : StatisticsInterop
    {
        private int totalItems;
        private int currentItems;

        /// <summary>
        /// Total Number of created Items.
        /// </summary>
        public int TotalItems { get; private set; }

        /// <summary>
        /// Current Number of created Items.
        /// </summary>
        public int CurrentItems { get; private set; }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        public TotalStatisticsInterop(Faction faction, FactoryInterop interop) : this(faction, interop, null) { }

        /// <summary>
        /// Specialized Constructor with an additional Filter Type Definition.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        /// <param name="filterType">Required Base Type</param>
        public TotalStatisticsInterop(Faction faction, FactoryInterop interop, Type filterType) : base(faction, interop, filterType) { }

        /// <summary>
        /// Gets a call on new Items (filtered by FilterType, FactionItem and right Faction)
        /// </summary>
        /// <param name="item">New Item</param>
        protected override void OnInsert(FactionItem item)
        {
            totalItems++;
            currentItems++;
        }

        /// <summary>
        /// Gets a call on removed Items (filtered by FilterType, FactionItem and right Faction)
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected override void OnRemove(FactionItem item)
        {
            currentItems--;
        }
    }
}
