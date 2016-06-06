using System;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Materials used in Maps.
    /// </summary>
    public abstract class MapMaterial : ISerializableState
    {
        public MapMaterial(float speed)
        {
            Speed = speed;
        }

        /// <summary>
        /// Speed Multiplier for walking Units.
        /// </summary>
        public float Speed { get; protected set; }

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
