namespace AntMe
{
    /// <summary>
    /// Base Class for all Faction related Game Items.
    /// </summary>
    public abstract class FactionItem : Item
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="resolver">Reference to the default Type Resolver</param>
        /// <param name="faction">Reference to the related Faction</param>
        /// <param name="position">First Position of this Item</param>
        /// <param name="radius">Radius of this Item</param>
        /// <param name="orientation">First Orientation of this Item</param>
        public FactionItem(ITypeResolver resolver, Faction faction, Vector2 position, float radius, Angle orientation)
            : base(resolver, position, radius, orientation)
        {
            Faction = faction;
        }

        /// <summary>
        /// Returns a reference to the related Faction.
        /// </summary>
        public Faction Faction { get; private set; }
    }
}