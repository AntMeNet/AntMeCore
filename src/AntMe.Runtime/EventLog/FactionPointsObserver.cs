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
                    Points.Add(faction.SlotIndex, faction.Points);
                    if (OnNewEvent != null)
                        OnNewEvent(new FactionPointsEntry() { 
                            Round = state.Round, 
                            SlotIndex = faction.SlotIndex, 
                            Points = faction.Points 
                        });
                }
                init = true;
            }

            foreach (var faction in state.Factions)
            {
                if (Points[faction.SlotIndex] != faction.Points)
                {
                    Points[faction.SlotIndex] = faction.Points;
                    if (OnNewEvent != null)
                        OnNewEvent(new FactionPointsEntry()
                        {
                            Round = state.Round,
                            SlotIndex = faction.SlotIndex,
                            Points = faction.Points
                        });
                }
            }
        }

        public event Log.LogEntry OnNewEvent;
    }
}
