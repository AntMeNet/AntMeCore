namespace AntMe.Factions.Bugs
{
    public class BugFactionSettings
    {
        /// <summary>
        /// Maximum of all bugs in one game, including killed ones.
        /// </summary>
        public int BugMaxTotalCount = int.MaxValue;

        /// <summary>
        /// Delay in ticks until a new bug spawns.
        /// </summary>
        public int BugRespawnDelay = 20;

        /// <summary>
        /// Maximum of all bugs currently present in one level.
        /// </summary>
        public int BugMaxConcurrentCount = 5;

        /// <summary>
        /// Number of bugs initially in the level.
        /// </summary>
        public int InitialBugCount = 0;
    }
}
