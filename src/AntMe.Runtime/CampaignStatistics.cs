using System;

namespace AntMe.Runtime
{
    [Serializable]
    public sealed class CampaignStatistics
    {
        public Guid Guid { get; set; }
        public int Played { get; set; }
        public byte[] Settings { get; set; }
    }
}
