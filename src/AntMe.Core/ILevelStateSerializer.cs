using System;

namespace AntMe
{
    /// <summary>
    /// Interface for all Versions of a LevelState Serializer.
    /// </summary>
    internal interface ILevelStateSerializer : IDisposable
    {
        /// <summary>
        /// Serializes the next State.
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>Frame Data</returns>
        byte[] Serialize(LevelState state);
    }
}
