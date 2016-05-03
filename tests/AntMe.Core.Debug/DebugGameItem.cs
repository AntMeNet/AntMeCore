using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntMe.Core;

namespace AntMe.Simulation.Debug
{
    public sealed class DebugGameItem : GameItem
    {
        public DebugGameItem() : base(new Core.Vector2(10, 10), Angle.Right) { }
    }

    public sealed class DebugGameState : ItemState
    {
        public DebugGameState(DebugGameItem item) : base(item)
        {

        }
    }
}
