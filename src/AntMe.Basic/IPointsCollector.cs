namespace AntMe.Basics
{
    /// <summary>
    /// Interface for all Item- and Faction-Properties who are relevant for the point sum.
    /// </summary>
    public interface IPointsCollector
    {
        /// <summary>
        /// Returns the Points Category.
        /// </summary>
        string PointsCategory { get; }

        /// <summary>
        /// Returns the current Amount of Points.
        /// </summary>
        int Points { get; }

        /// <summary>
        /// Defines if the Points should count.
        /// </summary>
        bool EnablePoints { get; }

        /// <summary>
        /// Defines of the Counter will be removed after Item Death.
        /// </summary>
        bool PermanentPoints { get; }

        /// <summary>
        /// Signal for changed Enable Flag.
        /// </summary>
        event ValueUpdate<IPointsCollector, bool> OnEnablePointsChanged;

        /// <summary>
        /// Signal for a changed Point Counter.
        /// </summary>
        event ValueUpdate<IPointsCollector, int> OnPointsChanged;
    }
}
