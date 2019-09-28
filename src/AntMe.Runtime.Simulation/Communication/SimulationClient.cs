using System.Threading.Tasks;
using AntMe.Runtime.Client;
using AntMe.Runtime.Client.Communication;
using AntMe.Runtime.Communication;

namespace AntMe.Runtime.Simulation.Communication
{
    public static class SimulationClient
    {
        /// <summary>
        /// Erstellt einen Simulationskäfig im aktuellen Prozessraum, allerdings mit AppDomain Sicherheitskäfig.
        /// </summary>
        /// <returns>Simulation Client</returns>
        public static ISimulationClient CreateSecure(string[] extensionPaths, ITypeResolver resolver)
        {
            return new SecureSimulationClient(extensionPaths, resolver);
        }

        /// <summary>
        /// Erstellt einen Simulationskäfig im aktuellen Prozessraum ohne Abschirmung. Bitte nur im Debug-Modus verwenden.
        /// </summary>
        /// <returns>Simulation Client</returns>
        public static ISimulationClient CreateUnsecure(string[] extensionPaths, ITypeResolver resolver)
        {
            return new UnsecureSimulationClient(extensionPaths, resolver);
        }

        ///// <summary>
        ///// Create a Client over Named Papes.
        ///// </summary>
        ///// <param name="pipeName">Pipe Name</param>
        ///// <returns>Instance of the Client</returns>
        //public static ISimulationClient CreateNamedPipe(string[] extensionPaths, string pipeName)
        //{
        //    WcfSimulationCallback callback = new WcfSimulationCallback();
        //    WcfSimulationClient client = new WcfSimulationClient(extensionPaths, callback);

        //    var factory =
        //        new DuplexChannelFactory<ISimulationService>(
        //            callback,
        //            new NetNamedPipeBinding(),
        //            new EndpointAddress("net.pipe://localhost/" + pipeName));

        //    var channel = factory.CreateChannel();

        //    client.Create(factory, channel);
        //    return client;
        //}

        /// <summary>
        /// Create a Client over TCP.
        /// </summary>
        /// <param name="address">Target IP</param>
        /// <param name="port">Port</param>
        /// <returns>Instance of the Client</returns>
        public static async Task<ISimulationClient> CreateSignalR(string[] extensionPaths, string uri)
        {
            var client = new SignalRSimulationClient(extensionPaths);

            await client.Create(uri);
            return client;
        }
    }
}
