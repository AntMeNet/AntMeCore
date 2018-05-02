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
        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public MapState() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="map">Reference to the related Map</param>
        public MapState(Map map)
        {
            var size = map.GetCellCount();
            Tiles = new MapTileState[size.X, size.Y];
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
            return new Vector2(cells.X * Map.Cellsize, cells.Y * Map.Cellsize);
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeFirst(BinaryWriter stream, byte version)
        {
            if (version != 2)
                throw new NotSupportedException("Stream Version not supported");

            // Serialize basics
            Index2 cells = GetCellCount();
            stream.Write(BlockBorder);
            stream.Write((byte)cells.X);
            stream.Write((byte)cells.Y);
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
            if (version != 2)
                throw new NotSupportedException("Stream Version not supported");

            BlockBorder = stream.ReadBoolean();
            Tiles = new MapTileState[
                stream.ReadByte(), 
                stream.ReadByte()];
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