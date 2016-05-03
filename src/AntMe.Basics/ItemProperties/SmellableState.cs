using System;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class SmellableState : ItemStateProperty
    {
        public SmellableState() : base() { }

        public SmellableState(Item item, SmellableProperty property) : base(item, property)
        {

        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }
}
