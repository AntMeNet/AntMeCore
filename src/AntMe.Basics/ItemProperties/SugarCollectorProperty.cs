namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Item Property for all Sugar Collector Items.
    /// </summary>
    public sealed class SugarCollectorProperty : CollectorProperty<SugarCollectableProperty>
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public SugarCollectorProperty(Item item) : base(item) { }
    }
}
