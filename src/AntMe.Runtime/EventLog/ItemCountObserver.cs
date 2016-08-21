using System.Collections.Generic;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Überwacht den Stream auf neue oder entfernte Items.
    /// </summary>
    public sealed class ItemCountObserver : ILogObserver
    {
        private List<int> ids = new List<int>();

        public void Update(Frame state)
        {
            List<int> updated = new List<int>();

            // Neue Items finden
            foreach (var item in state.Items)
            {
                // Neues Item gefunden.
                if (!ids.Contains(item.Id))
                {
                    if (item is FactionItemState)
                    {
                        // Faction-Item
                        var fi = item as FactionItemState;
                        OnNewEvent?.Invoke(new AddFactionItemEntry()
                        {
                            Round = state.Round,
                            Id = fi.Id,
                            SlotIndex = fi.SlotIndex
                        });
                    }
                    else
                    {
                        // Factionloses Item
                        OnNewEvent?.Invoke(new AddItemEntry()
                        {
                            Round = state.Round,
                            Id = item.Id
                        });
                    }

                    ids.Add(item.Id);
                }

                updated.Add(item.Id);
            }

            // Entfernte Items identifizieren
            foreach (var id in ids)
            {
                if (!updated.Contains(id))
                {
                    // Item nicht mehr vorhanden
                    OnNewEvent?.Invoke(new RemoveItemEntry()
                    {
                        Round = state.Round,
                        Id = id
                    });
                }
            }
        }

        public event Log.LogEntry OnNewEvent;
    }
}
