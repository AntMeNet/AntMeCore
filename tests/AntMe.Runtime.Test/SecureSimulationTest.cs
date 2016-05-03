using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace AntMe.Runtime.Test
{
    [TestClass]
    public class SecureSimulationTest
    {
        private SecureSimulation sim;

        #region Init

        [TestInitialize]
        public void Init()
        {
            sim = new SecureSimulation();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (sim != null)
            {
                sim.Dispose();
                sim = null;
            }
        }

        #endregion

        // TODO: Appdomain trennung (assemblies getrennt geladen? anderer Prozessraum?)
        // TODO: Schadcode durch statische Konustruktoren?
        // TODO: statische Variablen erkennen
        // TODO: Abhängigkeiten richtig auflösen
        // TODO: Kaputte Assemblies (broken, falsche Runtime, falsche Architektur,...)
        // TODO: Trennung bei identischer Assembly-ID (also bsp. zwei mal die selbe Ameise gegeneinander oder neuere gegen ältere Version der Ameise)
        // TODO: Unterschiedliche Fehlerfälle und ihre Auswirkungen checken
        // TODO: Aufrufe der methoden in unterschiedlichen Modes (Start in Running z.B:)

        #region Start() Tests

        /// <summary>
        /// Prüft, ob die Properties ohne Initialisierung richtig sind.
        /// </summary>
        [TestMethod]
        public void PropertiesBeforeInit()
        {
            Assert.AreEqual(SimulationState.Stopped, sim.State);
        }

        /// <summary>
        /// Testet Start ohne Parameter. Sollte scheitern
        /// </summary>
        [TestMethod]
        public void InitWithoutParameter()
        {
            try
            {
                sim.Start(null);
                Assert.Fail("Should throw Exception");
            }
            catch (ArgumentNullException) { }

            Assert.AreEqual(SimulationState.Failed, sim.State);
        }

        /// <summary>
        /// Testet mit vollkommen leeren Settings
        /// </summary>
        [TestMethod]
        public void InitWithEmptyParameter()
        {
            try
            {
                Settings settings = new Settings();
                settings.Colors = null;
                settings.Player = null;

                sim.Start(settings);
                Assert.Fail("Should throw Exception");
            }
            catch (ArgumentNullException) { }

            Assert.AreEqual(SimulationState.Failed, sim.State);
        }

        /// <summary>
        /// Testet mit default Setting
        /// </summary>
        public void InitWithDefaultParameter()
        {
            try
            {
                Settings settings = new Settings();

                sim.Start(settings);
                Assert.Fail("Should throw Exception");
            }
            catch (ArgumentNullException) { }

            Assert.AreEqual(SimulationState.Failed, sim.State);
        }

        /// <summary>
        /// Testet mit einem Level ohne Attribut
        /// </summary>
        [TestMethod]
        public void InitWithLevelWithoutAttribute()
        {
            try
            {
                Settings settings = new Settings();
                settings.Level = GetTypeInfo(typeof(TestLevel));
                sim.Start(settings);
                Assert.Fail("Should throw Exception");
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(SimulationState.Failed, sim.State);
        }

        /// <summary>
        /// Testet mit einem level, das eine Exception im Init wirft.
        /// </summary>
        [TestMethod]
        public void InitWithLevelWithExceptionOnInit()
        {
            try
            {
                Settings settings = new Settings();
                settings.Level = GetTypeInfo(typeof(TestLevelInitException));
                sim.Start(settings);
                Assert.Fail("Should throw Exception");
            }
            catch (NotImplementedException) { }

            Assert.AreEqual(SimulationState.Failed, sim.State);
        }

        /// <summary>
        /// Testet mit einem level, das vollständig initialisiert werden sollte.
        /// </summary>
        [TestMethod]
        public void InitWithLevelFullWorking()
        {
            Settings settings = new Settings();
            settings.Level = GetTypeInfo(typeof(TestLevelFullWorking));
            sim.Start(settings);

            Assert.AreEqual(SimulationState.Running, sim.State);
        }

        /// <summary>
        /// Testet mit funktionierendem Level und Factions
        /// </summary>
        [TestMethod]
        public void InitWithLevelAndFaction()
        {
            Settings settings = new Settings();
            settings.Level = GetTypeInfo(typeof(TestLevelFullWorking));
            settings.Player[2] = GetTypeInfo(typeof(RawColony));
            sim.Start(settings);

            Assert.AreEqual(SimulationState.Running, sim.State);
        }

        #endregion

        #region NextState() Tests

        /// <summary>
        /// Simuliert ein Level ohne Player für 1000 Runden.
        /// </summary>
        [TestMethod]
        public void Update1000TimesWithoutPlayer()
        {
            Settings settings = new Settings();
            settings.Level = GetTypeInfo(typeof(TestLevelFullWorking));
            sim.Start(settings);

            Assert.AreEqual(SimulationState.Running, sim.State);

            for (int i = 0; i < 1000; i++)
            {
                MainState state = sim.NextState();
                Assert.AreEqual(0, state.Factions.Count);
                Assert.AreEqual(0, state.Items.Count);
                Assert.IsNotNull(state.Map);
                Assert.AreEqual(i, state.Round);
                Assert.AreEqual(LevelMode.Running, state.Mode); 
                Assert.AreEqual(SimulationState.Running, sim.State);
            }
        }

        /// <summary>
        /// Simuliert ein Level ohne Player für 1000 Runden.
        /// </summary>
        [TestMethod]
        public void Update1000TimesWith1Player()
        {
            Settings settings = new Settings();
            settings.Level = GetTypeInfo(typeof(TestLevelFullWorking));
            settings.Player[2] = GetTypeInfo(typeof(RawColony));
            sim.Start(settings);

            Assert.AreEqual(SimulationState.Running, sim.State);

            for (int i = 0; i < 1000; i++)
            {
                MainState state = sim.NextState();
                Assert.AreEqual(1, state.Factions.Count);
                Assert.IsNotNull(state.Map);
                Assert.AreEqual(i, state.Round);
                Assert.AreEqual(LevelMode.Running, state.Mode);
                Assert.AreEqual(SimulationState.Running, sim.State);
            }
        }

        /// <summary>
        /// Testet das Verhalten bei Exception im Update
        /// </summary>
        [TestMethod]
        public void UpdateWithException()
        {
            Settings settings = new Settings();
            settings.Level = GetTypeInfo(typeof(TestLevelUpdateException));
            sim.Start(settings);

            Assert.AreEqual(SimulationState.Running, sim.State);

            MainState state = null;

            // Erste Runde
            state = sim.NextState();
            Assert.AreEqual(0, state.Round);
            Assert.AreEqual(LevelMode.Running, state.Mode);
            Assert.AreEqual(SimulationState.Running, sim.State);

            // Zweite Runde
            state = sim.NextState();
            Assert.AreEqual(1, state.Round);
            Assert.AreEqual(LevelMode.Running, state.Mode);
            Assert.AreEqual(SimulationState.Running, sim.State);

            // Dritte Runde
            state = sim.NextState();
            Assert.AreEqual(2, state.Round);
            Assert.AreEqual(LevelMode.FailedSystem, state.Mode);
            Assert.AreEqual(SimulationState.Failed, sim.State);
            Assert.IsNotNull(sim.LastException);
        }

        /// <summary>
        /// Testet das verhalten bei Draw-Call im Update.
        /// </summary>
        [TestMethod]
        public void UpdateWithFinilizer()
        {
            Settings settings = new Settings();
            settings.Level = GetTypeInfo(typeof(TestLevelUpdateDraw));
            sim.Start(settings);

            Assert.AreEqual(SimulationState.Running, sim.State);

            MainState state = null;

            // Erste Runde
            state = sim.NextState();
            Assert.AreEqual(0, state.Round);
            Assert.AreEqual(LevelMode.Running, state.Mode);
            Assert.AreEqual(SimulationState.Running, sim.State);

            // Zweite Runde
            state = sim.NextState();
            Assert.AreEqual(1, state.Round);
            Assert.AreEqual(LevelMode.Running, state.Mode);
            Assert.AreEqual(SimulationState.Running, sim.State);

            // Dritte Runde
            state = sim.NextState();
            Assert.AreEqual(2, state.Round);
            Assert.AreEqual(LevelMode.Draw, state.Mode);
            Assert.AreEqual(SimulationState.Finished, sim.State);
            Assert.IsNull(sim.LastException);
        }

        #endregion

        #region Stop() Tests

        // TODO

        #endregion

        private TypeInfo GetTypeInfo(Type levelType)
        {
            TypeInfo result = new TypeInfo();
            Stream stream = levelType.Assembly.GetFiles()[0];
            result.AssemblyFile = new byte[stream.Length];
            stream.Read(result.AssemblyFile, 0, (int)stream.Length);
            result.AssemblyName = levelType.Assembly.FullName;
            result.TypeName = levelType.FullName;
            return result;
        }
    }

    #region Lokale Levels

    [LevelDescription("{7E1C7E3A-FB15-4E2A-940E-FE570990271E}",
        typeof(Map),
        "Testlevel",
        "Testlevel Description")]
    public class TestLevel : Level
    {
        public TestLevel(ITypeResolver resolver) : base(resolver)
        {
        }
    }

    [LevelDescription]
    public class TestLevelInitException : TestLevel
    {
        protected override void OnInit()
        {
            throw new NotImplementedException();
        }
    }

    [LevelDescription]
    public class TestLevelUpdateException : TestLevel
    {
        protected override void OnUpdate()
        {
            if (Round == 2)
                throw new NotImplementedException();
        }
    }

    [LevelDescription]
    public class TestLevelUpdateDraw: TestLevel
    {
        protected override void OnUpdate()
        {
            if (Round == 2)
                Draw();
        }
    }

    [LevelDescription]
    public class TestLevelFullWorking : TestLevel
    {
    }

    #endregion
}
