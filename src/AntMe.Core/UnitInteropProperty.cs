namespace AntMe
{
    /// <summary>
    /// Base Class for all Unit Interop Properties.
    /// </summary>
    public abstract class UnitInteropProperty : InteropProperty
    {
        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        protected readonly FactionItem Item;

        /// <summary>
        /// Reference to the related Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        /// Reference to the main Interop.
        /// </summary>
        protected readonly UnitInterop Interop;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        /// <param name="interop">UnitInterop</param>
        public UnitInteropProperty(Faction faction, FactionItem item, UnitInterop interop)
        {
            Faction = faction;
            Item = item;
            Interop = interop;
        }
    }
}
