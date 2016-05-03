using AntMe.ItemProperties.Basics;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class VisibleState : ItemStateProperty
    {
        public VisibleState() : base() { }

        public VisibleState(VisibleProperty property) : base(property)
        {

        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }
    }
}
