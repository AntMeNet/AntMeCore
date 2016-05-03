using System.ComponentModel;
using System.IO;

namespace AntMe.Items.Basics
{
    public sealed class MarkerState : FactionItemState
    {
        public MarkerState() : base() { }

        public MarkerState(MarkerItem item) : base(item)
        {
            // keine StateProperties
        }

        /// <summary>
        /// Gibt die Information dieser Markierung zurück.
        /// </summary>
        [DisplayName("Information")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public int Information { get; set; }
        
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
            stream.Write(Information);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);
            Information = stream.ReadInt32();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
        }

        public override string ToString()
        {
            return "Marker (" + Information.ToString() + ")";
        }
    }
}