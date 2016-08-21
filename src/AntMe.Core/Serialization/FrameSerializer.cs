using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AntMe.Serialization
{
    /// <summary>
    /// Default Serializer for the AntVideo Format.
    /// </summary>
    public sealed class FrameSerializer
    {
        private const string STREAM_HELLOMESSAGE = "AntMe! Replay";

        private Stream stream;

        private BinaryReader reader;

        private BinaryWriter writer;

        private SimulationContext context;

        private IFrameSerializer serializer;

        private IFrameDeserializer deserializer;

        /// <summary>
        /// Returns the current Stream Version.
        /// </summary>
        public byte? Version { get; private set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="stream">Input- or Output-Stream</param>
        /// <param name="context">Simulation Context</param>
        public FrameSerializer(Stream stream, SimulationContext context)
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
        public void Serialize(Frame state)
        {
            Serialize(state, null);
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="version">Target Stream Version</param>
        /// <returns>Frame Data</returns>
        public void Serialize(Frame state, byte version)
        {
            Serialize(state, version);
        }

        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="version">Target Stream Version</param>
        /// <returns>Frame Data</returns>
        private void Serialize(Frame state, byte? version)
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
                    case 2: serializer = new FrameSerializerV2(context); break;
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
                using (GZipStream zip = new GZipStream(mem, CompressionMode.Compress, true))
                {
                    using (BinaryWriter localWriter = new BinaryWriter(zip))
                    {
                        serializer.Serialize(localWriter, state);
                    }
                }

                byte[] buffer = mem.GetBuffer();
                if (mem.Position > ushort.MaxValue)
                    throw new NotSupportedException("Frame is too big");

                writer.Write((ushort)mem.Position);
                writer.Write(buffer, 0, (ushort)mem.Position);
            }
        }

        /// <summary>
        /// Deserializes the next Frame of the Stream.
        /// </summary>
        /// <returns>Deserialized State</returns>
        public Frame Deserialize()
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
                    throw new Exception("This is not a AntMe! Replay");
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
                    case 2: deserializer = new FrameDeserializerV2(context); break;
                    default:
                        throw new NotSupportedException("Invalid Version Number");
                }

                // Reader
                reader = new BinaryReader(stream);
            }

            byte[] buffer = reader.ReadBytes(reader.ReadUInt16());
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
            writer?.Dispose();
            writer = null;

            reader?.Dispose();
            reader = null;

            serializer?.Dispose();
            serializer = null;

            deserializer?.Dispose();
            deserializer = null;
        }
    }
}
