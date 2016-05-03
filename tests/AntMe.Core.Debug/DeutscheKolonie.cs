using AntMe.Simulation.Deutsch;
using AntMe.Simulation.Factions.Ants.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    [Spieler(
        Name = "Deutsche Ameisen", 
        Autor = "Tom Wendel")]
    public sealed class DeutscheKolonie : BasisKolonie
    {
        public override Type ErzeugeAmeise()
        {
            return typeof(DeutscheAmeise);
        }
    }
}
