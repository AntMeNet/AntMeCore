using AntMe.Simulation.English;
using AntMe.Simulation.Factions.Ants.English;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    [AntMe.Simulation.English.Player(
        PlayerName = "English Ant Colony", 
        AuthorName = "Tom Wendel")]
    public sealed class EnglishColony : BaseColony
    {
        public override Type CreateAnt()
        {
            return typeof(EnglishAnt);
        }
    }
}
