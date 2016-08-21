using System;
using System.Collections.Generic;

namespace AntMe.Basics.Factions
{
    /// <summary>
    /// Statistical Interop Extension for the Factory Interop.
    /// </summary>
    public abstract class StatisticsInterop : FactoryInteropProperty
    {
        /// <summary>
        /// Default Definition for the "recent" Time Range.
        /// </summary>
        public const int RecentCenturiesDefault = 10;

        /// <summary>
        /// Defines a Base Type that limits the counting Items.
        /// </summary>
        protected readonly Type FilterType;

        private int recentCenturies;

        /// <summary>
        /// Gets or sets the Number of Centuries that defines "recent".
        /// </summary>
        public int RecentCenturies
        {
            get { return recentCenturies; }
            set
            {
                value = Math.Max(1, value);
                recentCenturies = value;
            }
        }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        public StatisticsInterop(Faction faction, FactoryInterop interop) : this(faction, interop, null) { }

        /// <summary>
        /// Specialized Constructor with an additional Filter Type Definition.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        /// <param name="filterType">Required Base Type</param>
        public StatisticsInterop(Faction faction, FactoryInterop interop, Type filterType) : base(faction, interop)
        {
            FilterType = filterType;

            faction.Level.InsertItem += Engine_OnInsertItem;
            faction.Level.RemoveItem += Engine_OnRemoveItem;
        }

        private void Engine_OnInsertItem(Item item)
        {
            // Count only Faction Items
            FactionItem factionItem = item as FactionItem;
            if (factionItem == null)
                return;

            // Ignore Items that are not assineable to the FilterType
            if (FilterType != null && !FilterType.IsAssignableFrom(item.GetType()))
                return;

            // Count only own Items
            if (Faction != factionItem.Faction)
                return;

            // Call inherited Method
            OnInsert(factionItem);
        }

        private void Engine_OnRemoveItem(Item item)
        {
            // Count only Faction Items
            FactionItem factionItem = item as FactionItem;
            if (factionItem == null)
                return;

            // Ignore Items that are not assineable to the FilterType
            if (FilterType != null && !FilterType.IsAssignableFrom(item.GetType()))
                return;

            // Count only own Items
            if (Faction != factionItem.Faction)
                return;

            // Call inherited Method
            OnRemove(factionItem);
        }

        /// <summary>
        /// Gets a call on new Items (filtered by FilterType, FactionItem and right Faction)
        /// </summary>
        /// <param name="item">New Item</param>
        protected abstract void OnInsert(FactionItem item);

        /// <summary>
        /// Gets a call on removed Items (filtered by FilterType, FactionItem and right Faction)
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected abstract void OnRemove(FactionItem item);
    }

    /// <summary>
    /// Statistical Interop Extension for the Factory Interop.
    /// </summary>
    public abstract class StatisticsInterop<T> : StatisticsInterop
    {
        private Dictionary<T, int> totalItems;
        private Dictionary<T, int> currentItems;
        //private List<Dictionary<T, int>> recentItemsCache;
        //private Dictionary<T, int> recentItems;

        /// <summary>
        /// Total Number of created Ants grouped by Type.
        /// </summary>
        public ReadOnlyDictionary<T, int> TotalItems { get; private set; }

        /// <summary>
        /// Current Number of created Ants grouped by Type.
        /// </summary>
        public ReadOnlyDictionary<T, int> CurrentItems { get; private set; }

        ///// <summary>
        ///// Number of recently created Ants grouped by Type.
        ///// </summary>
        //public ReadOnlyDictionary<T, int> RecentItems { get; private set; }


        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        public StatisticsInterop(Faction faction, FactoryInterop interop) : base(faction, interop, null) { }

