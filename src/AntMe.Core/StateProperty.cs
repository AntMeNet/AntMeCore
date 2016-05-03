using System.IO;

namespace AntMe
{
    /// <summary>
    /// Basis-Klasse für alle State Properties.
    /// </summary>
    public abstract class StateProperty : Property, ISerializableState
    {
        public StateProperty() { }

        public StateProperty(ItemProperty property) { }

        /// <summary>
        /// Serializer-Methode für das erste Vorkommen dieser Instanz.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void SerializeFirst(BinaryWriter stream, byte version);

        /// <summary>
        /// Serializer-Methode für das erneute Vorkommen dieser Instanz.
        /// </summary>
        /// <param name="stream">Output-Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void SerializeUpdate(BinaryWriter stream, byte version);

        /// <summary>
        /// Deserializer-Methode für das erste Vorkommen dieser Instanz.
        /// </summary>
        /// <param name="stream">Input-Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void DeserializeFirst(BinaryReader stream, byte version);

        /// <summary>
        /// Deserializer-Methode für das erneute Vorkommen dieser Instanz.
        /// </summary>
        /// <param name="stream">Input-Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void DeserializeUpdate(BinaryReader stream, byte version);
    }
}
