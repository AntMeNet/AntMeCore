using AntMe.Basics.Items;
using System;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    /// Death counter for Ants.
    /// </summary>
    public sealed class AntDeathCounterProperty : DeathCountProperty<AntItem>
    {
        /// <summary>
        /// Default Constructor for Type Mapper.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="faction">Reference to the Faction</param>
        public AntDeathCounterProperty(SimulationContext context, Faction faction) : base(context, faction) { }

        /// <summary>
        /// Returns the Points Category.
        /// </summary>
        public override string PointsCategory { get { return "DeadAnts"; } }

        /// <summary>
        /// Calculates the Points for the current Counter States.
        /// </summary>
        /// <returns>Points</returns>
        protected override int RecalculatePoints()
        {
            return -5 * KilledItemsCount;
        }
    }
}
