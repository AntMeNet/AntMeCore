using System.IO;

namespace AntMe
{
    /// <summary>
    ///     Interface for all serializable States.
    /// </summary>
    public interface ISerializableState
    {
        /// <summary>
        ///     Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        void SerializeFirst(BinaryWriter stream, byte version);

        /// <summary>
        ///     Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        void SerializeUpdate(BinaryWriter stream, byte version);

        /// <summary>
        ///     Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        void DeserializeFirst(BinaryReader stream, byte version);

        /// <summary>
        ///     Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        void DeserializeUpdate(BinaryReader stream, byte version);
    }
}