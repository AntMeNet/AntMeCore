using System;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für Spielelemente, die miteinander kollidieren können.
    /// </summary>
    public sealed class CollidableProperty : ItemProperty
    {
        private bool collisionFixed = true;
        private float collisionMass = 1f;
        private float collisionRadius = 1f;

        /// <summary>
        ///     Erstellt eine fixierte Masse mit angegebenem Radius.
        /// </summary>
        /// <param name="item">Radius</param>
        public CollidableProperty(Item item) : base(item)
        {
            CollisionRadius = item.Radius;
            item.RadiusChanged += (i, v) =>
            {
                CollisionRadius = v;
            };

            CollisionFixed = true;
            CollisionMass = 0f;
        }

        /// <summary>
        ///     Erstellt eine bewegliche Masse mit angegebenem Radius.
        /// </summary>
        /// <param name="radius">Radius</param>
        /// <param name="mass">Masse</param>
        public CollidableProperty(Item item, float mass) : this(item)
        {
            CollisionFixed = false;
            CollisionMass = mass;
        }

        /// <summary>
        ///     Gibt den Kollisionsradius des Elementes an. Das entspricht grob
        ///     dem Radius des Körpers.
        /// </summary>
        [DisplayName("Collision Radius")]
        [Description("")]
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
        ///     Gibt die Kollisionsmasse des Körpers zurück. Dies wird benötigt,
        ///     um die Kollisionsauflösung zu berechnet. Schwerere Elemente lassen
        ///     sich nur schwer von leichteren Elementen verschieben.
        /// </summary>
        [DisplayName("Collision Mass")]
        [Description("")]
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
        ///     GIbt an, ob das Element in der Welt fixiert ist. Dies trifft auf
        ///     Spielelemente zu, die nicht durch Kollisionen verschoben werden
        ///     können. Ist dieser Wert true, spielt die Masse keine Rolle mehr
        ///     bei der Berechnung.
        /// </summary>
        [DisplayName("Fixed Position")]
        [Description("")]
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

        #region Internal Calls

        /// <summary>
        ///     Wird von der Extension aufgerufen, wenn das Element mit einem anderen
        ///     Element kollidiert.
        /// </summary>
        /// <param name="item">Kollidierendes Element</param>
        internal void CollideItem(Item item)
        {
            if (OnCollision != null)
                OnCollision(Item, item);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Event, das bei Änderung des Kollisionsradius geworfen werden muss.
        /// </summary>
        public event ValueChanged<float> OnCollisionRadiusChanged;

        /// <summary>
        ///     Event, das bei Änderung der Kollisionsmasse geworfen werden muss.
        /// </summary>
        public event ValueChanged<float> OnCollisionMassChanged;

        /// <summary>
        ///     Event, das bei Änderung des Fixed-Flags geworfen werden muss.
        /// </summary>
        public event ValueChanged<bool> OnCollisionFixedChanged;

        /// <summary>
        ///     Event, das eine Kollision mit einem anderen Element signalisiert.
        /// </summary>
        public event ValueChanged<Item> OnCollision;

        #endregion
    }
}