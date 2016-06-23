using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all State Properties.
    /// </summary>
    public abstract class StateProperty : Property, ISerializableState
    {
        /// <summary>
        /// Reference to the related Property.
        /// </summary>
        protected readonly Property Property;

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public StateProperty() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="property">Reference to the related Engine Property</param>
        public StateProperty(Property property)
        {
            Property = property;
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void SerializeFirst(BinaryWriter stream, byte version);

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void SerializeUpdate(BinaryWriter stream, byte version);

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void DeserializeFirst(BinaryReader stream, byte version);

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public abstract void DeserializeUpdate(BinaryReader stream, byte version);
    }
}
