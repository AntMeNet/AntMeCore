namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Highlight State for Notifications.
    /// </summary>
    public sealed class NotificationHighlightsStateProperty : HighlightsStateProperty<NotificationHighlight>
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public NotificationHighlightsStateProperty()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Related Level</param>
        /// <param name="property">Related Level Property</param>
        public NotificationHighlightsStateProperty(Level level, NotificationHighlightsLevelProperty property)
            : base(level, property)
        {
        }
    }
}