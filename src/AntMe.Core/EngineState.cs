using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe
{
    /// <summary>
    ///     Status der Simulation Engine.
    /// </summary>
    public enum EngineState
    {
        /// <summary>
        ///     Uninitialisierter Status. Rufe Init() auf, um das zu ändern.
        /// </summary>
        Uninitialized,

        /// <summary>
        ///     Die Engine ist bereits für die nächste Simulationsrunde.
        /// </summary>
        Simulating,

        /// <summary>
        /// Die Engine hatte einen Fehler und ist deshalb beendet.
        /// </summary>
        Failed,

        /// <summary>
        /// Die Simulation ist beendet.
        /// </summary>
        Finished,
    }
}
