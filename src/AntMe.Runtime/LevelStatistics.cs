using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime
{
    [Serializable]
    public sealed class LevelStatistics
    {
        public Guid Guid { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Rating { get; set; }
    }
}
