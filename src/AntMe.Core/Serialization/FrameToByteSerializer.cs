using System;
using System.IO;

namespace AntMe.Serialization
{
    /// <summary>
    /// Specialized Level State Serializer to handle Frames as Byte[].
    /// </summary>
    public sealed class FrameToByteSerializer : IDisposable
    {
        private MemoryStream stream;

        private FrameSerializer serializer;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public FrameToByteSerializer(SimulationContext context)
        {
            stream = new MemoryStream();
            serializer = new FrameSerializer(stream, context);
        }

        /// <summary>
        /// Serializes a Level State into a Byte Array.
        /// </summary>
        /// <param name="state">Level State</param>
        /// <returns>Raw Data</returns>
        public byte[] Serialize(Frame state)
        {
            stream.Seek(0, SeekOrigin.Begin);
            serializer.Serialize(state);

            byte[] output = new byte[stream.Position];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(output, 0, output.Length);

            return output;
        }

        /// <summary>
        /// Deserializes a Level State out of a Byte Array.
        /// </summary>
        /// <param name="data">Raw Data</param>
        /// <returns>Level State</returns>
        public Frame Deserialize(byte[] data)
        {
            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(data, 0, data.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return serializer.Deserialize();
        }

        /// <summary>
        /// Disposes all Resources.
        /// </summary>
        public void Dispose()
        {
            serializer?.Dispose();
            serializer = null;

            stream?.Dispose();
            stream = null;
        }
    }
}
