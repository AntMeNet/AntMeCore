using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State Property for all collidable Items.
    /// </summary>
    public sealed class CollidableState : ItemStateProperty
    {
        /// <summary>
        /// Is the Item a fixed Mass
        /// </summary>
        [DisplayName("Fixed Mass")]
        [Description("Is the Item a fixed Mass")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public bool Fixed { get; set; }

        /// <summary>
        /// Item Mass
        /// </summary>
        [DisplayName("Mass")]
        [Description("Item Mass")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float Mass { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public CollidableState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public CollidableState(Item item, CollidableProperty property) : base(item, property)
        {
            // Bind Fixed to Item Fixed
            Fixed = property.CollisionFixed;
            property.OnCollisionFixedChanged += (i, v) => { Fixed = v; };

            // Bind Mass to Item Mass
            Mass = property.CollisionMass;
            property.OnCollisionMassChanged += (i, v) => { Mass = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(Fixed);
            stream.Write(Mass);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(Fixed);
            stream.Write(Mass);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            Fixed = stream.ReadBoolean();
            Mass = stream.ReadSingle();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            Fixed = stream.ReadBoolean();
            Mass = stream.ReadSingle();
        }
    }
}
