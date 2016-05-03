using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    public abstract class UnitInteropProperty : InteropProperty
    {
        protected readonly Faction Faction;

        protected readonly FactionItem Item;

        public UnitInteropProperty(Faction faction, FactionItem item)
        {
            Faction = faction;
            Item = item;
        }
    }
}
