namespace AntMe
{
    /// <summary>
    ///     Trigger zur Benachrichtigung bei einer bestimmten Rundenzahl.
    /// </summary>
    public sealed class TimerTrigger : ITrigger
    {
        public TimerTrigger()
        {
            Enabled = false;
            TargetTime = 0;
        }

        public TimerTrigger(int targetTime)
        {
            TargetTime = targetTime;
            Enabled = true;
        }

        public int TargetTime { get; set; }

        #region ITrigger Members

        public bool Enabled { get; set; }

        public bool Update(Engine engine)
        {
            if (Enabled && engine.Round == TargetTime)
            {
                if (OnTimeReached != null)
                    OnTimeReached(this);
                return true;
            }
            return false;
        }

        #endregion

        public event TriggerEvent OnTimeReached;
    }
}