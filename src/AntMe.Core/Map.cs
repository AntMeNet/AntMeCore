using System;
using System.IO;
using System.Runtime.Serialization;

namespace AntMe
{
    /// <summary>
    /// Represents the topological information about a level Map.
    /// </summary>
    public sealed class Map : PropertyList<MapProperty>
    {
        #region Constants

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

        /// <summary>
        /// Holds all Map Tiles.
        /// </summary>
        private MapTile[,] tiles;

        /// <summary>
        /// Gets or sets the border behavior of this map.
        /// </summary>
        public bool BlockBorder { get; set; }

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

        public Map(int width, int height, bool blockBorder)
        {
            // Check Parameter
            if (width < MIN_WIDTH)
                throw new ArgumentOutOfRangeException(string.Format("Map must have at least {0} Columns", MIN_WIDTH));
            if (width > MAX_WIDTH)
                throw new ArgumentOutOfRangeException(string.Format("Map must have a max of {0} Columns", MAX_WIDTH));
            if (height < MIN_HEIGHT)
                throw new ArgumentOutOfRangeException(string.Format("Map must have at least {0} Rows", MIN_HEIGHT));
            if (height > MAX_HEIGHT)
                throw new ArgumentOutOfRangeException(string.Format("Map must have a max of {0} Rows", MAX_HEIGHT));

            BlockBorder = blockBorder;

            // Create Tiles
            tiles = new MapTile[width, height];

            // Create Players
            int dx = width / 6;
            int dy = height / 6;
            StartPoints = new Index2[8];
            StartPoints[0] = new Index2(dx, dy);
            StartPoints[1] = new Index2(5 * dx, 5 * dy);
            StartPoints[2] = new Index2(5 * dx, dy);
            StartPoints[3] = new Index2(dx, 5 * dy);
            StartPoints[4] = new Index2(3 * dx, dy);
            StartPoints[5] = new Index2(3 * dx, 5 * dy);
            StartPoints[6] = new Index2(dx, 3 * dy);
            StartPoints[7] = new Index2(5 * dx, 3 * dy);
        }

        public void Update(int round) { }

        #region Statische Methoden (Generatoren und Serialisierer)

        /// <summary>
        /// Deserialize a Map
        /// </summary>
        /// <param name="stream">Source</param>
        /// <param name="old">Is Map in an old Format</param>
        /// <returns>Map</returns>
        public static Map Deserialize(Stream stream, bool old)
        {
            var reader = new BinaryReader(stream);

            // Intro (Typ und Version)
            if (reader.ReadString() != "AntMe! Map")
                throw new Exception("This is not a AntMe! Map");

            byte version = reader.ReadByte();
            switch (version)
            {
                case 1:
                    return old ? DeserializeOld(reader) : DeserializeV1(reader);
                default:
                    throw new NotSupportedException("Invalid Version Number");
            }
        }

        private static Map DeserializeOld(BinaryReader reader)
        {
            // Globale Infos (Border, Width, Height)
            bool blockBorder = reader.ReadBoolean();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            int playercount = reader.ReadByte();

            var map = new Map(width, height, blockBorder);

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
                            orientation = Compass.South;
                            break;
                        case 0x12:
                            typeName = "AntMe.Basics.MapTiles.RampMapTile";
                            orientation = Compass.North;
                            break;
                        case 0x13:
                            typeName = "AntMe.Basics.MapTiles.RampMapTile";
                            orientation = Compass.West;
                            break;

                        // Wall
                        case 0x20:
                            typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                            orientation = Compass.East;
                            break;
                        case 0x21:
                            typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                            orientation = Compass.South;
                            break;
                        case 0x22:
                            typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                            orientation = Compass.North;
                            break;
                        case 0x23:
                            typeName = "AntMe.Basics.MapTiles.WallCliffMapTile";
                            orientation = Compass.West;
                            break;

                        // Concave Corners
                        case 0x30:
                            typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                            orientation = Compass.East;
                            break;
                        case 0x31:
                            typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                            orientation = Compass.South;
                            break;
                        case 0x32:
                            typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                            orientation = Compass.West;
                            break;
                        case 0x33:
                            typeName = "AntMe.Basics.MapTiles.ConcaveCliffMapTile";
                            orientation = Compass.North;
                            break;

                        // Convex Cordners
                        case 0x40:
                            typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                            orientation = Compass.North;
                            break;
                        case 0x41:
                            typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                            orientation = Compass.South;
                            break;
                        case 0x42:
                            typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                            orientation = Compass.East;
                            break;
                        case 0x43:
                            typeName = "AntMe.Basics.MapTiles.ConvexCliffMapTile";
                            orientation = Compass.West;
                            break;
                    }

                    MapTile tile = Activator.CreateInstance(Type.GetType(typeName + ", AntMe.Basics, Version=2.0.0.63, Culture=neutral, PublicKeyToken=null")) as MapTile;
                    tile.HeightLevel = level;
                    tile.Orientation = orientation;
                    tile.Material = Activator.CreateInstance(Type.GetType(materialName + ", AntMe.Basics, Version=2.0.0.63, Culture=neutral, PublicKeyToken=null")) as MapMaterial;
                    map[x, y] = tile;
                }

