using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugSmellableItem : Item
    {
        private SmellableProperty smellable;

        public DebugSmellableItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            smellable = new SmellableProperty(this, 10);

            AddProperty(smellable);
        }
    }
}
