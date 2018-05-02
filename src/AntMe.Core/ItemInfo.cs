using System;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Info Objects.
    /// </summary>
    public class ItemInfo : PropertyList<ItemInfoProperty>
    {
        /// <summary>
        /// Reference to the observed Item.
        /// </summary>
        protected readonly Item Item;

        /// <summary>
        /// Reference to the observing Item.
        /// </summary>
        protected readonly Item Observer;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Reference to the observed Item.</param>
        /// <param name="observer">Reference to the observing Item.</param>
        public ItemInfo(Item item, Item observer)
        {
            Item = item;
            Observer = observer;
        }

        /// <summary>
        /// Gets the real Distance (Radius to Radius) to the Item.
        /// </summary>
        public float Distance => Math.Max(0, GetDistance(Observer, Item) - Radius - Observer.Radius);

        /// <summary>
        /// Gets the direction to the Item.
        /// </summary>
        public int Direction => GetDirection(Observer, Item);

        /// <summary>
        /// Returns the Item Radius.
        /// </summary>
        public float Radius => Item.Radius;

        /// <summary>
        /// Returns true of the Item is still alive.
        /// </summary>
        public bool IsAlive => Item.Id > 0;

        /// <summary>
        /// Internal call to get the related Item.
        /// </summary>
        /// <returns>Related Item</returns>
        internal Item GetItem()
        {
            return Item;
        }

        /// <summary>
        /// Generates a unique Hash Code for this Info.
        /// </summary>
        /// <returns>Hash Code</returns>
        public override int GetHashCode()
        {
            return Item.Id.GetHashCode() + 
                Observer.Id.GetHashCode();
        }

        /// <summary>
        /// Compares two Info Items.
        /// </summary>
        /// <param name="obj">Other Item</param>
        /// <returns>Is this the same Info?</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ItemInfo))
                return false;

            var other = (ItemInfo) obj;
            return Item == other.Item && 
                Observer == other.Observer;
        }
        
        #region Static Helper

        /// <summary>
        /// Calculates the Distance between two Items.
        /// </summary>
        /// <param name="item1">Item 1</param>
        /// <param name="item2">Item 2</param>
        /// <returns></returns>
        private static float GetDistance(Item item1, Item item2)
        {
            return Item.GetDistance(item1, item2);
        }

        /// <summary>
        /// Calculates the Direction from Item 1 to Item 2.
        /// </summary>
        /// <param name="source">Item 1</param>
        /// <param name="target">Item 2</param>
        /// <returns></returns>
        private static int GetDirection(Item source, Item target)
        {
            return Item.GetDirection(source, target).Degree;
        }

        #endregion
        
    }
}