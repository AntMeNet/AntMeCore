using AntMe.ItemProperties.Basics;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    public sealed class CarrierState : ItemStateProperty
    {
        /// <summary>
        /// Gibt die Stärke der Einheit zurück oder legt diese fest. Bestimmt, 
        /// wie viel Masse die Einheit ohne Geschwindigkeitsverlust aufnehmen 
        /// kann.
        /// </summary>
        [DisplayName("Carrier Strength")]
        [Description("")]
        public float CarrierStrength {get;set;}

        public CarrierState() : base() { }

        public CarrierState(CarrierProperty property) : base(property)
        {
            CarrierStrength = property.CarrierStrength;
            property.OnCarrierStrengthChanged += (i, v) => { CarrierStrength = v; };
        }

        public override void SerializeFirst(System.IO.BinaryWriter stream, byte version)
        {
            stream.Write(CarrierStrength);
        }

        public override void SerializeUpdate(System.IO.BinaryWriter stream, byte version)
        {
            stream.Write(CarrierStrength);
        }

        public override void DeserializeFirst(System.IO.BinaryReader stream, byte version)
        {
            CarrierStrength = stream.ReadSingle();
        }

        public override void DeserializeUpdate(System.IO.BinaryReader stream, byte version)
        {
            CarrierStrength = stream.ReadSingle();
        }
    }
}
