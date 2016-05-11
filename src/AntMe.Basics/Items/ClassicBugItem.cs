using AntMe.ItemProperties.Basics;
using System;

namespace AntMe.Items.Basics
{
    public class ClassicBugItem : Item
    {
        public const float BugRadius = 4f;

        public const int BugRotationSpeed = 10;

        public const float BugWalkingSpeed = 1f;

        private WalkingProperty walking;

        private int roundsToTurn = 0;

        private int roundsToWalk = 0;

        public ClassicBugItem(SimulationContext context, Vector2 position, Angle orientation)
            : base(context, position, BugRadius, orientation)
        {
            walking = GetProperty<WalkingProperty>();
            if (walking != null)
                walking.MaximumSpeed = BugWalkingSpeed;
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
                    walking.Speed = BugWalkingSpeed;
                }
                else
                {
                    walking.Speed = 0;
                }
            }
        }
    }
}
