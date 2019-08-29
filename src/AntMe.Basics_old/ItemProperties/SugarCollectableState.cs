namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State Property for all Collectable Sugar Properties.
    /// </summary>
    public sealed class SugarCollectableState : CollectableState
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public SugarCollectableState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public SugarCollectableState(Item item, SugarCollectableProperty property) : base(item, property) { }
    }
}
