using System;
using System.IO;
using System.Text;

namespace AntMe.Serialization
{
    /// <summary>
    /// Default Map Serializer for AntMe! Maps.
    /// </summary>
    public static class MapSerializer
    {
        private const string STREAM_HELLOMESSAGE = "AntMe! Map";

        /// <summary>
        /// Deserialize a Map
        /// </summary>
        /// <param name="context">Reference to the Simulation Context</param>
        /// <param name="filedump">Source</param>
        /// <returns></returns>
        public static Map Deserialize(SimulationContext context, byte[] filedump)
        {
            using (MemoryStream stream = new MemoryStream(filedump))
            {
                return Deserialize(context, stream);
            }
        }

        /// <summary>
        /// Deserialize a Map
        /// </summary>
        /// <param name="context">Reference to the Simulation Context</param>
        /// <param name="stream">Source</param>
        /// <returns>Map</returns>
        public static Map Deserialize(SimulationContext context, Stream stream)
        {
            // Intro Text
            byte[] intro = Encoding.ASCII.GetBytes(STREAM_HELLOMESSAGE);
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
                case 1:
                    using (IMapDeserializer deserializer = new MapDeserializerV1())
                    {
                        return deserializer.Deserialize(context, stream);
                    }
                case 2:
                    using (IMapDeserializer deserializer = new MapDeserializerV2())
                    {
                        return deserializer.Deserialize(context, stream);
                    }
                default:
                    throw new NotSupportedException("Invalid Version Number");
            }
        }
        

        /// <summary>
        /// Serializes the given Map into a stream.
        /// </summary>
        /// <param name="stream">Target Stream</param>
        /// <param name="map">Map</param>
        /// <param name="version">File Format Version</param>
        public static void Serialize(Stream stream, Map map, byte version = 2)
        {
            // Check Map
            if (map == null)
                throw new ArgumentNullException("map");
            map.ValidateMap();

            // Check Stream
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is read only");

            // Version Number
            if (version != 2)
                throw new ArgumentException("Invalid Version");


            // Write Intro
            int count = STREAM_HELLOMESSAGE.Length;
            stream.WriteByte((byte)count);
            stream.Write(Encoding.ASCII.GetBytes(STREAM_HELLOMESSAGE), 0, count);

            // Write Version
            stream.WriteByte(version);

            switch (version)
            {
                case 2:
                    using (IMapSerializer serializer = new MapSerializerV2())
                    {
                        serializer.Serialize(stream, map);
                    }
                    break;
                default:
                    throw new NotSupportedException("Invalid Version Number");
            }
        }
    }
}
