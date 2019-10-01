namespace AntMe.Basics.ItemProperties
{
    public sealed class AttackerInfo : ItemInfoProperty
    {
        private readonly AttackerProperty _property;

        public AttackerInfo(Item item, ItemProperty property)
            : base(item, property)
        {
            this._property = property as AttackerProperty;
        }

        /// <summary>
        ///     Attack Range.
        /// </summary>
        public float Range => _property.AttackRange;

        /// <summary>
        ///     Recovery Time between Hits.
        /// </summary>
        public int RecoveryTime => _property.AttackRecoveryTime;

        /// <summary>
        ///     Attacker Strength.
        /// </summary>
        public int Strength => _property.AttackStrength;
    }
}