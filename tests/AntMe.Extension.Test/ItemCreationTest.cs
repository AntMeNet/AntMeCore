using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime;
using System.Linq;
using System.Collections.Generic;

namespace AntMe.Extension.Test
{
    [TestClass]
    public class ItemCreationTest
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

        #region Basic Item

        /// <summary>
        /// Macht einen Instanz-Test mit den Standard-Konstruktoren
        /// </summary>
        [TestMethod]
        public void CreateInstance()
        {
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");
            Assert.AreEqual(1, mapper.Items.Count());

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            var state = item.GetState();
            Assert.AreEqual(typeof(DebugItemState), state.GetType());

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(typeof(DebugItemInfo), info.GetType());
        }

        /// <summary>
        /// Macht einen Instanz-Test mit einem CreateStateDelegate
        /// </summary>
        [TestMethod]
        public void CreateInstanceStateDelegate()
        {
            int caller = 0;
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return new DebugItemState(i);
            });

            Assert.AreEqual(1, mapper.Items.Count());

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            var state = item.GetState();
            Assert.AreEqual(typeof(DebugItemState), state.GetType());
            Assert.AreEqual(1, caller);
        }

        /// <summary>
        /// Macht einen Instanz-Test mit einem CreateStateDelegate und Null-Result
        /// </summary>
        [TestMethod]
        public void CreateInstanceStateDelegateNull()
        {
            int caller = 0;
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return null;
            });

            Assert.AreEqual(1, mapper.Items.Count());

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            try
            {
                var state = item.GetState();
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Macht einen Instanz-Test mit einem CreateStateDelegate mit falschem Type
        /// </summary>
        [TestMethod]
        public void CreateInstanceStateDelegateWrongType()
        {
            int caller = 0;
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return new DebugItemStateSpecialized(i);
            });

            Assert.AreEqual(1, mapper.Items.Count());

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            try
            {
                var state = item.GetState();
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        /// <summary>
        /// Erstellt eine Instanz ohne vorherige Registrierung.
        /// </summary>
        [TestMethod]
        public void CreateInstanceWithoutRegister()
        {
            try
            {
                DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
                Assert.Fail();
            }
            catch (NotSupportedException) { }
        }

        #endregion

        #region Item Attachments

        /// <summary>
        /// Hängt ein Standard-Property an ein Item an.
        /// </summary>
        [TestMethod]
        public void AttachItemProperty()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemPropertySI<DebugItemProperty1, DebugItemStateProperty1, DebugItemInfoProperty1>(extensionPack, "Debug");

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());

            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);
        }

        /// <summary>
        /// Hängt ein Property mit Create Delegat und Null-Rückgabewert an
        /// </summary>
        [TestMethod]
        public void AttachItemPropertyWithDelegateNull()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemPropertySI<DebugItemProperty1, DebugItemStateProperty1, DebugItemInfoProperty1>(extensionPack, "Debug");

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug", (i) =>
            {
                return null;
            });

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            Assert.AreEqual(0, item.Properties.Count());
        }

        /// <summary>
        /// Hängt Property mit Create Delegate und richtigem Rückgabewert an
        /// </summary>
        [TestMethod]
        public void AttachItemPropertyWithDelegateValue()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemPropertySI<DebugItemProperty1, DebugItemStateProperty1, DebugItemInfoProperty1>(extensionPack, "Debug");

            // Attach Property
            int caller = 0;
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return new DebugItemProperty1(i);
            });

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            Assert.AreEqual(1, caller);

            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);
        }

        /// <summary>
        /// Registriert ein Property auf den Basis-Typen
        /// </summary>
        [TestMethod]
        public void AttachOnBaseType()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemPropertySI<DebugItemProperty1, DebugItemStateProperty1, DebugItemInfoProperty1>(extensionPack, "Debug");

            // Attach Property
            mapper.AttachItemProperty<Item, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());

            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);
        }

        #endregion

        #region Item Properties

        /// <summary>
        /// Testet neues Item mit Property
        /// </summary>
        [TestMethod]
        public void PropertyOnly()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit Property (Create Delegate)
        /// </summary>
        [TestMethod]
        public void PropertyOnlyDelegate()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");

            // Attach Property
            int caller = 0;
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return new DebugItemProperty1(i);
            });

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(2, caller);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit Property (Create Delegate return null)
        /// </summary>
        [TestMethod]
        public void PropertyOnlyDelegateNull()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");

            // Attach Property
            int caller = 0;
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return null;
            });

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(2, caller);

            Assert.AreEqual(0, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit Property (Create Delegate return specialized)
        /// </summary>
        [TestMethod]
        public void PropertyOnlyDelegateSpecialized()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemProperty<DebugItemProperty1>(extensionPack, "Debug");

            // Attach Property
            int caller = 0;
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug", (i) =>
            {
                caller++;
                return new DebugItemProperty1Specialized(i);
            });

            try
            {
                DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
                Assert.Fail();
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(1, caller);
        }

        /// <summary>
        /// Testet neues Item mit State Property
        /// </summary>
        [TestMethod]
        public void PropertyWithState()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStateProperty1>(extensionPack, "Debug");

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(1, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNotNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit State Property und Delegate (Null-Result)
        /// </summary>
        [TestMethod]
        public void PropertyWithStateAndDelegateNull()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            int caller = 0;
            mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStateProperty1>(extensionPack, "Debug", (i, p) =>
            {
                caller++;
                return null;
            });

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);
            Assert.AreEqual(1, caller);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit State Property und Delegate
        /// </summary>
        [TestMethod]
        public void PropertyWithStateAndDelegate()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            int caller = 0;
            mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStateProperty1>(extensionPack, "Debug", (i, p) =>
            {
                caller++;
                return new DebugItemStateProperty1(i, (DebugItemProperty1)p);
            });

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(1, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNotNull(stateProp);
            Assert.AreEqual(1, caller);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit State Property und Delegate (Falscher Type)
        /// </summary>
        [TestMethod]
        public void PropertyWithStateAndDelegateWrongType()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            int caller = 0;
            mapper.RegisterItemPropertyS<DebugItemProperty1, DebugItemStateProperty1>(extensionPack, "Debug", (i, p) =>
            {
                caller++;
                return new DebugItemStateProperty1Specialized(i, (DebugItemProperty1)p);
            });

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            try
            {
                var state = item.GetState();
                Assert.Fail();
            }
            catch (NotSupportedException) { }

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit Info Property
        /// </summary>
        [TestMethod]
        public void PropertyWithInfo()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            mapper.RegisterItemPropertyI<DebugItemProperty1, DebugItemInfoProperty1>(extensionPack, "Debug");

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(1, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNotNull(infoProp);
        }

        /// <summary>
        /// Testet neues Item mit Info Property und Delegate (Null-Result)
        /// </summary>
        [TestMethod]
        public void PropertyWithInfoAndDelegateNull()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            int caller = 0;
            mapper.RegisterItemPropertyI<DebugItemProperty1, DebugItemInfoProperty1>(extensionPack, "Debug", (i, p, o) =>
            {
                caller++;
                return null;
            });

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(0, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNull(infoProp);

            Assert.AreEqual(1, caller);
        }

        /// <summary>
        /// Testet neues Item mit Info Property und Delegate
        /// </summary>
        [TestMethod]
        public void PropertyWithInfoAndDelegate()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            int caller = 0;
            mapper.RegisterItemPropertyI<DebugItemProperty1, DebugItemInfoProperty1>(extensionPack, "Debug", (i, p, o) =>
            {
                caller++;
                return new DebugItemInfoProperty1(i, (DebugItemProperty1)p, o);
            });

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            var info = item.GetItemInfo(item2);
            Assert.AreEqual(1, info.Properties.Count());
            var infoProp = info.GetProperty<DebugItemInfoProperty1>();
            Assert.IsNotNull(infoProp);

            Assert.AreEqual(1, caller);
        }

        /// <summary>
        /// Testet neues Item mit Info Property und Delegate (Falscher Type)
        /// </summary>
        [TestMethod]
        public void PropertyWithInfoAndDelegateWrongType()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            // Register Property
            int caller = 0;
            mapper.RegisterItemPropertyI<DebugItemProperty1, DebugItemInfoProperty1>(extensionPack, "Debug", (i, p, o) =>
            {
                caller++;
                return new DebugItemInfoProperty1Specialized(i, (DebugItemProperty1)p, o);
            });

            // Attach Property
            mapper.AttachItemProperty<DebugItem, DebugItemProperty1>(extensionPack, "Debug");

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Right);
            DebugItem item2 = new DebugItem(mapper, Vector2.Zero, Angle.Right);

            Assert.AreEqual(1, item.Properties.Count());
            var prop = item.GetProperty<DebugItemProperty1>();
            Assert.IsNotNull(prop);

            var state = item.GetState();
            Assert.AreEqual(0, state.Properties.Count());
            var stateProp = state.GetProperty<DebugItemStateProperty1>();
            Assert.IsNull(stateProp);

            try
            {
                var info = item.GetItemInfo(item2);
                Assert.Fail();
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(1, caller);
        }

        #endregion

        #region Item Extender

        /// <summary>
        /// Testet einen Item Extender auf das neue Objekt.
        /// </summary>
        [TestMethod]
        public void ExtenderOnItem()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            List<int> caller = new List<int>();

            // Register Extender
            mapper.RegisterItemExtender<DebugItem>(extensionPack, "Debug", (i) =>
            {
                caller.Add(1);
            }, 10);

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Left);

            Assert.AreEqual(1, caller.Count);
            Assert.AreEqual(1, caller[0]);
        }

        /// <summary>
        /// Testet einen Item Extender auf die Basis-Klasse.
        /// </summary>
        [TestMethod]
        public void ExtenderOnBaseItem()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            List<int> caller = new List<int>();

            // Register Extender
            mapper.RegisterItemExtender<Item>(extensionPack, "Debug", (i) =>
            {
                caller.Add(1);
            }, 10);

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Left);

            Assert.AreEqual(1, caller.Count);
            Assert.AreEqual(1, caller[0]);
        }

        /// <summary>
        /// Testet mehrere Extender auf das lokale und die Basis-Klasse und deren Aufruf-Reihenfolge.
        /// </summary>
        [TestMethod]
        public void ExtenderOrder()
        {
            // Register Item
            mapper.RegisterItem<DebugItem, DebugItemState, DebugItemInfo>(extensionPack, "Debug");

            List<int> caller = new List<int>();

            // Register Extender
            mapper.RegisterItemExtender<DebugItem>(extensionPack, "Debug", (i) =>
            {
                caller.Add(1);
            }, 10);

            mapper.RegisterItemExtender<DebugItem>(extensionPack, "Debug", (i) =>
            {
                caller.Add(2);
            }, 20);

            mapper.RegisterItemExtender<Item>(extensionPack, "Debug", (i) =>
            {
                caller.Add(3);
            }, 5);

            mapper.RegisterItemExtender<Item>(extensionPack, "Debug", (i) =>
            {
                caller.Add(4);
            }, 15);

            DebugItem item = new DebugItem(mapper, Vector2.Zero, Angle.Left);

            Assert.AreEqual(4, caller.Count);
            Assert.AreEqual(3, caller[0]);
            Assert.AreEqual(1, caller[1]);
            Assert.AreEqual(4, caller[2]);
            Assert.AreEqual(2, caller[3]);
        }

        #endregion
    }
}
