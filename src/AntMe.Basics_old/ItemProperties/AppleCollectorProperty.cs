namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Item Property for all Apple Collector Items.
    /// </summary>
    public sealed class AppleCollectorProperty : CollectorProperty<AppleCollectableProperty>
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public AppleCollectorProperty(Item item) : base(item) { }
    }
}
