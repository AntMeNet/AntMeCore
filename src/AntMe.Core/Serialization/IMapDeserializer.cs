using System;
using System.IO;

namespace AntMe.Serialization
{
    /// <summary>
    /// Internal Interface for all Map Deserializer.
    /// </summary>
    internal interface IMapDeserializer : IDisposable
    {
        /// <summary>
        /// Deserializes the Content of the given Stream.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="stream">Input Stream</param>
        /// <returns>Deserialized Map</returns>
        Map Deserialize(SimulationContext context, Stream stream);
    }
}
