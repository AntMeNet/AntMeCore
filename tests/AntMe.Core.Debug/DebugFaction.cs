using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    public sealed class DebugFaction : Faction<DebugFactionSettings>
    {
        public DebugFaction(DebugFactionSettings settings, string name, PlayerColor color)
            : base(settings, name, color)
        {

        }

        public override void Init()
        {
            if (InitCall != null)
                InitCall();
        }

        public override void Update(int round)
        {
            if (UpdateCall != null)
                UpdateCall(round);
        }

        public override FactionState GetFactionState()
        {
            if (GetStateCall != null)
                GetStateCall();

            DebugFactionState state = new DebugFactionState();
            PrefillState(state);
            return state;
        }

        public override FactionInfo GetFactionInfo(GameItem observer)
        {
            throw new NotImplementedException();
        }

        public delegate void SimpleDelegate();
        public delegate void UpdateDelegate(int round);

        public event SimpleDelegate InitCall;
        public event UpdateDelegate UpdateCall;
        public event SimpleDelegate GetStateCall;
    }
}
