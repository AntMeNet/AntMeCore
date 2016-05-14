using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Base State for all Collectable Good Properties.
    /// </summary>
    public abstract class CollectableState : GoodsState
    {
        /// <summary>
        /// Gets the Collectable Radius.
        /// </summary>
        [DisplayName("Collectable Radius")]
        [Description("The radius inside which a collector can collect this collectable.")]
        [ReadOnly(true)]
        [Category("static")]
        public float CollectableRadius { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public CollectableState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public CollectableState(Item item, CollectableProperty property) : base(item, property)
        {
            // Bind Radius
            CollectableRadius = property.CollectableRadius;
            property.OnCollectableRadiusChanged += (i,v) => { CollectableRadius = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            base.SerializeFirst(stream, version);
            stream.Write(CollectableRadius);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            base.SerializeUpdate(stream, version);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            base.DeserializeFirst(stream, version);
            CollectableRadius = stream.ReadSingle();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            base.DeserializeUpdate(stream, version);
        }
    }
}
