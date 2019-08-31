namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Highlight to give a user a Point of Interest.
    /// </summary>
    public class MapMarkerHighlight : Highlight
    {
        /// <summary>
        /// Marker Position on Map.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Should the Client be forced to focus this Point?
        /// </summary>
        public bool Focus { get; set; }
    }
}