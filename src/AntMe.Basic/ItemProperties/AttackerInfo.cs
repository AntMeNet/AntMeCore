namespace AntMe.Basics.ItemProperties
{
    public sealed class AttackerInfo : ItemInfoProperty
    {
        private readonly AttackerProperty property;

        /// <summary>
        /// Attack Range.
        /// </summary>
        public float Range { get { return property.AttackRange; } }

        /// <summary>
        /// Recovery Time between Hits.
        /// </summary>
        public int RecoveryTime { get { return property.AttackRecoveryTime; } }

        /// <summary>
        /// Attacker Strength.
        /// </summary>
        public int Strength { get { return property.AttackStrength; } }

        public AttackerInfo(Item item, ItemProperty property, Item observer)
            : base(item, property, observer)
        {
            this.property = property as AttackerProperty;
        }
    }
}
