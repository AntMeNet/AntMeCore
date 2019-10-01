using System;
using System.Net;
using System.ServiceModel;

namespace AntMe.Runtime.Communication
{
    public static class SimulationClient
    {
        /// <summary>
        ///     Erstellt einen Simulationskäfig im aktuellen Prozessraum, allerdings mit AppDomain Sicherheitskäfig.
        /// </summary>
        /// <returns>Simulation Client</returns>
        public static ISimulationClient CreateSecure(string[] extensionPaths, ITypeResolver resolver)
        {
            return new SecureSimulationClient(extensionPaths, resolver);
        }

        /// <summary>
        ///     Erstellt einen Simulationskäfig im aktuellen Prozessraum ohne Abschirmung. Bitte nur im Debug-Modus verwenden.
        /// </summary>
        /// <returns>Simulation Client</returns>
        public static ISimulationClient CreateUnsecure(string[] extensionPaths, ITypeResolver resolver)
        {
            return new UnsecureSimulationClient(extensionPaths, resolver);
        }

        /// <summary>
        ///     Create a Client over Named Papes.
        /// </summary>
        /// <param name="pipeName">Pipe Name</param>
        /// <returns>Instance of the Client</returns>
        public static ISimulationClient CreateNamedPipe(string[] extensionPaths, string pipeName)
        {
            var callback = new WcfSimulationCallback();
            var client = new WcfSimulationClient(extensionPaths, callback);

            var factory =
                new DuplexChannelFactory<ISimulationService>(
                    callback,
                    new NetNamedPipeBinding(),
                    new EndpointAddress("net.pipe://localhost/" + pipeName));

            var channel = factory.CreateChannel();

            client.Create(factory, channel);
            return client;
        }

        /// <summary>
        ///     Create a Client over TCP.
        /// </summary>
        /// <param name="address">Target IP</param>
        /// <param name="port">Port</param>
        /// <returns>Instance of the Client</returns>
        public static ISimulationClient CreateTcp(string[] extensionPaths, IPAddress address, int port)
        {
            var callback = new WcfSimulationCallback();
            var client = new WcfSimulationClient(extensionPaths, callback);

            throw new NotImplementedException();

            // TODO:
            var factory =
                new DuplexChannelFactory<ISimulationService>(
                    callback,
                    new NetTcpBinding(),
                    new EndpointAddress(""));

            var channel = factory.CreateChannel();

            client.Create(factory, channel);
            return client;
        }
    }
}