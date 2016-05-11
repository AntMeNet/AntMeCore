using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    public abstract class UnitInteropProperty : InteropProperty
    {
        protected readonly Faction Faction;

        protected readonly UnitInterop Interop;

        public UnitInteropProperty(Faction faction, UnitInterop interop)
        {
            Faction = faction;
            Interop = Interop;
        }
    }
}
