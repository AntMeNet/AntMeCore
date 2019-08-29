namespace AntMe
{
    /// <summary>
    /// Base Class for all Item Properties.
    /// </summary>
    public abstract class ItemProperty : Property
    {
        private readonly Item item;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Reference to the related Item.</param>
        public ItemProperty(Item item)
        {
            this.item = item;
        }

        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        public Item Item { get { return item; } }
    }
}