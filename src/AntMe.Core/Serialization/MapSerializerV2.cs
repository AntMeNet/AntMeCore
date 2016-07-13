﻿using System.IO;
using System.IO.Compression;

namespace AntMe.Serialization
{
    internal sealed class MapSerializerV2 : IMapSerializer
    {
        private const byte VERSION = 2;

        public void Serialize(Stream stream, Map map)
        {
            using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress))
            {
                using (BinaryWriter writer = new BinaryWriter(zip))
                {
                    // Header Information
                    Index2 size = map.GetCellCount();
                    writer.Write((byte)size.X);
                    writer.Write((byte)size.Y);

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
