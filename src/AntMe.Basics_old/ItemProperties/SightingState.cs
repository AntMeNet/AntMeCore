using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State Property for all sighting Items.
    /// </summary>
    public sealed class SightingState : ItemStateProperty
    {
        /// <summary>
        /// View Range.
        /// </summary>
        [DisplayName("View Range")]
        [Description("View Range")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float ViewRange { get; set; }

        /// <summary>
        /// View Direction.
        /// </summary>
        [DisplayName("View Direction")]
        [Description("View Direction")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public short ViewDirection { get; set; }

        /// <summary>
        /// View Angle.
        /// 0 = Item can't see anything
        /// 90 = View Range is between -45 and 45 Degrees to the Direction
        /// 360 = No Limitations within the View Radius
        /// </summary>
        [DisplayName("View Angle")]
        [Description("View Angle")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float ViewAngle { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public SightingState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public SightingState(Item item, SightingProperty property) : base(item, property)
        {
            // Bind Direction to the Item Direction
            ViewDirection = (short)property.ViewDirection.Degree;
            property.OnViewDirectionChanged += (i, v) => { ViewDirection = (short)v.Degree; };

            // Bind Angle to the Item Angle
            ViewAngle = property.ViewAngle;
            property.OnViewAngleChanged += (i, v) => { ViewAngle = v; };

            // Bind View Range to the Item View Range
            ViewRange = property.ViewRange;
            property.OnViewRangeChanged += (i, v) => { ViewRange = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(ViewRange);
            stream.Write(ViewAngle);
            stream.Write(ViewDirection);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(ViewRange);
            stream.Write(ViewAngle);
            stream.Write(ViewDirection);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            ViewRange = stream.ReadSingle();
            ViewAngle = stream.ReadSingle();
            ViewDirection = stream.ReadInt16();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            ViewRange = stream.ReadSingle();
            ViewAngle = stream.ReadSingle();
            ViewDirection = stream.ReadInt16();
        }
    }
}
