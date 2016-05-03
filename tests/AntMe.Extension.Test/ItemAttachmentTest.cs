using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime;
using System.Linq;

namespace AntMe.Extension.Test
{
    [TestClass]
    public class ItemAttachmentTest
    {
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
        /// Registriert ohne Extension Pack
        /// </summary>
        [TestMethod]
        public void AttachNoExtensionPack()
        {
            try
            {
                mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(null, "Debug");
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        /// <summary>
        /// Register ohne Name
        /// </summary>
        [TestMethod]
        public void AttachNoName()
        {
            try
            {
                mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "");
                Assert.Fail();
            }
            catch (ArgumentNullException) { }
        }

        /// <summary>
        /// Hängt einen nicht registrierten Item Type an
        /// </summary>
        [TestMethod]
        public void AttachOnUnregisteredItem()
        {
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");

            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            Assert.AreEqual(1, mapper.ItemAttachments.Count());

            var entry = mapper.ItemAttachments.First();
            Assert.AreEqual(typeof(DebugItemProperty1), entry.AttachmentType);
            Assert.AreEqual(typeof(DebugItem), entry.Type);
        }

        /// <summary>
        /// Hängt ein nicht registriertes Property.
        /// </summary>
        [TestMethod]
        public void AttachUnregisteredProperty()
        {
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            try
            {
                mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");
                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        /// <summary>
        /// Erfolgreicher Attach
        /// </summary>
        [TestMethod]
        public void Attach()
        {
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            Assert.AreEqual(1, mapper.ItemAttachments.Count());

            var entry = mapper.ItemAttachments.First();
            Assert.AreEqual(typeof(DebugItemProperty1), entry.AttachmentType);
            Assert.AreEqual(typeof(DebugItem), entry.Type);
        }

        /// <summary>
        /// Registriert ein Property mit fehlendem Standard-Konstruktor.
        /// </summary>
        [TestMethod]
        public void AttachWrongPropertyConstructor()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registriert ein Property mit fehlendem Standard-Konstruktor aber CreateDelegat.
        /// </summary>
        [TestMethod]
        public void AttachWrongPropertyConstructorWithDelegate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registrierung der selben Kombi
        /// </summary>
        [TestMethod]
        public void AttachCollision()
        {
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            try
            {
                mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug 2");
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }
    }
}
