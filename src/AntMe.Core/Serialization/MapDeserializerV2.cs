using System.IO;
using System.IO.Compression;

namespace AntMe.Serialization
{
    /// <summary>
    /// Internal Deserializer for Version 2.
    /// </summary>
    internal sealed class MapDeserializerV2 : IMapDeserializer
    {
        /// <summary>
        /// Deserializes the Map Format in Version 2.
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="stream">Input Stream</param>
        /// <returns>Map Instance</returns>
        public Map Deserialize(SimulationContext context, Stream stream)
        {
            using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
            {
                using (BinaryReader reader = new BinaryReader(zip))
                {
                    int width = reader.ReadByte();
                    int height = reader.ReadByte();

                    Map map = new Map(context, width, height);
                    map.DeserializeFirst(reader, 2);

                    return map;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
