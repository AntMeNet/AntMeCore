namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse für Item-bezogene Event im Faction-Kontext.
    /// </summary>
    public abstract class FactionItemEntry : ItemEntry
    {
        public int SlotIndex { get; set; }
    }
}
