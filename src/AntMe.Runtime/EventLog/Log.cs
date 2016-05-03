using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Klasse zur Beobachtung aller wichtigen Events innerhalb der Simulation.
    /// </summary>
    public sealed class Log
    {
        private List<ILogObserver> observers = new List<ILogObserver>();
        private List<Entry> entries = new List<Entry>();

        public static Log CreateLog(bool autoregister)
        {
            Log result = new Log();

            if (autoregister)
            {
                // TODO: Finde alle registierten Observer
                // result.Register(new FactionPointsObserver());
                // result.Register(new ItemCountObserver());
            }

            return result;
        }

        private Log()
        {
        }

        /// <summary>
        /// Gibt alle bisher angefallenen Events zurück.
        /// </summary>
        public ReadOnlyCollection<Entry> Entries 
        { 
            get { return entries.AsReadOnly(); } 
        }

        /// <summary>
        /// Registriert neue Observer im Event-Log.
        /// </summary>
        /// <param name="observer"></param>
        public void Register(ILogObserver observer)
        {
            observers.Add(observer);
            observer.OnNewEvent += observer_OnNewEvent;
        }

        private void observer_OnNewEvent(Entry entry)
        {
            entries.Add(entry);

            if (OnLogEvent != null)
                OnLogEvent(entry);
        }

        /// <summary>
        /// Führt ein Update des Logs durch.
        /// </summary>
        /// <param name="state"></param>
        public void Update(LevelState state)
        {
            foreach (var observer in observers)
                observer.Update(state);
        }

        /// <summary>
        /// Informiert über neue Events.
        /// </summary>
        public event LogEntry OnLogEvent;

        public delegate void LogEntry(Entry entry);
    }
}
