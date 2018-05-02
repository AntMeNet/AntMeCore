namespace AntMe
{
    /// <summary>
    /// Base Class for all Faction Property States.
    /// </summary>
    public abstract class FactionStateProperty : StateProperty
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        protected FactionStateProperty() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Related Faction</param>
        /// <param name="property">Related Faction Property</param>
        protected FactionStateProperty(Faction faction, FactionProperty property) : base(property) { }
    }
}
