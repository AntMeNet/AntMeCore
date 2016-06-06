using System;
using System.IO;

namespace AntMe
{
    public class MapTileStateProperty : StateProperty
    {
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
