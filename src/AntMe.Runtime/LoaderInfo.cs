using System;
using System.Collections.Generic;

namespace AntMe.Runtime
{
    [Serializable]
    public sealed class LoaderInfo
    {
        public List<LevelInfo> Levels { get; set; }
        public List<CampaignInfo> Campaigns { get; set; }
        public List<Exception> Errors { get; set; }
        public List<PlayerInfo> Players { get; set; }
        public List<CodeGeneratorAttribute> CodeGenerators { get; set; }

        public LoaderInfo()
        {
            Levels = new List<LevelInfo>();
            Campaigns = new List<CampaignInfo>();
            Errors = new List<Exception>();
            Players = new List<PlayerInfo>();
            CodeGenerators = new List<CodeGeneratorAttribute>();
        }

        public void AddRange(LoaderInfo info)
        {
            Campaigns.AddRange(info.Campaigns);
            Errors.AddRange(info.Errors);
            Levels.AddRange(info.Levels);
            Players.AddRange(info.Players);
            CodeGenerators.AddRange(info.CodeGenerators);
        }
    }
}
