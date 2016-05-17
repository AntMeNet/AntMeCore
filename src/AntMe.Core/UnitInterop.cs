using System;

namespace AntMe
{
    public abstract class UnitInterop : Interop
    {
        protected readonly Faction Faction;

        protected readonly FactionItem Item;

        public UnitInterop(Faction faction, FactionItem item)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Faction soll bereits teil eines Levels sein.
            if (faction.Level == null)
                throw new ArgumentException("Faction is not Part of a Level");

            // AntItem darf nicht null sein.
            if (item == null)
                throw new ArgumentNullException("item");

            Faction = faction;
            Item = item;
        }
    }
}
