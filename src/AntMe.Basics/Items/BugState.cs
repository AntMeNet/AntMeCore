using System.IO;

namespace AntMe.Items.Basics
{
    /// <summary>
    /// State for a Bug.
    /// </summary>
    public sealed class BugState : ItemState
    {
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public BugState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper (Faction Bug).
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        public BugState(BugItem item) : base(item)
        {

        }

        /// <summary>
        /// Default Constructor for the Type Mapper (Classic Bug).
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        public BugState(ClassicBugItem item) : base(item)
        {

        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
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
