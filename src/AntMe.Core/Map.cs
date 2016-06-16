using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.IO.Compression;

namespace AntMe
{
    /// <summary>
    /// Represents the topological information about a level Map.
    /// </summary>
    public sealed class Map : PropertyList<MapProperty>, ISerializableState
    {
        #region Constants

        private const string STREAM_HELLOMESSAGE = "AntMe! Map";

        /// <summary>
        /// Size of a single Cell in World Units.
        /// </summary>
        public const float CELLSIZE = 50f;

        /// <summary>
        /// Minimum Number of Cells in X Direction.
        /// </summary>
        public const int MIN_WIDTH = 10;

        /// <summary>
        /// Minimum Number of Cells in Y Direction.
        /// </summary>
        public const int MIN_HEIGHT = 10;

        /// <summary>
        /// Maximum Number of Cells in X Direction.
        /// </summary>
        public const int MAX_WIDTH = 100;

        /// <summary>
        /// Maximum Number of Cells in Y Direction.
        /// </summary>
        public const int MAX_HEIGHT = 100;

        /// <summary>
        /// Minimum Count of Start Points.
        /// </summary>
        public const int MIN_STARTPOINTS = 1;

        /// <summary>
        /// Maximum Count of Start Points.
        /// </summary>
        public const int MAX_STARTPOINTS = Level.MAX_SLOTS;

        /// <summary>
        /// Minimum Hight of Units.
        /// </summary>
        public const float MIN_Z = 0f;

        /// <summary>
        /// Maximum Hight of Units.
        /// </summary>
        public const float MAX_Z = 50f;

        /// <summary>
        /// Height of a single Height Level.
        /// </summary>
        public const float LEVELHEIGHT = 20f;

        /// <summary>
        /// Maximum Height Level.
        /// </summary>
        public const byte MAX_LEVEL = 2;

        #endregion

        private MapState state;

        private SimulationContext context;

        /// <summary>
        /// Holds all Map Tiles.
        /// </summary>
        private MapTile[,] tiles;

        /// <summary>
        /// Gets or sets the border behavior of this map.
        /// </summary>
        public bool BlockBorder { get; set; }

        /// <summary>
        /// Dictionary of missing Properties during Deserialization.
        /// </summary>
        public Dictionary<string, byte[]> UnknownProperties { get; private set; }

        /// <summary>
        /// Gets or sets the Map Tiles.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Map Tile</returns>
        public MapTile this[int x, int y]
        {
            get { return tiles[x, y]; }
            set { tiles[x, y] = value; }
        }

        /// <summary>
        /// List of Start Point for the available Player Slots.
        /// </summary>
        public Index2[] StartPoints { get; set; }

        /// <summary>
        /// Default Constructor to initialize the Tile Array.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="width">Number of Cols</param>
        /// <param name="height">Number of Rows</param>
        public Map(SimulationContext context, int width, int height) : this(context, width, height, 0) { }

        /// <summary>
        /// Constructor with predefined Player Startpoints.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="playerCount"></param>
        public Map(SimulationContext context, int width, int height, int playerCount)
        {
            this.context = context;

            UnknownProperties = new Dictionary<string, byte[]>();

            // Check Parameter
            if (width < MIN_WIDTH)
                throw new ArgumentOutOfRangeException(string.Format("Map must have at least {0} Columns", MIN_WIDTH));
            if (width > MAX_WIDTH)
                throw new ArgumentOutOfRangeException(string.Format("Map must have a max of {0} Columns", MAX_WIDTH));
            if (height < MIN_HEIGHT)
                throw new ArgumentOutOfRangeException(string.Format("Map must have at least {0} Rows", MIN_HEIGHT));
            if (height > MAX_HEIGHT)
                throw new ArgumentOutOfRangeException(string.Format("Map must have a max of {0} Rows", MAX_HEIGHT));
            if (playerCount < 0)
                throw new ArgumentOutOfRangeException("Player Count can't me smaller than zero");
            if (playerCount > 8)
                throw new ArgumentOutOfRangeException("Player Count can't be greater than 8");

            // Create Tiles
            tiles = new MapTile[width, height];

            // Create Players
            int dx = width / 6;
            int dy = height / 6;
            StartPoints = new Index2[playerCount];
            if (playerCount >= 1)
                StartPoints[0] = new Index2(dx, dy);
            if (playerCount >= 2)
                StartPoints[1] = new Index2(5 * dx, 5 * dy);
            if (playerCount >= 3)
                StartPoints[2] = new Index2(5 * dx, dy);
            if (playerCount >= 4)
                StartPoints[3] = new Index2(dx, 5 * dy);
            if (playerCount >= 5)
                StartPoints[4] = new Index2(3 * dx, dy);
            if (playerCount >= 6)
                StartPoints[5] = new Index2(3 * dx, 5 * dy);
            if (playerCount >= 7)
                StartPoints[6] = new Index2(dx, 3 * dy);
            if (playerCount >= 8)
                StartPoints[7] = new Index2(5 * dx, 3 * dy);

            this.context.Resolver.ResolveMap(context, this);
        }

