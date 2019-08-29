using System;
using System.IO;

namespace AntMe.Serialization
{
    /// <summary>
    /// Interface for all Versions of a LevelState Serializer.
    /// </summary>
    internal interface ILevelStateSerializer : IDisposable
    {
        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="state">State</param>
        void Serialize(BinaryWriter writer, LevelState state);
    }
}
