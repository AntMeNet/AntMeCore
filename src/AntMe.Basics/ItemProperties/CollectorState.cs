using AntMe.ItemProperties.Basics;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    public sealed class CollectorState : ItemStateProperty
    {
        /// <summary>
        /// Liefert den Sammelradius des Spielelements oder legt diesen fest.
        /// </summary>
        [DisplayName("Collector Range")]
        [Description("")]
        public float CollectorRange { get; set; }

        public CollectorState() : base() { }

        public CollectorState(Item item, CollectorProperty property) : base(item, property)
        {
            CollectorRange = property.CollectorRange;
            property.OnCollectorRangeChanged += (i, v) => { CollectorRange = v; };
        }

        public override void SerializeFirst(System.IO.BinaryWriter stream, byte version)
        {
            stream.Write(CollectorRange);
        }

        public override void SerializeUpdate(System.IO.BinaryWriter stream, byte version)
        {
            stream.Write(CollectorRange);
        }

        public override void DeserializeFirst(System.IO.BinaryReader stream, byte version)
        {
            CollectorRange = stream.ReadSingle();
        }

        public override void DeserializeUpdate(System.IO.BinaryReader stream, byte version)
        {
            CollectorRange = stream.ReadSingle();
        }
    }
}
