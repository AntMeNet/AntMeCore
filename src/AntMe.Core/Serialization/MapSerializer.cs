using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace AntMe.Serialization
{
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
                case 1: return DeserializeV1(context, stream);
                case 2: return DeserializeV2(context, stream);
                default:
                    throw new NotSupportedException("Invalid Version Number");
            }
        }

        /// <summary>
        /// Deserializes the Map Format in Version 1 (Beta Version).
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="stream">Input Stream</param>
        /// <returns>Map Instance</returns>
        private static Map DeserializeV1(SimulationContext context, Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                // Globale Infos (Border, Width, Height)
                bool blockBorder = reader.ReadBoolean();
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                int playercount = reader.ReadByte();

                var map = new Map(context, width, height);
                map.BlockBorder = blockBorder;
                map.BaseLevel = 2;

                if (playercount < Map.MIN_STARTPOINTS)
                    throw new Exception("Too less Player in this Map");
                if (playercount > Map.MAX_STARTPOINTS)
                    throw new Exception("Too many Player in this Map");

                // Startpunkte einlesen
                map.StartPoints = new Index2[playercount];
                for (int i = 0; i < playercount; i++)
                {
                    map.StartPoints[i] = new Index2(
                        reader.ReadInt32(),
                        reader.ReadInt32());
                }

                if (width < Map.MIN_WIDTH || width > Map.MAX_WIDTH)
                    throw new Exception(string.Format("Dimensions (Width) are out of valid values ({0}...{1})", Map.MIN_WIDTH,
                        Map.MAX_WIDTH));
                if (height < Map.MIN_HEIGHT || height > Map.MAX_HEIGHT)
                    throw new Exception(string.Format("Dimensions (Width) are out of valid values ({0}...{1})", Map.MIN_HEIGHT,
                        Map.MAX_HEIGHT));

                // Zellen einlesen
                // map.Tiles = new MapTile[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte shape = reader.ReadByte();
                        byte speed = reader.ReadByte();
                        byte level = reader.ReadByte();

                        string typeName = string.Empty;
                        string materialName = string.Empty;
                        MapTileOrientation orientation = MapTileOrientation.NotRotated;

                        switch (speed)
                        {
                            case 1: materialName = "AntMe.Basics.MapTiles.LavaMaterial"; break;
                            case 2: materialName = "AntMe.Basics.MapTiles.MudMaterial"; break;
                            case 3: materialName = "AntMe.Basics.MapTiles.SandMaterial"; break;
                            case 4: materialName = "AntMe.Basics.MapTiles.GrasMaterial"; break;
                            case 5: materialName = "AntMe.Basics.MapTiles.StoneMaterial"; break;
                        }

                        switch (shape)
                        {
                            // Flat Map Tile
                            case 0x00:
                                typeName = "AntMe.Basics.MapTiles.FlatMapTile";
                                orientation = MapTileOrientation.NotRotated;
                                break;

                            // Ramp
                            case 0x10:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = MapTileOrientation.NotRotated;
                                break;
                            case 0x11:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = MapTileOrientation.RotBy270Degrees;
                                break;
                            case 0x12:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = MapTileOrientation.RotBy180Degrees;
                                break;
                            case 0x13:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = MapTileOrientation.RotBy90Degrees;
                                break;

                            // Wall
                            case 0x20:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = MapTileOrientation.NotRotated;
                                break;
                            case 0x21:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = MapTileOrientation.RotBy270Degrees;
                                break;
                            case 0x22:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = MapTileOrientation.RotBy180Degrees;
                                break;
                            case 0x23:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = MapTileOrientation.RotBy90Degrees;
                                break;

                            // Concave Corners
                            case 0x30:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = MapTileOrientation.RotBy90Degrees;
                                break;
                            case 0x31:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = MapTileOrientation.NotRotated;
                                break;
                            case 0x32:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = MapTileOrientation.RotBy270Degrees;
                                break;
                            case 0x33:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = MapTileOrientation.RotBy180Degrees;
                                break;

                            // Convex Cordners
                            case 0x40:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = MapTileOrientation.RotBy90Degrees;
                                break;
                            case 0x41:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = MapTileOrientation.NotRotated;
                                break;
                            case 0x42:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = MapTileOrientation.RotBy270Degrees;
                                break;
                            case 0x43:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = MapTileOrientation.RotBy180Degrees;
                                break;
                        }

                        // Lookup Map Tile
                        var tileMap = context.Mapper.MapTiles.First(t => t.Type.FullName.Equals(typeName));
                        MapTile tile = Activator.CreateInstance(tileMap.Type, context) as MapTile;
                        tile.HeightLevel = level;
                        tile.Orientation = orientation;

                        var materialMap = context.Mapper.MapMaterials.First(m => m.Type.FullName.Equals(materialName));
                        tile.Material = Activator.CreateInstance(materialMap.Type, context) as MapMaterial;
                        map[x, y] = tile;
                    }
                }

                return map;
            }
        }

        /// <summary>
        /// Deserializes the Map Format in Version 2.
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="stream">Input Stream</param>
        /// <returns>Map Instance</returns>
        private static Map DeserializeV2(SimulationContext context, Stream stream)
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
                case 2: SerializeV2(stream, map); break;
            }
        }

        private static void SerializeV2(Stream stream, Map map)
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
                    map.SerializeFirst(writer, 2);
                }
            }
        }
    }
}
