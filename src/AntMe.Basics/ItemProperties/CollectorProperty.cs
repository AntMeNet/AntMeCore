using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     Base Class for all Collector Properties.
    /// </summary>
    public abstract class CollectorProperty : GoodsProperty
    {
        private float collectorRange;

        /// <summary>
        ///     Gets or sets the Collectable Radius.
        /// </summary>
        public CollectorProperty(Item item, Type collectionType) : base(item)
        {
            CollectorRange = item.Radius;
            AcceptedCollectableType = collectionType;
        }

        /// <summary>
        ///     Gets the fitting Type of Collectable Properties.
        /// </summary>
        public Type AcceptedCollectableType { get; }

        /// <summary>
        ///     Gets or sets the Collectors Range.
        /// </summary>
        public float CollectorRange
        {
            get => collectorRange;
            set
            {
                collectorRange = Math.Max(value, 0f);
                OnCollectorRangeChanged?.Invoke(Item, collectorRange);
            }
        }

        /// <summary>
        ///     Takes the requested amount from the Property.
        /// </summary>
        /// <param name="property">Source Property</param>
        /// <param name="amount">Requested Amount</param>
        /// <returns>Actual Amount</returns>
        public int Take(CollectableProperty property, int amount)
        {
            // Check for Right Collectable Type
            if (property.GetType() != AcceptedCollectableType)
                throw new NotSupportedException("Not supported Collectable Property");

            // Ensure both Items are Part of the same Engine
            if (Item.Engine == null)
                throw new NotSupportedException("Collector is not part of the Engine");
            if (property.Item.Engine == null)
                throw new NotSupportedException("Collectable is not part of the Engine");
            if (Item.Engine != property.Item.Engine)
                throw new NotSupportedException("Collector and Collectable are not part of the same Engine");

            // Check the right distance
            if (Item.GetDistance(Item, property.Item) > CollectorRange + property.CollectableRadius)
                return 0;

            // Cap to the available Capacity
            amount = Math.Min(amount, Capacity - Amount);

            // Request Good
            var result = property.Take(this, amount);

            // Finalize transfer.
            Amount += result;

            return result;
        }

        /// <summary>
        ///     Give the given amount to the given Destination.
        /// </summary>
        /// <param name="property">Destination Property</param>
        /// <param name="amount">Requested Amount</param>
        /// <returns>Actual Amount</returns>
        public int Give(CollectableProperty property, int amount)
        {
            // Check for Right Collectable Type
            if (property.GetType() != AcceptedCollectableType)
                throw new NotSupportedException("Not supported Collectable Property");

            // Ensure both Items are Part of the same Engine
            if (Item.Engine == null)
                throw new NotSupportedException("Collector is not part of the Engine");
            if (property.Item.Engine == null)
                throw new NotSupportedException("Collectable is not part of the Engine");
            if (Item.Engine != property.Item.Engine)
                throw new NotSupportedException("Collector and Collectable are not part of the same Engine");

            // Distanz überprüfen
            if (Item.GetDistance(Item, property.Item) > CollectorRange + property.CollectableRadius)
                return 0;

            // Check for available Amount
            amount = Math.Min(amount, Amount);

            // Request
            var result = property.Give(this, amount);

            // Finalize transfer
            Amount -= result;

            return result;
        }

        /// <summary>
        ///     Signal for a changed Collector Range.
        /// </summary>
        public event ValueChanged<float> OnCollectorRangeChanged;
    }

    /// <summary>
    ///     Base Class for all Collector Properties.
    /// </summary>
    /// <typeparam name="T">Type of fitting Collectable Property</typeparam>
    public abstract class CollectorProperty<T> : CollectorProperty where T : CollectableProperty
    {
        /// <summary>
        ///     Gets or sets the Collectable Radius.
        /// </summary>
        public CollectorProperty(Item item) : base(item, typeof(T))
        {
        }
    }
}