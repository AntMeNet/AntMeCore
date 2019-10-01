namespace AntMe
{
    /// <summary>
    ///     Base Info Class for all Faction Items.
    /// </summary>
    public abstract class FactionItemInfo : ItemInfo
    {
        /// <summary>
        ///     Reference to the related Faction Item.
        /// </summary>
        private readonly FactionItem _factionItem;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Reference to the Item</param>
        public FactionItemInfo(FactionItem item)
            : base(item)
        {
            _factionItem = item;
        }

        /// <summary>
        ///     Gets detailed Faction Information.
        /// </summary>
        public FactionInfo Faction
        {
            get
            {
                Item observer = null; // TODO: Get observer
                return _factionItem.Faction.GetFactionInfo(observer);
            }
        }

        /// <summary>
        ///     Is Item from same Faction? (Same Slot)
        /// </summary>
        public bool IsFriendly
        {
            get
            {
                Item observer = null; // TODO: Get observer
                if (observer is FactionItem factionObserver)
                {
                    return _factionItem.Faction.SlotIndex == factionObserver.Faction.SlotIndex;
                }

                return false;
            }
        }


        /// <summary>
        ///     Is Item from an allied Faction? (Same Team)
        /// </summary>
        public bool IsAllied
        {
            get
            {
                Item observer = null; // TODO: Get observer
                if (observer is FactionItem factionObserver)
                {
                    return _factionItem.Faction.TeamIndex == factionObserver.Faction.TeamIndex;
                }

                return false;
            }
        }

        /// <summary>
        ///     Is Item an Enemy?
        /// </summary>
        public bool IsEnemy
        {
            get
            {
                Item observer = null; // TODO: Get observer
                if (observer is FactionItem factionObserver)
                {
                    return _factionItem.Faction.TeamIndex != factionObserver.Faction.TeamIndex;
                }

                return false;
            }
        }
    }
}