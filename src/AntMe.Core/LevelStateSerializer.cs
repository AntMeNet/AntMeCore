using System;

namespace AntMe
{
    /// <summary>
    /// Default Serializer for the AntVideo Format.
    /// </summary>
    public sealed class LevelStateSerializer
    {
        private ILevelStateSerializer serializer;
        private ILevelStateDeserializer deserializer;

        /// <summary>
        /// Returns the current Stream Version.
        /// </summary>
        public byte? Version { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public LevelStateSerializer()
        {
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>Frame Data</returns>
        public byte[] Serialize(LevelState state)
        {
            return Serialize(state, null);
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="version">Target Stream Version</param>
        /// <returns>Frame Data</returns>
        public byte[] Serialize(LevelState state, byte version)
        {
            return Serialize(state, version);
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="version">Target Stream Version</param>
        /// <returns>Frame Data</returns>
        public byte[] Serialize(LevelState state, byte? version)
        {
            if (deserializer != null)
                throw new NotSupportedException("Serializer is in Deserialize Mode");

            if (serializer == null)
            {
                // Set Default version
                if (!version.HasValue)
                    version = 2;

                switch (version.Value)
                {
                    case 1: serializer = new LevelStateSerializerV1(); break;
                    case 2: serializer = new LevelStateSerializerV2(); break;
                    default:
                        throw new ArgumentException("Not supported Stream Version");
                }
                Version = version.Value;
            }

            if (version.HasValue && Version != version.Value)
                throw new NotSupportedException("Version does not match the initial Version");

            return serializer.Serialize(state);
        }

        /// <summary>
        /// Deserializes the next Frame of the Stream.
        /// </summary>
        /// <param name="data">Frame Data</param>
        /// <returns>Deserialized State</returns>
        public LevelState Deserialize(byte[] data)
        {
            if (serializer != null)
                throw new NotSupportedException("Serializer is in Serialize Mode");

            if (deserializer == null)
            {
                // TODO: Find the right Version

                deserializer = new LevelStateDeserializerV1();
            }

            return deserializer.Deserialize(data);
        }

        /// <summary>
        /// Disposed den Serializer.
        /// </summary>
        public void Dispose()
        {
            if (serializer != null)
            {
                serializer.Dispose();
                serializer = null;
            }
        }

        
    }
}
