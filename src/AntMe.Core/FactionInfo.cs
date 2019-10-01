namespace AntMe
{
    /// <summary>
    ///     Base Class for Faction Infos.
    /// </summary>
    public class FactionInfo : PropertyList<InfoProperty>
    {
        /// <summary>
        ///     Reference to the observed Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        ///     Reference to the observing Item.
        /// </summary>
        protected readonly Item Observer;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction Reference</param>
        /// <param name="observer">Observing Item</param>
        public FactionInfo(Faction faction, Item observer)
        {
            Faction = faction;
            Observer = observer;

            IsOwnFaction = false;
            IsFriendlyFaction = false;
            IsEnemyFaction = true;

            if (observer is FactionItem)
            {
                var factionObserver = observer as FactionItem;
                IsOwnFaction = faction.SlotIndex == factionObserver.Faction.SlotIndex;
                IsFriendlyFaction = faction.TeamIndex == factionObserver.Faction.TeamIndex;
                IsEnemyFaction = !IsFriendlyFaction;
            }

            // TODO: Collect Faction Infos (Faction Name?)
        }

        /// <summary>
        ///     Is Faction own Faction?
        /// </summary>
        public bool IsOwnFaction { get; }

        /// <summary>
        ///     Is Faction in the same Team?
        /// </summary>
        public bool IsFriendlyFaction { get; }

        /// <summary>
        ///     Is Faction an Enemy?
        /// </summary>
        public bool IsEnemyFaction { get; }
    }
}