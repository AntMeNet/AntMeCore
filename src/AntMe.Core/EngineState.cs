namespace AntMe
{
    /// <summary>
    ///     State of the simulation engine.
    /// </summary>
    public enum EngineState
    {
        /// <summary>
        ///     Not initialized state. Call Init() to change this.
        /// </summary>
        Uninitialized,

        /// <summary>
        ///     Engine is still ready for the next round of simulation.
        /// </summary>
        Simulating,

        /// <summary>
        ///     Engine got an error and exit.
        /// </summary>
        Failed,

        /// <summary>
        ///     Simulation is over.
        /// </summary>
        Finished
    }
}