using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     State Property for Carrier Items.
    /// </summary>
    public sealed class CarrierState : ItemStateProperty
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public CarrierState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public CarrierState(Item item, CarrierProperty property) : base(item, property)
        {
            // Bind Strength to the Item Strength
            CarrierStrength = property.CarrierStrength;
            property.OnCarrierStrengthChanged += (i, v) => { CarrierStrength = v; };
        }

        /// <summary>
        ///     Carrier Strength.
        /// </summary>
        [DisplayName("Carrier Strength")]
        [Description("Carrier Strength")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float CarrierStrength { get; set; }

        /// <summary>
        ///     Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(CarrierStrength);
        }

        /// <summary>
        ///     Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(CarrierStrength);
        }

        /// <summary>
        ///     Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            CarrierStrength = stream.ReadSingle();
        }

        /// <summary>
        ///     Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            CarrierStrength = stream.ReadSingle();
        }
    }
}