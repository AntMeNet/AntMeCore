using System;

namespace AntMe.Factions.Ants
{
    /// <summary>
    /// Interop Klasse zwischen der Runtime und der User-Implementierung.
    /// </summary>
    public sealed class AntFactoryInterop : FactoryInterop<AntFaction>
    {
        public AntFactoryInterop(AntFaction faction) : base(faction) { }

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
