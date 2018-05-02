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
        protected FactionItemState() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        protected FactionItemState(FactionItem item) : base(item)
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
        /// Returns a representive String for this State.
        /// </summary>
        /// <returns>State Description</returns>
        public override string ToString()
        {
            return $"{GetType().Name} ({Id}/{SlotIndex})";
        }
    }
}