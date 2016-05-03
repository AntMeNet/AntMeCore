using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime;
using System.Linq;

namespace AntMe.Extension.Test
{
    [TestClass]
    public class ItemPropertiesTest
    {
        DebugExtensionPack extensionPack = new DebugExtensionPack();
        TypeMapper mapper;

        /*
        - Alle Sorten -> 3 Varianten + mit und ohne Delegaten
        - Register (alle Sorten) mit falschen Parametern (falscher Type)
        - Register (alle Sorten) und erfolgreicher Init
        - Falsche Konstruktoren bei Item-, State- und Info-Property
        */

        [TestInitialize]
        public void Init()
        {
            mapper = new TypeMapper();
        }

        [TestCleanup]
        public void Cleanup()
        {
            mapper = null;
        }

        #region Basis Parameter Tests

        /// <summary>
        /// Registriert ein simples ItemProperty ohne ExtensionPack
        /// </summary>
        [TestMethod]
        public void RegisterItemOnlyNoExtensionPack()
        {
            try
            {
                mapper.RegisterItemProperty<ItemProperty>(null, "Debug");
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        /// <summary>
        /// Registriert ein Property ohne Namen
        /// </summary>
        [TestMethod]
        public void RegisterItemOnlyNoName()
        {
            try
            {
                mapper.RegisterItemProperty<ItemProperty>(extensionPack, "");
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        #endregion

        #region Item Property Registration

        /// <summary>
        /// Registriert die Basisklasse für ItemProperties
        /// </summary>
        [TestMethod]
        public void RegisterItemBaseProperty()
        {
            try
            {
                mapper.RegisterItemProperty<ItemProperty>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Erfolgreiche Registrierung eines Item Properties
        /// </summary>
        [TestMethod]
        public void RegisterItemProperty()
        {
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.ItemProperties.Count());

            var entry = mapper.ItemProperties.First();
            Assert.AreEqual(typeof(DebugItemProperty1), entry.Type);
            Assert.AreEqual(null, entry.StateType);
            Assert.AreEqual(null, entry.InfoType);
        }

        [TestMethod]
        public void RegisterItemWrongConstructors()
        {
            // TODO: Items mit falschen Konstruktoren? Lässt sich das testen
            throw new NotImplementedException();
        }

        /// <summary>
        /// Versuche den selben Type mehrfach zu registrieren
        /// </summary>
        [TestMethod]
        public void RegisterItemDouble()
        {
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");
            try
            {
                mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug 2");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        #endregion

        #region State Property Registration

        /// <summary>
        /// Registriert die Basisklasse für StateProperties
        /// </summary>
        [TestMethod]
        public void RegisterStateBaseProperty()
        {
            try
            {
                mapper.RegisterItemPropertyS<DebugItemProperty1, ItemStateProperty>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Registriert State ohne Konstruktoren
        /// </summary>
        [TestMethod]
        public void RegisterStatePropertyNoConstructors()
        {
            try
            {
                mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStatePropertyNoConstructors>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Registriert State ohne empty Konstruktor
        /// </summary>
        [TestMethod]
        public void RegisterStatePropertyNoEmptyConstructor()
        {
            try
            {
                mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStatePropertyNoEmptyConstructor>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Erfolgreiche Registrierung eines Item Properties
        /// </summary>
        [TestMethod]
        public void RegisterStateProperty()
        {
            mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStateProperty1>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.ItemProperties.Count());

            var entry = mapper.ItemProperties.First();
            Assert.AreEqual(typeof(DebugItemProperty1), entry.Type);
            Assert.AreEqual(typeof(DebugItemStateProperty1), entry.StateType);
            Assert.AreEqual(null, entry.InfoType);
        }

        #endregion

        #region Info Property Registration

        /// <summary>
        /// Registriert die Basisklasse für Info Properties
        /// </summary>
        [TestMethod]
        public void RegisterInfoBaseProperty()
        {
            try
            {
                mapper.RegisterItemPropertyI<DebugItemProperty1, ItemInfoProperty>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Registriert Info ohne Konstruktoren
        /// </summary>
        [TestMethod]
        public void RegisterInfoPropertyNoConstructors()
        {
            try
            {
                mapper.RegisterItemPropertyI<DebugItemProperty1, DebugItemInfoPropertyNoConstructors>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Erfolgreiche Registrierung eines Item Properties
        /// </summary>
        [TestMethod]
        public void RegisterInfoProperty()
        {
            mapper.RegisterItemPropertyI<DebugItemProperty1, DebugItemInfoProperty1>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.ItemProperties.Count());

            var entry = mapper.ItemProperties.First();
            Assert.AreEqual(typeof(DebugItemProperty1), entry.Type);
            Assert.AreEqual(null, entry.StateType);
            Assert.AreEqual(typeof(DebugItemInfoProperty1), entry.InfoType);
        }

        #endregion
    }
}
