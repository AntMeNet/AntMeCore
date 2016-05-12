using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    /// State Property for all Attacker Items.
    /// </summary>
    public sealed class AttackerState : ItemStateProperty
    {
        /// <summary>
        /// Attack Range.
        /// </summary>
        [DisplayName("Attack Range")]
        [Description("Attack Range")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public float AttackRange { get; set; }

        /// <summary>
        /// Recovery Time between Hits.
        /// </summary>
        [DisplayName("Attack Recover Time")]
        [Description("Recovery Time between Hits")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int AttackRecoveryTime { get; set; }

        /// <summary>
        /// Attacker Strength.
        /// </summary>
        [DisplayName("Attack Strength")]
        [Description("Attacker Strength")]
        [ReadOnly(true)]
        [Category("Dynamic")]
        public int AttackStrength { get; set; }

        /// <summary>
        /// Default Constructor for the Deserializer.
        /// </summary>
        public AttackerState() : base() { }

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="item">Related Engine Item</param>
        /// <param name="property">Related Engine Property</param>
        public AttackerState(Item item, AttackerProperty property) : base(item, property)
        {
            AttackRange = property.AttackRange;
            AttackRecoveryTime = property.AttackRecoveryTime;
            AttackStrength = property.AttackStrength;

            property.OnAttackRangeChanged += (i, v) => { AttackRange = v; };
            property.OnAttackRecoveryTimeChanged += (i, v) => { AttackRecoveryTime = v; };
            property.OnAttackStrengthChanged += (i, v) => { AttackStrength = v; };
        }

        /// <summary>
        /// Serializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(AttackStrength);
            stream.Write(AttackRecoveryTime);
            stream.Write(AttackRange);
        }

        /// <summary>
        /// Serializes following Frames of this State.
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(AttackStrength);
            stream.Write(AttackRecoveryTime);
            stream.Write(AttackRange);
        }

        /// <summary>
        /// Deserializes the first Frame of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            AttackStrength = stream.ReadInt32();
            AttackRecoveryTime = stream.ReadInt32();
            AttackRange = stream.ReadSingle();
        }

        /// <summary>
        /// Deserializes all following Frames of this State.
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            AttackStrength = stream.ReadInt32();
            AttackRecoveryTime = stream.ReadInt32();
            AttackRange = stream.ReadSingle();
        }
    }
}
