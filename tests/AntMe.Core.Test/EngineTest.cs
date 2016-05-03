using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Prüft, ob die Engine-Klasse richtig funktioniert und richtig auf 
    /// Aufrufe um falschen State reagiert.
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class EngineTest
    {
        private Engine _engine;
        private Map _map;
        private ITypeResolver resolver;

        #region Init

        [TestInitialize]
        public void Init()
        {
            _engine = new Engine(resolver);
            _map = Map.CreateMap(20, 20, true);
        }

        [TestCleanup]
        public void Uninit()
        {
            _engine = null;
            _map = null;
        }

        #endregion

        #region Uninit State

        [TestMethod]
        // Test Exception Handling in Uninit-State
        public void MethodCallsInUninitState()
        {
            var item = new DummyItem(resolver, new Vector2(), Angle.Right);

            // Properties der Engine testen
            Assert.AreEqual(_engine.Items.Count(), 0);
            Assert.AreEqual(_engine.Map, null);
            Assert.AreEqual(_engine.Round, -1);
            Assert.AreEqual(_engine.State, EngineState.Uninitialized);

            // Item hinzufügen
            try
            {
                _engine.InsertItem(item);
                Assert.Fail("AddItem should throw Exception");
            }
            catch (NotSupportedException) { }
            catch (Exception ex) { Assert.Fail(ex.Message); }

            // Item entfernen
            try
            {
                _engine.RemoveItem(item);
                Assert.Fail("RemoveItem should throw Exception");
            }
            catch (NotSupportedException) { }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            
            // Item hinzufügen
            try
            {
                _engine.Update();
                Assert.Fail("Update should throw Exception");
            }
            catch (NotSupportedException) { }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [TestMethod]
        // Initialisiert die Engine mit null
        public void InitNullMap()
        {
            try
            {
                _engine.Init(null);
                Assert.Fail("Init should throw Exception");
            }
            catch (ArgumentNullException) 
            {
                Assert.AreEqual(_engine.Items.Count(), 0);
                Assert.AreEqual(_engine.Map, null);
                Assert.AreEqual(_engine.Round, -1);
                Assert.AreEqual(_engine.State, EngineState.Uninitialized);
            }
        }

        [TestMethod]
        // Initialisiert die Engine mit einer kaputten Karte
        public void InitInvalidMap()
        {
            var map = new Map();

            try
            {
                _engine.Init(map);
                Assert.Fail("Init should throw Exception");
            }
            catch (InvalidMapException)
            {
                Assert.AreEqual(_engine.Items.Count(), 0);
                Assert.AreEqual(_engine.Map, null);
                Assert.AreEqual(_engine.Round, -1);
                Assert.AreEqual(_engine.State, EngineState.Uninitialized);
            }
        }

        [TestMethod]
        // Initialisiert die Engine mit einer funktionierenden Karte
        public void InitValidMap()
        {
            _engine.Init(_map);

            Assert.AreEqual(_engine.Items.Count(), 0);
            Assert.AreEqual(_engine.Map, _map);
            Assert.AreEqual(_engine.Round, -1);
            Assert.AreEqual(_engine.State, EngineState.Simulating);
        }

        [TestMethod]
        // Null Extension einfügen
        public void AddNullExtension()
        {
            try
            {
                // _engine.RegisterExtension(null, 0);
                Assert.Fail("RegisterExtension should throw Exception");
            }
            catch (ArgumentNullException)
            {
                Assert.AreEqual(_engine.Items.Count(), 0);
                Assert.AreEqual(_engine.Map, null);
                Assert.AreEqual(_engine.Round, -1);
                Assert.AreEqual(_engine.State, EngineState.Uninitialized);
            }
        }

        #endregion

        #region Ready State

        [TestMethod]
        // Test Exception Handling in Ready-State
        public void MethodCallsInReadyState()
        {
            _engine.Init(_map);

            var item = new DummyItem(resolver, new Vector2(), Angle.Right);

            // Item hinzufügen
            _engine.InsertItem(item);

            // Erneut initialisieren
            try
            {
                _engine.Init(_map);
                Assert.Fail("Init should throw Exception");
            }
            catch (NotSupportedException) { }
            catch (Exception ex) { Assert.Fail(ex.Message); }

            // RegisterExtension
            try
            {
                // _engine.RegisterExtension(null, 0);
                Assert.Fail("RegisterExtension should throw Exception");
            }
            catch (NotSupportedException) { }
            catch (Exception ex) { Assert.Fail(ex.Message); }

            // RemoveItem
            _engine.RemoveItem(item);

            // Update
            _engine.Update();
        }

        [TestMethod]
        // Testet die leere Engine mit 1000 Laufrunden
        public void RunUpdates()
        {
            _engine.Init(_map);

            for (int i = 0; i < 1000; i++)
            {
                _engine.Update();

                Assert.AreEqual(i, _engine.Round);
                Assert.AreEqual(EngineState.Simulating, _engine.State);
            }
        }

        #endregion

        #region Item Management

        [TestMethod]
        // Fügt das selbe Item mehrmals ein
        public void AddSameItemAgain()
        {
            _engine.Init(_map);

            var item = new DummyItem(resolver, new Vector2(), Angle.Right);
            _engine.InsertItem(item);

            try
            {
                _engine.InsertItem(item);
                Assert.Fail("AddItem should throw an Exception");
            }
            catch (InvalidOperationException) { }
        }

        [TestMethod]
        // Fügt Item hinzu, updatet die Engine und entfernt Item wieder
        public void AddUpdateRemoveItem()
        {
            _engine.Init(_map);
            Assert.AreEqual(_engine.Items.Count(), 0);
            Assert.AreEqual(_engine.Map, _map);
            Assert.AreEqual(_engine.Round, -1);
            Assert.AreEqual(_engine.State, EngineState.Simulating);


            var item1 = new DummyItem(resolver, new Vector2(), Angle.Right);
            var item2 = new DummyItem(resolver, new Vector2(), Angle.Right);


            // Phase 1 - Leeres Update
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 0);
            Assert.AreEqual(_engine.Round, 0);

            // Phase 2 - Item1 hinzufügen und Update
            _engine.InsertItem(item1);
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 1);
            Assert.AreEqual(_engine.Round, 1);

            // PHase 3 - Item2 hinzufügen und Update
            _engine.InsertItem(item2);
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 2);
            Assert.AreEqual(_engine.Round, 2);
            
            // Phase 4 - 2 Updates
            _engine.Update();
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 2);
            Assert.AreEqual(_engine.Round, 4);
            
            // Phase 5 - Item 1 entfernen und Update
            _engine.RemoveItem(item1);
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 1);
            Assert.AreEqual(_engine.Round, 5);
            
            // phase 6 - Item 1 hinzufügen und update
            _engine.InsertItem(item1);
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 2);
            Assert.AreEqual(_engine.Round, 6);
            
            // Phase 7 - Item 1 + 2 und update
            _engine.RemoveItem(item1);
            _engine.RemoveItem(item2);
            _engine.Update();
            Assert.AreEqual(_engine.Items.Count(), 0);
            Assert.AreEqual(_engine.Round, 7);
        }

        [TestMethod]
        // Checkt, ob die richtigen Events in der richtigen Reihenfolge aufgerufen werden
        public void CheckEventsForAdding()
        {
            _engine.Init(_map);

            var item = new DummyItem(resolver, new Vector2(), Angle.Right);

            _engine.OnRemoveItem += i => Assert.Fail("Engine RemoveItem shouldn't be called");
            item.OnRemovedEvent += () => Assert.Fail("Item Remove shouldn't be called");
            item.OnUpdatedEvent += () => Assert.Fail("Item Update shouldn't be called");
            item.OnUpdateEvent += () => Assert.Fail("Item Updated shouldn't be called");
            item.Removed += i => Assert.Fail("Item Remove shouldn't be called");

            int count = 0;

            // Erstes Event (Item)
            item.OnInsertEvent += () =>
            {
                Assert.AreEqual(item.Engine, _engine);
                Assert.AreEqual(1, item.Engine.Items.Count());
                Assert.AreEqual(0, count);

                count++;
            };

            // Zweites Event (Engine)
            _engine.OnInsertItem += i => 
            {
                Assert.AreEqual(item, i);
                Assert.AreEqual(1, _engine.Items.Count());
                Assert.AreEqual(1, count);

                count++;
            };

            _engine.InsertItem(item);

            Assert.AreEqual(1, _engine.Items.Count());
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        // Checkt, ob die richtigen Events in der richtigen Reihenfolge aufgerufen werden
        public void CheckEventsForRemove()
        {
            _engine.Init(_map);

            var item = new DummyItem(resolver, new Vector2(), Angle.Right);

            _engine.InsertItem(item);

            _engine.OnInsertItem += i => Assert.Fail("Engine InsertItem shouldn't be called");
            item.OnInsertEvent += () => Assert.Fail("Item Insert shouldn't be called");
            item.OnUpdatedEvent += () => Assert.Fail("Item Update shouldn't be called");
            item.OnUpdateEvent += () => Assert.Fail("Item Updated shouldn't be called");


            int count = 0;

            // Erstes Event der Engine
            _engine.OnRemoveItem += i => 
            {
                Assert.AreEqual(1, _engine.Items.Count());
                Assert.AreEqual(0, count);

                count++;
            };

            // Zweites Event Item intern
            item.OnRemovedEvent += () =>
            {
                Assert.AreEqual(0, _engine.Items.Count());
                Assert.AreEqual(1, count);

                count++;
            };

            // Drittes Event im Item
            item.Removed += i =>
            {
                Assert.AreEqual(0, _engine.Items.Count());
                Assert.AreEqual(2, count);

                count++;
            };

            _engine.RemoveItem(item);

            Assert.AreEqual(0, _engine.Items.Count());
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        // Checkt, ob die richtigen Events in der richtigen Reihenfolge aufgerufen werden
        public void CheckEventsForUpdate()
        {
            _engine.Init(_map);

            var item = new DummyItem(resolver, new Vector2(), Angle.Right);

            _engine.InsertItem(item);

            _engine.OnInsertItem += i => Assert.Fail("Engine InsertItem shouldn't be called");
            _engine.OnRemoveItem += i => Assert.Fail("Engine RemoveItem shouldn't be called");
            item.OnInsertEvent += () => Assert.Fail("Item Insert shouldn't be called");
            item.OnRemovedEvent += () => Assert.Fail("Item Remove shouldn't be called");
            item.Removed += i => Assert.Fail("Item Remove shouldn't be called");

            int count = 0;

            item.OnUpdateEvent += () => 
            {
                Assert.AreEqual(0, count);
                count++;
            };

            item.OnUpdatedEvent += () =>
            {
                Assert.AreEqual(1, count);
                count++;
            };

            _engine.Update();

            Assert.AreEqual(2, count);
        }

        #endregion

        #region Updating State

        [TestMethod]
        // Test Exception Handling in Updating-State
        public void MethodCallsInUpdateState()
        {
            _engine.Init(_map);

            var item1 = new DummyItem(resolver, new Vector2(), Angle.Right);
            var item2 = new DummyItem(resolver, new Vector2(), Angle.Right);
            var item3 = new DummyItem(resolver, new Vector2(), Angle.Right);

            _engine.InsertItem(item1);

            item1.OnUpdateEvent += () => 
            {
                // Item hinzufügen
                Assert.AreEqual(1, _engine.Items.Count());

                // Init
                try
                {
                    _engine.Init(_map);
                    Assert.Fail("Init should throw Exception");
                }
                catch (NotSupportedException) { }
                catch (Exception ex) { Assert.Fail(ex.Message); }

                // Add
                _engine.InsertItem(item2);

                // Item entfernen
                _engine.RemoveItem(item2);

                // Update
                try
                {
                    _engine.Update();
                    Assert.Fail("Update should throw Exception");
                }
                catch (NotSupportedException) { }
                catch (Exception ex) { Assert.Fail(ex.Message); }

                // RegisterExtension
                try
                {
                    // _engine.RegisterExtension(null, 1);
                    Assert.Fail("RegisterExtension should throw Exception");
                }
                catch (NotSupportedException) { }
                catch (Exception ex) { Assert.Fail(ex.Message); }
            };

            item1.OnUpdatedEvent += () => 
            {
                // Init
                try
                {
                    _engine.Init(_map);
                    Assert.Fail("Init should throw Exception");
                }
                catch (NotSupportedException) { }
                catch (Exception ex) { Assert.Fail(ex.Message); }

                // Add Item
                _engine.InsertItem(item3);

                // Item entfernen
                _engine.RemoveItem(item3);

                // Update
                try
                {
                    _engine.Update();
                    Assert.Fail("Update should throw Exception");
                }
                catch (NotSupportedException) { }
                catch (Exception ex) { Assert.Fail(ex.Message); }

                // RegisterExtension
                try
                {
                    // _engine.RegisterExtension(null, 1);
                    Assert.Fail("RegisterExtension should throw Exception");
                }
                catch (NotSupportedException) { }
                catch (Exception ex) { Assert.Fail(ex.Message); }
            };

            _engine.Update();
        }

        #endregion

        #region DummyItem

        internal class DummyItem : Item
        {
            public DummyItem(ITypeResolver resolver, Vector2 position, Angle orientation)
                : base(resolver, position, orientation)
            {
            }

            protected override void OnInsert()
            {
                if (OnInsertEvent != null)
                    OnInsertEvent();
            }

            protected override void OnRemoved()
            {
                if (OnRemovedEvent != null)
                    OnRemovedEvent();
            }

            protected override void OnUpdate()
            {
                if (OnUpdateEvent != null)
                    OnUpdateEvent();
            }

            protected override void OnUpdated()
            {
                if (OnUpdatedEvent != null)
                    OnUpdatedEvent();
            }

            public event EventDelegate OnInsertEvent;
            public event EventDelegate OnRemovedEvent;
            public event EventDelegate OnUpdateEvent;
            public event EventDelegate OnUpdatedEvent;

            public delegate void EventDelegate();
        }

        #endregion
    }
}