using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class WalkingState : ItemStateProperty
    {
        /// <summary>
        /// Gibt die maximale Geschwindigkeit
        /// </summary>
        [DisplayName("Maximum Speed")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public float MaximumSpeed { get; set; }

        /// <summary>
        /// Gibt die aktuelle Geschwindigkeit.
        /// </summary>
        [DisplayName("Speed")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float Speed { get; set; }

        /// <summary>
        /// Gibt die aktuelle Laufrichtung an.
        /// </summary>
        [DisplayName("Direction")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public short Direction { get; set; }

        public WalkingState() : base() { }

        public WalkingState(WalkingProperty property) : base(property)
        {
            Speed = property.Speed;
            MaximumSpeed = property.MaximumSpeed;
            Direction = (short)property.Direction.Degree;
            property.OnMoveSpeedChanged += (i, v) => { Speed = v; };
            property.OnMaximumMoveSpeedChanged += (i, v) => { MaximumSpeed = v; };
            property.OnMoveDirectionChanged += (i, v) => { Direction = (short)v.Degree; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumSpeed);
            stream.Write(Speed);
            stream.Write(Direction);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumSpeed);
            stream.Write(Speed);
            stream.Write(Direction);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            MaximumSpeed = stream.ReadSingle();
            Speed = stream.ReadSingle();
            Direction = stream.ReadInt16();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            MaximumSpeed = stream.ReadSingle();
            Speed = stream.ReadSingle();
            Direction = stream.ReadInt16();
        }
    }
}
