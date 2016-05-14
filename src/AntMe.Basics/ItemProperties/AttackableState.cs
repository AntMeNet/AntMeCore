using System.ComponentModel;
using System.IO;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// State for all attackable Items.
    /// </summary>
    public sealed class AttackableState : ItemStateProperty
    {
        /// <summary>
        /// Current Health.
        /// </summary>
        [DisplayName("Health")]
        [Description("Current Health")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Health { get; set; }

        /// <summary>
        /// Maximum Health.
        /// </summary>
        [DisplayName("Maximum Health")]
        [Description("Maximum Health")]
        [ReadOnly(true)]
        [Category("Static")]
        public int MaximumHealth { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public AttackableState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public AttackableState(Item item, AttackableProperty property) : base(item, property)
        {
            MaximumHealth = property.AttackableMaximumHealth;
            Health = property.AttackableHealth;
            property.OnAttackableHealthChanged += (i, v) => { Health = v; };
            property.OnAttackableMaximumHealthChanged += (i, v) => { MaximumHealth = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumHealth);
            stream.Write(Health);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumHealth);
            stream.Write(Health);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            MaximumHealth = stream.ReadInt32();
            Health = stream.ReadInt32();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            MaximumHealth = stream.ReadInt32();
            Health = stream.ReadInt32();
        }
    }
}
