namespace AntMe
{
    /// <summary>
    /// Base Class for all Faction Property Infos.
    /// </summary>
    public abstract class FactionInfoProperty : InfoProperty
    {
        /// <summary>
        /// Reference to the related Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        /// Reference to the related Property.
        /// </summary>
        protected readonly new FactionProperty Property;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Related Faction</param>
        /// <param name="property">Related Property</param>
        /// <param name="observer">Observer</param>
        public FactionInfoProperty(Faction faction, FactionProperty property, Item observer)
            : base(property, observer)
        {
            Faction = faction;
            Property = property;
        }
    }
}
