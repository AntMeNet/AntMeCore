using System;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Interop Class for Ant Factories.
    /// </summary>
    public sealed class AntFactoryInterop : FactoryInterop
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="faction">Faction</param>
        public AntFactoryInterop(SimulationContext context, AntFaction faction) 
            : base(context, faction) { }

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
