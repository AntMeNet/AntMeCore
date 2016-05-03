using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public class DebugCarrierItem : DebugCollisionItem
    {
        private CarrierProperty carrier;

        public DebugCarrierItem(ITypeResolver resolver, Vector2 pos)
            : base(resolver, pos)
        {
            carrier = new CarrierProperty(this, 10f);

            AddProperty(carrier);
        }

        public bool Carry(Item item)
        {
            if (item == null)
                return false;

            if (!item.ContainsProperty<PortableProperty>())
                return false;

            PortableProperty portable = item.GetProperty<PortableProperty>();
            return carrier.Carry(portable);
        }

        public void Drop()
        {
            carrier.Drop();
        }

        public float CarrierStrength
        {
            get { return carrier.CarrierStrength; }
            set { carrier.CarrierStrength = value; }
        }
    }
}
