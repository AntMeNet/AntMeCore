using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class SightingState : ItemStateProperty
    {
        /// <summary>
        ///     Liefert den Sichtradius des Elementes.
        /// </summary>
        [DisplayName("View Range")]
        [Description("")]
        public float ViewRange { get; set; }

        /// <summary>
        ///     Liefert die Sichtrichtung des Elements.
        /// </summary>
        [DisplayName("View Direction")]
        [Description("")]
        public short ViewDirection { get; set; }

        /// <summary>
        ///     Liefert den Öffnungswinkel des Sichtkegels.
        ///     0 = Element kann nichts sehen
        ///     90 = Sieht Elemente die sich zwischen -45 bis 45 Grad seiner Blickrichtung befinden
        ///     360 = Sieht alle Elemente innerhalb des Sichtradius
        /// </summary>
        [DisplayName("View Angle")]
        [Description("")]
        public float ViewAngle { get; set; }

        public SightingState() : base() { }

        public SightingState(Item item, SightingProperty property) : base(item, property)
        {
            ViewDirection = (short)property.ViewDirection.Degree;
            ViewAngle = property.ViewAngle;
            ViewRange = property.ViewRange;

            property.OnViewDirectionChanged += (i, v) => { ViewDirection = (short)v.Degree; };
            property.OnViewAngleChanged += (i, v) => { ViewAngle = v; };
            property.OnViewRangeChanged += (i, v) => { ViewRange = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(ViewRange);
            stream.Write(ViewAngle);
            stream.Write(ViewDirection);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(ViewRange);
            stream.Write(ViewAngle);
            stream.Write(ViewDirection);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            ViewRange = stream.ReadSingle();
            ViewAngle = stream.ReadSingle();
            ViewDirection = stream.ReadInt16();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            ViewRange = stream.ReadSingle();
            ViewAngle = stream.ReadSingle();
            ViewDirection = stream.ReadInt16();
        }
    }
}
