namespace AntMe
{
    /// <summary>
    /// Basisklasse für alle Faction Infos
    /// </summary>
    public abstract class FactionInfo : PropertyList<InfoProperty>
    {
        /// <summary>
        /// Konstruktor der Faction-Info.
        /// </summary>
        /// <param name="faction"></param>
        /// <param name="observer"></param>
        public FactionInfo(Faction faction, Item observer)
        {
            // TODO: Info Properties laden
        }
    }
}
