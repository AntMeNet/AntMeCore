using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse für Item-bezogene Einträge.
    /// </summary>
    public abstract class ItemEntry : Entry
    {
        public int Id { get; set; }
    }
}
