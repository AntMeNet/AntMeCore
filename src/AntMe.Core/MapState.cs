using System;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Map State.
    /// </summary>
    public sealed class MapState : PropertyList<MapStateProperty>, ISerializableState
    {
        private readonly Map Map;

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public MapState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="map">Reference to the related Map</param>
        public MapState(Map map) : base()
        {
            Map = map;
        }

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
        public MapTileState[,] Tiles { get; set; }

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
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
            //if (Tiles == null)
            //    throw new NotSupportedException("Tiles are not initialized");

            //// Serialize Tiles
            //stream.Write(BlockBorder);
            //stream.Write(Tiles.GetLength(0));
            //stream.Write(Tiles.GetLength(1));
            //for (int y = 0; y < Tiles.GetLength(1); y++)
            //    for (int x = 0; x < Tiles.GetLength(0); x++)
            //    {
            //        stream.Write((byte)Tiles[x, y].Shape);
            //        stream.Write((byte)Tiles[x, y].Speed);
            //        stream.Write((byte)Tiles[x, y].Height);
            //    }
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
            throw new NotImplementedException();
            //BlockBorder = stream.ReadBoolean();
            //int width = stream.ReadInt32();
            //int height = stream.ReadInt32();
            //Tiles = new MapTile[width, height];

            //for (int y = 0; y < height; y++)
            //    for (int x = 0; x < width; x++)
            //    {
            //        Tiles[x, y] = new MapTile()
            //        {
            //            Shape = (TileShape)stream.ReadByte(),
            //            Speed = (TileSpeed)stream.ReadByte(),
            //            Height = (TileHeight)stream.ReadByte()
            //        }; 
            //    }
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