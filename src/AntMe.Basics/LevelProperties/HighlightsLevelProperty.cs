using System;
using System.Collections.Generic;

namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Base Class for all Highlight Level Properties.
    /// </summary>
    /// <typeparam name="T">Type of Highlights</typeparam>
    public abstract class HighlightsLevelProperty<T> : LevelProperty where T : Highlight
    {
        private Queue<T> highlights = null;

        /// <summary>
        /// Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="level">Level</param>
        public HighlightsLevelProperty(Level level) : base(level)
        {
            highlights = new Queue<T>();
        }

        /// <summary>
        /// Adds a new Highlight to the Output Queue.
        /// </summary>
        /// <param name="highlight">Highlight</param>
        protected void AddHighlight(T highlight)
        {
            // Only allowed in Running Mode
            if (Level.Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            highlights.Enqueue(highlight);
        }
    }
}
