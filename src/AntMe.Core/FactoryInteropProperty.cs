namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für Properties von Factory Interops.
    /// </summary>
    public abstract class FactoryInteropProperty : InteropProperty
    {
        /// <summary>
        /// Related Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        /// Related Game Item.
        /// </summary>
        protected readonly FactionItem Item;

        /// <summary>
        /// Basic Interop.
        /// </summary>
        protected readonly FactoryInterop Interop;

        /// <summary>
        /// Konstruktor des Factory Interop Properties.
        /// </summary>
        /// <param name="faction">Referenz auf die Faction.</param>
        /// <param name="item"></param>
        /// <param name="interop">Referenz auf das Interop.</param>
        public FactoryInteropProperty(Faction faction, FactionItem item, FactoryInterop interop)
        {
            Faction = faction;
            Item = item;
            Interop = interop;
        }
    }
}
