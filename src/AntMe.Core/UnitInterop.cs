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
        protected readonly Faction faction;

        /// <summary>
        /// Reference to the related Item.
        /// </summary>
        protected readonly FactionItem item;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        /// <param name="item">Refernce to the Item</param>
        public UnitInterop(Faction faction, FactionItem item)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Faction soll bereits teil eines Levels sein.
            if (faction.Level == null)
                throw new ArgumentException("Faction is not Part of a Level");

            // AntItem darf nicht null sein.
            if (item == null)
                throw new ArgumentNullException("item");

            this.faction = faction;
            this.item = item;
        }

        #region Properties

        /// <summary>
        /// Gets more Information about the own Faction.
        /// </summary>
        public FactionInfo Faction { get { return faction.GetFactionInfo(item); } }

        /// <summary>
        /// Gets the Faction Randomizer.
        /// </summary>
        public Random Random { get { return faction.Context.Random; } }

        /// <summary>
        /// Gets the Id of the related Item.
        /// </summary>
        public int Id { get { return item.Id; } }

        /// <summary>
        /// Gets the current Orientation.
        /// </summary>
        public Angle Orientation { get { return item.Orientation; } }

        /// <summary>
        /// Gets the Radius of the Item.
        /// </summary>
        public float Radius { get { return item.Radius; } }

        #endregion
    }
}
