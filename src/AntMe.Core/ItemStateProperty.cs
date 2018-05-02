namespace AntMe
{
    /// <summary>
    /// Base Class for all Item State Properties.
    /// </summary>
    public abstract class ItemStateProperty : StateProperty
    {
        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        protected readonly Item Item;

        /// <summary>
        /// Reference to the Property.
        /// </summary>
        protected new readonly ItemProperty Property;

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        protected ItemStateProperty() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        protected ItemStateProperty(Item item, ItemProperty property) : base(property)
        {
            Item = item;
            Property = property;
        }
    }
}
