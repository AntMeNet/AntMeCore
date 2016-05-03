using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime;

namespace AntMe.Extension.Test
{
    /// <summary>
    /// Testet alles rund um die Engine Extensibility
    /// </summary>
    [TestClass]
    public class EngineExtensionsTest
    {
        // TODO: Weitere Tests mit mehreren Extensions
        // - vererbte Klassen
        // - abstract Klassen
        // - fehlerhaft implementierte Extension? (sofern das geht)

        DebugExtensionPack extensionPack = new DebugExtensionPack();
        DebugEngine engine = new DebugEngine();
        TypeMapper mapper;

        [TestInitialize]
        public void Init()
        {
            mapper = new TypeMapper();
            extensionPack.CallCounter = 0;
            DebugEngineExtension.CreateCalls = 0;
        }

        [TestCleanup]
        public void Cleanup()
        {
            mapper = null;
        }

        /// <summary>
        /// Testet den Register-Aufruf ohne Angabe eines Extension Packs
        /// </summary>
        [TestMethod]
        public void RegisterWithoutExtensionPack()
        {
            try
            {
                mapper.RegisterEngineExtension<DebugEngineExtension>(null, "Debug", 0);
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        /// <summary>
        /// Testet den Register mit dem Versuch die Extension-Basis-Klasse zu registrieren
        /// </summary>
        [TestMethod]
        public void RegisterBaseExtensionClass()
        {
            try
            {
                mapper.RegisterEngineExtension<EngineExtension>(extensionPack, "Debug", 0);
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Testet den Register ohne Angabe eines Extension Namens
        /// </summary>
        [TestMethod]
        public void RegisterWithEmptyName()
        {
            try
            {
                mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, string.Empty, 0);
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        /// <summary>
        /// Testet den Register beim Versuch die selbe Extension nochmal zu registrieren
        /// </summary>
        [TestMethod]
        public void RegisterExtensionTwoTimes()
        {
            mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, "Debug", 0);
            try
            {
                mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, "Debug2", 0);
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Testet ohne eine Engine Instanz
        /// </summary>
        [TestMethod]
        public void InstantiateExtensionWithoutEngine()
        {
            mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, "Debug", 0);

            try
            {
                var extensions = mapper.ResolveEngineExtensions(null);
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        /// <summary>
        /// Instanziiert die Extensions über den Standard-Konstruktor
        /// </summary>
        [TestMethod]
        public void InstantiateExtentionWithoutDelegate()
        {
            mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, "Debug", 0);
            var extensions = mapper.ResolveEngineExtensions(engine);
            Assert.AreEqual(1, extensions.Length);
            Assert.AreEqual(1, DebugEngineExtension.CreateCalls);
            Assert.IsTrue(extensions[0].GetType() == typeof(DebugEngineExtension));
        }

        /// <summary>
        /// Testet mit fehlerhaftem Delegaten
        /// </summary>
        [TestMethod]
        public void InstantiateExtentionWithMessedDelegate()
        {
            mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, "Debug", 0, (e) => { return null; });
            try
            {
                var extensions = mapper.ResolveEngineExtensions(engine);
                Assert.Fail();
            }
            catch (NullReferenceException) { }
        }

        /// <summary>
        /// Testet mit funktionierendem Delegaten
        /// </summary>
        [TestMethod]
        public void InstantiateExtentionWithDelegate()
        {
            mapper.RegisterEngineExtension<DebugEngineExtension>(extensionPack, "Debug", 0, (e) => { return new DebugEngineExtension(e); });
            var extensions = mapper.ResolveEngineExtensions(engine);
            Assert.AreEqual(1, extensions.Length);
            Assert.AreEqual(1, DebugEngineExtension.CreateCalls);
            Assert.IsTrue(extensions[0].GetType() == typeof(DebugEngineExtension));
        }
    }
}
