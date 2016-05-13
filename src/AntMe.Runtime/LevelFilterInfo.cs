﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime
{
    // TODO: Do we need this? Hows about the Filter Attribute?
    [Serializable]
    public sealed class LevelFilterInfo
    {
        public TypeInfo Type { get; set; }
        public string Comment { get; set; }
        public int SlotIndex { get; set; }
    }
}
