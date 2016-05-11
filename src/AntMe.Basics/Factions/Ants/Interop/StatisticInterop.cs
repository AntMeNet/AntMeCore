using AntMe.Items.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.Factions.Ants.Interop
{
    public sealed class StatisticInterop : FactoryInteropProperty
    {

        public int RECENT_CENTURIES_DEFAULT = 10;

        internal int _recentCenturies;

        /// <summary>
        /// Gibt die Anzahl an hundert Runden an, die in den kürzlichen statistiken zusammengefasst werden sollen. 
        /// </summary>
        public int RecentCenturies
        {
            get { return _recentCenturies; }
            set
            {
                value = Math.Max(1, value);
                if (value != _recentCenturies)
                {
                    _recentCenturies = value;
                    InvalidCenturies = true;
                }
            }
        }

        /// <summary>
        /// Returns the current time elapsed in game since start.
        /// </summary>
        public TimeSpan GameTime { get; internal set; }

        internal bool InvalidCenturies { get; set; }

        #region Ant Counter

        #region internal

        internal List<int> InternalRecentAntsCache { get; private set; }
        internal Dictionary<Type, int> InternalTotalAntsPerType { get; private set; }
        internal Dictionary<Type, int> InternalCurrentAntsPerType { get; private set; }
        internal Dictionary<string, int> InternalTotalAntsPerCaste { get; private set; }
        internal Dictionary<string, int> InternalCurrentAntsPerCaste { get; private set; }
        internal List<Dictionary<Type, int>> InternalRecentAntsPerTypeCache { get; private set; }
        internal Dictionary<Type, int> InternalRecentAntsPerType { get; private set; }
        internal List<Dictionary<string, int>> InternalRecentAntsPerCasteCache { get; private set; }
        internal Dictionary<string, int> InternalRecentAntsPerCaste { get; private set; }

        #endregion

        #region Total

        /// <summary>
        ///  Total Number of Ants.
        /// </summary>
        public int TotalAnts { get; internal set; }

        /// <summary>
        /// Total Number of created Ants grouped by Type.
        /// </summary>
        public ReadOnlyDictionary<Type, int> TotalAntsPerType { get; private set; }

        /// <summary>
        /// Total Number of created Ants grouped by Caste.
        /// </summary>
        public ReadOnlyDictionary<string, int> TotalAntsPerCaste { get; private set; }

        #endregion

        #region Current

        /// <summary>
        /// Current Number of Ants.
        /// </summary>
        public int CurrentAnts { get; internal set; }

        /// <summary>
        /// Current Number of created Ants grouped by Type.
        /// </summary>
        public ReadOnlyDictionary<Type, int> CurrentAntsPerType { get; private set; }

        /// <summary>
        /// Current Number of created Ants grouped by Caste.
        /// </summary>
        public ReadOnlyDictionary<string, int> CurrentAntsPerCaste { get; private set; }

        #endregion

        #region Recent

        /// <summary>
        /// Number of recently created Ants based on the timespan set in RecentCenturies.
        /// </summary>
        public int RecentAnts { get; internal set; }

        /// <summary>
        /// Number of recently created Ants grouped by Type.
        /// </summary>
        public ReadOnlyDictionary<Type, int> RecentAntsPerType { get; private set; }

        /// <summary>
        /// Number of recently created Ants grouped by Caste.
        /// </summary>
        public ReadOnlyDictionary<string, int> RecentAntsPerCaste { get; private set; }

        #endregion

        #endregion

        public StatisticInterop(Faction faction, FactionItem item, FactoryInterop interop) : base(faction, item, interop)
        {
            RecentCenturies = RECENT_CENTURIES_DEFAULT;

            // Total
            TotalAnts = 0;
            InternalTotalAntsPerType = new Dictionary<Type, int>();
            InternalTotalAntsPerCaste = new Dictionary<string, int>();
            TotalAntsPerType = new ReadOnlyDictionary<Type, int>(InternalTotalAntsPerType);
            TotalAntsPerCaste = new ReadOnlyDictionary<string, int>(InternalTotalAntsPerCaste);

            // Current
            CurrentAnts = 0;
            InternalCurrentAntsPerType = new Dictionary<Type, int>();
            InternalCurrentAntsPerCaste = new Dictionary<string, int>();
            CurrentAntsPerType = new ReadOnlyDictionary<Type, int>(InternalCurrentAntsPerType);
            CurrentAntsPerCaste = new ReadOnlyDictionary<string, int>(InternalCurrentAntsPerCaste);

            // Recent
            RecentAnts = 0;
            InternalRecentAntsCache = new List<int>();
            InternalRecentAntsPerTypeCache = new List<Dictionary<Type, int>>();
            InternalRecentAntsPerCasteCache = new List<Dictionary<string, int>>();
            InternalRecentAntsPerType = new Dictionary<Type, int>();
            InternalRecentAntsPerCaste = new Dictionary<string, int>();
            RecentAntsPerType = new ReadOnlyDictionary<Type, int>(InternalRecentAntsPerType);
            RecentAntsPerCaste = new ReadOnlyDictionary<string, int>(InternalRecentAntsPerCaste);

            faction.Level.Engine.OnInsertItem += Engine_OnInsertItem;
            faction.Level.Engine.OnRemoveItem += Engine_OnRemoveItem;
        }

        protected override void Update(int round)
        {
            // ColonyInterop Stats updaten
            int century = Faction.Level.Engine.Round / 100;
            if (InternalRecentAntsCache.Count - 1 < century)
            {
                InvalidCenturies = true;
                InternalRecentAntsCache.Add(0);
                InternalRecentAntsPerTypeCache.Add(new Dictionary<Type, int>());
                InternalRecentAntsPerCasteCache.Add(new Dictionary<string, int>());
            }

            // Neuberechnung der Recent-Werte
            if (InvalidCenturies)
            {
                // TODO: Test!

                int index = century;
                RecentAnts = 0;
                InternalRecentAntsPerType.Clear();
                InternalRecentAntsPerCaste.Clear();
                while (century - index < RecentCenturies && index >= 0)
                {
                    // Sum
                    RecentAnts += InternalRecentAntsCache[index];

                    // Per Type
                    foreach (Type key in InternalRecentAntsPerTypeCache[index].Keys)
                    {
                        if (!InternalRecentAntsPerType.ContainsKey(key))
                            InternalRecentAntsPerType.Add(key, 0);
                        InternalRecentAntsPerType[key] +=
                            InternalRecentAntsPerTypeCache[index][key];
                    }

                    // Per Caste
                    foreach (string key in InternalRecentAntsPerCasteCache[index].Keys)
                    {
                        if (!InternalRecentAntsPerCaste.ContainsKey(key))
                            InternalRecentAntsPerCaste.Add(key, 0);
                        InternalRecentAntsPerCaste[key] +=
                            InternalRecentAntsPerCasteCache[index][key];
                    }

                    index--;
                }
                InvalidCenturies = false;
            }
        }

        private void Engine_OnInsertItem(Item item)
        {
            if (item is AntItem && (item as AntItem).Faction == Faction)
            {
                AntItem ant = item as AntItem;

                // Total Ants
                TotalAnts++;
                if (!InternalTotalAntsPerType.ContainsKey(ant.GetType()))
                    InternalTotalAntsPerType.Add(ant.GetType(), 0);
                InternalTotalAntsPerType[ant.GetType()]++;
                //if (!InternalTotalAntsPerCaste.ContainsKey(ant.Caste))
                //    InternalTotalAntsPerCaste.Add(ant.Caste, 0);
                //InternalTotalAntsPerCaste[ant.Caste]++;

                // Current Ants
                CurrentAnts++;
                if (!InternalCurrentAntsPerType.ContainsKey(ant.GetType()))
                    InternalCurrentAntsPerType.Add(ant.GetType(), 0);
                InternalCurrentAntsPerType[ant.GetType()]++;
                //if (!InternalCurrentAntsPerCaste.ContainsKey(ant.Caste))
                //    InternalCurrentAntsPerCaste.Add(ant.Caste, 0);
                //InternalCurrentAntsPerCaste[ant.Caste]++;

                // Recent
                int century = Faction.Level.Engine.Round / 100;

                InternalRecentAntsCache[century]++;
                if (!InternalRecentAntsPerTypeCache[century].ContainsKey(ant.GetType()))
                    InternalRecentAntsPerTypeCache[century].Add(ant.GetType(), 0);
                InternalRecentAntsPerTypeCache[century][ant.GetType()]++;
                //if (!InternalRecentAntsPerCasteCache[century].ContainsKey(ant.Caste))
                //    InternalRecentAntsPerCasteCache[century].Add(ant.Caste, 0);
                //InternalRecentAntsPerCasteCache[century][ant.Caste]++;

                RecentAnts++;
                if (!InternalRecentAntsPerType.ContainsKey(ant.GetType()))
                    InternalRecentAntsPerType.Add(ant.GetType(), 0);
                InternalRecentAntsPerType[ant.GetType()]++;
                //if (!InternalRecentAntsPerCaste.ContainsKey(ant.Caste))
                //    InternalRecentAntsPerCaste.Add(ant.Caste, 0);
                //InternalRecentAntsPerCaste[ant.Caste]++;
            }
        }

        private void Engine_OnRemoveItem(Item item)
        {
            if (item is AntItem && (item as AntItem).Faction == Faction)
            {
                AntItem ant = item as AntItem;

                // Stats aktulaisieren
                CurrentAnts--;
                if (!InternalCurrentAntsPerType.ContainsKey(ant.GetType()))
                    InternalCurrentAntsPerType.Add(ant.GetType(), 0);
                InternalCurrentAntsPerType[ant.GetType()]--;
                //if (!InternalCurrentAntsPerCaste.ContainsKey(ant.Caste))
                //    InternalCurrentAntsPerCaste.Add(ant.Caste, 0);
                //InternalCurrentAntsPerCaste[ant.Caste]--;

                if (RecentCenturies > 0)
                {
                    RecentAnts--;
                    if (!InternalRecentAntsPerType.ContainsKey(ant.GetType()))
                        InternalRecentAntsPerType.Add(ant.GetType(), 0);
                    InternalRecentAntsPerType[ant.GetType()]--;
                    //if (!InternalRecentAntsPerCaste.ContainsKey(ant.Caste))
                    //    InternalRecentAntsPerCaste.Add(ant.Caste, 0);
                    //InternalRecentAntsPerCaste[ant.Caste]--;
                }

                int century = Faction.Level.Engine.Round / 100;
                if (InternalRecentAntsCache.Count <= century)
                    InternalRecentAntsCache.Add(0);
                InternalRecentAntsCache[century]--;

                if (InternalRecentAntsPerTypeCache.Count <= century)
                    InternalRecentAntsPerTypeCache.Add(new Dictionary<Type, int>());
                if (!InternalRecentAntsPerTypeCache[century].ContainsKey(ant.GetType()))
                    InternalRecentAntsPerTypeCache[century].Add(ant.GetType(), 0);
                InternalRecentAntsPerTypeCache[century][ant.GetType()]--;

                if (InternalRecentAntsPerCasteCache.Count <= century)
                    InternalRecentAntsPerCasteCache.Add(new Dictionary<string, int>());
                //if (!InternalRecentAntsPerCasteCache[century].ContainsKey(ant.Caste))
                //    InternalRecentAntsPerCasteCache[century].Add(ant.Caste, 0);
                //InternalRecentAntsPerCasteCache[century][ant.Caste]--;
            }
        }
    }
}
