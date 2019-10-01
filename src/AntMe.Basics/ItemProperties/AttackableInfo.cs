namespace AntMe.Basics.ItemProperties
{
    public sealed class AttackableInfo : ItemInfoProperty
    {
        private readonly AttackableProperty _property;

        public AttackableInfo(Item item, ItemProperty property)
            : base(item, property)
        {
            _property = property as AttackableProperty;
        }

        public int Health => _property.AttackableHealth;

        public int MaximumHealth => _property.AttackableMaximumHealth;
    }
}