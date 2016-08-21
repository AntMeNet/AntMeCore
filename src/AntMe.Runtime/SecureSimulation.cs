using AntMe.Serialization;
using System;
using System.Security.Policy;

namespace AntMe.Runtime
{
    /// <summary>
    /// Der sichere AppDomain Host für eine laufende Simulation. Mit Hilfe der 
    /// AppDomain kann sowohl unsicherer Code geladen, als auch die Libraries 
    /// nach Ende der Simulation wieder aus dem Prozessraum entfernt werden.
    /// </summary>
    public sealed class SecureSimulation : IDisposable
    {
        private AppDomain _appDomain;
        private SecureHost _host;
        private LevelStateByteSerializer _deserializer;
        private Frame _lastState;
        private string[] extensionPaths;


        /// <summary>
        /// Status der Simulationsinstanz
        /// </summary>
        public SimulationState State { get; private set; }

        /// <summary>
        /// Gibt die Exception zurück, die zum Abbruch der Simulation geführt hat.
        /// </summary>
        public Exception LastException { get; private set; }

        public SecureSimulation(string[] extensionPaths)
        {
            this.extensionPaths = extensionPaths;
            State = SimulationState.Stopped;
        }

        /// <summary>
        /// Initialisiert und startet die Simulation.
        /// </summary>
        /// <param name="settings">Simulationseinstellungen</param>
        public void Start(Setup settings)
        {
            // Check for right State
            if (State != SimulationState.Stopped)
                throw new NotSupportedException("Simulation is not stopped");

            Type t = typeof(SecureHost);

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Evidence evidence = new Evidence();

            try
            {
                _appDomain = AppDomain.CreateDomain("AntMe! Jail", evidence, setup);
                _host = _appDomain.CreateInstanceAndUnwrap(t.Assembly.FullName, t.FullName) as SecureHost;

                _host.Setup(extensionPaths, settings);

                State = SimulationState.Running;
            }
            catch
            {
                State = SimulationState.Failed;
                Stop();
                throw;
            }

            SimulationContext context = ExtensionLoader.CreateSimulationContext();
            _deserializer = new LevelStateByteSerializer(context);
        }

        public Frame NextState()
        {
            // Check for right State
            if (State != SimulationState.Running)
                throw new NotSupportedException("Simulation is not running");

            try
            {
                byte[] rawData = _host.NextFrame();
                Frame state = _deserializer.Deserialize(rawData);

                Exception ex = _host.GetLastException();
                if (ex != null)
                {
                    // Fehlerfall
                    State = SimulationState.Failed;
                    LastException = ex;
                    Stop();
                }

                // Simulation regulär zu Ende
                else if (state.Mode > AntMe.SimulationState.Running) 
                {
                    State = SimulationState.Finished;
                    Stop();
                }

                return state;
            }
            catch (Exception ex)
            {
                State = SimulationState.Failed;
                LastException = ex;
                Stop();
                throw;
            }
        }

        public void Stop()
        {
            if (_appDomain != null)
            {
                _host = null;
                AppDomain.Unload(_appDomain);
                _appDomain = null;

                _deserializer?.Dispose();
                _deserializer = null;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
