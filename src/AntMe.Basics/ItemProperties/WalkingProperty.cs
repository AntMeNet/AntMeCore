using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all walking Elements.
    /// </summary>
    public sealed class WalkingProperty : ItemProperty
    {
        private Angle direction;
        private float maxSpeed;
        private float speed;

        /// <summary>
        /// Creates a new Instance of a Walking Property.
        /// </summary>
        /// <param name="item">Reference to the related Item</param>
        public WalkingProperty(Item item) : base(item)
        {
            Direction = Angle.Right;
            Speed = 0f;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Walking Direction.
        /// </summary>
        public Angle Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                if (OnMoveDirectionChanged != null)
                    OnMoveDirectionChanged(Item, direction);
            }
        }

        /// <summary>
        /// Gets or sets the current Walking Speed limited by the current Max Speed.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set
            {
                speed = Math.Max(value, 0f);
                if (OnMoveSpeedChanged != null)
                    OnMoveSpeedChanged(Item, speed);
            }
        }

        /// <summary>
        /// Gets or sets the maximum Speed.
        /// </summary>
        public float MaximumSpeed
        {
            get { return maxSpeed; }
            set
            {
                maxSpeed = Math.Max(value, 0f);
                if (OnMaximumMoveSpeedChanged != null)
                    OnMaximumMoveSpeedChanged(Item, maxSpeed);
            }
        }


        /// <summary>
        /// Sets the current Speed Multiplier.
        /// </summary>
        public float MoveMalus { get; set; }

        #endregion

        #region Internal Calls

        /// <summary>
        /// Internal Call for hitting a Wall.
        /// </summary>
        /// <param name="direction">Direction</param>
        internal void HitWall(Compass direction)
        {
            if (OnHitWall != null)
                OnHitWall(Item, direction);
        }

        /// <summary>
        /// Internal Call for hitting the Map Border.
        /// </summary>
        /// <param name="direction">Direction</param>
        internal void HitBorder(Compass direction)
        {
            if (OnHitBorder != null)
                OnHitBorder(Item, direction);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event for a Direction Change.
        /// </summary>
        public event ValueChanged<Angle> OnMoveDirectionChanged;

        /// <summary>
        /// Event for Speed Change.
        /// </summary>
        public event ValueChanged<float> OnMoveSpeedChanged;

        /// <summary>
        /// Event for Maximum Speed Change.
        /// </summary>
        public event ValueChanged<float> OnMaximumMoveSpeedChanged;

        /// <summary>
        /// Event for hitting the Map Border.
        /// </summary>
        public event ValueChanged<Compass> OnHitBorder;

        /// <summary>
        /// Event for hitting a Wall.
        /// </summary>
        public event ValueChanged<Compass> OnHitWall;

        #endregion
    }
}