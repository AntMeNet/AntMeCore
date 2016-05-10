using System;

namespace AntMe
{
    /// <summary>
    /// Slot Configuration for the Level.
    /// </summary>
    public sealed class LevelSlot
    {
        /// <summary>
        /// Holds the Type of the Players Factory Class.
        /// </summary>
        public Type FactoryType { get; set; }

        /// <summary>
        /// Display Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Player Clolor.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Team Index.
        /// </summary>
        public byte Team { get; set; }
    }
}
