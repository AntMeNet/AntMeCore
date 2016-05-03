using AntMe.Simulation.Deutsch;
using AntMe.Simulation.Factions.Ants;
using AntMe.Simulation.Factions.Ants.Deutsch;
using AntMe.Simulation.Factions.Ants.Interop;
using AntMe.Simulation.Items.Deutsch;

namespace AntMe.Simulation.Debug
{
    public sealed class DeutscheAmeise : BasisAmeise
    {
        public override void Wartet()
        {
            Drehe(Zufall.Zahl(-60, 60));
            GeheGeradeaus(Zufall.Zahl(20, 50));
        }

        public override void Sieht(Zucker zucker)
        {
            if (AktuellesZiel == null)
            {
                GeheZu(zucker);
            }
        }

        public override void Sieht(Apfel apfel)
        {
            if (AktuellesZiel == null)
                GeheZu(apfel);
        }

        public override void Erreicht(Zucker zucker)
        {
            Nimm(zucker);
            SprüheMarkierung(-1, 200);
            GeheZuBau();
        }

        public override void Erreicht(Apfel apfel)
        {
            Trage(apfel);
            GeheZuBau();
        }

        public override void Sieht(Ameise ameise)
        {
        }

        public override void Riecht(Markierung markierung)
        {
            if (markierung.IstEigeneMarkierung && AktuellesZiel == null)
            {
                if (markierung.Information >= 0)
                {
                    DreheZu(markierung.Information);
                    GeheGeradeaus(100);
                }
                else
                    GeheZu(markierung);
            }
        }

        public override void Tick()
        {
            if (AktuelleZuckerLast > 0 || AktuelleApfelLast > 0)
            {
                SprüheMarkierung(Richtung + 180);
            }
        }
    }
}
