using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime
{
    [Serializable]
    public sealed class LevelFilterInfo
    {
        public TypeInfo Type { get; set; }
        public string Comment { get; set; }
        public int PlayerIndex { get; set; }
    }
}
