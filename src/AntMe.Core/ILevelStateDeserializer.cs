namespace AntMe
{
    /// <summary>
    /// Interface for all Versions of a LevelState Deserializer.
    /// </summary>
    internal interface ILevelStateDeserializer
    {
        /// <summary>
        /// Deserializes the next Frame of the Stream.
        /// </summary>
        /// <param name="data">Frame Data</param>
        /// <returns>Deserialized State</returns>
        LevelState Deserialize(byte[] data);
    }
}
