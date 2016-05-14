using AntMe.Basics.Factions.Ants;
using System;

namespace AntMe.Extension.Community.Players
{
    [Player(Hidden = false, Name = "Default Ants", Author = "Random Dude")]
    public class DefaultAntColony : AntFactory
    {
        public override void Init(FactoryInterop interop)
        {
            AntFactoryInterop antInterop = interop as AntFactoryInterop;
            antInterop.OnCreateMember += AntInterop_OnCreateMember;
        }

        private Type AntInterop_OnCreateMember()
        {
            return typeof(DefaultAnt);
        }
    }
}
