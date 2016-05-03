using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public class CollectableGoodState : ItemStateProperty
    {
        /// <summary>
        /// Gibt die Kapazität für dieses Gut an oder legt diese fest.
        /// </summary>
        [DisplayName("Capacity")]
        [Description("")]
        public int Capacity { get; set; }

        /// <summary>
        /// Gibt die aktuelle Ladung dieses Guts an oder legt diese fest.
        /// </summary>
        [DisplayName("Amount")]
        [Description("")]
        public int Amount { get; set; }

        public CollectableGoodState() : base() { }

        public CollectableGoodState(Item item, CollectableGoodProperty property) : base(item, property)
        {
            Amount = property.Amount;
            Capacity = property.Capacity;
            property.OnAmountChanged += (i, v) => { Amount = v; };
            property.OnCapacityChanged += (i, v) => { Capacity = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Capacity);
            stream.Write(Amount);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Capacity);
            stream.Write(Amount);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Capacity = stream.ReadInt32();
            Amount = stream.ReadInt32();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Capacity = stream.ReadInt32();
            Amount = stream.ReadInt32();
        }
    }
}
