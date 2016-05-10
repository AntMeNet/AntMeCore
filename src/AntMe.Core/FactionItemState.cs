using System.ComponentModel;

namespace AntMe
{
    /// <summary>
    /// Basisklasse für die Übertragung eines Fraktion bezogenen Item States. Für
    /// die Übertragung von unabhängigen Items bitte ItemState verwenden.
    /// </summary>
    public abstract class FactionItemState : ItemState
    {
        /// <summary>
        /// Leerer Konstruktur für den Deserializer.
        /// </summary>
        public FactionItemState() : base() { }

        /// <summary>
        /// Faction Item Konstruktor innerhalb der Simulation.
        /// </summary>
        /// <param name="item"></param>
        public FactionItemState(FactionItem item) : base(item)
        {
            PlayerIndex = item.Faction.SlotIndex;
        }

        /// <summary>
        /// Gibt den Spieler Index an oder legt diesen fest.
        /// </summary>
        [DisplayName("Player Index")]
        [Description("")]
        [ReadOnly(true)]
        [Category("Static")]
        public byte PlayerIndex { get; set; }
    }
}