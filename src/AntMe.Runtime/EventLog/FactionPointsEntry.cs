namespace AntMe.Runtime.EventLog
{
    /// <summary>
    ///     Überträgt Punktänderungen bei den Factions.
    /// </summary>
    public sealed class FactionPointsEntry : FactionEntry
    {
        public int Points { get; set; }
    }
}