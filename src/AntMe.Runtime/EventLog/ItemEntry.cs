namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse für Item-bezogene Einträge.
    /// </summary>
    public abstract class ItemEntry : Entry
    {
        public int Id { get; set; }
    }
}
