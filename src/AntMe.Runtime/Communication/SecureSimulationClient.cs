namespace AntMe.Runtime.Communication
{
    /// <summary>
    /// Wrapper um eine abgesicherte Simulation in einer AppDomain
    /// </summary>
    internal sealed class SecureSimulationClient : LocalSimulationClient
    {
        SecureSimulation simulation = null;

        public SecureSimulationClient(ITypeResolver resolver) : base(resolver) { }

        protected override void InitSimulation(int seed, LevelInfo level, PlayerInfo[] players, Slot[] slots)
        {
            simulation = new SecureSimulation();

            // Settings aufbauen
            Setup settings = new Setup();
            settings.Seed = seed;
            settings.Level = level.Type;
            settings.Player = new TypeInfo[8];
            settings.Colors = new PlayerColor[8];

            for (int i = 0; i < 8; i++)
            {
                // Farben übertragen
                settings.Colors[i] = slots[i].ColorKey;
                
                // KIs einladen
                if (players[i] != null)
                {
                    settings.Player[i] = players[i].Type;
                }
            }

            simulation.Start(settings);
        }

        protected override LevelState UpdateSimulation()
        {
            return simulation.NextState();
        }

        protected override void FinalizeSimulation()
        {
            simulation.Stop();
            simulation.Dispose();
            simulation = null;
        }
    }
}
