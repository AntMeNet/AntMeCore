using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.Items
{
    /// <summary>
    /// State for a Marker.
    /// </summary>
    public sealed class MarkerState : FactionItemState
    {
        /// <summary>
        /// Containing Marker Information.
        /// </summary>
        [DisplayName("Information")]
        [Description("Containing Marker Information")]
        [ReadOnly(true)]
        [Category("Static")]
        public int Information { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public MarkerState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        public MarkerState(MarkerItem item) : base(item)
        {
            // Transfer Information
            Information = item.Information;
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);

            stream.Write(Information);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);

            Information = stream.ReadInt32();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
        }
    }
}