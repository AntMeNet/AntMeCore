namespace AntMe
{
    /// <summary>
    /// Basisklasse für alle fraktionsbezogenen AntMe! Spielelemente, die vom Level
    /// verwaltet werden. Für unabhängige Elemente existiert Item.
    /// </summary>
    public abstract class FactionItem : Item
    {
        /// <summary>
        /// Standard Konstruktor
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="faction"></param>
        /// <param name="position"></param>
        /// <param name="orientation"></param>
        public FactionItem(ITypeResolver resolver, Faction faction, Vector2 position, Angle orientation)
            : base(resolver, position, orientation)
        {
            Faction = faction;
        }

        /// <summary>
        /// Liefert eine Referenz auf die umgebende Faction zurück.
        /// </summary>
        public Faction Faction { get; private set; }
    }
}