namespace AntMe
{
    /// <summary>
    /// Base Class for Item Property Infos.
    /// </summary>
    public abstract class ItemInfoProperty : InfoProperty
    {
        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        protected readonly Item Item;

        /// <summary>
        /// Reference to the related Property.
        /// </summary>
        protected readonly new ItemProperty Property;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Item</param>
        /// <param name="property">Related Property</param>
        /// <param name="observer">Observer</param>
        public ItemInfoProperty(Item item, ItemProperty property, Item observer) : base(property, observer)
        {
            Item = item;
            Property = property;
        }
    }
}
