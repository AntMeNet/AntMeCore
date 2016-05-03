using AntMe.ItemProperties.Basics;

namespace AntMe.ItemProperties.Basics
{
    public sealed class SnifferState : ItemStateProperty
    {
        public SnifferState() : base() { }

        public SnifferState(SnifferProperty property) : base(property) { }

        public override void SerializeFirst(System.IO.BinaryWriter stream, byte version)
        {
        }

        public override void SerializeUpdate(System.IO.BinaryWriter stream, byte version)
        {
        }

        public override void DeserializeFirst(System.IO.BinaryReader stream, byte version)
        {
        }

        public override void DeserializeUpdate(System.IO.BinaryReader stream, byte version)
        {
        }
    }
}
