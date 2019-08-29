namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State Property for all Sugar Collector Items.
    /// </summary>
    public sealed class SugarCollectorState : CollectorState
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public SugarCollectorState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public SugarCollectorState(Item item, SugarCollectorProperty property) : base(item, property) { }
    }
}
