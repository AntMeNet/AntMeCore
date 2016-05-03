using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Tests für die Item-Klasse.
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class ItemTest
    {
        private Engine Engine;
        private Map Map;
        private Vector2 Pos;
        private DummyItem Item;
        private ITypeResolver resolver;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine(resolver);
            Map = Map.CreateMap(20, 20, true);
            Engine.Init(Map);
            Pos = new Vector2(25, 25);

            Item = new DummyItem(resolver, Pos);
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        #endregion

        /// <summary>
        /// Prüft das ordnungsgemäße Hinzufügen und Entfernen von Items.
        /// </summary>
        [TestMethod]
        public void InsertAndRemove()
        {
            Assert.AreEqual(null, Item.Engine);
            Assert.AreEqual(Index2.Zero, Item.Cell);
            Assert.AreEqual(Pos.X, Item.Position.X);
            Assert.AreEqual(Pos.Y, Item.Position.Y);
            Assert.AreEqual(0, Item.Id);

            int count = 0;
            Item.OnInsertEvent += () =>
            {
                Assert.AreEqual(0, count);
                count++;
            };

            Item.OnRemovedEvent += () =>
            {
                Assert.AreEqual(1, count);
                count++;
            };

            Item.Removed += (i) =>
            {
                Assert.AreEqual(2, count);
                count++;
            };

            Engine.InsertItem(Item);

            Assert.AreEqual(Engine, Item.Engine);
            Assert.AreEqual(new Index2(0, 0), Item.Cell);
            Assert.AreEqual(Pos.X, Item.Position.X);
            Assert.AreEqual(Pos.Y, Item.Position.Y);
            Assert.AreEqual(1, Item.Id);
            Assert.AreEqual(1, count);

            Engine.RemoveItem(Item);

            Assert.AreEqual(null, Item.Engine);
            Assert.AreEqual(Index2.Zero, Item.Cell);
            Assert.AreEqual(Pos.X, Item.Position.X);
            Assert.AreEqual(Pos.Y, Item.Position.Y);
            Assert.AreEqual(0, Item.Id);
            Assert.AreEqual(3, count);
        }

        /// <summary>
        /// Prüft, ob die Remove-Mechanismen auch bei einem MarkToRemove funktionieren.
        /// </summary>
        [TestMethod]
        public void InternalRemove()
        {
            int count = 0;
            Item.OnRemovedEvent += () =>
            {
                Assert.AreEqual(0, count);
                count++;
            };

            Item.Removed += (i) =>
            {
                Assert.AreEqual(1, count);
                count++;
            };

            Item.OnUpdatedEvent += () =>
            {
                Engine.RemoveItem(Item);
            };

            Engine.InsertItem(Item);
            Engine.Update();

            Assert.AreEqual(null, Item.Engine);
            Assert.AreEqual(2, count);
        }

        #region Events

        /// <summary>
        /// Prüft, ob das Item ein Changed-Event bei Änderung der Orientierung wirft.
        /// </summary>
        [TestMethod]
        public void OrientationEvent()
        {
            Item.Orientation = Angle.UpperLeft;

            int count = 0;
            Item.OrientationChanged += (i, v) =>
            {
                Assert.AreEqual(v.Radian, Angle.Right.Radian);
                Assert.AreEqual(0, count);
                count++;
            };

            Item.Orientation = Angle.Right;
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Prüft, ob das Item ein Changed-Event bei Änderung der Position wirft.
        /// </summary>
        [TestMethod]
        public void PositionEvent()
        {
            Item.Position = new Vector3(20, 20, 0);

            int count = 0;
            Item.PositionChanged += (i, v) =>
            {
                Assert.AreEqual(v, new Vector3(30, 30, 0));
                Assert.AreEqual(0, count);
                count++;
            };

            Item.Position = new Vector3(30, 30, 0);
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Prüft, ob das Item ein Changed-Event bei Änderung der Zelle wirft.
        /// </summary>
        [TestMethod]
        public void CellEvent()
        {
            // Initialisieren auf 25/25
            Engine.InsertItem(Item);

            // Initiale Zelle prüfen
            Assert.AreEqual(new Index2(0, 0), Item.Cell);

            // Element in eine andere Zelle verschieben und schauen, ob das aktualisiert wird
            Item.Position = new Vector3(75, 25, 0);
            Assert.AreEqual(new Index2(1, 0), Item.Cell);

            // Event prüfen
            int count = 0;
            Item.CellChanged += (i, v) =>
            {
                Assert.AreEqual(new Index2(1, 1), v);
                Assert.AreEqual(0, count);
                count++;
            };

            Item.Position = new Vector3(75, 75, 0); // Zelle wechseln
            Item.Position = new Vector3(80, 75, 0); // In selber Zelle bewegen
            Assert.AreEqual(1, count);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Prüft, ob sich die Property-Methoden richtig verhalten.
        /// </summary>
        [TestMethod]
        public void InsertProperty()
        {
            Item = new DummyItem(resolver, Pos);
            CountProperties(Item, 0);
            Assert.AreEqual(false, Item.ContainsProperty<DummyProperty>());
            Assert.AreEqual(null, Item.GetProperty<DummyProperty>());

            DummyProperty prop = new DummyProperty(Item);
            Assert.AreEqual(null, prop.Item);

            // Insert Property
            Item.AddProperty(prop);
            CountProperties(Item, 1);
            Assert.AreEqual(Item, prop.Item);
            Assert.AreEqual(true, Item.ContainsProperty<DummyProperty>());
            Assert.AreEqual(prop, Item.GetProperty<DummyProperty>());

            // Insert Property again
            try
            {
                Item.AddProperty(prop);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }

            // insert a Property of the same Type
            DummyProperty prop2 = new DummyProperty(Item);
            try
            {
                Item.AddProperty(prop2);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }


            // Insert Property into running Engine.
            Item = new DummyItem(resolver, Pos);
            Engine.InsertItem(Item);
            prop = new DummyProperty(Item);

            try
            {
                Item.AddProperty(prop);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }
        }

        private void CountProperties(Item item, int expected)
        {
            int count = 0;
            foreach (var property in Item)
                count++;
            Assert.AreEqual(expected, count);
        }

        #endregion

        #region Dummy Item

        internal class DummyItem : Item
        {
            public DummyItem(ITypeResolver resolver, Vector2 position)
                : base(resolver, position, Angle.Right)
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

        #region DummyProperty

        internal class DummyProperty : ItemProperty
        {
            public DummyProperty(Item item) : base(item) { }
        }

        #endregion
    }
}
