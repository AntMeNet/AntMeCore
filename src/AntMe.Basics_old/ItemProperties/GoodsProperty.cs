using System;

namespace AntMe.Basics.ItemProperties
{
    /// <summary>
    /// Base Class for all Good related Item Properties.
    /// </summary>
    public abstract class GoodsProperty : ItemProperty
    {
        private int amount;
        private int capacity;

        /// <summary>
        /// Gets or sets the Maximum Capacity for this Good.
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = Math.Max(0, value);

                // Cap Amount
                if (Amount > capacity)
                    Amount = Math.Min(capacity, Amount);

                OnCapacityChanged?.Invoke(Item, capacity);
            }
        }

        /// <summary>
        /// Gets or sets the current Amount of the good.
        /// </summary>
        public int Amount
        {
            get { return amount; }
            set
            {
                // Cap to Capacity.
                amount = Math.Min(Capacity, Math.Max(0, value));
                OnAmountChanged?.Invoke(Item, amount);
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="item">Item</param>
        public GoodsProperty(Item item) : base(item) { }

        /// <summary>
        /// Signal for a changed Capacity
        /// </summary>
        public event ValueChanged<int> OnCapacityChanged;

        /// <summary>
        ///     Informiert über die Änderung der aktuellen Menge.
        /// </summary>
        public event ValueChanged<int> OnAmountChanged;
    }
}
