using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.Items
{
    /// <summary>
    /// State for the Ant Item.
    /// </summary>
    public sealed class AntState : FactionItemState
    {
        /// <summary>
        /// Current Ant Mode.
        /// </summary>
        [DisplayName("Mode")]
        [Description("Current Ant Mode")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public AntStateMode Mode { get; set; }

        /// <summary>
        /// Ant Name
        /// </summary>
        [DisplayName("Name")]
        [Description("Ant Name")]
        [ReadOnly(true)]
        [Category("Static")]
        public string Name { get; set; }

        /// <summary>
        /// Ant Caste.
        /// </summary>
        [DisplayName("Caste")]
        [Description("Ant Caste")]
        [ReadOnly(true)]
        [Category("Static")]
        public string Caste { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public AntState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        public AntState(AntItem item) : base(item)
        {
            Name = item.Name;
            Caste = string.Empty; // TODO: Copy Caste Name as soon the Ant holds a Caste name again
            Mode = AntStateMode.Idle; // TODO: Handle this right :)
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);

            stream.Write(Caste);
            stream.Write(Name);
            stream.Write((byte)Mode);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);

            stream.Write((byte)Mode);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);

            Caste = stream.ReadString();
            Name = stream.ReadString();
            Mode = (AntStateMode)stream.ReadByte();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);

            Mode = (AntStateMode)stream.ReadByte();
        }
    }

    /// <summary>
    /// List of possible Ant Modes.
    /// </summary>
    public enum AntStateMode
    {
        /// <summary>
        /// Ant is in Idle Mode.
        /// </summary>
        Idle = 0,

        /// <summary>
        /// Ant walks.
        /// </summary>
        Walk = 1,

        /// <summary>
        /// Ant Fights.
        /// </summary>
        Fight = 2,

        /// <summary>
        /// Ant carries something.
        /// </summary>
        Carry = 3
    }
}