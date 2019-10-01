namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     State for all Apple Collector Properties.
    /// </summary>
    public sealed class AppleCollectorState : CollectorState
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public AppleCollectorState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public AppleCollectorState(Item item, AppleCollectorProperty property) : base(item, property)
        {
        }
    }
}