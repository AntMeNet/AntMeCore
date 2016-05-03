using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Core.Test.Helper
{
    internal class DebugItem : Item
    {
        public DebugItem(ITypeResolver resolver, Vector2 position, Angle orientation) : base(resolver, position, orientation)
        {
        }
    }
}
