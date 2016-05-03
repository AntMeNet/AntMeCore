using System.Collections.Generic;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Beobachtet den Punktestand einer Faction.
    /// </summary>
    public sealed class FactionPointsObserver : ILogObserver
    {
        private bool init = false;
        private Dictionary<int, int> Points = new Dictionary<int, int>();

        public void Update(LevelState state)
        {
            if (!init)
            {
                foreach (var faction in state.Factions)
                {
                    Points.Add(faction.PlayerIndex, faction.Points);
                    if (OnNewEvent != null)
                        OnNewEvent(new FactionPointsEntry() { 
                            Round = state.Round, 
                            PlayerIndex = faction.PlayerIndex, 
                            Points = faction.Points 
                        });
                }
                init = true;
            }

            foreach (var faction in state.Factions)
            {
                if (Points[faction.PlayerIndex] != faction.Points)
                {
                    Points[faction.PlayerIndex] = faction.Points;
                    if (OnNewEvent != null)
                        OnNewEvent(new FactionPointsEntry()
                        {
                            Round = state.Round,
                            PlayerIndex = faction.PlayerIndex,
                            Points = faction.Points
                        });
                }
            }
        }

        public event Log.LogEntry OnNewEvent;
    }
}
