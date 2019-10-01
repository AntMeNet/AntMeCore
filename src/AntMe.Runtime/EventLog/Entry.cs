namespace AntMe.Runtime.EventLog
{
    /// <summary>
    ///     Basisklasse eines Log-Eintrags
    /// </summary>
    public abstract class Entry
    {
        public int Round { get; set; }
    }
}