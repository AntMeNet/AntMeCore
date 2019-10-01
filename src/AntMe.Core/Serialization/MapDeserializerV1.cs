using System;
using System.IO;
using System.Linq;

namespace AntMe.Serialization
{
    /// <summary>
    ///     Deserializer for the old Map Format from the Beta Version.
    /// </summary>
    internal sealed class MapDeserializerV1 : IMapDeserializer
    {
        /// <summary>
        ///     Deserializes the Map Format in Version 1 (Beta Version).
        /// </summary>
        /// <param name="context">Current Simulation Context</param>
        /// <param name="stream">Input Stream</param>
        /// <returns>Map Instance</returns>
        public Map Deserialize(SimulationContext context, Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                // Globale Infos (Border, Width, Height)
                var blockBorder = reader.ReadBoolean();
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
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
                for (var i = 0; i < playercount; i++)
                    map.StartPoints[i] = new Index2(
                        reader.ReadInt32(),
                        reader.ReadInt32());

                if (width < Map.MIN_WIDTH || width > Map.MAX_WIDTH)
                    throw new Exception(string.Format("Dimensions (Width) are out of valid values ({0}...{1})",
                        Map.MIN_WIDTH,
                        Map.MAX_WIDTH));
                if (height < Map.MIN_HEIGHT || height > Map.MAX_HEIGHT)
                    throw new Exception(string.Format("Dimensions (Width) are out of valid values ({0}...{1})",
                        Map.MIN_HEIGHT,
                        Map.MAX_HEIGHT));

                // Zellen einlesen
                // map.Tiles = new MapTile[width, height];
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var shape = reader.ReadByte();
                    var speed = reader.ReadByte();
                    var level = reader.ReadByte();

                    var typeName = string.Empty;
                    var materialName = string.Empty;
                    var orientation = MapTileOrientation.NotRotated;

                    switch (speed)
                    {
                        case 1:
                            materialName = "AntMe.Basics.MapTiles.LavaMaterial";
                            break;
                        case 2:
                            materialName = "AntMe.Basics.MapTiles.MudMaterial";
                            break;
                        case 3:
                            materialName = "AntMe.Basics.MapTiles.SandMaterial";
                            break;
                        case 4:
                            materialName = "AntMe.Basics.MapTiles.GrasMaterial";
                            break;
                        case 5:
                            materialName = "AntMe.Basics.MapTiles.StoneMaterial";
                            break;
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
                    var tile = Activator.CreateInstance(tileMap.Type, context) as MapTile;
                    tile.HeightLevel = level;
                    tile.Orientation = orientation;

                    var materialMap = context.Mapper.MapMaterials.First(m => m.Type.FullName.Equals(materialName));
                    tile.Material = Activator.CreateInstance(materialMap.Type, context) as MapMaterial;
                    map[x, y] = tile;
                }

                return map;
            }
        }

        public void Dispose()
        {
        }
    }
}