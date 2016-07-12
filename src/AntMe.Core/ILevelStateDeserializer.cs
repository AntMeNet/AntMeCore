using System;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Interface for all Versions of a LevelState Deserializer.
    /// </summary>
    internal interface ILevelStateDeserializer : IDisposable
    {
        /// <summary>
        /// Deserializes the next Frame of the Stream.
        /// </summary>
        /// <param name="reader">Input Stream</param>
        /// <returns>Deserialized State</returns>
        LevelState Deserialize(BinaryReader reader);
    }
}
