using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse für Item-bezogene Event im Faction-Kontext.
    /// </summary>
    public abstract class FactionItemEntry : ItemEntry
    {
        public int SlotIndex { get; set; }
    }
}
