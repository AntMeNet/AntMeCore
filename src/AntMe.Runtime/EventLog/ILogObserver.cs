namespace AntMe.Runtime.EventLog
{
    /// <summary>
    ///     Interface für alle Observer, die im EventLog aktiv werden wollen.
    /// </summary>
    public interface ILogObserver
    {
        /// <summary>
        ///     Methode zur Übergabe des aktuellen Simulationsstands.
        /// </summary>
        /// <param name="state"></param>
        void Update(LevelState state);

        /// <summary>
        ///     Benachrichtigt das Log über neue Events.
        /// </summary>
        event Log.LogEntry OnNewEvent;
    }
}