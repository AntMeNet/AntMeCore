using System;
using AntMe.Basics.Items;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Interop Class for the Ant Faction Units (Ants)
    /// </summary>
    public sealed class AntUnitInterop : UnitInterop<AntFaction, AntItem>
    {
        private int markerDelay;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Faction</param>
        /// <param name="item">Item</param>
        public AntUnitInterop(AntFaction faction, AntItem item) : base(faction, item) { }

        /// <summary>
        ///     Wird von der Faction beim Update dieser Einheit aufgerufen.
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
            var marker = new MarkerItem(Faction.Context, Faction,
                Item.Position.ToVector2XY(), size, information);

            Item.Engine.InsertItem(marker);
            markerDelay = Faction.Settings.GetInt<AntItem>("MarkerDelay").Value;
            return true;
        }

        #region Eigenschaften

        /// <summary>
        ///     Liefert die Faction-weite Instanz eines Zufallsgenerators.
        /// </summary>
        public Random Random { get { return Faction.Context.Random; } }

        /// <summary>
        /// Gibt die Ausrichtung der Ameise zurück.
        /// </summary>
        public Angle Orientation { get { return Item.Orientation; } }

        #endregion

        #region Events

        /// <summary>
        ///     Event wird in jeder Runde einmal aufgerufen.
        /// </summary>
        public event InteropProperty.InteropEvent Tick;

        #endregion
    }
}