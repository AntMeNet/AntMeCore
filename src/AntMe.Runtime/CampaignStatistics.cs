using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
