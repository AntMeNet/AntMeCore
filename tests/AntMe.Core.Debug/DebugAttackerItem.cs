using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugAttackerItem : Item
    {
        private AttackerProperty attacker;

        public DebugAttackerItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            attacker = new AttackerProperty(this, 5, 10, 1);

            AddProperty(attacker);
        }
    }
}
