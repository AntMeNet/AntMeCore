using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class PortableState : ItemStateProperty
    {
        [DisplayName("Weight")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public float Weight { get; set; }

        public PortableState() : base() { }

        public PortableState(Item item, PortableProperty property) : base(item, property)
        {
            Weight = property.PortableWeight;
            property.OnPortableWeightChanged += (i, v) => { Weight = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Weight);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Weight);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Weight = stream.ReadSingle();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Weight = stream.ReadSingle();
        }
    }
}
