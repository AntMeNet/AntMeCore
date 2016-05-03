using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace AntMe.Runtime.Communication
{
    public static class SimulationClient
    {
        /// <summary>
        /// Erstellt einen Simulationskäfig im aktuellen Prozessraum, allerdings mit AppDomain Sicherheitskäfig.
        /// </summary>
        /// <returns>Simulation Client</returns>
        public static ISimulationClient CreateSecure(ITypeResolver resolver)
        {
            return new SecureSimulationClient(resolver);
        }

        /// <summary>
        /// Erstellt einen Simulationskäfig im aktuellen Prozessraum ohne Abschirmung. Bitte nur im Debug-Modus verwenden.
        /// </summary>
        /// <returns>Simulation Client</returns>
        public static ISimulationClient CreateUnsecure(ITypeResolver resolver)
        {
            return new UnsecureSimulationClient(resolver);
        }

        /// <summary>
        /// Create a Client over Named Papes.
        /// </summary>
        /// <param name="pipeName">Pipe Name</param>
        /// <returns>Instance of the Client</returns>
        public static ISimulationClient CreateNamedPipe(string pipeName)
        {
            WcfSimulationCallback callback = new WcfSimulationCallback();
            WcfSimulationClient client = new WcfSimulationClient(callback);

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
        /// Create a Client over TCP.
        /// </summary>
        /// <param name="address">Target IP</param>
        /// <param name="port">Port</param>
        /// <returns>Instance of the Client</returns>
        public static ISimulationClient CreateTcp(IPAddress address, int port)
        {
            WcfSimulationCallback callback = new WcfSimulationCallback();
            WcfSimulationClient client = new WcfSimulationClient(callback);

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
