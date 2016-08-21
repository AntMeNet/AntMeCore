using System;
using System.IO;

namespace AntMe.Serialization
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
        Frame Deserialize(BinaryReader reader);
    }
}
