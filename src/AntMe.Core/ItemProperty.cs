using System.ComponentModel;

namespace AntMe
{
    /// <summary>
    /// Basisklasse für jegliche Art von Item Properties.
    /// </summary>
    public abstract class ItemProperty : Property
    {
        private readonly Item item;

        /// <summary>
        /// Konstruktor des Item Properties.
        /// </summary>
        /// <param name="item">Referenz auf das zugehörige Item.</param>
        public ItemProperty(Item item)
        {
            this.item = item;
        }

        /// <summary>
        /// Liefert die Referenz auf das betroffene Item zurück.
        /// </summary>
        [Browsable(false)]
        public Item Item { get { return item; } }
    }
}