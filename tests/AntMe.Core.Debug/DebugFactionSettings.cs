using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    public sealed class DebugFactionSettings : FactionSettings
    {
        public override void CheckSettings()
        {
            if (CheckSettingsCall != null)
                CheckSettingsCall();
        }

        public delegate void CheckSettingsDelegate();

        public event CheckSettingsDelegate CheckSettingsCall;
    }
}
