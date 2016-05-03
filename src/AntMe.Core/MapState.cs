using System;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Klasse für die Haltung des Map States. Durch Vererbung
    ///     können Map Packs weitere Informationen hier unterbringen.
    /// </summary>
    public class MapState : ISerializableState
    {
        /// <summary>
        /// Gibt an, ob der Rand die Spiel-Elemente blockiert.
        /// </summary>
        [DisplayName("Border Block")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public bool BlockBorder { get; set; }

        /// <summary>
        /// Gibt die Zelleninformationen in einem 2-dimensionalen Array zurück 
        /// oder legt diese fest.
        /// </summary>
        public MapTile[,] Tiles;

        /// <summary>
        /// Gibt die Anzahl an Zeilen und Spalten zurück.
        /// </summary>
        /// <returns></returns>
        public Index2 GetCellCount()
        {
            if (Tiles != null)
                return new Index2(Tiles.GetLength(0), Tiles.GetLength(1));
            return Index2.Zero;
        }

        /// <summary>
        /// Berechnet die Gesamtgröße der Karte in Simulationseinheiten.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize()
        {
            Index2 cells = GetCellCount();
            return new Vector2(cells.X * Map.CELLSIZE, cells.Y * Map.CELLSIZE);
        }

        /// <summary>
        /// Liefert die Zellenhöhe an der angefragten Position.
        /// </summary>
        /// <param name="x">X-Koordinate</param>
        /// <param name="y">Y-Koordinate</param>
        /// <returns>Höhe der Zelle</returns>
        public float GetHeight(float x, float y)
        {
            return GetHeight(new Vector2(x, y));
        }

        /// <summary>
        /// Liefert die Zellenhöhe an der angefragten Position.
        /// </summary>
        /// <param name="point">Koordinate</param>
        /// <returns>Höhe der Zelle</returns>
        public float GetHeight(Vector2 point)
        {
            // Falls keien Map vorhanden ist...
            if (Tiles == null)
                return 0f;

            Index2 cellCount = GetCellCount();
            Index2 cell = Map.GetCellIndex(point, cellCount);

            return Map.GetHeight(Tiles[cell.X, cell.Y],
                new Vector2(
                    point.X - (cell.X * Map.CELLSIZE), 
                    point.Y - (cell.Y * Map.CELLSIZE)));
        }

        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            if (Tiles == null)
                throw new NotSupportedException("Tiles are not initialized");

            // Serialize Tiles
            stream.Write(BlockBorder);
            stream.Write(Tiles.GetLength(0));
            stream.Write(Tiles.GetLength(1));
            for (int y = 0; y < Tiles.GetLength(1); y++)
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    stream.Write((byte)Tiles[x, y].Shape);
                    stream.Write((byte)Tiles[x, y].Speed);
                    stream.Write((byte)Tiles[x, y].Height);
                }
        }

        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }

        public void DeserializeFirst(BinaryReader stream, byte version)
        {
            BlockBorder = stream.ReadBoolean();
            int width = stream.ReadInt32();
            int height = stream.ReadInt32();
            Tiles = new MapTile[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Tiles[x, y] = new MapTile()
                    {
                        Shape = (TileShape)stream.ReadByte(),
                        Speed = (TileSpeed)stream.ReadByte(),
                        Height = (TileHeight)stream.ReadByte()
                    }; 
                }
        }

        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }
    }
}