namespace AntMe
{
    /// <summary>
    ///     Klasse zur Signalisierung einem Bereich auf der Karte.
    /// </summary>
    public class ScreenMapArea : ScreenMapMarker
    {
        /// <summary>
        ///     Gibt den zweiten Eckpunkt der Markierung an.
        /// </summary>
        public Vector2 Position2 { get; set; }
    }
}