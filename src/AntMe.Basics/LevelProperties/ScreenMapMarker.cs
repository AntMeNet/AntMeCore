namespace AntMe
{
    /// <summary>
    ///     Klasse zur Signalisierung eines Punktes auf der Karte. Der klassische
    ///     Point of Interest.
    /// </summary>
    public class ScreenMapMarker : ScreenHighlight
    {
        /// <summary>
        ///     Gibt die position des MapMarkers an.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     Gibt an, ob der Client diese Stelle automatisch fokusieren sollte.
        /// </summary>
        public bool Focus { get; set; }
    }
}