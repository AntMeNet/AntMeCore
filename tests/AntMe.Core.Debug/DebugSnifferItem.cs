using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugSnifferItem : Item
    {
        private SnifferProperty sniffer;

        public DebugSnifferItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos, Angle.Right)
        {
            sniffer = new SnifferProperty(this)
            {
            };

            AddProperty(sniffer);
        }
    }
}