        /// <summary>
        /// Specialized Constructor with an additional Filter Type Definition.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="interop">Related Interop</param>
        /// <param name="filterType">Required Base Type</param>
        public StatisticsInterop(Faction faction, FactoryInterop interop, Type filterType) : base(faction, interop, filterType)
        {
            RecentCenturies = RecentCenturiesDefault;

            totalItems = new Dictionary<T, int>();
            TotalItems = new ReadOnlyDictionary<T, int>(totalItems);

            currentItems = new Dictionary<T, int>();
            CurrentItems = new ReadOnlyDictionary<T, int>(currentItems);

            //recentItemsCache = new List<Dictionary<T, int>>();
            //recentItems = new Dictionary<T, int>();
            //RecentItems = new ReadOnlyDictionary<T, int>(recentItems);
        }

        protected override void Update(int round)
        {
            //// ColonyInterop Stats updaten
            //int century = Faction.Level.Engine.Round / 100;
            //if (InternalRecentItemsCache.Count - 1 < century)
            //{
            //    InvalidCenturies = true;
            //    InternalRecentItemsCache.Add(0);
            //    InternalRecentItemsPerTypeCache.Add(new Dictionary<Type, int>());
            //    InternalRecentItemsPerCasteCache.Add(new Dictionary<string, int>());
            //}

            //// Neuberechnung der Recent-Werte
            //if (InvalidCenturies)
            //{
            //    // TODO: Test!

            //    int index = century;
            //    RecentItems = 0;
            //    InternalRecentItemsPerType.Clear();
            //    InternalRecentItemsPerCaste.Clear();
            //    while (century - index < RecentCenturies && index >= 0)
            //    {
            //        // Sum
            //        RecentItems += InternalRecentItemsCache[index];

            //        // Per Type
            //        foreach (Type key in InternalRecentItemsPerTypeCache[index].Keys)
            //        {
            //            if (!InternalRecentItemsPerType.ContainsKey(key))
            //                InternalRecentItemsPerType.Add(key, 0);
            //            InternalRecentItemsPerType[key] +=
            //                InternalRecentItemsPerTypeCache[index][key];
            //        }

            //        // Per Caste
            //        foreach (string key in InternalRecentItemsPerCasteCache[index].Keys)
            //        {
            //            if (!InternalRecentItemsPerCaste.ContainsKey(key))
            //                InternalRecentItemsPerCaste.Add(key, 0);
            //            InternalRecentItemsPerCaste[key] +=
            //                InternalRecentItemsPerCasteCache[index][key];
            //        }

            //        index--;
            //    }
            //    InvalidCenturies = false;
            //}
        }

        /// <summary>
        /// Gets a call on new Items (filtered by FilterType, FactionItem and right Faction)
        /// </summary>
        /// <param name="item">New Item</param>
        protected override void OnInsert(FactionItem item)
        {
            T key;
            if (CategorizeItem(item, out key))
            {
                // Increase total Counter
                if (!totalItems.ContainsKey(key))
                    totalItems.Add(key, 0);
                totalItems[key]++;

                // Increase current Counter
                if (!currentItems.ContainsKey(key))
                    currentItems.Add(key, 0);
                currentItems[key]++;

                // TODO: Recent Counter
                // int century = Faction.Level.Engine.Round / 100;
            }
        }

        /// <summary>
        /// Gets a call on removed Items (filtered by FilterType, FactionItem and right Faction)
        /// </summary>
        /// <param name="item">Removed Item</param>
        protected override void OnRemove(FactionItem item)
        {
            T key;
            if (CategorizeItem(item, out key))
            {
                // Decrease total Counter
                if (totalItems.ContainsKey(key))
                {
                    totalItems[key]++;

                    // Remove if empty
                    if (totalItems[key] <= 0)
                        totalItems.Remove(key);
                }

                // Decrease current Counter
                if (currentItems.ContainsKey(key))
                {
                    currentItems[key]++;

                    // Remove of empty
                    if (currentItems[key] <= 0)
                        currentItems.Remove(key);
                }

                // int century = Faction.Level.Engine.Round / 100;
                // TODO: Recent Counter
            }
        }

        /// <summary>
        /// Callback to identify the group key.
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="group">Group Key</param>
        /// <returns>true if the Item is relevant for this Statistic</returns>
        protected abstract bool CategorizeItem(Item item, out T group);
    }
}
