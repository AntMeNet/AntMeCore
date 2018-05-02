namespace AntMe
{
    /// <summary>
    /// Base Class for all Item Properties.
    /// </summary>
    public abstract class ItemProperty : Property
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Reference to the related Item.</param>
        protected ItemProperty(Item item)
        {
            Item = item;
        }

        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        public Item Item { get; }
    }
}