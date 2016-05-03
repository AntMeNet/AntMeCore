using System;
using System.ComponentModel;

namespace AntMe.ItemProperties.Basics
{
    /// <summary>
    ///     Basisklasse für alle Ressourcen.
    /// </summary>
    public abstract class CollectableGoodProperty : ItemProperty
    {
        public delegate void CollectableValueChanged(CollectableGoodProperty good, int newValue);

        private int amount;
        private int capacity;

        public CollectableGoodProperty(Item item) : base(item)
        {
            Capacity = capacity;
            Amount = amount;
        }

        /// <summary>
        ///     Liefert die Referenz auf die betroffene Eigenschaft zurück.
        /// </summary>
        [Browsable(false)]
        public ItemProperty Property { get; internal set; }

        /// <summary>
        ///     Gibt die Kapazität für dieses Gut an oder legt diese fest.
        /// </summary>
        [DisplayName("Capacity")]
        [Description("")]
        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = Math.Max(0, value);

                // Menge auf Kapazität deckeln
                if (Amount > capacity)
                    Amount = Math.Min(capacity, Amount);

                if (OnCapacityChanged != null)
                    OnCapacityChanged(this, capacity);
            }
        }

        /// <summary>
        ///     Gibt die aktuelle Ladung dieses Guts an oder legt diese fest.
        /// </summary>
        [DisplayName("Amount")]
        [Description("")]
        public int Amount
        {
            get { return amount; }
            set
            {
                // Menge auf [0,Capacity] kappen.
                amount = Math.Min(Capacity, Math.Max(0, value));
                if (OnAmountChanged != null)
                    OnAmountChanged(this, amount);
            }
        }

        /// <summary>
        ///     Transferiert die gesamte Menge des Guts in das Target.
        /// </summary>
        /// <param name="target">Ziel-Gut</param>
        /// <returns>
        ///     Erfolgsmeldung. False, wenn Typen nicht passen oder nicht
        ///     ausreichend Kapazitäten vorhanden sind.
        /// </returns>
        public bool Transfer(CollectableGoodProperty target)
        {
            // Prüfen, ob die Güter zueinander passen
            if (GetType() != target.GetType())
                return false;

            if (Amount > target.Capacity - target.Amount)
            {
                // Kapazitäten im Ziel unzureichend
                int temp = target.Capacity - target.Amount;
                Amount -= temp;
                target.Amount += temp;
                return false;
            }
            else
            {
                // Vollständiger Transfer zum Ziel
                int temp = Amount;
                Amount = 0;
                target.Amount += temp;
                return true;
            }
        }

        /// <summary>
        ///     Informiert über die Änderung der Kapazität dieses Guts.
        /// </summary>
        public event CollectableValueChanged OnCapacityChanged;

        /// <summary>
        ///     Informiert über die Änderung der aktuellen Menge.
        /// </summary>
        public event CollectableValueChanged OnAmountChanged;
    }
}