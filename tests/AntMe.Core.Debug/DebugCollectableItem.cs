using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugCollectableItem : Item
    {
        private CollectableProperty collectable;

        public DebugCollectableItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            collectable = new CollectableProperty(this);

            AddProperty(collectable);
        }
    }
}
