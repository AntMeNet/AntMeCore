using AntMe.Items.Basics;
using AntMe.Simulation;
using System;
using System.Collections.Generic;

namespace AntMe.Factions.Bugs
{
    public sealed class BugUnitInterop : UnitInterop<BugFaction, BugItem>
    {
        private readonly BugItem bugItem;
        private readonly BugFaction faction;

        private List<ItemInfo> visibleItems = new List<ItemInfo>();
        private List<ItemInfo> smellableItems = new List<ItemInfo>();
        private List<ItemInfo> collidedItems = new List<ItemInfo>();
        private List<ItemInfo> attackerItems = new List<ItemInfo>();

        public BugUnitInterop(BugFaction faction, BugItem item) : base(faction, item)
        {
        }

        #region Event Handler

        #endregion

        internal void Update()
        {
        }

        #region Spieler Eigenschaften

        #endregion

        #region Spieler Methoden

        #endregion

        #region Spieler Events

        #endregion
    }
}
