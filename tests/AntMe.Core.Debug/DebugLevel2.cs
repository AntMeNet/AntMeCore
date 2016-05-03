using AntMe.Core;
using AntMe.Simulation.Factions.Bugs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    [LevelDescription(
    MinPlayerCount = 1,
    MaxPlayerCount = 1
    )]
    [FactionFilter(FactionType=typeof(BugFaction), PlayerIndex=0, Comment="Weils geht")]
    public sealed class DebugLevel2 : Level
    {
        public override Guid Guid { get { return Guid.Parse("{C86FE4E5-4152-4654-A476-0F5A72FB0C6F}"); } }
        public override string Name { get { return "Debug Level 2"; } }
        public override string Description { get { return "Eine Map mit unterschiedlicher Höhenmap"; } }

        public override Map GetMap()
        {
            Map map = Map.CreateMap(MapPreset.Small, true);

            map.StartPoints = new Core.Index2[1];
            map.StartPoints[0] = new Core.Index2(5, 10);

            return map;
        }
    }
}
