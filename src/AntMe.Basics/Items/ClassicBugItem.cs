using System;
using AntMe.Basics.ItemProperties;

namespace AntMe.Basics.Items
{
    /// <summary>
    ///     Represents a Classic Bug (like in AntMe! 1.0)
    /// </summary>
    public class ClassicBugItem : Item
    {
        /// <summary>
        ///     Default Radius of a Bug.
        /// </summary>
        public const float BugRadius = 4f;

        /// <summary>
        ///     Default Rotation Speed of a Bug (Degrees per Round).
        /// </summary>
        public const int BugRotationSpeed = 10;

        private readonly CollidableProperty collidable;

        private int roundsToTurn;

        private int roundsToWalk;

        private readonly WalkingProperty walking;

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="position">Position for the new Bug</param>
        /// <param name="orientation">Start Orientation</param>
        public ClassicBugItem(SimulationContext context, Vector2 position, Angle orientation)
            : base(context, position, BugRadius, orientation)
        {
            walking = GetProperty<WalkingProperty>();
            if (walking == null)
                throw new NotSupportedException("Classic Bug does not have a Walking Property");

            collidable = GetProperty<CollidableProperty>();
            if (collidable == null)
                throw new NotSupportedException("Classic Bug does not have a Collidable Property");

            walking.OnHitBorder += OnHitBorder;
            walking.OnHitWall += OnHitBorder;
        }

        private void OnHitBorder(Item item, Compass direction)
        {
            if (direction == Compass.North || direction == Compass.South)
                Orientation = Orientation.InvertY();
            else
                Orientation = Orientation.InvertX();
        }

        protected override void OnUpdate()
        {
            if (roundsToTurn > 0)
            {
                Orientation += Angle.FromRadian(0.1f);
                roundsToTurn--;
            }
            else if (roundsToTurn < 0)
            {
                Orientation -= Angle.FromRadian(0.1f);
                roundsToTurn++;
            }
            else if (roundsToWalk > 0)
            {
                roundsToWalk--;
            }
            else
            {
                roundsToTurn = Random.Next(-10, 10);
                roundsToWalk = Random.Next(100, 300);
            }

            if (walking != null)
            {
                if (roundsToTurn == 0 && roundsToWalk > 0)
                    walking.Speed = walking.MaximumSpeed;
                else
                    walking.Speed = 0;
            }
        }
    }
}