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
        /// Reference to the Faction.
        /// </summary>
        private readonly new AntFaction Faction;

        /// <summary>
        /// Reference to the related Ant Item.
        /// </summary>
        private readonly new AntItem Item;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        public AntUnitInterop(AntFaction faction, AntItem item)
            : base(faction, item)
        {
            Faction = faction;
            Item = item;
        }

        /// <summary>
        /// Gets called by the Engine on Round Update.
        /// </summary>
        protected override void OnUpdate()
        {
            // Update Marker Delay
            markerDelay = Math.Max(0, markerDelay - 1);

            // Tick ausführen
            Tick?.Invoke();
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
            var marker = new MarkerItem(Faction.Context, Faction,
                Item.Position.ToVector2XY(), size, information);

            Item.Level.Insert(marker);
            markerDelay = Faction.Settings.GetInt<AntItem>("MarkerDelay").Value;
            return true;
        }

        #region Properties

        /// <summary>
        /// Gets the Name of this Item.
        /// </summary>
        public string Name { get { return Item.Name; } }

        /// <summary>
        /// Returns the Caste Name for this Item.
        /// </summary>
        public string Caste { get { return Item.Attributes?.Name ?? string.Empty; } }

        #endregion

        #region Events

        /// <summary>
        /// Signal for a new Round.
        /// </summary>
        public event InteropProperty.InteropEvent Tick;

        #endregion
    }
}