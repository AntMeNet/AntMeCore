using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugItemProperty1 : ItemProperty
    {
        public DebugItemProperty1(Item item) : base(item) { }
    }

    internal class DebugItemProperty1Specialized : DebugItemProperty1
    {
        public DebugItemProperty1Specialized(Item item) : base(item) { }
    }
}
