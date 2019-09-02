namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse für Faction-bezogene Event Einträge.
    /// </summary>
    public abstract class FactionEntry : Entry
    {
        public int SlotIndex { get; set; }
    }
}
