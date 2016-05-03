using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugAttackableItem : Item
    {
        private AttackableProperty attackable;

        public DebugAttackableItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            attackable = new AttackableProperty(this, 5, 100, 100);

            AddProperty(attackable);
        }
    }
}
