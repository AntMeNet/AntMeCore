using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class CollectableState : ItemStateProperty
    {
        /// <summary>
        ///     Gibt den Radius an, innerhalb dessen gesammelt werden kann.
        /// </summary>
        [DisplayName("Collectable radius")]
        [Description("The radius inside which a collector can collect this collectable.")]
        public float CollectableRadius { get; set; }

        public CollectableState() : base() { }

        public CollectableState(CollectableProperty property) : base(property)
        {
            CollectableRadius = property.CollectableRadius;
            property.OnCollectableRadiusChanged += (i,v) => { CollectableRadius = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(CollectableRadius);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(CollectableRadius);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            CollectableRadius = stream.ReadSingle();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            CollectableRadius = stream.ReadSingle();
        }
    }
}
