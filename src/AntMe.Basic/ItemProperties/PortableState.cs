using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State Property for all portable Items.
    /// </summary>
    public sealed class PortableState : ItemStateProperty
    {
        /// <summary>
        /// Weight.
        /// </summary>
        [DisplayName("Weight")]
        [Description("Weight")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float Weight { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public PortableState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public PortableState(Item item, PortableProperty property) : base(item, property)
        {
            // Bind Weight Property to the Item Weight
            Weight = property.PortableWeight;
            property.OnPortableWeightChanged += (i, v) => { Weight = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Weight);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Weight);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Weight = stream.ReadSingle();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Weight = stream.ReadSingle();
        }
    }
}
