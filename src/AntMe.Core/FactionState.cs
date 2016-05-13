using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base State Class for all Faction States.
    /// </summary>
    public class FactionState : PropertyList<FactionStateProperty>, ISerializableState
    {
        /// <summary>
        /// Name of the Player.
        /// </summary>
        [DisplayName("Name")]
        [Description("Name of the Player")]
        [ReadOnly(true)]
        [Category("Static")]
        public string Name { get; set; }

        /// <summary>
        /// Slot Index.
        /// </summary>
        [DisplayName("Slot")]
        [Description("Slot Index")]
        [ReadOnly(true)]
        [Category("Static")]
        public byte SlotIndex { get; set; }

        /// <summary>
        /// Faction Name.
        /// </summary>
        [DisplayName("Faction Name")]
        [Description("Name of the Faction")]
        [ReadOnly(true)]
        [Category("Static")]
        public string FactionName { get; set; }

        /// <summary>
        /// Player Color.
        /// </summary>
        [DisplayName("Color")]
        [Description("Player Color")]
        [ReadOnly(true)]
        [Category("Static")]
        public PlayerColor PlayerColor { get; set; }

        /// <summary>
        /// Start Point on Map.
        /// </summary>
        [DisplayName("Start Point")]
        [Description("Start Point")]
        [ReadOnly(true)]
        [Category("Static")]
        public Vector2 StartPoint { get; set; }

        /// <summary>
        /// Total Points.
        /// </summary>
        [DisplayName("Points")]
        [Description("Total Points")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Points { get; set; }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Name ?? string.Empty);
            stream.Write(FactionName ?? string.Empty);
            stream.Write((byte)PlayerColor);
            stream.Write(StartPoint.X);
            stream.Write(StartPoint.Y);
            stream.Write(Points);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Points);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeFirst(BinaryReader stream, byte version)
        {
            Name = stream.ReadString();
            FactionName = stream.ReadString();
            PlayerColor = (PlayerColor)stream.ReadByte();
            StartPoint = new Vector2(
                stream.ReadSingle(),
                stream.ReadSingle());
            Points = stream.ReadInt32();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Points = stream.ReadInt32();
        }

        /// <summary>
        /// Returns a representive String for this State.
        /// </summary>
        /// <returns>State Description</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1}/{2})", Name, FactionName, SlotIndex);
        }
    }

    /// <summary>
    /// List of all possible Player Colors.
    /// </summary>
    public enum PlayerColor
    {
        /// <summary>
        /// Black
        /// </summary>
        Black = 0,

        /// <summary>
        /// Red
        /// </summary>
        Red = 1,

        /// <summary>
        /// Blue
        /// </summary>
        Blue = 2,

        /// <summary>
        /// Cyan
        /// </summary>
        Cyan = 3,

        /// <summary>
        /// Purple
        /// </summary>
        Purple = 4,

        /// <summary>
        /// Orange
        /// </summary>
        Orange = 5,

        /// <summary>
        /// Green
        /// </summary>
        Green = 6,

        /// <summary>
        /// White
        /// </summary>
        White = 7,

        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 8
    }
}