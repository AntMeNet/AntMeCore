using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugWalkingItem : Item
    {
        private WalkingProperty moving;

        public DebugWalkingItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            moving = new WalkingProperty(this, 5);

            AddProperty(moving);
        }

        public Angle MoveDirection
        {
            get { return moving.Direction; }
            set { moving.Direction = value; }
        }

        public float MoveSpeed
        {
            get { return moving.Speed; }
            set { moving.Speed = value; }
        }
    }
}
