using System.IO;
using System.IO.Compression;

namespace AntMe.Serialization
{
    internal sealed class MapSerializerV2 : IMapSerializer
    {
        private const byte VERSION = 2;

        public void Serialize(Stream stream, Map map)
        {
            using (var zip = new GZipStream(stream, CompressionMode.Compress))
            {
                using (var writer = new BinaryWriter(zip))
                {
                    // Header Information
                    var size = map.GetCellCount();
                    writer.Write((byte) size.X);
                    writer.Write((byte) size.Y);

                    // Serialize Stuff
                    map.SerializeFirst(writer, VERSION);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}