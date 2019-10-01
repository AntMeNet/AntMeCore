using System;
using System.Security.Policy;
using AntMe.Serialization;

namespace AntMe.Runtime
{
    /// <summary>
    ///     Der sichere AppDomain Host für eine laufende Simulation. Mit Hilfe der
    ///     AppDomain kann sowohl unsicherer Code geladen, als auch die Libraries
    ///     nach Ende der Simulation wieder aus dem Prozessraum entfernt werden.
    /// </summary>
    public sealed class SecureSimulation : IDisposable
    {
        private AppDomain _appDomain;
        private LevelStateByteSerializer _deserializer;
        private SecureHost _host;
        private LevelState _lastState;
        private readonly string[] extensionPaths;

        public SecureSimulation(string[] extensionPaths)
        {
            this.extensionPaths = extensionPaths;
            State = SimulationState.Stopped;
        }


        /// <summary>
        ///     Status der Simulationsinstanz
        /// </summary>
        public SimulationState State { get; private set; }

        /// <summary>
        ///     Gibt die Exception zurück, die zum Abbruch der Simulation geführt hat.
        /// </summary>
        public Exception LastException { get; private set; }

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        ///     Initialisiert und startet die Simulation.
        /// </summary>
        /// <param name="settings">Simulationseinstellungen</param>
        public void Start(Setup settings)
        {
            // Check for right State
            if (State != SimulationState.Stopped)
                throw new NotSupportedException("Simulation is not stopped");

            var t = typeof(SecureHost);

            var setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var evidence = new Evidence();

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

            var context = ExtensionLoader.CreateSimulationContext();
            _deserializer = new LevelStateByteSerializer(context);
        }

        public LevelState NextState()
        {
            // Check for right State
            if (State != SimulationState.Running)
                throw new NotSupportedException("Simulation is not running");

            try
            {
                var rawData = _host.NextFrame();
                var state = _deserializer.Deserialize(rawData);

                var ex = _host.GetLastException();
                if (ex != null)
                {
                    // Fehlerfall
                    State = SimulationState.Failed;
                    LastException = ex;
                    Stop();
                }

                // Simulation regulär zu Ende
                else if (state.Mode > LevelMode.Running)
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
    }
}