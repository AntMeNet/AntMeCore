namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Base Interface for all kind of Level Triggers.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Gets or sets if the Trigger is active and should be triggered.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets called in every Round to check Trigger Condition.
        /// </summary>
        /// <param name="level">Reference to the Level</param>
        /// <returns>Triggered?</returns>
        bool Update(Level level);
    }

    /// <summary>
    /// Default Delegate for Use in Triggers.
    /// </summary>
    /// <param name="trigger">Reference to the Trigger</param>
    public delegate void TriggerEvent(ITrigger trigger);

    /// <summary>
    /// Default Delegate for Use in Triggers.
    /// </summary>
    /// <typeparam name="T">Parameter Type</typeparam>
    /// <param name="trigger">Reference to the Trigger</param>
    /// <param name="param">Trigger Value</param>
    public delegate void TriggerEvent<T>(ITrigger trigger, T param);
}