        /// <summary>
        /// Updates the Map and its Properties.
        /// </summary>
        /// <param name="round">Current Round</param>
        public void Update(int round)
        {
            foreach (var property in Properties)
                property.Update(round);
        }

        /// <summary>
        /// Creates a Map State.
        /// </summary>
        /// <returns>Map State</returns>
        public MapState GetState()
        {
            if (state == null)
                state = context.Resolver.CreateMapState(this);
            return state;
        }

        #region Statische Methoden (Generatoren und Serialisierer)

        /// <summary>
        /// Deserialize a Map
        /// </summary>
        /// <param name="context">Reference to the Simulation Context</param>
        /// <param name="stream">Source</param>
        /// <returns>Map</returns>
        public static Map Deserialize(SimulationContext context, Stream stream)
        {
            // Intro Text
            byte[] intro = Encoding.ASCII.GetBytes("AntMe! Map");
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

                if (playercount < MIN_STARTPOINTS)
                    throw new Exception("Too less Player in this Map");
                if (playercount > MAX_STARTPOINTS)
                    throw new Exception("Too many Player in this Map");

                // Startpunkte einlesen
                map.StartPoints = new Index2[playercount];
                for (int i = 0; i < playercount; i++)
                {
                    map.StartPoints[i] = new Index2(
                        reader.ReadInt32(),
                        reader.ReadInt32());
                }

                if (width < MIN_WIDTH || width > MAX_WIDTH)
                    throw new Exception(string.Format("Dimensions (Width) are out of valid values ({0}...{1})", MIN_WIDTH,
                        MAX_WIDTH));
                if (height < MIN_HEIGHT || height > MAX_HEIGHT)
                    throw new Exception(string.Format("Dimensions (Width) are out of valid values ({0}...{1})", MIN_HEIGHT,
                        MAX_HEIGHT));

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
                        Compass orientation = Compass.East;

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
                                orientation = Compass.East;
                                break;

                            // Ramp
                            case 0x10:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = Compass.East;
                                break;
                            case 0x11:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = Compass.North;
                                break;
                            case 0x12:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = Compass.West;
                                break;
                            case 0x13:
                                typeName = "AntMe.Basics.MapTiles.RampMapTile";
                                orientation = Compass.South;
                                break;

                            // Wall
                            case 0x20:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = Compass.East;
                                break;
                            case 0x21:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = Compass.North;
                                break;
                            case 0x22:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = Compass.West;
                                break;
                            case 0x23:
                                typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                                orientation = Compass.South;
                                break;

                            // Concave Corners
                            case 0x30:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = Compass.South;
                                break;
                            case 0x31:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = Compass.East;
                                break;
                            case 0x32:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = Compass.North;
                                break;
                            case 0x33:
                                typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                                orientation = Compass.West;
                                break;

                            // Convex Cordners
                            case 0x40:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = Compass.South;
                                break;
                            case 0x41:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = Compass.East;
                                break;
                            case 0x42:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = Compass.North;
                                break;
                            case 0x43:
                                typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                                orientation = Compass.West;
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
            map.CheckMap();

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

        #endregion

        /// <summary>
        ///     Führt eine Plausibilitätsprüfung der Karten-Einstellungen durch
        /// </summary>
        public void CheckMap()
        {
            // Tiles prüfen
            if (tiles == null)
                throw new Exception("Tiles Array is null");

            Index2 cells = GetCellCount();

            // Karten Dimensionen checken
            if (cells.X < MIN_WIDTH)
                throw new Exception(string.Format("Map must have at least {0} Columns", MIN_WIDTH));
            if (cells.X > MAX_WIDTH)
                throw new Exception(string.Format("Map must have a maximum of {0} Columns", MAX_WIDTH));

            if (cells.Y < MIN_HEIGHT)
                throw new Exception(string.Format("Map must have at least {0} Rows", MIN_HEIGHT));
            if (cells.Y > MAX_HEIGHT)
                throw new Exception(string.Format("Map must have a maximum of {0} Rows", MAX_HEIGHT));

            // Startpunkte überprüfen
            if (StartPoints == null)
                throw new Exception("The List of StartPoints is null");

            // Spieleranzahl prüfen
            if (GetPlayerCount() < MIN_STARTPOINTS)
                throw new Exception(string.Format("There must be at least {0} player", MIN_STARTPOINTS));
            if (GetPlayerCount() > MAX_STARTPOINTS)
                throw new Exception(string.Format("The maximum Player Count is {0}", MAX_STARTPOINTS));

            // TODO: Check Cell-Structure

            // Alle Startpunkte überprüfen
            for (int i = 0; i < StartPoints.Length; i++)
            {
                // Prüfen, ob die Zelle existiert
                Index2 startPoint = StartPoints[i];
                if (startPoint.X < 0 || startPoint.X >= tiles.GetLength(0) ||
                    startPoint.Y < 0 || startPoint.Y >= tiles.GetLength(1))
                    throw new Exception(string.Format("StartPoint {0} is out of map bounds", i));

                // Prüfen, ob noch ein anderer Startpoint auf der selben Zelle ist.
                for (int j = 0; j < StartPoints.Length; j++)
                    if (i != j && StartPoints[i] == StartPoints[j])
                        throw new Exception(string.Format("StartPoints {0} and {1} are on the same Cell", i, j));
            }
        }

        /// <summary>
        /// Finds the Cell Index for a given Position.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Cell Index</returns>
        public Index2 GetCellIndex(Vector3 position)
        {
            return GetCellIndex(position.X, position.Y);
        }

        /// <summary>
        /// Finds the Cell Index for a given Position.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Cell Index</returns>
        public Index2 GetCellIndex(Vector2 position)
        {
            return GetCellIndex(position.X, position.Y);
        }

        /// <summary>
        /// Finds the Cell Index for a given Position.
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Cell Index</returns>
        public Index2 GetCellIndex(float x, float y)
        {
            // Wenns keine Map gibt
            if (tiles == null)
                return Index2.Zero;

            Index2 limit = GetCellCount();

            var indexX = (int)(x / CELLSIZE);
            var indexY = (int)(y / CELLSIZE);

            if (indexX < 0)
                indexX = 0;
            if (indexY < 0)
                indexY = 0;
            if (indexX >= limit.X)
                indexX = limit.X - 1;
            if (indexY >= limit.Y)
                indexY = limit.Y - 1;

            return new Index2(indexX, indexY);
        }

        /// <summary>
        /// Calculates the local Postion within the current Cell.
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Local Postion</returns>
        public Vector2 GetLocalPosition(Vector2 position)
        {
            return GetLocalPosition(position.X, position.Y);
        }

        /// <summary>
        /// Calculates the local Postion within the current Cell.
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Local Postion</returns>
        public Vector2 GetLocalPosition(Vector3 position)
        {
            return GetLocalPosition(position.X, position.Y);
        }

        /// <summary>
        /// Calculates the local Postion within the current Cell.
        /// </summary>
        /// <param name="x">Global X Position</param>
        /// <param name="y">Global Y Position</param>
        /// <returns>Local Postion</returns>
        public Vector2 GetLocalPosition(float x, float y)
        {
            return new Vector2(x % CELLSIZE, y % CELLSIZE);
        }

        /// <summary>
        /// Returns the Count of Rows and Cols of this Map.
        /// </summary>
        /// <returns>Cell Count</returns>
        public Index2 GetCellCount()
        {
            if (tiles != null)
                return new Index2(
                    tiles.GetLength(0),
                    tiles.GetLength(1));
            return Index2.Zero;
        }

        /// <summary>
        /// Caclulates the total size of the map in Game-Units.
        /// </summary>
        /// <returns>Map Dimensions</returns>
        public Vector2 GetSize()
        {
            Index2 cells = GetCellCount();
            return new Vector2(
                cells.X * CELLSIZE,
                cells.Y * CELLSIZE);
        }

        /// <summary>
        /// Returns the Number of SlotPositions.
        /// </summary>
        /// <returns>Count</returns>
        public int GetPlayerCount()
        {
            if (StartPoints != null)
                return StartPoints.Length;
            return 0;
        }

        /// <summary>
        /// Gets the Map Height at the given Position
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Height</returns>
        public float GetHeight(Vector2 position)
        {
            return GetHeight(position.X, position.Y);
        }

        /// <summary>
        /// Gets the Map Height at the given Position
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Height</returns>
        public float GetHeight(Vector3 position)
        {
            return GetHeight(position.X, position.Y);
        }

        /// <summary>
        /// Gets the Map Height at the given Position
        /// </summary>
        /// <param name="x">Global X Position</param>
        /// <param name="y">Global Y Position</param>
        /// <returns>Height</returns>
        public float GetHeight(float x, float y)
        {
            Index2 cell = GetCellIndex(x, y);
            Vector2 local = GetLocalPosition(x, y);
            return this[cell.X, cell.Y].GetHeight(local);
        }

        /// <summary>
        /// Serializes the Map.
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeFirst(BinaryWriter writer, byte version)
        {
            switch (version)
            {
                case 2: SerializeV2(writer); break;
                default: throw new ArgumentException("File Version not supported");
            }
        }

        /// <summary>
        /// Serializes the Map in Stream Format 2.
        /// </summary>
        /// <param name="writer">Output Writer</param>
        private void SerializeV2(BinaryWriter writer)
        {
            var size = GetCellCount();

            // Map Data
            writer.Write(BlockBorder);

            // Startpoints
            writer.Write((byte)StartPoints.Length);
            for (int i = 0; i < StartPoints.GetLength(0); i++)
            {
                writer.Write((short)StartPoints[i].X);
                writer.Write((short)StartPoints[i].Y);
            }

            // Map Properties
            writer.Write((byte)Properties.Count() + UnknownProperties.Count);
            foreach (var property in Properties)
            {
                writer.Write(property.GetType().FullName);
                SerializeType(writer, property);
            }

            // Add Data from unknown Property
            foreach (var unknownProperty in UnknownProperties)
            {
                writer.Write(unknownProperty.Key);
                writer.Write((short)unknownProperty.Value.Length);
                writer.Write(unknownProperty.Value, 0, unknownProperty.Value.Length);
            }

            // Collect MapTile/Material/TileProperty Types
            List<string> mapTileTypes = new List<string>();
            List<string> materialTypes = new List<string>();
            List<string> propertyTypes = new List<string>();
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    MapTile tile = this[x, y];

                    if (tile == null)
                    {
                        // No Tile -> skip
                        continue;
                    }
                    else if (tile is UnknownMapTile)
                    {
                        // Unknown Tile Type
                        UnknownMapTile unknownTile = tile as UnknownMapTile;
                        if (!mapTileTypes.Contains(unknownTile.MapTileType))
                            mapTileTypes.Add(unknownTile.MapTileType);
                    }
                    else
                    {
                        // Common Tile Type
                        Type mapTileType = tile.GetType();
                        if (!mapTileTypes.Contains(mapTileType.FullName))
                            mapTileTypes.Add(mapTileType.FullName);
                    }

                    // Common Properties
                    foreach (var property in tile.Properties)
                    {
                        Type propertyType = property.GetType();
                        if (!propertyTypes.Contains(propertyType.FullName))
                            propertyTypes.Add(propertyType.FullName);
                    }

                    // Unknown Properties
                    foreach (var property in tile.UnknownProperties.Keys)
                    {
                        if (!propertyTypes.Contains(property))
                            propertyTypes.Add(property);
                    }

                    // Materials
                    if (tile.Material == null)
                    {
                        // No Material -> Skip
                        continue;
                    }
                    else if (tile.Material is UnknownMaterial)
                    {
                        // Unknown Material
                        UnknownMaterial unknownMaterial = tile.Material as UnknownMaterial;
                        if (!materialTypes.Contains(unknownMaterial.MaterialType))
                            materialTypes.Add(unknownMaterial.MaterialType);
                    }
                    else
                    {
                        // Common Material
                        Type materialType = tile.Material.GetType();
                        if (!materialTypes.Contains(materialType.FullName))
                            materialTypes.Add(materialType.FullName);
                    }
                }
            }