            return map;
        }

        private static Map DeserializeV1(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        /////     Serialisiert eine Map in einen Stream.
        /////     TODO: Test!!!
        ///// </summary>
        ///// <param name="stream">Zielstream</param>
        ///// <param name="map">Zu serialisierende Map</param>
        //public static void Serialize(Stream stream, Map map)
        //{
        //    // Check Map
        //    if (map == null)
        //        throw new ArgumentNullException("map");
        //    map.CheckMap();

        //    // Check Stream
        //    if (stream == null)
        //        throw new ArgumentNullException("stream");
        //    if (!stream.CanWrite)
        //        throw new ArgumentException("Stream is read only");


        //    var writer = new BinaryWriter(stream);

        //    // Intro (Typ und Version)
        //    writer.Write("AntMe! Map");
        //    writer.Write((byte)1);

        //    // Global Infos (Block, Width, Height)
        //    writer.Write(map.BlockBorder);
        //    writer.Write(map.Tiles.GetLength(0));
        //    writer.Write(map.Tiles.GetLength(1));
        //    writer.Write((byte)map.StartPoints.GetLength(0));

        //    // Startpoints
        //    for (int i = 0; i < map.StartPoints.GetLength(0); i++)
        //    {
        //        writer.Write(map.StartPoints[i].X);
        //        writer.Write(map.StartPoints[i].Y);
        //    }

        //    // Zelleninfos
        //    for (int y = 0; y < map.Tiles.GetLength(1); y++)
        //        for (int x = 0; x < map.Tiles.GetLength(0); x++)
        //        {
        //            writer.Write((byte)map.Tiles[x, y].Shape);
        //            writer.Write((byte)map.Tiles[x, y].Speed);
        //            writer.Write((byte)map.Tiles[x, y].Height);
        //        }
        //}

        #endregion

        /// <summary>
        ///     Führt eine Plausibilitätsprüfung der Karten-Einstellungen durch
        /// </summary>
        public void CheckMap()
        {
            // Tiles prüfen
            if (tiles == null)
                throw new InvalidMapException("Tiles Array is null");

            Index2 cells = GetCellCount();

            // Karten Dimensionen checken
            if (cells.X < MIN_WIDTH)
                throw new InvalidMapException(string.Format("Map must have at least {0} Columns", MIN_WIDTH));
            if (cells.X > MAX_WIDTH)
                throw new InvalidMapException(string.Format("Map must have a maximum of {0} Columns", MAX_WIDTH));

            if (cells.Y < MIN_HEIGHT)
                throw new InvalidMapException(string.Format("Map must have at least {0} Rows", MIN_HEIGHT));
            if (cells.Y > MAX_HEIGHT)
                throw new InvalidMapException(string.Format("Map must have a maximum of {0} Rows", MAX_HEIGHT));

            // Startpunkte überprüfen
            if (StartPoints == null)
                throw new InvalidMapException("The List of StartPoints is null");

            // Spieleranzahl prüfen
            if (GetPlayerCount() < MIN_STARTPOINTS)
                throw new InvalidMapException(string.Format("There must be at least {0} player", MIN_STARTPOINTS));
            if (GetPlayerCount() > MAX_STARTPOINTS)
                throw new InvalidMapException(string.Format("The maximum Player Count is {0}", MAX_STARTPOINTS));

            // TODO: Check Cell-Structure

            // Alle Startpunkte überprüfen
            for (int i = 0; i < StartPoints.Length; i++)
            {
                // Prüfen, ob die Zelle existiert
                Index2 startPoint = StartPoints[i];
                if (startPoint.X < 0 || startPoint.X >= tiles.GetLength(0) ||
                    startPoint.Y < 0 || startPoint.Y >= tiles.GetLength(1))
                    throw new InvalidMapException(string.Format("StartPoint {0} is out of map bounds", i));

                // Prüfen, ob es sich um eine flache Zelle handelt
                if (!this[startPoint.X, startPoint.Y].CanEnter)
                    throw new InvalidMapException(string.Format("StartPoint {0} is not placed on a plane Cell", i));

                // Prüfen, ob noch ein anderer Startpoint auf der selben Zelle ist.
                for (int j = 0; j < StartPoints.Length; j++)
                    if (i != j && StartPoints[i] == StartPoints[j])
                        throw new InvalidMapException(string.Format("StartPoints {0} and {1} are on the same Cell", i, j));
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
    }

    /// <summary>
    ///     Basis Exception für alle Fehler innerhalb einer Map.
    /// </summary>
    public sealed class InvalidMapException : Exception
    {
        /// <summary>
        /// Erstellt eine neue Instanz der Map-Exception.
        /// </summary>
        public InvalidMapException()
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz der Map-Exception.
        /// </summary>
        /// <param name="message">Zusätzliche Nachricht</param>
        public InvalidMapException(string message) : base(message)
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz der Map-Exception.
        /// </summary>
        /// <param name="message">Zusätzliche Nachricht</param>
        /// <param name="innerException">Verpackte Exception</param>
        public InvalidMapException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz der Map-Exception.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public InvalidMapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}