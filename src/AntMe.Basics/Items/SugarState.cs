using System.ComponentModel;
using System.IO;

namespace AntMe.Items.Basics
{
    public sealed class SugarState : ItemState
    {
        public SugarState() : base() { }

        public SugarState(SugarItem item) : base(item)
        {
        }

        /// <summary>
        /// Gibt die Menge an Zucker an oder legt diese fest.
        /// </summary>
        [DisplayName("Amount")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Amount { get; set; }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
            stream.Write(Amount);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
            stream.Write(Amount);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);
            Amount = stream.ReadInt32();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
            Amount = stream.ReadInt32();
        }

        public override string ToString()
        {
            return "Sugar (" + Id + ")";
        }
    }
}