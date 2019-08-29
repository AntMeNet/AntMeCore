﻿using AntMe.Basics.Factions;
using AntMe.Basics.Factions.Ants;
using AntMe.Basics.Factions.Ants.Interop;
using System;

namespace AntMe.Extension.Community.Players
{
    [Player(Hidden = false, Name = "Default Ants", Author = "Random Dude")]
    public class DefaultAntColony : AntFactory
    {
        private AntFactoryInterop interop;
        private TotalStatisticsInterop totalStatistics;
        private ByTypeStatisticsInterop byTypeStatistics;
        private ByCasteStatisticsInterop byCasteStatistics;

        public override void Init(FactoryInterop interop)
        {
            this.interop = interop as AntFactoryInterop;
            this.interop.OnCreateMember += AntInterop_OnCreateMember;

            totalStatistics = interop.GetProperty<TotalStatisticsInterop>();
            if (totalStatistics == null)
                throw new ArgumentException("No Total Statistics");

            byTypeStatistics = interop.GetProperty<ByTypeStatisticsInterop>();
            if (byTypeStatistics == null)
                throw new ArgumentException("No By Type Statistics");

            byCasteStatistics = interop.GetProperty<ByCasteStatisticsInterop>();
            if (byCasteStatistics == null)
                throw new ArgumentException("No By Caste Statistics");
        }

        private Type AntInterop_OnCreateMember()
        {
            return typeof(DefaultAnt);
        }
    }
}
