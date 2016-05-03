using AntMe.ItemProperties.Basics;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class VisibleState : ItemStateProperty
    {
        public VisibleState() : base() { }

        public VisibleState(Item item, VisibleProperty property) : base(item, property)
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
