using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    ///     Property for Carrier Items.
    /// </summary>
    public sealed class CarrierProperty : ItemProperty
    {
        private PortableProperty carrierLoad;
        private float carrierStrength;

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public CarrierProperty(Item item) : base(item)
        {
        }

        /// <summary>
        ///     Gets or sets the Strength of this Item.
        /// </summary>
        public float CarrierStrength
        {
            get => carrierStrength;
            set
            {
                carrierStrength = Math.Max(value, 0f);
                OnCarrierStrengthChanged?.Invoke(Item, carrierStrength);
            }
        }

        /// <summary>
        ///     Gets the current Load of this Ant.
        /// </summary>
        public PortableProperty CarrierLoad
        {
            get => carrierLoad;
            private set
            {
                carrierLoad = value;
                OnCarrierLoadChanged?.Invoke(Item, value);
            }
        }

        /// <summary>
        ///     Pick up a new Item.
        /// </summary>
        /// <param name="item">Portable Item</param>
        public bool Carry(PortableProperty item)
        {
            // item == null means a Drop
            if (item == null)
            {
                Drop();
                return true;
            }

            // Handle the old Load
            if (CarrierLoad != null)
            {
                // Ignore if old load the same as the new load
                if (CarrierLoad == item)
                    return true;

                // Drop the old load
                Drop();
            }

            // Check if the new load is part of the simulation
            if (Item.Engine == null)
                throw new NotSupportedException("Carrier is not Part of the Simulation");
            if (item.Item.Engine == null || item.Item.Engine != Item.Engine)
                throw new NotSupportedException("Portable is not Part of the same Simulation");

            // Check if the load is not the carrier
            if (item.Item == Item)
                throw new NotSupportedException("Carrier can not carry itself");

            // TODO: Check for circular references (Carrier/Portable <-> Carrier/Portable)

            // Check if load is close enought
            if (item.PortableRadius < Item.GetDistance(Item, item.Item))
                return false;

            // Pick up Item
            CarrierLoad = item;
            item.AddCarrier(this);
            return true;
        }

        /// <summary>
        ///     Drops the current Load.
        /// </summary>
        public void Drop()
        {
            carrierLoad?.RemoveCarrier(this);
            CarrierLoad = null;
        }

        /// <summary>
        ///     Signal for a changed Strength.
        /// </summary>
        public event ValueChanged<float> OnCarrierStrengthChanged;

        /// <summary>
        ///     Signal for a changed Load.
        /// </summary>
        public event ValueChanged<PortableProperty> OnCarrierLoadChanged;
    }
}