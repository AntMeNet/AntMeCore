using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime.EventLog
{
    /// <summary>
    /// Basisklasse eines Log-Eintrags
    /// </summary>
    public abstract class Entry
    {
        public int Round { get; set; }
    }
}
