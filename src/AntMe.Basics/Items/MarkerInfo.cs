namespace AntMe.Basics.Items
{
    public sealed class MarkerInfo : ItemInfo
    {
        private readonly MarkerItem markerItem;

        public MarkerInfo(MarkerItem item, Item observer)
            : base(item, observer)
        {
            markerItem = item;
        }

        /// <summary>
        ///     Ermittelt, ob es sich um eine eigene Markierung handelt.
        /// </summary>
        public bool IsOwnMarker
        {
            get
            {
                if (Observer is FactionItem)
                {
                    var item = Observer as FactionItem;
                    return item.Faction.SlotIndex == markerItem.Faction.SlotIndex;
                }

                return false;
            }
        }

        /// <summary>
        ///     Gibt den Radius des Elementes zurück.
        /// </summary>
        public new float Radius => markerItem.Radius;

        /// <summary>
        ///     Gibt die enthaltene Information zurück.
        /// </summary>
        public int Information => markerItem.Information;

        /// <summary>
        ///     Gibt das maximale alter dieser Markierung in Runden zurück.
        /// </summary>
        public int TotalAge => markerItem.TotalAge;

        /// <summary>
        ///     Gibt das aktuelle Alter der Markierung in Runden zurück.
        /// </summary>
        public int CurrentAge => markerItem.CurrentAge;
    }
}