namespace AntMe.Items.Basics
{
    public class AnthillInfo : ItemInfo
    {
        private readonly AnthillItem anthillItem;

        public AnthillInfo(Item item, Item observer)
            : base(item, observer)
        {
            anthillItem = item as AnthillItem;
        }

        /// <summary>
        /// Gibt den Radius des Ameisenhügels zurück.
        /// </summary>
        public new float Radius
        {
            get { return anthillItem.Radius; }
        }

        /// <summary>
        /// Gibt die aktuellen Hitpoints des Hügels zurück.
        /// </summary>
        public int Hitpoints
        {
            get { return anthillItem.Hitpoints; }
        }

        /// <summary>
        /// Gibt die maximale Anzahl Hitpoints zurück.
        /// </summary>
        public int MaximumHitpoints
        {
            get { return anthillItem.MaximumHitpoints; }
        }

        /// <summary>
        /// Ermittelt, ob es sich um einen eigenen Ameisenhügel handelt.
        /// </summary>
        public bool IsOwnAnthill
        {
            get
            {
                if (Observer is FactionItem)
                {
                    FactionItem item = Observer as FactionItem;
                    return (item.Faction.PlayerIndex == anthillItem.Faction.PlayerIndex);
                }
                return false;
            }
        }
    }
}