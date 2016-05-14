using AntMe.Basics.ItemProperties;
using System;

namespace AntMe.Basics.Items
{
    /// <summary>
    /// Represents a Classic Bug (like in AntMe! 1.0)
    /// </summary>
    public class ClassicBugItem : Item
    {
        /// <summary>
        /// Default Radius of a Bug.
        /// </summary>
        public const float BugRadius = 4f;

        /// <summary>
        /// Default Rotation Speed of a Bug (Degrees per Round).
        /// </summary>
        public const int BugRotationSpeed = 10;

        private WalkingProperty walking;

        private int roundsToTurn = 0;

        private int roundsToWalk = 0;

        public ClassicBugItem(SimulationContext context, Vector2 position, Angle orientation)
            : base(context, position, BugRadius, orientation)
        {
            walking = GetProperty<WalkingProperty>();
            if (walking == null)
                throw new NotSupportedException("Classic Bug does not have a Walking Property");
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
                {
                    walking.Speed = walking.MaximumSpeed;
                }
                else
                {
                    walking.Speed = 0;
                }
            }
        }
    }
}
