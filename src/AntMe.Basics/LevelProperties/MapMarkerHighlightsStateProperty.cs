namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Map Marker Highlight Property State
    /// </summary>
    public sealed class MapMarkerHighlightsStateProperty : HighlightsStateProperty<MapMarkerHighlight>
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public MapMarkerHighlightsStateProperty()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Related Level</param>
        /// <param name="property">Related Level Property</param>
        public MapMarkerHighlightsStateProperty(Level level, MapMarkerHighlightsLevelProperty property)
            : base(level, property)
        {
        }
    }
}