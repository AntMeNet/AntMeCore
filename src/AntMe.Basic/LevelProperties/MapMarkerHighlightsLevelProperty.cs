namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Highlight Level Property for adding Map Marker and Point of Interest.
    /// </summary>
    public sealed class MapMarkerHighlightsLevelProperty : HighlightsLevelProperty<MapMarkerHighlight>
    {
        /// <summary>
        /// Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="level">Level</param>
        public MapMarkerHighlightsLevelProperty(Level level) : base(level) { }
    }
}
