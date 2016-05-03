using System;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Property für Items, die in der Lage sein sollen, tragbare Elemente
    ///     aufzunehmen und abzutransportieren.
    /// </summary>
    public sealed class CarrierProperty : ItemProperty
    {
        private PortableProperty carrierLoad;
        private float carrierStrength;

        public CarrierProperty(Item item) : base(item)
        {
            // CarrierStrength = strength;
        }

        /// <summary>
        ///     Gibt die Stärke der Einheit zurück oder legt diese fest. Bestimmt,
        ///     wie viel Masse die Einheit ohne Geschwindigkeitsverlust aufnehmen
        ///     kann.
        /// </summary>
        [DisplayName("Carrier Strength")]
        [Description("")]
        public float CarrierStrength
        {
            get { return carrierStrength; }
            set
            {
                carrierStrength = Math.Max(value, 0f);
                if (OnCarrierStrengthChanged != null)
                    OnCarrierStrengthChanged(Item, carrierStrength);
            }
        }

        /// <summary>
        ///     Referenz auf die aktuell getragene Last.
        /// </summary>
        [DisplayName("Load")]
        [Description("")]
        public PortableProperty CarrierLoad
        {
            get { return carrierLoad; }
            private set
            {
                carrierLoad = value;
                if (OnCarrierLoadChanged != null)
                    OnCarrierLoadChanged(Item, value);
            }
        }

        /// <summary>
        ///     Nimmt das angegebene Element auf.
        /// </summary>
        /// <param name="item">Aufzunehmendes Objekt</param>
        public bool Carry(PortableProperty item)
        {
            // Carry null wird als Drop interpretiert.
            if (item == null)
            {
                Drop();
                return true;
            }

            // Verarbeitung eines aktuell getragenen Objektes
            if (CarrierLoad != null)
            {
                // Falls bereits aufgeladen hier abbrechen
                if (CarrierLoad == item)
                    return true;

                // Altes Objekt fallen lassen
                Drop();
            }

            // Prüfen, ob Carrier Teil der Simulation ist
            if (Item.Engine == null)
                throw new NotSupportedException("Carrier is not Part of the Simulation");

            // Prüfen, ob Portable Teil der Simulation ist
            if (item.Item.Engine == null || item.Item.Engine != Item.Engine)
                throw new NotSupportedException("Portable is not Part of the same Simulation");

            // Prüfen, ob Träger und Getragener das selbe element ist.
            if (item.Item == Item)
                throw new NotSupportedException("Carrier can not carry itself");

            // Prüfen, ob das neue Objekt nah genug ist
            if (item.PortableRadius < Item.GetDistance(Item, item.Item))
                return false;

            // Item aufnehmen
            CarrierLoad = item;
            item.AddCarrier(this);
            return true;
        }

        /// <summary>
        ///     Lässt das aktuell getragene Objekt fallen.
        /// </summary>
        public void Drop()
        {
            if (carrierLoad != null)
            {
                carrierLoad.RemoveCarrier(this);
                CarrierLoad = null;
            }
        }

        /// <summary>
        ///     Event, das über die Änderung der Stärke informiert.
        /// </summary>
        public event ValueChanged<float> OnCarrierStrengthChanged;

        /// <summary>
        ///     Event, das über die Änderung des getragenen Objektes informiert.
        /// </summary>
        public event ValueChanged<PortableProperty> OnCarrierLoadChanged;
    }
}