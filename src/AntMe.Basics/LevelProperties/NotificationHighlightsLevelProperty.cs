namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Highlight Property for common Notifications.
    /// </summary>
    public sealed class NotificationHighlightsLevelProperty : HighlightsLevelProperty<NotificationHighlight>
    {
        /// <summary>
        /// Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="level">Level</param>
        public NotificationHighlightsLevelProperty(Level level) : base(level) { }
    }
}
