using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State Property for all walking Items.
    /// </summary>
    public sealed class WalkingState : ItemStateProperty
    {
        /// <summary>
        /// Maximum Speed.
        /// </summary>
        [DisplayName("Maximum Speed")]
        [Description("Maximum Speed")]
        [ReadOnly(true)]
        [Category("Static")]
        public float MaximumSpeed { get; set; }

        /// <summary>
        /// Current Speed.
        /// </summary>
        [DisplayName("Speed")]
        [Description("Current Speed.")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float Speed { get; set; }

        /// <summary>
        /// Current Direction.
        /// </summary>
        [DisplayName("Direction")]
        [Description("Current Direction.")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public short Direction { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public WalkingState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public WalkingState(Item item, WalkingProperty property) : base(item, property)
        {
            // Bind Speed to the Item Speed
            Speed = property.Speed;
            property.OnMoveSpeedChanged += (i, v) => { Speed = v; };

            // Bind Maximum Speed to the Item Maximum Speed
            MaximumSpeed = property.MaximumSpeed;
            property.OnMaximumMoveSpeedChanged += (i, v) => { MaximumSpeed = v; };

            // Bind Direction to the Item Direction
            Direction = (short)property.Direction.Degree;
            property.OnMoveDirectionChanged += (i, v) => { Direction = (short)v.Degree; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumSpeed);
            stream.Write(Speed);
            stream.Write(Direction);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumSpeed);
            stream.Write(Speed);
            stream.Write(Direction);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            MaximumSpeed = stream.ReadSingle();
            Speed = stream.ReadSingle();
            Direction = stream.ReadInt16();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            MaximumSpeed = stream.ReadSingle();
            Speed = stream.ReadSingle();
            Direction = stream.ReadInt16();
        }
    }
}
