using System.Collections.Generic;
using System.IO;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Base Class for all Highlighting Properties.
    /// </summary>
    /// <typeparam name="T">Type of Highlight</typeparam>
    public abstract class HighlightsStateProperty<T> : LevelStateProperty
    {
        /// <summary>
        /// List of Highlights
        /// </summary>
        public IList<T> Highlights { get; private set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public HighlightsStateProperty() : base()
        {
            Highlights = new List<T>();
        }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Related Level</param>
        /// <param name="property">Related Level Property</param>
        public HighlightsStateProperty(Level level, LevelProperty property) : base(level, property)
        {
            Highlights = new List<T>();
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
        }
    }
}
