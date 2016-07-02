namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Item Property for all Apple Collector Items 
    /// </summary>
    public sealed class AppleCollectableProperty : CollectableProperty<AppleCollectorProperty>
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public AppleCollectableProperty(Item item) : base(item, "Apple")
        {
            // Bind Points
            // TODO: Settings
            Points = Amount;
            OnAmountChanged += (i, v) => { Points = v; };

        }

    }
}
