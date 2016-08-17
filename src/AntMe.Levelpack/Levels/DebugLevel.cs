using AntMe.Basics.Factions.Ants;
using AntMe.Levelpack.Properties;

namespace AntMe.Levelpack.Levels
{
    [LevelDescription(
        "{1E5BC543-96FB-48FF-B46C-2F79D9ED732A}",
        "Toms Debug",
        "Debug Level")]
    public sealed class DebugLevel : Level
    {
        public DebugLevel(SimulationContext context) : base(context)
        {
            context.Settings.Set<AntFaction>("ConcurrentAntCount", 1);
        }

        protected override void DoSettings(KeyValueStore levelSettings, KeyValueStore[] slotSettings)
        {
            base.DoSettings(levelSettings, slotSettings);
        }

        public override byte[] GetMap()
        {
            return Resources.defaultmap;
        }
    }
}
