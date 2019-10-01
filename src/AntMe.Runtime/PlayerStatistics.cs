using System;

namespace AntMe.Runtime
{
    [Serializable]
    public sealed class PlayerStatistics
    {
        public Guid Guid { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int MaxPoints { get; set; }
        public int AvgPoints { get; set; }
    }
}