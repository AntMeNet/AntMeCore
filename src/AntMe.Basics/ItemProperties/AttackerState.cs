using AntMe.ItemProperties.Basics;
using System.ComponentModel;
using System.IO;

namespace AntMe.ItemProperties.Basics
{
    public sealed class AttackerState : ItemStateProperty
    {
        /// <summary>
        /// Liefert den Angriffsradius des Spielelements oder legt diesen fest.
        /// </summary>
        [DisplayName("Attack Range")]
        [Description("")]
        public float AttackRange { get; set; }

        /// <summary>
        /// Liefert die Wartezeit zwischen zwei Angriffen oder legt diese fest.
        /// </summary>
        [DisplayName("Attack Recover Time")]
        [Description("")]
        public int AttackRecoveryTime { get; set; }

        /// <summary>
        /// Liefert die Anzahl Lebenspunkte, die die Einheit bei einem Angriff abziehen kann.
        /// </summary>
        [DisplayName("Attack Strength")]
        [Description("")]
        public int AttackStrength { get; set; }

        public AttackerState() : base() { }

        public AttackerState(Item item, AttackerProperty property) : base(item, property)
        {
            AttackRange = property.AttackRange;
            AttackRecoveryTime = property.AttackRecoveryTime;
            AttackStrength = property.AttackStrength;

            property.OnAttackRangeChanged += (i, v) => { AttackRange = v; };
            property.OnAttackRecoveryTimeChanged += (i, v) => { AttackRecoveryTime = v; };
            property.OnAttackStrengthChanged += (i, v) => { AttackStrength = v; };
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            stream.Write(AttackStrength);
            stream.Write(AttackRecoveryTime);
            stream.Write(AttackRange);
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            stream.Write(AttackStrength);
            stream.Write(AttackRecoveryTime);
            stream.Write(AttackRange);
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            AttackStrength = stream.ReadInt32();
            AttackRecoveryTime = stream.ReadInt32();
            AttackRange = stream.ReadSingle();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            AttackStrength = stream.ReadInt32();
            AttackRecoveryTime = stream.ReadInt32();
            AttackRange = stream.ReadSingle();
        }
    }
}
