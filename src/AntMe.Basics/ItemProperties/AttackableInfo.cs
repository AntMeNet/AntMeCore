namespace AntMe.Basics.ItemProperties
{
    public sealed class AttackableInfo : ItemInfoProperty
    {
        private readonly AttackableProperty property;

        public AttackableInfo(Item item, ItemProperty property, Item observer)
            : base(item, property, observer)
        {
            this.property = property as AttackableProperty;
        }

        public int Health => property.AttackableHealth;

        public int MaximumHealth => property.AttackableMaximumHealth;
    }
}