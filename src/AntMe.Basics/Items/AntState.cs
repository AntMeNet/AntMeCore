using System.ComponentModel;
using System.IO;

namespace AntMe.Items.Basics
{
    public sealed class AntState : FactionItemState
    {
        public AntState() : base() { }

        public AntState(AntItem item) : base(item)
        {
        }

        /// <summary>
        /// Gibt den Modus der Ameise an.
        /// </summary>
        [DisplayName("Mode")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public AntStateMode Mode { get; set; }

        /// <summary>
        /// Gibt den Namen der Ameise an.
        /// </summary>
        [DisplayName("Name")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public string Name { get; set; }

        /// <summary>
        /// Gibt die Kaste dieser Ameise an.
        /// </summary>
        [DisplayName("Caste")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public string Caste { get; set; }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
            stream.Write(Caste);
            stream.Write(Name);
            stream.Write((byte)Mode);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
            stream.Write((byte)Mode);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);
            Caste = stream.ReadString();
            Name = stream.ReadString();
            Mode = (AntStateMode)stream.ReadByte();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
            Mode = (AntStateMode)stream.ReadByte();
        }
    }

    public enum AntStateMode
    {
        Idle = 0,
        Walk = 1,
        Fight = 2,
        Carry = 3
    }
}