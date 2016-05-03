using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugCollectorItem : Item
    {
        private CollectorProperty collector;

        public DebugCollectorItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            collector = new CollectorProperty(this, 20);
            AddProperty(collector);
        }
    }
}
