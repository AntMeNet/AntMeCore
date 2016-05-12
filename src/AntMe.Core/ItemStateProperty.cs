namespace AntMe
{
    /// <summary>
    /// Base Class for all Item State Properties.
    /// </summary>
    public abstract class ItemStateProperty : StateProperty
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public ItemStateProperty() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public ItemStateProperty(Item item, ItemProperty property) : base(property) { }
    }
}
