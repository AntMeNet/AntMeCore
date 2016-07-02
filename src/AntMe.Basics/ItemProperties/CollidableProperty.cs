using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for collidable Items.
    /// </summary>
    public sealed class CollidableProperty : ItemProperty
    {
        private bool collisionFixed = true;
        private float collisionMass = 1f;
        private float collisionRadius = 1f;

        /// <summary>
        /// Default Constructor with a fixed Body.
        /// </summary>
        /// <param name="item">Reference to the Item</param>
        public CollidableProperty(Item item) : base(item)
        {
            CollisionRadius = item.Radius;
            CollisionFixed = true;
            CollisionMass = 0f;
        }

        /// <summary>
        /// Constructor with a movable Body.
        /// </summary>
        /// <param name="item">Reference to the Item</param>
        /// <param name="mass">Mass of this Body</param>
        public CollidableProperty(Item item, float mass) : this(item)
        {
            CollisionFixed = false;
            CollisionMass = mass;
        }

        /// <summary>
        /// Gets or sets the collision Radius.
        /// </summary>
        public float CollisionRadius
        {
            get { return collisionRadius; }
            set
            {
                collisionRadius = Math.Max(value, 0f);
                if (OnCollisionRadiusChanged != null)
                    OnCollisionRadiusChanged(Item, collisionRadius);
            }
        }

        /// <summary>
        /// Gets or sets the collision Mass.
        /// </summary>
        public float CollisionMass
        {
            get { return collisionMass; }
            set
            {
                collisionMass = Math.Max(value, 0f);
                if (OnCollisionMassChanged != null)
                    OnCollisionMassChanged(Item, collisionMass);
            }
        }

        /// <summary>
        /// Gets or sets if the Body is fixed or movable.
        /// </summary>
        public bool CollisionFixed
        {
            get { return collisionFixed; }
            set
            {
                collisionFixed = value;
                if (OnCollisionFixedChanged != null)
                    OnCollisionFixedChanged(Item, value);
            }
        }

        public void test()
        { }

        #region Internal Calls

        /// <summary>
        /// Gets a call after a Collision with the given Item.
        /// </summary>
        /// <param name="item">Colliding Item</param>
        internal void CollideItem(Item item)
        {
            if (OnCollision != null)
                OnCollision(Item, item);
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a changed collision Radius.
        /// </summary>
        public event ValueChanged<float> OnCollisionRadiusChanged;

        /// <summary>
        /// Signal for a changed collision Mass.
        /// </summary>
        public event ValueChanged<float> OnCollisionMassChanged;

        /// <summary>
        /// Signal for a changed Fixed Body Flag.
        /// </summary>
        public event ValueChanged<bool> OnCollisionFixedChanged;

        /// <summary>
        /// Signal on Collision with another Item.
        /// </summary>
        public event ValueChanged<Item> OnCollision;

        #endregion
    }
}