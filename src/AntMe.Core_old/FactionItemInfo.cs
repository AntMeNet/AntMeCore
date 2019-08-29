namespace AntMe
{
    /// <summary>
    /// Base Info Class for all Faction Items.
    /// </summary>
    public abstract class FactionItemInfo : ItemInfo
    {
        /// <summary>
        /// Reference to the related Faction Item.
        /// </summary>
        private readonly FactionItem factionItem;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Reference to the Item</param>
        /// <param name="observer">Reference to the Observer item</param>
        public FactionItemInfo(FactionItem item, Item observer)
            : base(item, observer)
        {
            factionItem = item;

            IsFriendly = false;
            IsAllied = false;
            IsEnemy = true;
            if (observer is FactionItem)
            {
                FactionItem factionObserver = observer as FactionItem;
                IsFriendly = (factionItem.Faction.SlotIndex == factionObserver.Faction.SlotIndex);
                IsAllied = (factionItem.Faction.TeamIndex == factionObserver.Faction.TeamIndex);
                IsEnemy = !IsAllied;
            }
        }

        /// <summary>
        /// Gets detailed Faction Information.
        /// </summary>
        public FactionInfo Faction { get { return factionItem.Faction.GetFactionInfo(Observer); } }

        /// <summary>
        /// Is Item from same Faction? (Same Slot)
        /// </summary>
        public bool IsFriendly { get; private set; }

        /// <summary>
        /// Is Item from an allied Faction? (Same Team)
        /// </summary>
        public bool IsAllied { get; private set; }

        /// <summary>
        /// Is Item an Enemy?
        /// </summary>
        public bool IsEnemy { get; private set; }
    }
}
