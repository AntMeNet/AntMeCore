using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugItemInfoPropertyNoConstructors : ItemInfoProperty
    {

    }

    internal class DebugItemInfoProperty1 : ItemInfoProperty
    {
        public DebugItemInfoProperty1(Item item, DebugItemProperty1 prop, Item observer) { }
    }

    internal class DebugItemInfoProperty1Specialized : DebugItemInfoProperty1
    {
        public DebugItemInfoProperty1Specialized(Item item, DebugItemProperty1 prop, Item observer) : base(item, prop, observer) { }
    }
}
