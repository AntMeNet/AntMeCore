using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Base Class for all Collectable Properties.
    /// </summary>
    public abstract class CollectableProperty : GoodsProperty
    {
        private float collectableRadius;

        /// <summary>
        /// Gets the fitting Type of Collector Properties.
        /// </summary>
        public Type AcceptedCollectorType { get; private set; }

        /// <summary>
        /// Gets or sets the Collectable Radius.
        /// </summary>
        public float CollectableRadius
        {
            get { return collectableRadius; }
            set
            {
                collectableRadius = Math.Max(value, 0f);
                if (OnCollectableRadiusChanged != null)
                    OnCollectableRadiusChanged(Item, collectableRadius);
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="collectorType">Type of fitting Collector</param>
        public CollectableProperty(Item item, Type collectorType) : base(item)
        {
            CollectableRadius = item.Radius;
            AcceptedCollectorType = collectorType;
        }

        /// <summary>
        /// Internal call to take a requested amount.
        /// </summary>
        /// <param name="collector">Collecting Item Property</param>
        /// <param name="request">Requested Amount</param>
        /// <returns>Actual Amount</returns>
        internal int Take(CollectorProperty collector, int request)
        {
            // Be sure it's a positive Request
            request = Math.Max(0, request);

            // Check how many can be taken
            int collected = Math.Max(0, Math.Min(request, Amount));

            // Decrease requested amount
            if (collected > 0)
                Amount -= collected;
            return collected;
        }

        /// <summary>
        /// Internal call to give a requested amount.
        /// </summary>
        /// <param name="collector">Giving Collector Item Property</param>
        /// <param name="amount">Requested Amount</param>
        /// <returns>Actual Amount</returns>
        internal int Give(CollectorProperty collector, int amount)
        {
            // Check for available Space
            int given = Math.Min(amount, Capacity - Amount);

            // Increase requested amount
            if (given > 0)
                Amount += given;
            return given;
        }

        /// <summary>
        /// Signal for a changed Collectable Radius.
        /// </summary>
        public event ValueChanged<float> OnCollectableRadiusChanged;
    }

    /// <summary>
    /// Base Class for all Collectable Properties.
    /// </summary>
    /// <typeparam name="T">Type of fitting Collector Property</typeparam>
    public abstract class CollectableProperty<T> : CollectableProperty where T : CollectorProperty
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public CollectableProperty(Item item) : base(item, typeof(T)) { }
    }
}