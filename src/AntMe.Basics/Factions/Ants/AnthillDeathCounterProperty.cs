using AntMe.Basics.Items;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Destroyed Anthill Counter.
    /// </summary>
    class AnthillDeathCounterProperty : DeathCountProperty<AnthillItem>
    {
        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="faction">Reference to the Faction</param>
        public AnthillDeathCounterProperty(SimulationContext context, Faction faction) : base(context, faction) { }

        /// <summary>
        /// Returns the Points Category.
        /// </summary>
        public override string PointsCategory { get { return "DestroyedAnthills"; } }

        /// <summary>
        /// Calculates the Points for the current Counter States.
        /// </summary>
        /// <returns>Points</returns>
        protected override int RecalculatePoints()
        {
            // TODO: Use Settings
            return -1000 * KilledItemsCount;
        }
    }
}
