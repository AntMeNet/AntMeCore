using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugVisibleItem : Item
    {
        private VisibleProperty visible;

        public DebugVisibleItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            visible = new VisibleProperty(this);

            AddProperty(visible);
        }
    }
}
