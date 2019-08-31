
namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State for a Apple Collectable Properties.
    /// </summary>
    public sealed class AppleCollectableState : CollectableState
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public AppleCollectableState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public AppleCollectableState(Item item, AppleCollectableProperty property) : base(item, property) { }
    }
}
