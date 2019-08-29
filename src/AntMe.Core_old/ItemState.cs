using System;
using System.ComponentModel;
using System.IO;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Item States.
    /// </summary>
    public class ItemState : PropertyList<ItemStateProperty>, ISerializableState
    {
        /// <summary>
        /// Item Id.
        /// </summary>
        [DisplayName("ID")]
        [Description("Id of this Item")]
        [ReadOnly(true)]
        [Category("Static")]
        public int Id { get; set; }

        /// <summary>
        /// Item Position.
        /// </summary>
        [DisplayName("Position")]
        [Description("Item Position")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public Vector3 Position { get; set; }

        /// <summary>
        /// Item Radius.
        /// </summary>
        [DisplayName("Radius")]
        [Description("Item Radius")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float Radius { get; set; }

        /// <summary>
        /// Item Orientation.
        /// </summary>
        [DisplayName("Orientation")]
        [Description("Item Orientation")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public short Orientation { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public ItemState() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Reference to the related Engine Item</param>
        public ItemState(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            Id = item.Id;
            Position = item.Position;
            Orientation = (short)item.Orientation.Degree;
            Radius = item.Radius;

            item.PositionChanged += (i, v) => { Position = v; };
            item.OrientationChanged += (i, v) => { Orientation = (short)v.Degree; };
            item.RadiusChanged += (i, v) => { Radius = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Position.X);
            stream.Write(Position.Y);
            stream.Write(Position.Z);
            stream.Write(Radius);
            stream.Write(Orientation);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Position.X);
            stream.Write(Position.Y);
            stream.Write(Position.Z);
            stream.Write(Radius);
            stream.Write(Orientation);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeFirst(BinaryReader stream, byte version)
        {
            Position = new Vector3(
                stream.ReadSingle(),
                stream.ReadSingle(),
                stream.ReadSingle());
            Radius = stream.ReadSingle();
            Orientation = stream.ReadInt16();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public virtual void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Position = new Vector3(
                stream.ReadSingle(),
                stream.ReadSingle(),
                stream.ReadSingle());
            Radius = stream.ReadSingle();
            Orientation = stream.ReadInt16();
        }

        /// <summary>
        /// Returns a representive String for this State.
        /// </summary>
        /// <returns>State Description</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }
    }
}