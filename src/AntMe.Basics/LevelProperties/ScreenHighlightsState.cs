using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AntMe.Basics.LevelProperties
{
    public sealed class ScreenHighlightsState : LevelStateProperty
    {
        /// <summary>
        ///     Liefert eine Liste von Screen Highlights wie Notifications und Map Marker.
        /// </summary>
        public IList<ScreenHighlight> ScreenHighlights;

        public ScreenHighlightsState()
        {
            ScreenHighlights = new List<ScreenHighlight>();
        }

        public override void DeserializeFirst(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeFirst(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }

        public override void SerializeUpdate(BinaryWriter stream, byte version)
        {
            throw new NotImplementedException();
        }
    }
}
