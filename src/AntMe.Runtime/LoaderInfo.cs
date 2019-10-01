using System;
using System.Collections.Generic;

namespace AntMe.Runtime
{
    /// <summary>
    ///     Result Set for the individual Extension Loader.
    ///     Contains all discovered Elements and a list of occured errors.
    /// </summary>
    [Serializable]
    public sealed class LoaderInfo
    {
        /// <summary>
        ///     Creates a new instance of LoaderInfo.
        /// </summary>
        public LoaderInfo()
        {
            Levels = new List<LevelInfo>();
            Campaigns = new List<CampaignInfo>();
            Errors = new List<Exception>();
            Players = new List<PlayerInfo>();
        }

        /// <summary>
        ///     List of discovered Levels.
        /// </summary>
        public List<LevelInfo> Levels { get; }

        /// <summary>
        ///     List of discovered Campaigns.
        /// </summary>
        public List<CampaignInfo> Campaigns { get; }

        /// <summary>
        ///     List of occured errors.
        /// </summary>
        public List<Exception> Errors { get; }

        /// <summary>
        ///     List of discovered Players.
        /// </summary>
        public List<PlayerInfo> Players { get; set; }

        /// <summary>
        ///     Adds another LoaderInfo to the current result set.
        /// </summary>
        /// <param name="info">Additional LoaderInfo</param>
        public void AddRange(LoaderInfo info)
        {
            Campaigns.AddRange(info.Campaigns);
            Errors.AddRange(info.Errors);
            Levels.AddRange(info.Levels);
            Players.AddRange(info.Players);
        }
    }
}