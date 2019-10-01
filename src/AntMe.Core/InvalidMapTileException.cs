using System;
using System.Runtime.Serialization;

namespace AntMe
{
    /// <summary>
    ///     Exception for Cell/MapTile specific Validation Exceptions.
    /// </summary>
    [Serializable]
    public sealed class InvalidMapTileException : Exception
    {
        public InvalidMapTileException()
        {
        }

        public InvalidMapTileException(Index2 cellIndex, string message) : base(message)
        {
            CellIndex = cellIndex;
        }

        public InvalidMapTileException(Index2 cellIndex, string message, Exception innerException)
            : base(message, innerException)
        {
            CellIndex = cellIndex;
        }

        public InvalidMapTileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            CellIndex = new Index2(
                info.GetInt32("CellIndex.X"),
                info.GetInt32("CellIndex.Y"));
        }

        /// <summary>
        ///     Gets or sets the Index of the affected Map Tile.
        /// </summary>
        public Index2 CellIndex { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("CellIndex.X", CellIndex.X);
            info.AddValue("CellIndex.Y", CellIndex.Y);
        }
    }
}