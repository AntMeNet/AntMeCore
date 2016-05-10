using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Basis Item Info für alle Faction Items.
    /// </summary>
    public abstract class FactionItemInfo : ItemInfo
    {
        private readonly FactionItem factionItem;
        private readonly int observerFaction;

        /// <summary>
        /// Konstruktur des Item Info Objektes.
        /// </summary>
        /// <param name="item">Referenz auf das betroffene Item.</param>
        /// <param name="observer">Referenz auf das betrachtende Item.</param>
        public FactionItemInfo(FactionItem item, Item observer)
            : base(item, observer)
        {
            factionItem = item;

            observerFaction = -1;
            if (observer is FactionItem)
                observerFaction = (observer as FactionItem).Faction.SlotIndex;
        }

        /// <summary>
        /// Gibt an, ob das Objekt zum Spieler gehört.
        /// </summary>
        public bool IsFriendly
        {
            get { return (factionItem.Faction.SlotIndex == observerFaction); }
        }

        /// <summary>
        /// Gibt an, ob das Objekt zu einem anderen Spieler gehört.
        /// </summary>
        public bool IsEnemy
        {
            get { return (factionItem.Faction.SlotIndex != observerFaction); }
        }
    }
}
