namespace AntMe.Basics.ItemProperties
{
    public sealed class CollidableInfo : ItemInfoProperty
    {
        private readonly CollidableProperty property;

        public CollidableInfo(Item item, ItemProperty property, Item observer) : base(item, property, observer)
        {
            this.property = property as CollidableProperty;
        }

        /// <summary>
        ///     Is the Item a fixed Mass
        /// </summary>
        public bool Fixed => property.CollisionFixed;

        /// <summary>
        ///     Item Mass
        /// </summary>
        public float Mass => property.CollisionMass;
    }
}