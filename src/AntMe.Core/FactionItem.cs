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
        /// <param name="context">Simulation Context</param>
        /// <param name="faction">Reference to the related Faction</param>
        /// <param name="position">First Position of this Item</param>
        /// <param name="radius">Radius of this Item</param>
        /// <param name="orientation">First Orientation of this Item</param>
        public FactionItem(SimulationContext context, Faction faction, Vector2 position, float radius, Angle orientation)
            : this(context, null, faction, position, radius, orientation)
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="attributes">List of Unit Attributes</param>
        /// <param name="faction">Reference to the related Faction</param>
        /// <param name="position">First Position of this Item</param>
        /// <param name="radius">Radius of this Item</param>
        /// <param name="orientation">First Orientation of this Item</param>
        public FactionItem(SimulationContext context, UnitAttributeCollection attributes, Faction faction, Vector2 position, float radius, Angle orientation)
            : base(context, attributes, position, radius, orientation)
        {
            Faction = faction;
        }

        /// <summary>
        /// Returns a reference to the related Faction.
        /// </summary>
        public Faction Faction { get; private set; }
    }
}