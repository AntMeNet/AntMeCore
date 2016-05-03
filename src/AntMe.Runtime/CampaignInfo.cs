using System;
using System.Collections.Generic;

namespace AntMe.Runtime
{
    /// <summary>
    /// Info Klasse zur Beschreibung von kompletten Story-Sammlungen.
    /// </summary>
    [Serializable]
    public sealed class CampaignInfo
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public TypeInfo Type { get; set; }
        public CampaignDescriptionAttribute DescriptionAttribute { get; set; }
        public List<LevelInfo> Levels { get; set; }
        public CampaignStatistics Statistics { get; set; }

        public CampaignInfo()
        {
            Levels = new List<LevelInfo>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
