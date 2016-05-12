using System;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Map State.
    /// </summary>
    public sealed class MapState : ISerializableState
    {
        /// <summary>
        /// Is Border blocked.
        /// </summary>
        [DisplayName("Border Block")]
        [Description("Is Border blocked.")]
        [ReadOnly(true)]
        [Category("Static")]
        public bool BlockBorder { get; set; }

        /// <summary>
        /// Cell Description for the Map as a 2D Array of Map Tiles.
        /// </summary>
        [Browsable(false)]
        public MapTile[,] Tiles { get; set; }

        /// <summary>
        /// Returns the Cell Count of this Map.
        /// </summary>
        /// <returns>Cell Count</returns>
        public Index2 GetCellCount()
        {
            if (Tiles != null)
                return new Index2(Tiles.GetLength(0), Tiles.GetLength(1));
            return Index2.Zero;
        }

        /// <summary>
        /// Calculcates the real Map Size in World Units.
        /// </summary>
        /// <returns>Size of tha Map in World Units</returns>
        public Vector2 GetSize()
        {
            Index2 cells = GetCellCount();
            return new Vector2(cells.X * Map.CELLSIZE, cells.Y * Map.CELLSIZE);
        }

        /// <summary>
        /// Calculates the Z Coordinate in World Units at the given Position.
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Z Coordinate</returns>
        public float GetHeight(float x, float y)
        {
            return GetHeight(new Vector2(x, y));
        }

        /// <summary>
        /// Calculates the Z Coordinate in World Units at the given Position.
        /// </summary>
        /// <param name="point">Koordinate</param>
        /// <returns>Z Coordinate</returns>
        public float GetHeight(Vector2 point)
        {
            if (Tiles == null)
                throw new NotSupportedException("Map is not Initialized");

            // Determinate the right Cell
            Index2 cellCount = GetCellCount();
            Index2 cell = Map.GetCellIndex(point, cellCount);

            // Calculate Height based on the Cell Type
            return Map.GetHeight(Tiles[cell.X, cell.Y],
                new Vector2(
                    point.X - (cell.X * Map.CELLSIZE), 
                    point.Y - (cell.Y * Map.CELLSIZE)));
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
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

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
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

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }
    }
}