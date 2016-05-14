namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Item Property for all Collectable Sugar Items.
    /// </summary>
    public sealed class SugarCollectableProperty : CollectableProperty<SugarCollectorProperty>
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public SugarCollectableProperty(Item item) : base(item) { }
    }
}