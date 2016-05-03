using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse für Faction-bezogene Event Einträge.
    /// </summary>
    public abstract class FactionEntry : Entry
    {
        public int PlayerIndex { get; set; }
    }
}
