using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugItemInfo : ItemInfo
    {
        public DebugItemInfo(Item item, Item observer)
            : base(item, observer)
        {

        }
    }
}
