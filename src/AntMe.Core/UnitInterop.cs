using System;

namespace AntMe
{
    /// <summary>
    /// Base Class for all Faction Unit Interops.
    /// </summary>
    public abstract class UnitInterop : Interop
    {
        /// <summary>
        /// Reference to the own Faction.
        /// </summary>
        protected readonly Faction Faction;

        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        protected readonly FactionItem Item;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="item">Refernce to the Item</param>
        protected UnitInterop(Faction faction, FactionItem item)
        {
            if (faction == null)
                throw new ArgumentNullException(nameof(faction));

            // Faction soll bereits teil eines Levels sein.
            if (faction.Level == null)
                throw new ArgumentException("Faction is not Part of a Level");

            // AntItem darf nicht null sein.

            Faction = faction;
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        #region Properties

        /// <summary>
        /// Gets the Faction Randomizer.
        /// </summary>
        public Random Random { get { return Faction.Context.Random; } }

        /// <summary>
        /// Gets the Id of the related Item.
        /// </summary>
        public int Id { get { return Item.Id; } }

        /// <summary>
        /// Gets the current Orientation.
        /// </summary>
        public Angle Orientation { get { return Item.Orientation; } }

        /// <summary>
        /// Gets the Radius of the Item.
        /// </summary>
        public float Radius { get { return Item.Radius; } }

        #endregion
    }
}
