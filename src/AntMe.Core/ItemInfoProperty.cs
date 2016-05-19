namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle Info Eigenschaften
    /// </summary>
    public abstract class ItemInfoProperty : InfoProperty
    {
        protected readonly Item Item;

        protected readonly ItemProperty Property;

        protected readonly Item Observer;

        public ItemInfoProperty(Item item, ItemProperty property, Item observer)
        {
            Item = item;
            Property = property;
            Observer = observer;
        }
    }
}
