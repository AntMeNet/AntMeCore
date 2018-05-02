using System;
using System.Runtime.Serialization;

namespace AntMe
{
    /// <summary>
    /// Exception for Cell/MapTile specific Validation Exceptions.
    /// </summary>
    [Serializable]
    public sealed class InvalidMapTileException : Exception
    {
        /// <summary>
        /// Gets or sets the Index of the affected Map Tile.
        /// </summary>
        public Index2 CellIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public InvalidMapTileException() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <param name="message"></param>
        public InvalidMapTileException(Index2 cellIndex, string message) : base(message)
        {
            CellIndex = cellIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidMapTileException(Index2 cellIndex, string message, Exception innerException)
            : base(message, innerException)
        {
            CellIndex = cellIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public InvalidMapTileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            CellIndex = new Index2(
                info.GetInt32("CellIndex.X"), 
                info.GetInt32("CellIndex.Y"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("CellIndex.X", CellIndex.X);
            info.AddValue("CellIndex.Y", CellIndex.Y);
        }
    }
}
