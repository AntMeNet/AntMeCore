namespace AntMe.Basics.Items
{
    public sealed class MarkerInfo : ItemInfo
    {
        private readonly MarkerItem _markerItem;

        public MarkerInfo(MarkerItem item)
            : base(item)
        {
            _markerItem = item;
        }

        /// <summary>
        ///     Ermittelt, ob es sich um eine eigene Markierung handelt.
        /// </summary>
        public bool IsOwnMarker
        {
            get
            {
                Item observer = null; // TODO: Get Observer
                if (observer is FactionItem factionObserver)
                {
                    return factionObserver.Faction.SlotIndex == _markerItem.Faction.SlotIndex;
                }

                return false;
            }
        }

        /// <summary>
        ///     Gibt die enthaltene Information zurück.
        /// </summary>
        public int Information => _markerItem.Information;

        /// <summary>
        ///     Gibt das maximale alter dieser Markierung in Runden zurück.
        /// </summary>
        public int TotalAge => _markerItem.TotalAge;

        /// <summary>
        ///     Gibt das aktuelle Alter der Markierung in Runden zurück.
        /// </summary>
        public int CurrentAge => _markerItem.CurrentAge;
    }
}