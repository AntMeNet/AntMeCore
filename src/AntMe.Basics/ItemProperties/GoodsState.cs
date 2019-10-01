using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     Base State for all Goods related Properties.
    /// </summary>
    public abstract class GoodsState : ItemStateProperty
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public GoodsState()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public GoodsState(Item item, GoodsProperty property) : base(item, property)
        {
            // Bind Amount
            Amount = property.Amount;
            property.OnAmountChanged += (i, v) => { Amount = v; };

            // Bind Capacity
            Capacity = property.Capacity;
            property.OnCapacityChanged += (i, v) => { Capacity = v; };
        }

        /// <summary>
        ///     Gets the current Amount of the good.
        /// </summary>
        [DisplayName("Current Amount")]
        [Description("Gets the current Amount of the good.")]
        [ReadOnly(true)]
        [Category("dynamic")]
        public int Amount { get; set; }

        /// <summary>
        ///     Gets the Maximum Capacity for this Good.
        /// </summary>
        [DisplayName("Maximum Capacity")]
        [Description("Gets the Maximum Capacity for this Good.")]
        [ReadOnly(true)]
        [Category("static")]
        public int Capacity { get; set; }

        /// <summary>
        ///     Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Capacity);
            stream.Write(Amount);
        }

        /// <summary>
        ///     Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Amount);
        }

        /// <summary>
        ///     Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Capacity = stream.ReadInt32();
            Amount = stream.ReadInt32();
        }

        /// <summary>
        ///     Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Amount = stream.ReadInt32();
        }
    }
}