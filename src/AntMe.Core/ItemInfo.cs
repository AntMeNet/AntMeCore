using System;

namespace AntMe
{
    /// <summary>
    ///     Base Class for all Info Objects.
    /// </summary>
    public abstract class ItemInfo : PropertyList<ItemInfoProperty>
    {
        /// <summary>
        ///     Reference to the observed Item.
        /// </summary>
        protected readonly Item Item;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Reference to the observed Item.</param>
        protected ItemInfo(Item item)
        {
            Item = item;
        }

        /// <summary>
        ///     Gets the real Distance (Radius to Radius) to the Item.
        /// </summary>
        public float Distance
        {
            get
            {
                Item observer = null; // TODO: Get observer
                return Math.Max(0, GetDistance(observer, Item) - Radius - observer.Radius);
            }
        }

        /// <summary>
        ///     Gets the direction to the Item.
        /// </summary>
        public int Direction
        {
            get
            {
                Item observer = null; // TODO: Get observer
                return GetDirection(observer, Item);
            }
        }

        /// <summary>
        ///     Returns the Item Radius.
        /// </summary>
        public float Radius => Item.Radius;

        /// <summary>
        ///     Returns true of the Item is still alive.
        /// </summary>
        public bool IsAlive => Item.Id > 0;

        /// <summary>
        ///     Internal call to get the related Item.
        /// </summary>
        /// <returns>Related Item</returns>
        internal Item GetItem()
        {
            return Item;
        }

        #region Static Helper

        /// <summary>
        ///     Calculates the Distance between two Items.
        /// </summary>
        /// <param name="item1">Item Info 1</param>
        /// <param name="item2">Item Info 2</param>
        /// <returns></returns>
        private static float GetDistance(ItemInfo item1, ItemInfo item2)
        {
            return GetDistance(item1.Item, item2.Item);
        }

        /// <summary>
        ///     Calculates the Distance between two Items.
        /// </summary>
        /// <param name="item1">Item 1</param>
        /// <param name="item2">Item 2</param>
        /// <returns></returns>
        private static float GetDistance(Item item1, Item item2)
        {
            return Item.GetDistance(item1, item2);
        }

        /// <summary>
        ///     Calculates the Direction from Item 1 to Item 2.
        /// </summary>
        /// <param name="source">Item Info 1</param>
        /// <param name="target">Item Info 2</param>
        /// <returns></returns>
        private static int GetDirection(ItemInfo source, ItemInfo target)
        {
            return Item.GetDirection(source.Item, target.Item).Degree;
        }

        /// <summary>
        ///     Calculates the Direction from Item 1 to Item 2.
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