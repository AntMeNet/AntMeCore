namespace AntMe.Basics.ItemProperties
{
    public sealed class AttackerInfo : ItemInfoProperty
    {
        private readonly AttackerProperty property;

        public AttackerInfo(Item item, ItemProperty property, Item observer)
            : base(item, property, observer)
        {
            this.property = property as AttackerProperty;
        }

        /// <summary>
        ///     Attack Range.
        /// </summary>
        public float Range => property.AttackRange;

        /// <summary>
        ///     Recovery Time between Hits.
        /// </summary>
        public int RecoveryTime => property.AttackRecoveryTime;

        /// <summary>
        ///     Attacker Strength.
        /// </summary>
        public int Strength => property.AttackStrength;
    }
}