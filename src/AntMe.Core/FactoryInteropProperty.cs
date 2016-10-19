namespace AntMe
{
    /// <summary>
    /// Base Class for properties of Factoy Interops.
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
        /// Constructor of the Factory Interop Properties.
        /// </summary>
        /// <param name="faction">Reference to Faction.</param>
        /// <param name="interop">Reference to the Interop.</param>
        public FactoryInteropProperty(Faction faction, FactoryInterop interop)
        {
            Faction = faction;
            Interop = interop;
        }
    }
}
