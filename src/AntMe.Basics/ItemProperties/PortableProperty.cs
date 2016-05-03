using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für Items, die in der Lage sein sollen, von Trägern weggetragen
    ///     zu werden. Das trifft beispielsweise für Äpfel zu.
    /// </summary>
    public sealed class PortableProperty : ItemProperty
    {
        private readonly List<CarrierProperty> carrierItems = new List<CarrierProperty>();
        private float portableRadius;
        private float portableWeight;

        public PortableProperty(Item item, float radius, float weight) : base(item)
        {
            PortableRadius = radius;
            PortableWeight = weight;
        }

        /// <summary>
        ///     Liste der tragenden Elemente
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<CarrierProperty> CarrierItems
        {
            get { return carrierItems.AsReadOnly(); }
        }

        /// <summary>
        ///     Gibt den Radius des Items zurück, in dem das Objekt aufgenommen
        ///     werden kann oder legt diesen fest.
        /// </summary>
        [DisplayName("Pickup Radius")]
        [Description("")]
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
        ///     Gibt die Gesamtmasse des Elementes zurück oder legt diese fest.
        /// </summary>
        [DisplayName("Weight")]
        [Description("")]
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

        #region Events

        /// <summary>
        ///     Event, das die Änderung des Trageradius signalisiert.
        /// </summary>
        public event ValueChanged<float> OnPortableRadiusChanged;

        /// <summary>
        ///     Event, das die Änderung der Masse signalisiert.
        /// </summary>
        public event ValueChanged<float> OnPortableWeightChanged;

        /// <summary>
        ///     Wird geworfen, wenn das Objekt von einem weiteren Träger aufgenommen
        ///     wurde.
        /// </summary>
        public event ChangeItem<CarrierProperty> OnNewCarrierItem;

        /// <summary>
        ///     Wird geworfen, wenn ein Träger das Objekt fallen gelassen hat.
        /// </summary>
        public event ChangeItem<CarrierProperty> OnLostCarrierItem;

        #endregion

        /// <summary>
        ///     Wird von der Extension aufgerufen, um einen Träger in die Liste der
        ///     tragenden Einheiten einzufügen.
        /// </summary>
        /// <param name="carrier">Neuer Träger</param>
        internal void AddCarrier(CarrierProperty carrier)
        {
            if (carrierItems.Contains(carrier))
                return;

            carrierItems.Add(carrier);
            if (OnNewCarrierItem != null)
                OnNewCarrierItem(carrier);
        }

        /// <summary>
        ///     Wird von der Extension aufgerufen, um einen Träger aus der Liste
        ///     der tragenden Einheiten zu entfernen.
        /// </summary>
        /// <param name="carrier">Träger</param>
        internal void RemoveCarrier(CarrierProperty carrier)
        {
            if (!carrierItems.Contains(carrier))
                return;

            carrierItems.Remove(carrier);
            if (OnLostCarrierItem != null)
                OnLostCarrierItem(carrier);
        }
    }
}