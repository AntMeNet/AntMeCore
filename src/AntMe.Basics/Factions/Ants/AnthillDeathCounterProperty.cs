using AntMe.Basics.Items;

namespace AntMe.Basics.Factions.Ants
{
    /// <summary>
    ///     Destroyed Anthill Counter.
    /// </summary>
    internal class AnthillDeathCounterProperty : DeathCountProperty<AnthillItem>
    {
        /// <summary>
        ///     Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Reference to the Faction</param>
        public AnthillDeathCounterProperty(Faction faction) : base(faction)
        {
        }

        /// <summary>
        ///     Returns the Points Category.
        /// </summary>
        public override string PointsCategory => "DestroyedAnthills";

        /// <summary>
        ///     Calculates the Points for the current Counter States.
        /// </summary>
        /// <returns>Points</returns>
        protected override int RecalculatePoints()
        {
            // TODO: Use Settings
            return -1000 * KilledItemsCount;
        }
    }
}