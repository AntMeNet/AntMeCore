using System;

namespace AntMe.Basics.Factions.Bugs
{
    public sealed class BugFactoryInterop : FactoryInterop
    {
        public BugFactoryInterop(SimulationContext context, BugFaction faction) 
            : base(context, faction)
        {

        }

        internal Type RequestCreateMember()
        {
            if (OnCreateMember != null)
                return OnCreateMember();
            return null;
        }

        public event CreateMember OnCreateMember;

        public delegate Type CreateMember();
    }
}