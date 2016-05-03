using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Extension.Test
{
    internal class DebugItem : Item
    {
        public DebugItem(ITypeResolver resolver, Vector2 pos, Angle orientation)
            : base(resolver, pos, orientation)
        {

        }
    }
}
