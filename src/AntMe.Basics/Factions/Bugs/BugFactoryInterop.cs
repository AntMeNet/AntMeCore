using System;

namespace AntMe.Factions.Bugs
{
    public abstract class BugFactoryInterop : FactoryInterop
    {
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