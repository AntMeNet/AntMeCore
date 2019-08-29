namespace AntMe.Basics.ItemProperties
{
    public sealed class AttackableInfo : ItemInfoProperty
    {
        private readonly AttackableProperty property;

        public int Health { get { return property.AttackableHealth; } }

        public int MaximumHealth { get { return property.AttackableMaximumHealth; } }

        public AttackableInfo(Item item, ItemProperty property, Item observer)
            : base(item, property, observer)
        {
            this.property = property as AttackableProperty;
        }
    }
}
