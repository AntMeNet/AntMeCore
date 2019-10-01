namespace AntMe.Basics.ItemProperties
{
    public sealed class CollidableInfo : ItemInfoProperty
    {
        private readonly CollidableProperty _property;

        public CollidableInfo(Item item, ItemProperty property) : base(item, property)
        {
            this._property = property as CollidableProperty;
        }

        /// <summary>
        ///     Is the Item a fixed Mass
        /// </summary>
        public bool Fixed => _property.CollisionFixed;

        /// <summary>
        ///     Item Mass
        /// </summary>
        public float Mass => _property.CollisionMass;
    }
}