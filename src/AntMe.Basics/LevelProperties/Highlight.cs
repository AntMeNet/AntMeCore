using System.IO;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Base Class for all Highlight Types.
    /// </summary>
    public abstract class Highlight
    {
        /// <summary>
        ///     Serializes the Hightlight into the given Binary Writer.
        /// </summary>
        /// <param name="writer">Output Writer</param>
        public virtual void Serialize(BinaryWriter writer)
        {
        }

        /// <summary>
        ///     Deserializes a Highlight from the given Binary Reader.
        /// </summary>
        /// <param name="reader">Input Reader</param>
        public virtual void Deserialize(BinaryReader reader)
        {
        }
    }
}