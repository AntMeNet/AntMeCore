namespace AntMe.Basics.ItemProperties
{
    public sealed class CollidableInfo : ItemInfoProperty
    {
        private readonly CollidableProperty property;

        /// <summary>
        /// Is the Item a fixed Mass
        /// </summary>
        public bool Fixed { get { return property.CollisionFixed; } }

        /// <summary>
        /// Item Mass
        /// </summary>
        public float Mass { get { return property.CollisionMass; } }

        public CollidableInfo(Item item, ItemProperty property, Item observer) : base(item, property, observer)
        {
            this.property = property as CollidableProperty;
        }
    }
}
