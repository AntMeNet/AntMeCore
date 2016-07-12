using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AntMe
{
    /// <summary>
    /// Default Serializer for the AntVideo Format.
    /// </summary>
    public sealed class LevelStateSerializer
    {
        private const string STREAM_HELLOMESSAGE = "AntMe! Replay";

        private Stream stream;

        private BinaryReader reader;

        private BinaryWriter writer;

        private SimulationContext context;

        private ILevelStateSerializer serializer;

        private ILevelStateDeserializer deserializer;

        /// <summary>
        /// Returns the current Stream Version.
        /// </summary>
        public byte? Version { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="stream">Input- or Output-Stream</param>
        /// <param name="context">Simulation Context</param>
        public LevelStateSerializer(Stream stream, SimulationContext context)
        {
            // Stream must be available
            if (stream == null)
                throw new ArgumentNullException("stream");

            // Context must be set
            if (context == null)
                throw new ArgumentNullException("context");

            this.stream = stream;
            this.context = context;
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>Frame Data</returns>
        public void Serialize(LevelState state)
        {
            Serialize(state, null);
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="version">Target Stream Version</param>
        /// <returns>Frame Data</returns>
        public void Serialize(LevelState state, byte version)
        {
            Serialize(state, version);
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="version">Target Stream Version</param>
        /// <returns>Frame Data</returns>
        public void Serialize(LevelState state, byte? version)
        {
            // Serializer is already in Deserialize Mode
            if (deserializer != null)
                throw new NotSupportedException("Serializer is in Deserialize Mode");

            // Stream must be writable
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is not ready for write");

            // Empty State
            if (state == null)
                throw new ArgumentNullException("state");

            if (serializer == null)
            {
                // Set Default version
                if (!version.HasValue)
                    version = 2;

                switch (version.Value)
                {
                    case 2: serializer = new LevelStateSerializerV2(context); break;
                    default:
                        throw new ArgumentException("Not supported Stream Version");
                }
                Version = version.Value;

                // Write Intro
                int count = STREAM_HELLOMESSAGE.Length;
                stream.WriteByte((byte)count);
                stream.Write(Encoding.ASCII.GetBytes(STREAM_HELLOMESSAGE), 0, count);

                // Write Version
                stream.WriteByte(Version.Value);

                writer = new BinaryWriter(stream);
            }

            if (version.HasValue && Version != version.Value)
                throw new NotSupportedException("Version does not match the initial Version");

            using (MemoryStream mem = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(mem, CompressionMode.Compress))
                {
                    using (BinaryWriter localWriter = new BinaryWriter(zip))
                    {
                        serializer.Serialize(localWriter, state);
                    }
                }

                byte[] buffer = mem.GetBuffer();
                writer.Write((short)mem.Position);
                writer.Write(buffer, 0, (int)mem.Position);
            }
        }

        /// <summary>
        /// Deserializes the next Frame of the Stream.
        /// </summary>
        /// <returns>Deserialized State</returns>
        public LevelState Deserialize()
        {
            if (serializer != null)
                throw new NotSupportedException("Serializer is in Serialize Mode");

            // Stream must be writable
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not ready for read");

            if (deserializer == null)
            {
                // Intro Text
                byte[] intro = Encoding.ASCII.GetBytes("AntMe! Replay");
                if (intro.Length != stream.ReadByte())
                    throw new Exception("This is not a AntMe! Map");
                for (int i = 0; i < intro.Length; i++)
                {
                    byte c = (byte)stream.ReadByte();
                    if (intro[i] != c)
                        throw new Exception("This is not a AntMe! Map");
                }

                // Version
                byte version = (byte)stream.ReadByte();
                switch (version)
                {
                    case 2: deserializer = new LevelStateDeserializerV2(context); break;
                    default:
                        throw new NotSupportedException("Invalid Version Number");
                }

                // Reader
                reader = new BinaryReader(stream);
            }

            byte[] buffer = new byte[reader.ReadInt16()];
            reader.Read(buffer, 0, buffer.Length);

            using (MemoryStream mem = new MemoryStream(buffer))
            {
                using (GZipStream zip = new GZipStream(mem, CompressionMode.Decompress))
                {
                    using (BinaryReader localReader = new BinaryReader(zip))
                    {
                        return deserializer.Deserialize(localReader);
                    }
                }
            }
        }

        /// <summary>
        /// Disposed den Serializer.
        /// </summary>
        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }

            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            if (serializer != null)
            {
                serializer.Dispose();
                serializer = null;
            }

            if (deserializer != null)
            {
                deserializer.Dispose();
                deserializer = null;
            }
        }

        
    }
}
