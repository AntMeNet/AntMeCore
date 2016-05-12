namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    /// State Property for all sniffing Items.
    /// </summary>
    public sealed class SnifferState : ItemStateProperty
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public SnifferState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public SnifferState(Item item, SnifferProperty property) : base(item, property) { }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(System.IO.BinaryWriter stream, byte version)
        {
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(System.IO.BinaryWriter stream, byte version)
        {
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(System.IO.BinaryReader stream, byte version)
        {
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(System.IO.BinaryReader stream, byte version)
        {
        }
    }
}
