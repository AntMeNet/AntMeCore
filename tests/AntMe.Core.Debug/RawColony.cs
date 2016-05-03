using AntMe.Simulation.Factions.Ants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    [RawPlayer(Name = "Raw Debug Ant", Author = "")]
    public sealed class RawColony : PrimordialAntColony
    {
        public override void Init(AntColonyInterop interop)
        {
            interop.OnCreateMember += interop_OnCreateMember;
        }

        private Type interop_OnCreateMember()
        {
            return typeof(RawAnt);
        }
    }

    [PlayerAttributeMapping(NameProperty = "Name", AuthorProperty = "Author")]
    public sealed class RawPlayerAttribute : PlayerAttribute
    {
        public string Name { get; set; }

        public string Author { get; set; }
    }
}
