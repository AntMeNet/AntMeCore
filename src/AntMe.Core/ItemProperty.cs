using System;

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

        /// <summary>
        /// Gets a Call before the Update Process.
        /// </summary>
        public virtual void OnBeforeUpdate() { }

        /// <summary>
        /// Gets a Call during Update Process.
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// Gets a Call after Update Process.
        /// </summary>
        public virtual void OnAfterUpdate() { }
    }
}