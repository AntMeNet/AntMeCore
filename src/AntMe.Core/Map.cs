using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace AntMe
{
    /// <summary>
    ///     Repräsentiert eine Karte mit einer Höhenmap und einer Flächendefinition.
    /// </summary>
    [Serializable]
    public class Map
    {
        #region Constants

        /// <summary>
        ///     Kantenlänge einer Zelle (quadratisch).
        /// </summary>
        public const float CELLSIZE = 50f;

        /// <summary>
        ///     Minimale Anzahl Spalten einer Karte.
        /// </summary>
        public const int MIN_WIDTH = 10;

        /// <summary>
        ///     Minimale Anzahl Zeilen einer Karte.
        /// </summary>
        public const int MIN_HEIGHT = 10;

        /// <summary>
        ///     Maximale Anzahl Spalten einer Karte.
        /// </summary>
        public const int MAX_WIDTH = 100;

        /// <summary>
        ///     Maximale Anzahl Zeilen einer Karte.
        /// </summary>
        public const int MAX_HEIGHT = 100;

        /// <summary>
        ///     Minimale Anzahl Startpunkte einer Karte.
        /// </summary>
        public const int MIN_STARTPOINTS = 1;

        /// <summary>
        ///     Maximale Anzahl Startpunkte einer Karte.
        /// </summary>
        public const int MAX_STARTPOINTS = 8;

        /// <summary>
        ///     Minimaler Höhenwert von Einheiten.
        /// </summary>
        public const float MIN_Z = 0f;

        /// <summary>
        ///     Maximaler Höhenwert von Einheiten.
        /// </summary>
        public const float MAX_Z = 50f;

        #endregion

        /// <summary>
        ///     Definition des Randverhaltens
        /// </summary>
        [DisplayName("Border Behavior")]
        [Description("Legt fest, ob der Rand des Spielfeldes blockt.")]
        public bool BlockBorder { get; set; }

        /// <summary>
        ///     Tiles für die einzelnen Zellen.
        ///     TODO: Test schreiben
        /// </summary>
        [DisplayName("Tiles")]
        [Description("Definition der Tiles für einzelne Zellen")]
        public MapTile[,] Tiles { get; set; }

        /// <summary>
        ///     Auflistung der Startpunkte.
        /// </summary>
        [DisplayName("Start Points")]
        [Description("Auflistung der Startpunkte.")]
        public Index2[] StartPoints { get; set; }

        #region Statische Methoden (Generatoren und Serialisierer)

        /// <summary>
        ///     Erzeugt eine Standard-Karte im angegebenen Format.
        /// </summary>
        /// <param name="width">Anzahl Spalten</param>
        /// <param name="height">Anzahl Zeilen</param>
        /// <param name="blockBorder">Wird der Rand blockieren?</param>
        /// <param name="initialSpeed">Inititale Bodenbeschaffenheit</param>
        /// <param name="initialHeight">Inititale Höhe</param>
        /// <returns>Neue Map-Instanz</returns>
        public static Map CreateMap(int width, int height, bool blockBorder,
            TileSpeed initialSpeed = TileSpeed.Normal,
            TileHeight initialHeight = TileHeight.Medium)
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

            var map = new Map();
            map.BlockBorder = blockBorder;

            // Create Tiles
            map.Tiles = new MapTile[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    map.Tiles[x, y] = new MapTile
                    {
                        Height = initialHeight,
                        Shape = TileShape.Flat,
                        Speed = initialSpeed
                    };
                }

            // Create Players
            int dx = width / 6;
            int dy = height / 6;
            map.StartPoints = new Index2[8];
            map.StartPoints[0] = new Index2(dx, dy);
            map.StartPoints[1] = new Index2(5 * dx, 5 * dy);
            map.StartPoints[2] = new Index2(5 * dx, dy);
            map.StartPoints[3] = new Index2(dx, 5 * dy);
            map.StartPoints[4] = new Index2(3 * dx, dy);
            map.StartPoints[5] = new Index2(3 * dx, 5 * dy);
            map.StartPoints[6] = new Index2(dx, 3 * dy);
            map.StartPoints[7] = new Index2(5 * dx, 3 * dy);

            return map;
        }

        /// <summary>
        ///     Deserialisiert eine Map aus einem gegebenen Stream.
        ///     TODO: Test!!!
        /// </summary>
        /// <param name="stream">Quellstream</param>
        /// <returns>Deserialisierte Map</returns>
        public static Map Deserialize(Stream stream)
        {
            var reader = new BinaryReader(stream);

            // Intro (Typ und Version)
            if (reader.ReadString() != "AntMe! Map")
                throw new Exception("This is not a AntMe! Map");
            if (reader.ReadByte() != 1)
                throw new Exception("Wrong Version");

            var map = new Map();

            // Globale Infos (Border, Width, Height)
            map.BlockBorder = reader.ReadBoolean();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            int playercount = reader.ReadByte();

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
            map.Tiles = new MapTile[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    map.Tiles[x, y] = new MapTile
                    {
                        Shape = (TileShape)reader.ReadByte(),
                        Speed = (TileSpeed)reader.ReadByte(),
                        Height = (TileHeight)reader.ReadByte()
                    };
                }

            return map;
        }

        /// <summary>
        ///     Serialisiert eine Map in einen Stream.
        ///     TODO: Test!!!
        /// </summary>
        /// <param name="stream">Zielstream</param>
        /// <param name="map">Zu serialisierende Map</param>
        public static void Serialize(Stream stream, Map map)
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


            var writer = new BinaryWriter(stream);

            // Intro (Typ und Version)
            writer.Write("AntMe! Map");
            writer.Write((byte)1);

            // Global Infos (Block, Width, Height)
            writer.Write(map.BlockBorder);
            writer.Write(map.Tiles.GetLength(0));
            writer.Write(map.Tiles.GetLength(1));
            writer.Write((byte)map.StartPoints.GetLength(0));

            // Startpoints
            for (int i = 0; i < map.StartPoints.GetLength(0); i++)
            {
                writer.Write(map.StartPoints[i].X);
                writer.Write(map.StartPoints[i].Y);
            }

            // Zelleninfos
            for (int y = 0; y < map.Tiles.GetLength(1); y++)
                for (int x = 0; x < map.Tiles.GetLength(0); x++)
                {
                    writer.Write((byte)map.Tiles[x, y].Shape);
                    writer.Write((byte)map.Tiles[x, y].Speed);
                    writer.Write((byte)map.Tiles[x, y].Height);
                }
        }

        /// <summary>
        ///     Ermittelt die Höhe der Position auf Basis der Zellen- und Positionsangaben.
        ///     TODO: Test!
        /// </summary>
        /// <param name="tile">Aktuelle Zelle</param>
        /// <param name="relativePosition">Position relativ zur Zelle</param>
        /// <returns>Höhe in Spieleinheiten</returns>
        public static float GetHeight(MapTile tile, Vector2 relativePosition)
        {
            var cropped = new Vector2(
                Math.Max(0, Math.Min(CELLSIZE, relativePosition.X)),
                Math.Max(0, Math.Min(CELLSIZE, relativePosition.Y)));

            // Basic Height
            float basic = 0;
            switch (tile.Height)
            {
                case TileHeight.Low:
                    basic = MapTile.HEIGHT_LOW;
                    break;
                case TileHeight.Medium:
                    basic = MapTile.HEIGHT_MEDIUM;
                    break;
                case TileHeight.High:
                    basic = MapTile.HEIGHT_HIGH;
                    break;
                default:
                    throw new ArgumentException("Height of tile is undefined");
            }

            // Im Falle eines flachen Tile einfach basiswert zurück geben
            if (tile.Shape == TileShape.Flat)
                return basic;

            // In allen anderen Fällen darf der Height-Wert nicht auf max sein.
            if (tile.Height == TileHeight.High)
                throw new ArgumentException("Height=High is not valid for ramps and canyon Tiles");

            // Diff ermitteln
            float diff = 0;
            switch (tile.Height + 1)
            {
                case TileHeight.Low:
                    diff = MapTile.HEIGHT_LOW - basic;
                    break;
                case TileHeight.Medium:
                    diff = MapTile.HEIGHT_MEDIUM - basic;
                    break;
                case TileHeight.High:
                    diff = MapTile.HEIGHT_HIGH - basic;
                    break;
                default:
                    throw new ArgumentException("Height has an invalid value");
            }

            var projected = new Vector2(relativePosition.X / CELLSIZE, relativePosition.Y / CELLSIZE);

            Func<float, float> ramp = v => ((v * diff) + basic);

            // Shape berücksichtigen
            switch (tile.Shape)
            {
                case TileShape.CanyonBottom:
                case TileShape.RampBottom:
                    // Curve from bottom to top
                    return ramp(1 - projected.Y);
                case TileShape.CanyonTop:
                case TileShape.RampTop:
                    // Curve from top to bottom
                    return ramp(projected.Y);
                case TileShape.CanyonLeft:
                case TileShape.RampLeft:
                    // Curve from left to right
                    return ramp(projected.X);
                case TileShape.CanyonRight:
                case TileShape.RampRight:
                    // Curve from right to left
                    return ramp(1 - projected.X);
                case TileShape.CanyonUpperLeftConvex:
                    return (projected.Y > projected.X) ? ramp(projected.X) : ramp(projected.Y);
                case TileShape.CanyonUpperRightConvex:
                    return ((1 - projected.Y) > projected.X) ? ramp(projected.Y) : ramp(1 - projected.X);
                case TileShape.CanyonLowerLeftConvex:
                    return ((1 - projected.Y) > projected.X) ? ramp(projected.X) : ramp(1 - projected.Y);
                case TileShape.CanyonLowerRightConvex:
                    return (projected.Y > projected.X) ? ramp(1 - projected.Y) : ramp(1 - projected.X);
                case TileShape.CanyonUpperLeftConcave:
                    return (projected.Y > projected.X) ? ramp(1 - projected.X) : ramp(1 - projected.Y);
                case TileShape.CanyonUpperRightConcave:
                    return ((1 - projected.Y) > projected.X) ? ramp(1 - projected.Y) : ramp(projected.X);
                case TileShape.CanyonLowerLeftConcave:
                    return ((1 - projected.Y) > projected.X) ? ramp(1 - projected.X) : ramp(projected.Y);
                case TileShape.CanyonLowerRightConcave:
                    return (projected.Y > projected.X) ? ramp(projected.Y) : ramp(projected.X);

                default:
                    throw new ArgumentException("Unexpected Tile Shape");
            }
        }

        /// <summary>
        ///     Ermittelt die Zelle, in der sich die angegebene Position gefindet
        ///     und limitiert das Ergebnis auf die verfügbaren Zellen.
        /// </summary>
        /// <param name="position">Gegebene Position</param>
        /// <param name="cellCount">Zellenmenge</param>
        /// <returns>Zellenindex</returns>
        public static Index2 GetCellIndex(Vector2 position, Index2 cellCount)
        {
            var indexX = (int)(position.X / CELLSIZE);
            var indexY = (int)(position.Y / CELLSIZE);

            if (indexX < 0)
                indexX = 0;
            if (indexY < 0)
                indexY = 0;
            if (indexX >= cellCount.X)
                indexX = cellCount.X - 1;
            if (indexY >= cellCount.Y)
                indexY = cellCount.Y - 1;

            return new Index2(indexX, indexY);
        }

        #endregion

        /// <summary>
        ///     Führt eine Plausibilitätsprüfung der Karten-Einstellungen durch
        /// </summary>
        public void CheckMap()
        {
            // Tiles prüfen
            if (Tiles == null)
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

            // Alles Tiles prüfen
            string message;
            for (int y = 0; y < Tiles.GetLength(1); y++)
                for (int x = 0; x < Tiles.GetLength(0); x++)
                    if (!CheckCell(x, y, out message))
                        throw new InvalidMapException(string.Format("Cell {0}/{1} has the following Error: {2}", x, y,
                            message));

            // Alle Startpunkte überprüfen
            for (int i = 0; i < StartPoints.Length; i++)
            {
                // Prüfen, ob die Zelle existiert
                Index2 startPoint = StartPoints[i];
                if (startPoint.X < 0 || startPoint.X >= Tiles.GetLength(0) ||
                    startPoint.Y < 0 || startPoint.Y >= Tiles.GetLength(1))
                    throw new InvalidMapException(string.Format("StartPoint {0} is out of map bounds", i));

                // Prüfen, ob es sich um eine flache Zelle handelt
                if (Tiles[startPoint.X, startPoint.Y].Shape != TileShape.Flat)
                    throw new InvalidMapException(string.Format("StartPoint {0} is not placed on a plane Cell", i));

                // Prüfen, ob noch ein anderer Startpoint auf der selben Zelle ist.
                for (int j = 0; j < StartPoints.Length; j++)
                    if (i != j && StartPoints[i] == StartPoints[j])
                        throw new InvalidMapException(string.Format("StartPoints {0} and {1} are on the same Cell", i, j));
            }
        }

        /// <summary>
        ///     Converts a given position into a valid index on the map.
        ///     If the position lies outside the map, the closest index on the map is taken.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Index2 GetCellIndex(Vector3 position)
        {
            // Wenns keine Map gibt
            if (Tiles == null)
                return Index2.Zero;

            Index2 limit = GetCellCount();
            return GetCellIndex(new Vector2(position.X, position.Y), limit);
        }

        /// <summary>
        ///     Returns the Count of Rows and Cols of this Map.
        ///     TODO: Test
        /// </summary>
        /// <returns>Row- and Column-Count</returns>
        public Index2 GetCellCount()
        {
            if (Tiles != null)
                return new Index2(
                    Tiles.GetLength(0),
                    Tiles.GetLength(1));
            return Index2.Zero;
        }

        /// <summary>
        ///     Caclulates the total size of the map in Game-Units.
        ///     TODO: Test
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
        ///     Gibt die Anzahl verfügbarer Spieler-Startpunkte zurück.
        ///     TODO: test
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCount()
        {
            if (StartPoints != null)
                return StartPoints.Length;
            return 0;
        }

        /// <summary>
        ///     Ermittelt die Bodenhöhe zur gegebenen Position auf der aktuellen Karte.
        /// </summary>
        /// <param name="position">Kartenposition</param>
        /// <returns>Höhe</returns>
        public float GetHeight(Vector2 position)
        {
            Index2 cell = GetCellIndex(new Vector3(position.X, position.Y, 0));
            return GetHeight(
                Tiles[cell.X, cell.Y],
                new Vector2(position.X % CELLSIZE, position.Y % CELLSIZE));
        }

        #region CellChecking

        /// <summary>
        /// Prüft die angegebe Zelle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CheckCell(int x, int y, out string message)
        {
            if (x < 0 || x >= Tiles.GetLength(0))
                throw new ArgumentOutOfRangeException("X is out of range");
            if (y < 0 || y >= Tiles.GetLength(1))
                throw new ArgumentOutOfRangeException("Y is out of range");

            switch (Tiles[x, y].Shape)
            {
                case TileShape.Flat:
                    return CheckFlatCell(x, y, out message);
                case TileShape.CanyonTop:
                    return CheckCanyonTopCell(x, y, out message);
                case TileShape.CanyonBottom:
                    return CheckCanyonBottomCell(x, y, out message);
                case TileShape.CanyonLeft:
                    return CheckCanyonLeftCell(x, y, out message);
                case TileShape.CanyonRight:
                    return CheckCanyonRightCell(x, y, out message);
                case TileShape.CanyonUpperLeftConvex:
                    return CheckCanyonUpperLeftConvexCell(x, y, out message);
                case TileShape.CanyonUpperRightConvex:
                    return CheckCanyonUpperRightConvexCell(x, y, out message);
                case TileShape.CanyonLowerLeftConvex:
                    return CheckCanyonLowerLeftConvexCell(x, y, out message);
                case TileShape.CanyonLowerRightConvex:
                    return CheckCanyonLowerRightConvexCell(x, y, out message);
                case TileShape.CanyonUpperLeftConcave:
                    return CheckCanyonUpperLeftConcaveCell(x, y, out message);
                case TileShape.CanyonUpperRightConcave:
                    return CheckCanyonUpperRightConcaveCell(x, y, out message);
                case TileShape.CanyonLowerLeftConcave:
                    return CheckCanyonLowerLeftConcaveCell(x, y, out message);
                case TileShape.CanyonLowerRightConcave:
                    return CheckCanyonLowerRightConcaveCell(x, y, out message);
                case TileShape.RampTop:
                    return CheckCanyonTopCell(x, y, out message);
                case TileShape.RampLeft:
                    return CheckCanyonLeftCell(x, y, out message);
                case TileShape.RampRight:
                    return CheckCanyonRightCell(x, y, out message);
                case TileShape.RampBottom:
                    return CheckCanyonBottomCell(x, y, out message);
                default:
                    throw new Exception("Unknown CellShape");
            }
        }

        private bool CheckFlatCell(int x, int y, out string message)
        {
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonTopCell(int x, int y, out string message)
        {
            // Linke Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.West, out message, false,
                TileShape.CanyonTop,
                TileShape.CanyonUpperLeftConvex,
                TileShape.CanyonLowerLeftConcave,
                TileShape.RampTop))
                return false;

            // Rechte Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.East, out message, false,
                TileShape.CanyonTop,
                TileShape.CanyonUpperRightConvex,
                TileShape.CanyonLowerRightConcave,
                TileShape.RampTop))
                return false;

            // Obere Zelle anschauen
            if (!IsFlat(x, y, Compass.North, Tiles[x, y].Height, out message))
                return false;

            // Untere Zelle anschauen
            if (!IsFlat(x, y, Compass.South, Tiles[x, y].Height + 1, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonBottomCell(int x, int y, out string message)
        {
            // Linke Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.West, out message, false,
                TileShape.CanyonBottom,
                TileShape.CanyonLowerLeftConvex,
                TileShape.CanyonUpperLeftConcave,
                TileShape.RampBottom))
                return false;

            // Rechte Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.East, out message, false,
                TileShape.CanyonBottom,
                TileShape.CanyonLowerRightConvex,
                TileShape.CanyonUpperRightConcave,
                TileShape.RampBottom))
                return false;

            // Obere Zelle anschauen
            if (!IsFlat(x, y, Compass.North, Tiles[x, y].Height + 1, out message))
                return false;

            // Untere Zelle anschauen
            if (!IsFlat(x, y, Compass.South, Tiles[x, y].Height, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonLeftCell(int x, int y, out string message)
        {
            // Obere Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.North, out message, false,
                TileShape.CanyonLeft,
                TileShape.CanyonUpperLeftConvex,
                TileShape.CanyonUpperRightConcave,
                TileShape.RampLeft))
                return false;

            // Untere Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.South, out message, false,
                TileShape.CanyonLeft,
                TileShape.CanyonLowerLeftConvex,
                TileShape.CanyonLowerRightConcave,
                TileShape.RampLeft))
                return false;

            // Linke Zelle anschauen
            if (!IsFlat(x, y, Compass.West, Tiles[x, y].Height, out message))
                return false;

            // Rechte Zelle anschauen
            if (!IsFlat(x, y, Compass.East, Tiles[x, y].Height + 1, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonRightCell(int x, int y, out string message)
        {
            // Obere Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.North, out message, false,
                TileShape.CanyonRight,
                TileShape.CanyonUpperRightConvex,
                TileShape.CanyonUpperLeftConcave,
                TileShape.RampRight))
                return false;

            // Untere Zelle anschauen
            if (!IsAllowedShape(x, y, Compass.South, out message, false,
                TileShape.CanyonRight,
                TileShape.CanyonLowerRightConvex,
                TileShape.CanyonLowerLeftConcave,
                TileShape.RampRight))
                return false;

            // Linke Zelle anschauen
            if (!IsFlat(x, y, Compass.West, Tiles[x, y].Height + 1, out message))
                return false;

            // Rechte Zelle anschauen
            if (!IsFlat(x, y, Compass.East, Tiles[x, y].Height, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonUpperLeftConvexCell(int x, int y, out string message)
        {
            // Lower cell
            if (!IsAllowedShape(x, y, Compass.South, out message, false,
                TileShape.CanyonLeft,
                TileShape.CanyonLowerRightConcave,
                TileShape.RampLeft))
                return false;

            // Right cell
            if (!IsAllowedShape(x, y, Compass.East, out message, false,
                TileShape.CanyonTop,
                TileShape.CanyonLowerRightConcave,
                TileShape.RampTop))
                return false;

            // Left
            if (!IsFlat(x, y, Compass.West, Tiles[x, y].Height, out message))
                return false;

            // Upperleft
            if (!IsFlat(x, y, Compass.NorthWest, Tiles[x, y].Height, out message))
                return false;

            // Top
            if (!IsFlat(x, y, Compass.North, Tiles[x, y].Height, out message))
                return false;

            // Lower Right
            if (!IsFlat(x, y, Compass.SouthEast, Tiles[x, y].Height + 1, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonUpperRightConvexCell(int x, int y, out string message)
        {
            // Left cell
            if (!IsAllowedShape(x, y, Compass.West, out message, false,
                TileShape.CanyonTop,
                TileShape.CanyonLowerLeftConcave,
                TileShape.RampTop))
                return false;

            // Lower cell
            if (!IsAllowedShape(x, y, Compass.South, out message, false,
                TileShape.CanyonRight,
                TileShape.CanyonLowerLeftConcave,
                TileShape.RampRight))
                return false;

            // Top
            if (!IsFlat(x, y, Compass.North, Tiles[x, y].Height, out message))
                return false;

            // UpperRight
            if (!IsFlat(x, y, Compass.NorthEast, Tiles[x, y].Height, out message))
                return false;

            // Right
            if (!IsFlat(x, y, Compass.East, Tiles[x, y].Height, out message))
                return false;

            // Lower Left
            if (!IsFlat(x, y, Compass.SouthWest, Tiles[x, y].Height + 1, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonLowerLeftConvexCell(int x, int y, out string message)
        {
            // Upper cell
            if (!IsAllowedShape(x, y, Compass.North, out message, false,
                TileShape.CanyonLeft,
                TileShape.CanyonUpperRightConcave,
                TileShape.RampLeft))
                return false;

            // Right cell
            if (!IsAllowedShape(x, y, Compass.East, out message, false,
                TileShape.CanyonBottom,
                TileShape.CanyonUpperRightConcave,
                TileShape.RampBottom))
                return false;

            // Left
            if (!IsFlat(x, y, Compass.West, Tiles[x, y].Height, out message))
                return false;

            // Lowerleft
            if (!IsFlat(x, y, Compass.SouthWest, Tiles[x, y].Height, out message))
                return false;

            // Down
            if (!IsFlat(x, y, Compass.South, Tiles[x, y].Height, out message))
                return false;

            // Upper Right
            if (!IsFlat(x, y, Compass.NorthEast, Tiles[x, y].Height + 1, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonLowerRightConvexCell(int x, int y, out string message)
        {
            // Left cell
            if (!IsAllowedShape(x, y, Compass.West, out message, false,
                TileShape.CanyonBottom,
                TileShape.CanyonUpperLeftConcave,
                TileShape.RampBottom))
                return false;

            // Top cell
            if (!IsAllowedShape(x, y, Compass.North, out message, false,
                TileShape.CanyonRight,
                TileShape.CanyonUpperLeftConcave,
                TileShape.RampRight))
                return false;

            // Bottom
            if (!IsFlat(x, y, Compass.South, Tiles[x, y].Height, out message))
                return false;

            // LowerRight
            if (!IsFlat(x, y, Compass.SouthEast, Tiles[x, y].Height, out message))
                return false;

            // Right
            if (!IsFlat(x, y, Compass.East, Tiles[x, y].Height, out message))
                return false;

            // Upper Left
            if (!IsFlat(x, y, Compass.NorthWest, Tiles[x, y].Height + 1, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonUpperLeftConcaveCell(int x, int y, out string message)
        {
            // Lower cell
            if (!IsAllowedShape(x, y, Compass.South, out message, false,
                TileShape.CanyonRight,
                TileShape.CanyonLowerRightConvex,
                TileShape.RampRight))
                return false;

            // Right cell
            if (!IsAllowedShape(x, y, Compass.East, out message, false,
                TileShape.CanyonBottom,
                TileShape.CanyonLowerRightConvex,
                TileShape.RampBottom))
                return false;

            // Left
            if (!IsFlat(x, y, Compass.West, Tiles[x, y].Height + 1, out message))
                return false;

            // UpperLeft
            if (!IsFlat(x, y, Compass.NorthWest, Tiles[x, y].Height + 1, out message))
                return false;

            // Top
            if (!IsFlat(x, y, Compass.North, Tiles[x, y].Height + 1, out message))
                return false;

            // Lower Right
            if (!IsFlat(x, y, Compass.SouthEast, Tiles[x, y].Height, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonUpperRightConcaveCell(int x, int y, out string message)
        {
            // Left cell
            if (!IsAllowedShape(x, y, Compass.West, out message, false,
                TileShape.CanyonBottom,
                TileShape.CanyonLowerLeftConvex,
                TileShape.RampBottom))
                return false;

            // Bottom cell
            if (!IsAllowedShape(x, y, Compass.South, out message, false,
                TileShape.CanyonLeft,
                TileShape.CanyonLowerLeftConvex,
                TileShape.RampLeft))
                return false;

            // Top
            if (!IsFlat(x, y, Compass.North, Tiles[x, y].Height + 1, out message))
                return false;

            // Upper Right
            if (!IsFlat(x, y, Compass.NorthEast, Tiles[x, y].Height + 1, out message))
                return false;

            // Right
            if (!IsFlat(x, y, Compass.East, Tiles[x, y].Height + 1, out message))
                return false;

            // Lower Left
            if (!IsFlat(x, y, Compass.SouthWest, Tiles[x, y].Height, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonLowerLeftConcaveCell(int x, int y, out string message)
        {
            // Top cell
            if (!IsAllowedShape(x, y, Compass.North, out message, false,
                TileShape.CanyonRight,
                TileShape.CanyonUpperRightConvex,
                TileShape.RampRight))
                return false;

            // Right cell
            if (!IsAllowedShape(x, y, Compass.East, out message, false,
                TileShape.CanyonTop,
                TileShape.CanyonUpperRightConvex,
                TileShape.RampTop))
                return false;

            // Left
            if (!IsFlat(x, y, Compass.West, Tiles[x, y].Height + 1, out message))
                return false;

            // Lower Left
            if (!IsFlat(x, y, Compass.SouthWest, Tiles[x, y].Height + 1, out message))
                return false;

            // Bottom
            if (!IsFlat(x, y, Compass.South, Tiles[x, y].Height + 1, out message))
                return false;

            // Upper Right
            if (!IsFlat(x, y, Compass.NorthEast, Tiles[x, y].Height, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        private bool CheckCanyonLowerRightConcaveCell(int x, int y, out string message)
        {
            // Left cell
            if (!IsAllowedShape(x, y, Compass.West, out message, false,
                TileShape.CanyonTop,
                TileShape.CanyonUpperLeftConvex,
                TileShape.RampTop))
                return false;

            // Upper cell
            if (!IsAllowedShape(x, y, Compass.North, out message, false,
                TileShape.CanyonLeft,
                TileShape.CanyonUpperLeftConvex,
                TileShape.RampLeft))
                return false;

            // Bottom
            if (!IsFlat(x, y, Compass.South, Tiles[x, y].Height + 1, out message))
                return false;

            // Lower Right
            if (!IsFlat(x, y, Compass.SouthEast, Tiles[x, y].Height + 1, out message))
                return false;

            // Right
            if (!IsFlat(x, y, Compass.East, Tiles[x, y].Height + 1, out message))
                return false;

            // Upper Left
            if (!IsFlat(x, y, Compass.NorthWest, Tiles[x, y].Height, out message))
                return false;

            // Alles gut
            message = string.Empty;
            return true;
        }

        /// <summary>
        ///     Ermittelt, ob die angegebene Zelle außerhalb des Rands liegt oder Shape None hat.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsNothing(int x, int y)
        {
            return (
                x < 0 || x >= Tiles.GetLength(0) ||
                y < 0 || y >= Tiles.GetLength(1));
        }

        /// <summary>
        ///     Ermittelt, ob die Zelle eine plane Fläche ist.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <param name="height"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool IsFlat(int x, int y, Compass direction, TileHeight height, out string message)
        {
            Index2 index = GetCellIndex(x, y, direction);

            // Auf definition prüfen
            if (IsNothing(index.X, index.Y))
            {
                message = GetCellName(direction) + " is not defined";
                return false;
            }

            // Shape prüfen
            if (Tiles[index.X, index.Y].Shape != TileShape.Flat)
            {
                message = GetCellName(direction) + " is not plain";
                return false;
            }

            // Höhe prüfen
            if (Tiles[index.X, index.Y].Height != height)
            {
                message = GetCellName(direction) + " has the wrong height";
                return false;
            }

            message = string.Empty;
            return true;
        }

        /// <summary>
        ///     Prüft, ob eine der erlaubten Shapes enthalten ist.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <param name="message"></param>
        /// <param name="nothing"></param>
        /// <param name="shapes"></param>
        /// <returns></returns>
        private bool IsAllowedShape(int x, int y, Compass direction, out string message, bool nothing,
            params TileShape[] shapes)
        {
            Index2 index = GetCellIndex(x, y, direction);

            // Auf Definition prüfen
            if (IsNothing(index.X, index.Y))
            {
                if (nothing)
                {
                    message = string.Empty;
                    return true;
                }
                message = GetCellName(direction) + " is not defined";
                return false;
            }

            // Finde Treffer
            TileShape tile = Tiles[index.X, index.Y].Shape;
            foreach (TileShape shape in shapes)
                if (shape == tile)
                {
                    message = string.Empty;
                    return true;
                }

            message = GetCellName(direction) + " has no valid shape";
            return false;
        }

        private Index2 GetCellIndex(int x, int y, Compass compass)
        {
            switch (compass)
            {
                case Compass.North:
                    return new Index2(x, y - 1);
                case Compass.South:
                    return new Index2(x, y + 1);
                case Compass.West:
                    return new Index2(x - 1, y);
                case Compass.East:
                    return new Index2(x + 1, y);
                case Compass.NorthWest:
                    return new Index2(x - 1, y - 1);
                case Compass.NorthEast:
                    return new Index2(x + 1, y - 1);
                case Compass.SouthWest:
                    return new Index2(x - 1, y + 1);
                case Compass.SouthEast:
                    return new Index2(x + 1, y + 1);
            }

            return Index2.Zero;
        }

        private string GetCellName(Compass compass)
        {
            switch (compass)
            {
                case Compass.North:
                    return "Upper Cell";
                case Compass.South:
                    return "Lower Cell";
                case Compass.West:
                    return "Left Cell";
                case Compass.East:
                    return "Right Cell";
                case Compass.NorthWest:
                    return "Upper Left Cell";
                case Compass.NorthEast:
                    return "Upper Right Cell";
                case Compass.SouthWest:
                    return "Lower Left Cell";
                case Compass.SouthEast:
                    return "Lower Right Cell";
            }
            return string.Empty;
        }

        #endregion
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