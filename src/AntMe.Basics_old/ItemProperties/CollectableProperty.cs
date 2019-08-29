using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Base Class for all Collectable Properties.
    /// </summary>
    public abstract class CollectableProperty : GoodsProperty, IPointsCollector
    {
        private bool enabled = true;

        private int points = 0;

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
                OnCollectableRadiusChanged?.Invoke(Item, collectableRadius);
            }
        }

        /// <summary>
        /// Returns the Points Category.
        /// </summary>
        public string PointsCategory { get; private set; }

        /// <summary>
        /// Defines if the Points should count.
        /// </summary>
        public bool EnablePoints
        {
            get { return enabled; }
            set
            {
                enabled = value;
                OnEnablePointsChanged?.Invoke(this, enabled);
            }
        }

        /// <summary>
        /// Defines of the Counter will be removed after Item Death.
        /// </summary>
        public bool PermanentPoints { get; set; }

        /// <summary>
        /// Returns the current Amount of Points.
        /// </summary>
        public int Points
        {
            get { return points; }
            protected set
            {
                points = value;
                OnPointsChanged?.Invoke(this, points);
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="collectorType">Type of fitting Collector</param>
        /// <param name="pointCategory">Name of the Point Category</param>
        public CollectableProperty(Item item, Type collectorType, string pointCategory) : base(item)
        {
            PointsCategory = pointCategory;
            CollectableRadius = item.Radius;
            AcceptedCollectorType = collectorType;

            EnablePoints = true;
            PermanentPoints = false;
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

        /// <summary>
        /// Signal for changed Enable Flag.
        /// </summary>
        public event ValueUpdate<IPointsCollector, bool> OnEnablePointsChanged;

        /// <summary>
        /// Signal for a changed Point Counter.
        /// </summary>
        public event ValueUpdate<IPointsCollector, int> OnPointsChanged;
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
        /// <param name="pointCategory">Name of the Point Category</param>
        public CollectableProperty(Item item, string pointCategory)
            : base(item, typeof(T), pointCategory) { }
    }
}