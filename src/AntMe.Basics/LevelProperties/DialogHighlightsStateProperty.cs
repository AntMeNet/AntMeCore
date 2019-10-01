namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    ///     Highlight State for Dialogs.
    /// </summary>
    public sealed class DialogHighlightsStateProperty : HighlightsStateProperty<DialogHighlight>
    {
        /// <summary>
        ///     Default Constructor for the Deserializer.
        /// </summary>
        public DialogHighlightsStateProperty()
        {
        }

        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="level">Related Level</param>
        /// <param name="property">Related Level Property</param>
        public DialogHighlightsStateProperty(Level level, DialogHighlightsLevelProperty property) : base(level,
            property)
        {
        }
    }
}