using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Runtime
{
    /// <summary>
    /// List of possible Simulation States.
    /// </summary>
    public enum SimulationState
    {
        /// <summary>
        /// Not initialized
        /// </summary>
        Stopped = 1,

        /// <summary>
        /// Started and running
        /// </summary>
        Running = 2,

        /// <summary>
        /// Paused
        /// </summary>
        Paused = 3,

        /// <summary>
        /// Failed Simulation
        /// </summary>
        Failed = 4,

        /// <summary>
        /// Finished Simulation
        /// </summary>
        Finished = 5
    }
}
