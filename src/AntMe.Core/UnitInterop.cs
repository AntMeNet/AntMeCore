using System;

namespace AntMe
{
    public abstract class UnitInterop : Interop { }

    public abstract class UnitInterop<F, I> : UnitInterop
        where F : Faction
        where I : FactionItem
    {
        protected readonly F Faction;

        protected readonly I Item;

        public UnitInterop(F faction, I item)
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