            // Write Map Tile Types
            writer.Write((byte)mapTileTypes.Count);
            foreach (var type in mapTileTypes)
                writer.Write(type);

            // Write Map Tile Property Types
            writer.Write((byte)propertyTypes.Count);
            foreach (var type in propertyTypes)
                writer.Write(type);

            // Write Material Types
            writer.Write((byte)materialTypes.Count);
            foreach (var type in materialTypes)
                writer.Write(type);

            // Zelleninfos
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    MapTile tile = this[x, y];

                    // Map Tile
                    if (tile != null)
                    {
                        int index;
                        if (tile is UnknownMapTile) index = mapTileTypes.IndexOf((tile as UnknownMapTile).MapTileType) + 1;
                        else index = mapTileTypes.IndexOf(tile.GetType().FullName) + 1;

                        writer.Write((byte)index);
                        SerializeType(writer, tile);

                        // Material
                        if (tile.Material != null)
                        {
                            int materialIndex;
                            if (tile.Material is UnknownMaterial)
                                materialIndex = materialTypes.IndexOf((tile.Material as UnknownMaterial).MaterialType) + 1;
                            else materialIndex = materialTypes.IndexOf(tile.Material.GetType().FullName) + 1;
                            writer.Write((byte)materialIndex);
                            SerializeType(writer, tile.Material);
                        }
                        else
                        {
                            writer.Write((byte)0);
                        }

                        // Properties
                        writer.Write((byte)tile.Properties.Count() + tile.UnknownProperties.Count);
                        foreach (var property in tile.Properties)
                        {
                            int propertyIndex = propertyTypes.IndexOf(property.GetType().FullName);
                            writer.Write((byte)propertyIndex);
                            SerializeType(writer, property);
                        }
                        foreach (var property in tile.UnknownProperties)
                        {
                            int propertyIndex = propertyTypes.IndexOf(property.Key);
                            writer.Write((byte)propertyIndex);
                            writer.Write(property.Value.Length);
                            writer.Write(property.Value, 0, property.Value.Length);
                        }
                    }
                    else
                    {
                        writer.Write((byte)0);
                        continue;
                    }
                }
            }
        }

        private void DeserializeType(BinaryReader reader, ISerializableState state)
        {
            int dumpCount = reader.ReadInt16();
            byte[] dump = reader.ReadBytes(dumpCount);

            using (MemoryStream mem = new MemoryStream(dump))
            {
                using (BinaryReader memReader = new BinaryReader(mem))
                {
                    state.DeserializeFirst(memReader, 2);
                }
            }
        }

        /// <summary>
        /// Serializes a single Type into the output Stream.
        /// Format: length(byte), payload(byte[length])
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="state"></param>
        private void SerializeType(BinaryWriter writer, ISerializableState state)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter memWriter = new BinaryWriter(mem))
                {
                    // Serialize
                    state.SerializeFirst(memWriter, 2);

                    // Copy to Buffer
                    int count = (int)mem.Position;
                    byte[] buffer = new byte[count];
                    mem.Position = 0;
                    mem.Read(buffer, 0, count);

                    // Write to main Stream
                    writer.Write((short)count);
                    writer.Write(buffer, 0, count);
                }
            }
        }

        /// <summary>
        /// Serializes following Frames. (Not supported with the Map)
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter writer, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map");
        }

        /// <summary>
        /// Deserializes the Map.
        /// </summary>
        /// <param name="reader">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeFirst(BinaryReader reader, byte version)
        {
            switch (version)
            {
                case 2: DeserializeV2(reader); break;
                default: throw new ArgumentException("File Version not supported");
            }
        }

        private void DeserializeV2(BinaryReader reader)
        {
            UnknownProperties.Clear();
            Index2 size = GetCellCount();

            // Map Data
            BlockBorder = reader.ReadBoolean();

            // Startpoints
            StartPoints = new Index2[reader.ReadByte()];
            for (int i = 0; i < StartPoints.Length; i++)
                StartPoints[i] = new Index2(reader.ReadInt16(), reader.ReadInt16());

            // Map Properties
            int mapPropertyCount = reader.ReadByte();
            for (int i = 0; i < mapPropertyCount; i++)
            {
                string propertyName = reader.ReadString();
                MapProperty property = Properties.FirstOrDefault(p => p.GetType().FullName.Equals(propertyName));
                if (property != null)
                {
                    DeserializeType(reader, property);
                }
                else
                {
                    // Dump Data in Unknown Properties
                    int dumpCount = reader.ReadInt16();
                    byte[] dump = reader.ReadBytes(dumpCount);
                    UnknownProperties.Add(propertyName, dump);
                }
            }

            // Read Map Tile Types
            int mapTileTypeCount = reader.ReadByte();
            List<string> mapTileTypes = new List<string>(mapTileTypeCount);
            for (int i = 0; i < mapTileTypeCount; i++)
                mapTileTypes.Add(reader.ReadString());

            // Read Property Types
            int propertyTypeCount = reader.ReadByte();
            List<string> propertyTypes = new List<string>(propertyTypeCount);
            for (int i = 0; i < propertyTypeCount; i++)
                propertyTypes.Add(reader.ReadString());

            // Read Material Types
            int materialTypeCount = reader.ReadByte();
            List<string> materialTypes = new List<string>(materialTypeCount);
            for (int i = 0; i < materialTypeCount; i++)
                materialTypes.Add(reader.ReadString());

            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    short tileTypeIndex = reader.ReadByte();

                    // Empty Cell
                    if (tileTypeIndex == 0)
                        continue;

                    // Identify Map Tile Type
                    string tileTypeName = mapTileTypes[tileTypeIndex];
                    var tileTypeMap = context.Mapper.MapTiles.FirstOrDefault(t => t.Type.FullName.Equals(tileTypeName));
                    MapTile mapTile;
                    if (tileTypeMap != null)
                    {
                        // Deserialize known Type
                        mapTile = Activator.CreateInstance(tileTypeMap.Type, context) as MapTile;
                        DeserializeType(reader, mapTile);
                    }
                    else
                    {
                        // Handle missing Map Tile Type
                        int bufferSize = reader.ReadInt16();
                        byte[] payload = reader.ReadBytes(bufferSize);
                        mapTile = new UnknownMapTile(context, tileTypeName, payload);
                    }

                    // Handle Material
                    short materialIndex = reader.ReadByte();
                    if (materialIndex > 0)
                    {
                        string materialTypeName = materialTypes[materialIndex];
                        var materialMap = context.Mapper.MapMaterials.FirstOrDefault(m => m.Type.FullName.Equals(materialTypeName));
                        if (materialMap != null)
                        {
                            // Deserialize known Type
                            mapTile.Material = Activator.CreateInstance(materialMap.Type, context) as MapMaterial;
                            DeserializeType(reader, mapTile.Material);
                        }
                        else
                        {
                            // Handle missing Material Type
                            int bufferSize = reader.ReadInt16();
                            byte[] payload = reader.ReadBytes(bufferSize);
                            mapTile.Material = new UnknownMaterial(context, materialTypeName, payload);
                        }
                    }

                    // Handle Map Tile Properties
                    int propertyCount = reader.ReadByte();
                    for (int i = 0; i < propertyCount; i++)
                    {
                        int propertyIndex = reader.ReadByte();
                        string propertyName = propertyTypes[propertyIndex];
                        var property = mapTile.Properties.FirstOrDefault(p => p.GetType().FullName.Equals(propertyName));
                        if (property != null)
                        {
                            // Deserialize Data
                            DeserializeType(reader, property);
                        }
                        else
                        {
                            // Handle missing Property Type
                            int bufferSize = reader.ReadInt16();
                            byte[] payload = reader.ReadBytes(bufferSize);
                            UnknownProperties.Add(propertyName, payload);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes all following Frames. (Not supported with the Map)
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map");
        }
    }
}