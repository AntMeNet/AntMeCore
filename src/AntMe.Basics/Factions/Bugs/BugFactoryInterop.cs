using System;

namespace AntMe.Basics.Factions.Bugs
{
    public sealed class BugFactoryInterop : FactoryInterop
    {
        public delegate Type CreateMember();

        public BugFactoryInterop(BugFaction faction)
            : base(faction)
        {
        }

        internal Type RequestCreateMember()
        {
            return OnCreateMember?.Invoke();
        }

        public event CreateMember OnCreateMember;
    }
}