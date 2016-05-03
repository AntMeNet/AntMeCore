using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class AttackableState : ItemStateProperty
    {
        /// <summary>
        /// Gibt die Hitpoints dieses Ameisenhügels zurück.
        /// </summary>
        [DisplayName("Health")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int Health { get; set; }

        /// <summary>
        /// Gibt die maximale Menge an Hitpoints zurück.
        /// </summary>
        [DisplayName("Maximum Health")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public int MaximumHealth { get; set; }

        public AttackableState() : base() { }

        public AttackableState(Item item, AttackableProperty property) : base(item, property)
        {
            MaximumHealth = property.AttackableMaximumHealth;
            Health = property.AttackableHealth;
            property.OnAttackableHealthChanged += (i, v) => { Health = v; };
            property.OnAttackableMaximumHealthChanged += (i, v) => { MaximumHealth = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumHealth);
            stream.Write(Health);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(MaximumHealth);
            stream.Write(Health);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            MaximumHealth = stream.ReadInt32();
            Health = stream.ReadInt32();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            MaximumHealth = stream.ReadInt32();
            Health = stream.ReadInt32();
        }
    }
}
