using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime;
using System.Linq;

namespace AntMe.Extension.Test
{
    /// <summary>
    /// Testet alles rund um die Erweiterbarkeit von Items
    /// </summary>
    [TestClass]
    public class ItemTest
    {
        // TODO
        // Delegaten testen
        // Aufrufhäufigkeit überprüfen (State pro Item, Info pro Item/Item-Kombi)
        // State und Info mit fehlenden Konstruktoren

        DebugExtensionPack extensionPack = new DebugExtensionPack();
        TypeMapper mapper;

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

        /// <summary>
        /// Registriert ein Item ohne Extension
        /// </summary>
        [TestMethod]
        public void RegisterItemWithoutEngine()
        {
            try
            {
                mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(null, "Debug");
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Registriert ein Item ohne Namen
        /// </summary>
        [TestMethod]
        public void RegisterItemWithoutName()
        {
            try
            {
                mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, string.Empty);
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Registriert die Basis-Klasse von Item
        /// </summary>
        [TestMethod]
        public void RegisterBaseItemType()
        {
            try
            {
                mapper.RegisterItem<Item, DebugItemState, DebugItemInfo>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Registriert das Item mit dem Basis-State
        /// </summary>
        [TestMethod]
        public void RegisterStateBaseType()
        {
            mapper.RegisterItem<DebugItem, ItemState, DebugItemInfo>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.Items.Count());
        }

        /// <summary>
        /// Registriert das Item mit der Basis-Info
        /// </summary>
        [TestMethod]
        public void RegisterInfoBaseType()
        {
            mapper.RegisterItem<DebugItem, DebugItemState, ItemInfo>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.Items.Count());
        }

        /// <summary>
        /// Registriert vollständig
        /// </summary>
        [TestMethod]
        public void Register()
        {
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.Items.Count());
        }

        /// <summary>
        /// Versucht den selben Typen zwei mal zu Registrieren
        /// </summary>
        [TestMethod]
        public void RegisterDouble()
        {
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");
            try
            {
                mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug 2");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
            Assert.AreEqual(1, mapper.Items.Count());
        }
    }
}
