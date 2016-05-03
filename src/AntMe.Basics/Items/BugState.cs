using System.IO;

namespace AntMe.Items.Basics
{
    public sealed class BugState : ItemState
    {
        public BugState() : base() { }

        public BugState(BugItem item) : base(item)
        {

        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
        }
    }
}
