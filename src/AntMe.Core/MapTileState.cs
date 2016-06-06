using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AntMe
{
    public abstract class MapTileState : PropertyList<MapTileStateProperty>, ISerializableState
    {
        public int HeightLevel { get; set; }

        public MapMaterial Material { get; set; }

        public bool CanEnter { get; set; }

        public void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }
}
