using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugCollisionItem : DebugWalkingItem
    {
        private CollidableProperty collidable;

        public DebugCollisionItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos)
        {
            collidable = new CollidableProperty(this, 1);

            AddProperty(collidable);
        }

        public bool CollisionFixed
        {
            get { return collidable.CollisionFixed; }
            set { collidable.CollisionFixed = value; }
        }

        public float CollisionMass
        {
            get { return collidable.CollisionMass; }
            set { collidable.CollisionMass = value; }
        }

        public float CollisionRadius
        {
            get { return collidable.CollisionRadius; }
            set { collidable.CollisionRadius = value; }
        }
    }
}
