namespace AntMe
{
    /// <summary>
    ///     Base Class for Item Property Infos.
    /// </summary>
    public abstract class ItemInfoProperty : InfoProperty
    {
        /// <summary>
        ///     Reference to the related Item.
        /// </summary>
        protected readonly Item Item;

        /// <summary>
        ///     Reference to the related Property.
        /// </summary>
        protected new readonly ItemProperty Property;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Item</param>
        /// <param name="property">Related Property</param>
        protected ItemInfoProperty(Item item, ItemProperty property) : base(property)
        {
            Item = item;
            Property = property;
        }
    }
}