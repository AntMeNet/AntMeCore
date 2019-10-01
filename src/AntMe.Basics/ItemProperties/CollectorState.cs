using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     Base State for all Good Collector Properties.
    /// </summary>
    public abstract class CollectorState : GoodsState
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public CollectorState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public CollectorState(Item item, CollectorProperty property) : base(item, property)
        {
            // Bind Range
            CollectorRange = property.CollectorRange;
            property.OnCollectorRangeChanged += (i, v) => { CollectorRange = v; };
        }

        /// <summary>
        ///     Gets the Collector Range.
        /// </summary>
        [DisplayName("Collector Range")]
        [Description("Gets the Collector Range.")]
        [ReadOnly(true)]
        [Category("static")]
        public float CollectorRange { get; set; }

        /// <summary>
        ///     Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
            stream.Write(CollectorRange);
        }

        /// <summary>
        ///     Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
        }

        /// <summary>
        ///     Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);
            CollectorRange = stream.ReadSingle();
        }

        /// <summary>
        ///     Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
        }
    }
}