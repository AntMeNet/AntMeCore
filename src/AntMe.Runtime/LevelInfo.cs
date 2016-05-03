
using System;

namespace AntMe.Runtime
{
    [Serializable]
    public sealed class LevelInfo
    {
        public TypeInfo Type { get; set; }
        public LevelDescriptionAttribute LevelDescription { get; set; }
        public LevelFilterInfo[] FactionFilter { get; set; }
        public LevelStatistics Statistics { get; set; }

        public override string ToString()
        {
            if (LevelDescription != null)
                return LevelDescription.Name;
            return string.Empty;
        }
    }
}
