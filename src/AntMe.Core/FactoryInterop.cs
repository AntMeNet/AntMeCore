using System;

namespace AntMe
{
    /// <summary>
    /// Empty Base Class for all Factory Interops.
    /// </summary>
    public abstract class FactoryInterop : Interop
    {
        /// <summary>
        /// Current Game Round.
        /// </summary>
        public int Round { get; private set; }

        /// <summary>
        /// Current Game Time (based on the Frames Per Second Constant).
        /// </summary>
        public TimeSpan GameTime { get; private set; }

        /// <summary>
        /// Updates the current Interop.
        /// </summary>
        /// <param name="round">Current Simulation Round</param>
        protected override void Update(int round)
        {
            Round = round;
            GameTime = TimeSpan.FromSeconds((double)round / Level.FRAMES_PER_SECOND);

            base.Update(round);
        }
    }

    /// <summary>
    /// Specialized Base Class for all Factory Interops.
    /// </summary>
    /// <typeparam name="F">Type of related Faction</typeparam>
    public abstract class FactoryInterop<F> : FactoryInterop
        where F : Faction
    {
        /// <summary>
        /// Protected Reference to the related Faction.
        /// </summary>
        protected readonly F Faction;

        /// <summary>
        /// Default Constructor for the Type Mapper.
        /// </summary>
        /// <param name="faction">Instance of the related Faction</param>
        public FactoryInterop(F faction)
        {
            if (faction == null)
                throw new ArgumentNullException("faction");

            // Faction soll bereits teil eines Levels sein.
            if (faction.Level == null)
                throw new ArgumentException("Faction is not Part of a Level");

            Faction = faction;
        }
    }
}
