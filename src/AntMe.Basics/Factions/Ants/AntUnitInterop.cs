using System;
using AntMe.Basics.Items;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Interop Class for the Ant Faction Units (Ants)
    /// </summary>
    public sealed class AntUnitInterop : UnitInterop
    {
        /// <summary>
        /// Delay Counter for Markers.
        /// </summary>
        private int markerDelay;

        /// <summary>
        /// Reference to the related Ant Item.
        /// </summary>
        private new readonly AntItem item;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        public AntUnitInterop(AntFaction faction, AntItem item) : base(faction, item)
        {
            this.item = item;
        }

        /// <summary>
        /// Gets called by the Engine on Round Update.
        /// </summary>
        protected override void Update(int round)
        {
            // Update Marker Delay
            markerDelay = Math.Max(0, markerDelay - 1);

            // Tick ausführen
            if (Tick != null)
                Tick();
        }

        /// <summary>
        /// Creates a Mark at the Ants Position.
        /// </summary>
        /// <param name="information">Information</param>
        /// <param name="size">Maxium Size</param>
        /// <returns>Succes Result</returns>
        public bool MakeMark(int information, float size)
        {
            // Wartezeit abwarten
            if (markerDelay > 0)
                return false;

            // Marker erstellen
            var marker = new MarkerItem(faction.Context, faction,
                item.Position.ToVector2XY(), size, information);

            item.Engine.InsertItem(marker);
            markerDelay = faction.Settings.GetInt<AntItem>("MarkerDelay").Value;
            return true;
        }

        #region Properties

        /// <summary>
        /// Gets the Name of this Item.
        /// </summary>
        public string Name { get { return item.Name; } }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a new Round.
        /// </summary>
        public event InteropProperty.InteropEvent Tick;

        #endregion
    }
}