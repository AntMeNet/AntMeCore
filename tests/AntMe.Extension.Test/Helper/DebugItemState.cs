using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugItemState : ItemState
    {
        public DebugItemState() { }

        public DebugItemState(Item item) : base(item) { }
    }

    internal class DebugItemStateSpecialized : DebugItemState
    {
        public DebugItemStateSpecialized(Item item) : base(item) { }
    }
}
