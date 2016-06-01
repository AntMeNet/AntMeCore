using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Property for all portable Items.
    /// </summary>
    public sealed class PortableProperty : ItemProperty
    {
        private readonly HashSet<CarrierProperty> carrierItems = new HashSet<CarrierProperty>();
        private float portableRadius;
        private float portableWeight;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public PortableProperty(Item item) : base(item)
        {
            PortableRadius = item.Radius;
        }

        /// <summary>
        /// List of all carriing Items.
        /// </summary>
        public IQueryable<CarrierProperty> CarrierItems
        {
            get { return carrierItems.AsQueryable(); }
        }

        /// <summary>
        /// Gets or sets the Pickup Radius.
        /// </summary>
        public float PortableRadius
        {
            get { return portableRadius; }
            set
            {
                portableRadius = Math.Max(value, 0f);
                if (OnPortableRadiusChanged != null)
                    OnPortableRadiusChanged(Item, portableRadius);
            }
        }

        /// <summary>
        /// Gets or sets the Weight of this Item.
        /// </summary>
        public float PortableWeight
        {
            get { return portableWeight; }
            set
            {
                portableWeight = Math.Max(value, 0f);
                if (OnPortableWeightChanged != null)
                    OnPortableWeightChanged(Item, portableWeight);
            }
        }

        #region Internal Calls

        /// <summary>
        /// Internal Call to add another Carrier to the List.
        /// </summary>
        /// <param name="carrier">New Item</param>
        internal void AddCarrier(CarrierProperty carrier)
        {
            if (carrierItems.Contains(carrier))
                return;

            carrierItems.Add(carrier);
            if (OnNewCarrierItem != null)
                OnNewCarrierItem(carrier);
        }

        /// <summary>
        /// Internal Call to remove a Carrier.
        /// </summary>
        /// <param name="carrier">Lost Carrier</param>
        internal void RemoveCarrier(CarrierProperty carrier)
        {
            if (!carrierItems.Contains(carrier))
                return;

            carrierItems.Remove(carrier);
            if (OnLostCarrierItem != null)
                OnLostCarrierItem(carrier);
        }

        #endregion

        #region Events

        /// <summary>
        /// Signal about a changed Portable Radius.
        /// </summary>
        public event ValueChanged<float> OnPortableRadiusChanged;

        /// <summary>
        /// Signal for a changed Weight.
        /// </summary>
        public event ValueChanged<float> OnPortableWeightChanged;

        /// <summary>
        /// Signal for a new Carrier.
        /// </summary>
        public event ChangeItem<CarrierProperty> OnNewCarrierItem;

        /// <summary>
        /// Signal for a lost Carrier.
        /// </summary>
        public event ChangeItem<CarrierProperty> OnLostCarrierItem;

        #endregion
    }
}