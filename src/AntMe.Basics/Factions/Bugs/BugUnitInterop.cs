using AntMe.Basics.Items;
using System.Collections.Generic;

namespace AntMe.Basics.Factions.Bugs
{
    public sealed class BugUnitInterop : UnitInterop
    {
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
