using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class CollidableState : ItemStateProperty
    {
        [DisplayName("Fixed Mass")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public bool Fixed { get; set; }

        [DisplayName("Mass")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public float Mass { get; set; }

        public CollidableState() : base() { }

        public CollidableState(CollidableProperty property) : base(property)
        {
            Fixed = property.CollisionFixed;
            Mass = property.CollisionMass;
            property.OnCollisionFixedChanged += (i, v) => { Fixed = v; };
            property.OnCollisionMassChanged += (i, v) => { Mass = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Fixed);
            stream.Write(Mass);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Fixed);
            stream.Write(Mass);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Fixed = stream.ReadBoolean();
            Mass = stream.ReadSingle();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Fixed = stream.ReadBoolean();
            Mass = stream.ReadSingle();
        }
    }
}
