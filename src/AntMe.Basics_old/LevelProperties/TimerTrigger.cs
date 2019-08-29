namespace AntMe.Basics.LevelProperties
{
    /// <summary>
    /// Level Trigger to get informed about a specific Time / Round.
    /// </summary>
    public sealed class TimerTrigger : ITrigger
    {
        /// <summary>
        /// Default Constructor without Parameter.
        /// </summary>
        public TimerTrigger()
        {
            Enabled = false;
            TargetTime = 0;
        }

        /// <summary>
        /// Initialize this Trigger with the given Time.
        /// </summary>
        /// <param name="targetTime">Trigger Time</param>
        public TimerTrigger(int targetTime)
        {
            TargetTime = targetTime;
            Enabled = true;
        }

        /// <summary>
        /// Gets or sets the Time when the Trigger should throw the Event.
        /// </summary>
        public int TargetTime { get; set; }

        /// <summary>
        /// Gets or sets if the Trigger is active and should be triggered.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets called in every Round to check Trigger Condition.
        /// </summary>
        /// <param name="level">Reference to the Engine</param>
        /// <returns>Triggered?</returns>
        public bool Update(Level level)
        {
            if (Enabled && level.Engine.Round == TargetTime)
            {
                OnTimeReached?.Invoke(this);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Event to inform about reaching Trigger Time.
        /// </summary>
        public event TriggerEvent OnTimeReached;
    }
}