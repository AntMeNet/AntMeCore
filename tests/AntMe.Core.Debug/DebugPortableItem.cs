using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugPortableItem : DebugCollisionItem
    {
        private PortableProperty portable;

        public DebugPortableItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos)
        {
            portable = new PortableProperty(this, 10, 10);

            AddProperty(portable);
        }

        public float PortableMass
        {
            get { return portable.PortableWeight; }
            set { portable.PortableWeight = value; }
        }

        public float PortableRadius
        {
            get { return portable.PortableRadius; }
            set { portable.PortableRadius = value; }
        }
    }
}
