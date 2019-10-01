namespace AntMe.Basics.Items
{
    /// <summary>
    ///     State for an Apple Item.
    /// </summary>
    public sealed class AppleState : ItemState
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public AppleState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        public AppleState(AppleItem item) : base(item)
        {
        }
    }
}