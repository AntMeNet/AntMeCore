using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base State Class for all Faction Items. 
    /// </summary>
    public abstract class FactionItemState : ItemState
    {
        /// <summary>
        /// Slot Index
        /// </summary>
        [DisplayName("Slot Index")]
        [Description("Slot Index")]
        [ReadOnly(true)]
        [Category("Static")]
        public byte SlotIndex { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public FactionItemState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        public FactionItemState(FactionItem item) : base(item)
        {
            SlotIndex = item.Faction.SlotIndex;
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
            stream.Write(SlotIndex);
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
            SlotIndex = stream.ReadByte();
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

        /// <summary>
        /// Returns a representive String for this State.
        /// </summary>
        /// <returns>State Description</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1}/{2})", GetType().Name, Id, SlotIndex);
        }
    }
}