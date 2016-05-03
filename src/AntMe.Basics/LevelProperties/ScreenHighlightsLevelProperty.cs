using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Basics.LevelProperties
{
    public sealed class ScreenHighlightsLevelProperty : LevelProperty
    {
        private Queue<ScreenHighlight> screenHighlights = null;

        public ScreenHighlightsLevelProperty(Level level) : base(level) { }

        /// <summary>
        ///     Fügt der aktuellen Runde ein Screen Highlight hinzu.
        /// </summary>
        /// <param name="highlight">Screenhighlight</param>
        protected void AddScreenHighlight(ScreenHighlight highlight)
        {
            // Nur im Running-Mode erlaubt.
            if (Level.Mode != LevelMode.Running)
                throw new NotSupportedException("Level is not running");

            screenHighlights.Enqueue(highlight);
        }

        public override void OnInit()
        {
            screenHighlights = new Queue<ScreenHighlight>();
        }
    }
}
