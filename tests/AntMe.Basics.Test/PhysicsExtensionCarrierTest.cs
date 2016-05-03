using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Core.Extensions;
using AntMe.Core.Debug;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet das Verhalten eines Item-Clusters
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class PhysicsExtensionCarrierTest
    {
        private Engine Engine;
        private Map Map;
        private DebugCarrierItem Item1;
        private WalkingProperty Moving1;
        private CollidableProperty Collidable1;
        private CarrierProperty Carrier1;
        private DebugCarrierItem Item2;
        private WalkingProperty Moving2;
        private CollidableProperty Collidable2;
        private CarrierProperty Carrier2;
        private DebugPortableItem Item3;
        private WalkingProperty Moving3;
        private CollidableProperty Collidable3;
        private PortableProperty Portable3;
        private DebugPortableItem Item4;
        private WalkingProperty Moving4;
        private CollidableProperty Collidable4;
        private PortableProperty Portable4;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine();
            Engine.RegisterExtension(new PhysicsExtension(), 1);
        }

        private void InitItem(Vector2 carrier1, Vector2 portable1, Vector2? carrier2, Vector2? portable2)
        {
            Item1 = new DebugCarrierItem(carrier1);
            Moving1 = Item1.GetProperty<WalkingProperty>();
            Collidable1 = Item1.GetProperty<CollidableProperty>();
            Carrier1 = Item1.GetProperty<CarrierProperty>();

            Item3 = new DebugPortableItem(portable1);
            Moving3 = Item3.GetProperty<WalkingProperty>();
            Collidable3 = Item3.GetProperty<CollidableProperty>();
            Portable3 = Item3.GetProperty<PortableProperty>();


            if (carrier2.HasValue)
            {
                Item2 = new DebugCarrierItem(carrier2.Value);
                Moving2 = Item2.GetProperty<WalkingProperty>();
                Collidable2 = Item2.GetProperty<CollidableProperty>();
                Carrier2 = Item2.GetProperty<CarrierProperty>();
            }

            if (portable2.HasValue)
            {
                Item4 = new DebugPortableItem(portable2.Value);
                Moving4 = Item4.GetProperty<WalkingProperty>();
                Collidable4 = Item4.GetProperty<CollidableProperty>();
                Portable4 = Item4.GetProperty<PortableProperty>();
            }
        }

        private void InitFlat(bool blockBorder)
        {
            Map = Map.CreateMap(MapPreset.Small, true);
            Engine.Init(Map);

            Engine.InsertItem(Item1);
            if (Item2 != null)
                Engine.InsertItem(Item2);
            if (Item3 != null)
                Engine.InsertItem(Item3);
            if (Item4 != null)
                Engine.InsertItem(Item4);
        }

        [TestCleanup]
        public void CleanupEngine()
        {
            Item1 = null;
            Moving1 = null;
            Collidable1 = null;
            Carrier1 = null;
            Item2 = null;
            Moving2 = null;
            Collidable2 = null;
            Carrier2 = null;
            Item3 = null;
            Moving3 = null;
            Collidable3 = null;
            Portable3 = null;
            Item4 = null;
            Moving4 = null;
            Collidable4 = null;
            Portable4 = null;
            Map = null;
            Engine = null;
        }

        #endregion

        #region Property Events (done)

        /// <summary>
        /// Prüft das Event zur Benachrichtung der Landungsänderung.
        /// </summary>
        [TestMethod]
        public void CheckCarrierLoadChanged()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 50), null, new Vector2(150, 150));
            InitFlat(true);
            Portable3.PortableRadius = 1000f;
            Portable4.PortableRadius = 1000f;

            int triggered = 0;
            PortableProperty expected1 = null;
            PortableProperty expected2 = null;
            bool result = false;

            Carrier1.OnCarrierLoadChanged += (i, v) =>
            {
                if (triggered == 0)
                    Assert.AreEqual(expected1, v);
                else if (triggered == 1)
                    Assert.AreEqual(expected2, v);
                triggered++;
            };

            // Item 1 aufnehmen
            expected1 = Portable3;
            result = Carrier1.Carry(Portable3);
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Item 1 wieder fallen lassen
            expected1 = null;
            Carrier1.Drop();
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Item 1 wieder aufnehmen
            expected1 = Portable3;
            result = Carrier1.Carry(Portable3);
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Item 2 aufnehmen, solange 1 noch getragen
            expected1 = null;
            expected2 = Portable4;
            result = Carrier1.Carry(Portable4);
            Assert.AreEqual(true, result);
            Assert.AreEqual(2, triggered);
            triggered = 0;
        }

        /// <summary>
        /// Prüft das Event zur Benachrichtung der Stärkenänderung.
        /// </summary>
        [TestMethod]
        public void CheckCarrierStrengthChanged()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 50), null, null);

            int triggered = 0;
            float expected = 5f;

            Carrier1.OnCarrierStrengthChanged += (i, v) =>
            {
                Assert.AreEqual(expected, v, 0.001f);
                triggered++;
            };

            // Normale Zahl eingeben
            expected = 10f;
            Carrier1.CarrierStrength = 10f;
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Große Zahl einfügen
            expected = float.MaxValue;
            Carrier1.CarrierStrength = float.MaxValue;
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Negative Zahl eingeben
            expected = 0f;
            Carrier1.CarrierStrength = -10f;
            Assert.AreEqual(1, triggered);
            triggered = 0;
        }

        /// <summary>
        /// Prüft das Event zur Benachrichtung des Trageradius.
        /// </summary>
        [TestMethod]
        public void CheckPortableRadiusChanged()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 50), null, null);

            int triggered = 0;
            float expected = 5f;

            Portable3.OnPortableRadiusChanged += (i, v) =>
            {
                Assert.AreEqual(expected, v, 0.001f);
                triggered++;
            };

            // Normale Zahl eingeben
            expected = 10f;
            Portable3.PortableRadius = 10f;
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Große Zahl einfügen
            expected = float.MaxValue;
            Portable3.PortableRadius = float.MaxValue;
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Negative Zahl eingeben
            expected = 0f;
            Portable3.PortableRadius = -10f;
            Assert.AreEqual(1, triggered);
            triggered = 0;
        }

        /// <summary>
        /// Prüft das Event zur Benachrichtung der Masseänderung.
        /// </summary>
        [TestMethod]
        public void CheckPortableWeightChanged()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 50), null, null);

            int triggered = 0;
            float expected = 5f;

            Portable3.OnPortableWeightChanged += (i, v) =>
            {
                Assert.AreEqual(expected, v, 0.001f);
                triggered++;
            };

            // Normale Zahl eingeben
            expected = 10f;
            Portable3.PortableWeight = 10f;
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Große Zahl einfügen
            expected = float.MaxValue;
            Portable3.PortableWeight = float.MaxValue;
            Assert.AreEqual(1, triggered);
            triggered = 0;

            // Negative Zahl eingeben
            expected = 0f;
            Portable3.PortableWeight = -10f;
            Assert.AreEqual(1, triggered);
            triggered = 0;
        }

        /// <summary>
        /// Prüft das Event zur Benachrichtung neuer oder verlorener Träger.
        /// </summary>
        [TestMethod]
        public void CheckPortableItemAddedAndRemoved()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 50), new Vector2(150, 150), null);
            InitFlat(true);
            Portable3.PortableRadius = 1000f;

            int triggeredNew = 0;
            int triggeredLost = 0;
            CarrierProperty expected = null;
            bool result = false;

            Portable3.OnNewCarrierItem += (v) =>
            {
                Assert.AreEqual(expected, v);
                triggeredNew++;
            };

            Portable3.OnLostCarrierItem += (v) =>
            {
                Assert.AreEqual(expected, v);
                triggeredLost++;
            };

            // Item1 aufnehmen
            expected = Carrier1;
            result = Carrier1.Carry(Portable3);
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, triggeredNew);
            Assert.AreEqual(0, triggeredLost);
            triggeredNew = 0;
            triggeredLost = 0;
            Assert.AreEqual(1, Portable3.CarrierItems.Count);
            Assert.AreEqual(true, Portable3.CarrierItems.Contains(Carrier1));
            Assert.AreEqual(false, Portable3.CarrierItems.Contains(Carrier2));

            // Item1 fallen lassen
            expected = Carrier1;
            Carrier1.Drop();
            Assert.AreEqual(0, triggeredNew);
            Assert.AreEqual(1, triggeredLost);
            triggeredNew = 0;
            triggeredLost = 0;
            Assert.AreEqual(0, Portable3.CarrierItems.Count);
            Assert.AreEqual(false, Portable3.CarrierItems.Contains(Carrier1));
            Assert.AreEqual(false, Portable3.CarrierItems.Contains(Carrier2));

            // Item2 aufnehmen
            expected = Carrier2;
            result = Carrier2.Carry(Portable3);
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, triggeredNew);
            Assert.AreEqual(0, triggeredLost);
            triggeredNew = 0;
            triggeredLost = 0;
            Assert.AreEqual(1, Portable3.CarrierItems.Count);
            Assert.AreEqual(false, Portable3.CarrierItems.Contains(Carrier1));
            Assert.AreEqual(true, Portable3.CarrierItems.Contains(Carrier2));

            // Item1 aufnehmen
            expected = Carrier1;
            result = Carrier1.Carry(Portable3);
            Assert.AreEqual(true, result);
            Assert.AreEqual(1, triggeredNew);
            Assert.AreEqual(0, triggeredLost);
            triggeredNew = 0;
            triggeredLost = 0;
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(true, Portable3.CarrierItems.Contains(Carrier1));
            Assert.AreEqual(true, Portable3.CarrierItems.Contains(Carrier2));

            // Item2 fallen lassen
            expected = Carrier2;
            Carrier2.Drop();
            Assert.AreEqual(0, triggeredNew);
            Assert.AreEqual(1, triggeredLost);
            triggeredNew = 0;
            triggeredLost = 0;
            Assert.AreEqual(1, Portable3.CarrierItems.Count);
            Assert.AreEqual(true, Portable3.CarrierItems.Contains(Carrier1));
            Assert.AreEqual(false, Portable3.CarrierItems.Contains(Carrier2));
        }

        #endregion

        #region Movement

        /// <summary>
        /// Überprüft die Fortbewegung des Clusters mit unterschiedlichen 
        /// Stärke- & Gewichtsverhältnissen.
        /// </summary>
        [TestMethod]
        public void MovementSingleItem()
        {
            InitItem(new Vector2(100, 100), new Vector2(104, 100), null, null);
            InitFlat(true);

            Carrier1.Carry(Portable3);

            // Move Left (fast)
            Move(Angle.Left, 20f, null, null);

            // Move Up (slow)
            Move(Angle.Up, 3f, null, null);

            // Move Right (stop)
            Move(Angle.Right, 0f, null, null);

            // Change Weight
            Portable3.PortableWeight = 1000;
            Move(Angle.Left, 20f, null, null);

            // Change Strength
            Carrier1.CarrierStrength = 1000;
            Move(Angle.Right, 20f, null, null);
        }

        

        /// <summary>
        /// Überprüft die Fortbewegung des Clusters mit mehreren Items und 
        /// der selben Bewegungsrichtung.
        /// </summary>
        [TestMethod]
        public void MovementMultipleItemsSameDirection()
        {
            InitItem(new Vector2(100, 100), new Vector2(104, 100), new Vector2(96, 100), null);
            InitFlat(true);

            Carrier1.Carry(Portable3);
            Carrier2.Carry(Portable3);

            // Move Left (fast)
            Move(Angle.Left, 20f, Angle.Left, 20f);

            // Move Up (slow)
            Move(Angle.Up, 3f, Angle.Up, 3f);

            // Move Right (stop)
            Move(Angle.Right, 0f, Angle.Right, 0f);

            // Change Weight
            Portable3.PortableWeight = 1000;
            Move(Angle.Left, 20f, Angle.Left, 20f);

            // Change Strength
            Carrier1.CarrierStrength = 1000;
            Carrier2.CarrierStrength = 1000;
            Move(Angle.Right, 20f, Angle.Right, 20f);
        }

        /// <summary>
        /// Überprüft die Fortbewegung des Clusters mit mehreren Trägern 
        /// und unterschiedlichen Fortbewegungsrichtungen.
        /// </summary>
        [TestMethod]
        public void MovementMultipleItemsDifferentDirections()
        {
            InitItem(new Vector2(100, 100), new Vector2(104, 100), new Vector2(96, 100), null);
            InitFlat(true);

            Carrier1.Carry(Portable3);
            Carrier2.Carry(Portable3);

            // Move Left (fast)
            Move(Angle.Left, 20f, Angle.Right, 20f);

            // Move Up (slow)
            Move(Angle.Up, 3f, Angle.Right, 3f);

            // Move Right (stop)
            Move(Angle.Right, 0f, Angle.UpperRight, 0f);

            // Change Weight
            Portable3.PortableWeight = 1000;
            Move(Angle.Left, 20f, Angle.Left, 20f);

            // Change Strength
            Carrier1.CarrierStrength = 1000;
            Carrier2.CarrierStrength = 1;
            Move(Angle.Right, 20f, Angle.Right, 20f);
        }

        #endregion

        #region Border Behavior

        /// <summary>
        /// Prüft das Verhalten eines Clusters der mit einem Carrier zuerst 
        /// an einen blockierenden Rand stößt.
        /// </summary>
        [TestMethod]
        public void ClusterItemBorderCollisionBlock()
        {
            InitItem(new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), null);
            Portable3.PortableRadius = 10;
            Collidable3.CollisionRadius = 3;
            InitFlat(true);

            // Linke Kante
            Item1.Position = new Vector3(2, 100, 0);
            Item2.Position = new Vector3(10, 105, 0);
            Item3.Position = new Vector3(4, 105, 0);

            Assert.AreEqual(true, Carrier1.Carry(Portable3));
            Assert.AreEqual(true, Carrier2.Carry(Portable3));

            Moving1.Direction = Angle.Left;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Left;
            Moving2.Speed = 20f;
            
            Engine.Update();

            Assert.AreEqual(2, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(8, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(3, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
            Assert.AreEqual(Portable3, Carrier2.CarrierLoad);

            // Rechte Kante
            Item1.Position = new Vector3(198, 100, 0);
            Item2.Position = new Vector3(190, 105, 0);
            Item3.Position = new Vector3(196, 105, 0);

            Moving1.Direction = Angle.Right;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Right;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(198, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(192, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(197, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);

            // Obere Kante
            Item1.Position = new Vector3(100, 2, 0);
            Item2.Position = new Vector3(105, 10, 0);
            Item3.Position = new Vector3(105, 4, 0);

            Moving1.Direction = Angle.Up;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Up;
            Moving2.Speed = 20f;
            Engine.Update();

            Assert.AreEqual(100, Item1.Position.X, 0.001f);
            Assert.AreEqual(2, Item1.Position.Y, 0.001f);
            Assert.AreEqual(105, Item2.Position.X, 0.001f);
            Assert.AreEqual(8, Item2.Position.Y, 0.001f);
            Assert.AreEqual(105, Item3.Position.X, 0.001f);
            Assert.AreEqual(3, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
        }

        /// <summary>
        /// Prüft das Verhalten eines Clusters der mit einem Carrier zuerst 
        /// vom Rand fällt.
        /// </summary>
        [TestMethod]
        public void ClusterItemBorderCollisionDrop()
        {
            InitItem(new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), null);
            Portable3.PortableRadius = 10;
            Collidable3.CollisionRadius = 3;
            InitFlat(false);

            // Linke Kante
            Item1.Position = new Vector3(2, 100, 0);
            Item2.Position = new Vector3(10, 105, 0);
            Item3.Position = new Vector3(4, 105, 0);

            Assert.AreEqual(true, Carrier1.Carry(Portable3));
            Assert.AreEqual(true, Carrier2.Carry(Portable3));

            Moving1.Direction = Angle.Left;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Left;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(2, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(8, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(3, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
            Assert.AreEqual(Portable3, Carrier2.CarrierLoad);

            // Rechte Kante
            Item1.Position = new Vector3(198, 100, 0);
            Item2.Position = new Vector3(190, 105, 0);
            Item3.Position = new Vector3(196, 105, 0);

            Moving1.Direction = Angle.Right;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Right;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(198, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(192, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(197, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);

            // Obere Kante
            Item1.Position = new Vector3(100, 2, 0);
            Item2.Position = new Vector3(105, 10, 0);
            Item3.Position = new Vector3(105, 4, 0);

            Moving1.Direction = Angle.Up;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Up;
            Moving2.Speed = 20f;
            Engine.Update();

            Assert.AreEqual(100, Item1.Position.X, 0.001f);
            Assert.AreEqual(2, Item1.Position.Y, 0.001f);
            Assert.AreEqual(105, Item2.Position.X, 0.001f);
            Assert.AreEqual(8, Item2.Position.Y, 0.001f);
            Assert.AreEqual(105, Item3.Position.X, 0.001f);
            Assert.AreEqual(3, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
        }

        /// <summary>
        /// Prüft das Verhalten eines Clusters der mit dem Portable zuerst 
        /// an einen blockierenden Rand stößt.
        /// </summary>
        [TestMethod]
        public void ClusterPortableBorderCollisionBlock()
        {
            InitItem(new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), null);
            Portable3.PortableRadius = 10;
            Collidable3.CollisionRadius = 3;
            InitFlat(true);

            // Linke Kante
            Item1.Position = new Vector3(4, 100, 0);
            Item2.Position = new Vector3(10, 105, 0);
            Item3.Position = new Vector3(4, 105, 0);

            Assert.AreEqual(true, Carrier1.Carry(Portable3));
            Assert.AreEqual(true, Carrier2.Carry(Portable3));

            Moving1.Direction = Angle.Left;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Left;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(2, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(8, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(3, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
            Assert.AreEqual(Portable3, Carrier2.CarrierLoad);

            // Rechte Kante
            Item1.Position = new Vector3(196, 100, 0);
            Item2.Position = new Vector3(190, 105, 0);
            Item3.Position = new Vector3(196, 105, 0);

            Moving1.Direction = Angle.Right;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Right;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(198, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(192, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(197, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);

            // Obere Kante
            Item1.Position = new Vector3(100, 4, 0);
            Item2.Position = new Vector3(105, 10, 0);
            Item3.Position = new Vector3(105, 4, 0);

            Moving1.Direction = Angle.Up;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Up;
            Moving2.Speed = 20f;
            Engine.Update();

            Assert.AreEqual(100, Item1.Position.X, 0.001f);
            Assert.AreEqual(2, Item1.Position.Y, 0.001f);
            Assert.AreEqual(105, Item2.Position.X, 0.001f);
            Assert.AreEqual(8, Item2.Position.Y, 0.001f);
            Assert.AreEqual(105, Item3.Position.X, 0.001f);
            Assert.AreEqual(3, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
        }

        /// <summary>
        /// Prüft das Verhalten eines Clusters der mit dem Portable zuerst 
        /// vom Rand fällt.
        /// </summary>
        [TestMethod]
        public void ClusterPortableBorderCollisionDrop()
        {
            InitItem(new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), null);
            Portable3.PortableRadius = 10;
            Collidable3.CollisionRadius = 3;
            InitFlat(false);

            // Linke Kante
            Item1.Position = new Vector3(4, 100, 0);
            Item2.Position = new Vector3(10, 105, 0);
            Item3.Position = new Vector3(4, 105, 0);

            Assert.AreEqual(true, Carrier1.Carry(Portable3));
            Assert.AreEqual(true, Carrier2.Carry(Portable3));

            Moving1.Direction = Angle.Left;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Left;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(2, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(8, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(3, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
            Assert.AreEqual(Portable3, Carrier2.CarrierLoad);

            // Rechte Kante
            Item1.Position = new Vector3(196, 100, 0);
            Item2.Position = new Vector3(190, 105, 0);
            Item3.Position = new Vector3(196, 105, 0);

            Moving1.Direction = Angle.Right;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Right;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(198, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(192, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(197, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);

            // Obere Kante
            Item1.Position = new Vector3(100, 4, 0);
            Item2.Position = new Vector3(105, 10, 0);
            Item3.Position = new Vector3(105, 4, 0);

            Moving1.Direction = Angle.Up;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Up;
            Moving2.Speed = 20f;
            Engine.Update();

            Assert.AreEqual(100, Item1.Position.X, 0.001f);
            Assert.AreEqual(2, Item1.Position.Y, 0.001f);
            Assert.AreEqual(105, Item2.Position.X, 0.001f);
            Assert.AreEqual(8, Item2.Position.Y, 0.001f);
            Assert.AreEqual(105, Item3.Position.X, 0.001f);
            Assert.AreEqual(3, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
        }

        #endregion

        #region Cell Behavior

        /// <summary>
        /// Überprüft das Verhalten bei Wall Collision mit niedrigerem 
        /// Grenzwert und einem Item als Erstkontakt.
        /// </summary>
        [TestMethod]
        public void ClusterItemWallCollision()
        {
            InitItem(new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), null);
            Portable3.PortableRadius = 10;
            Collidable3.CollisionRadius = 3;
            InitFlat(true);

            // Erste Reihe hoch setzen
            Index2 cells = Map.GetCellCount();
            var w = cells.X - 1;
            for (int y = 0; y < cells.Y; y++)
            {
                Map.Tiles[0, y].Height = TileHeight.High;
                Map.Tiles[w, y].Height = TileHeight.High;
            }
            for (int x = 0; x < cells.X; x++)
                Map.Tiles[x, 0].Height = TileHeight.High;

            // Linke Kante
            Item1.Position = new Vector3(22, 100, 0);
            Item2.Position = new Vector3(30, 105, 0);
            Item3.Position = new Vector3(24, 105, 0);

            Assert.AreEqual(true, Carrier1.Carry(Portable3));
            Assert.AreEqual(true, Carrier2.Carry(Portable3));

            Moving1.Direction = Angle.Left;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Left;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(22, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(28, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(23, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
            Assert.AreEqual(Portable3, Carrier2.CarrierLoad);

            // Rechte Kante
            Item1.Position = new Vector3(178, 100, 0);
            Item2.Position = new Vector3(170, 105, 0);
            Item3.Position = new Vector3(176, 105, 0);

            Moving1.Direction = Angle.Right;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Right;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(178, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(172, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(177, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);

            // Obere Kante
            Item1.Position = new Vector3(100, 22, 0);
            Item2.Position = new Vector3(105, 30, 0);
            Item3.Position = new Vector3(105, 24, 0);

            Moving1.Direction = Angle.Up;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Up;
            Moving2.Speed = 20f;
            Engine.Update();

            Assert.AreEqual(100, Item1.Position.X, 0.001f);
            Assert.AreEqual(22, Item1.Position.Y, 0.001f);
            Assert.AreEqual(105, Item2.Position.X, 0.001f);
            Assert.AreEqual(28, Item2.Position.Y, 0.001f);
            Assert.AreEqual(105, Item3.Position.X, 0.001f);
            Assert.AreEqual(23, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
        }

        /// <summary>
        /// Überprüft das Verhalten bei Wall Collision mit niedrigerem 
        /// Grenzwert und dem Portable als Erstkontakt.
        /// </summary>
        [TestMethod]
        public void ClusterPortableWallCollision()
        {
            InitItem(new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), null);
            Portable3.PortableRadius = 10;
            Collidable3.CollisionRadius = 3;
            InitFlat(true);

            // Erste Reihe hoch setzen
            Index2 cells = Map.GetCellCount();
            var w = cells.X - 1;
            for (int y = 0; y < cells.Y; y++)
            {
                Map.Tiles[0, y].Height = TileHeight.High;
                Map.Tiles[w, y].Height = TileHeight.High;
            }
            for (int x = 0; x < cells.X; x++)
                Map.Tiles[x, 0].Height = TileHeight.High;

            // Linke Kante
            Item1.Position = new Vector3(24, 100, 0);
            Item2.Position = new Vector3(30, 105, 0);
            Item3.Position = new Vector3(24, 105, 0);

            Assert.AreEqual(true, Carrier1.Carry(Portable3));
            Assert.AreEqual(true, Carrier2.Carry(Portable3));

            Moving1.Direction = Angle.Left;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Left;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(22, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(28, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(23, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
            Assert.AreEqual(Portable3, Carrier2.CarrierLoad);

            // Rechte Kante
            Item1.Position = new Vector3(176, 100, 0);
            Item2.Position = new Vector3(170, 105, 0);
            Item3.Position = new Vector3(176, 105, 0);

            Moving1.Direction = Angle.Right;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Right;
            Moving2.Speed = 20f;

            Engine.Update();

            Assert.AreEqual(178, Item1.Position.X, 0.001f);
            Assert.AreEqual(100, Item1.Position.Y, 0.001f);
            Assert.AreEqual(172, Item2.Position.X, 0.001f);
            Assert.AreEqual(105, Item2.Position.Y, 0.001f);
            Assert.AreEqual(177, Item3.Position.X, 0.001f);
            Assert.AreEqual(105, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);

            // Obere Kante
            Item1.Position = new Vector3(100, 24, 0);
            Item2.Position = new Vector3(105, 30, 0);
            Item3.Position = new Vector3(105, 24, 0);

            Moving1.Direction = Angle.Up;
            Moving1.Speed = 20f;
            Moving2.Direction = Angle.Up;
            Moving2.Speed = 20f;
            Engine.Update();

            Assert.AreEqual(100, Item1.Position.X, 0.001f);
            Assert.AreEqual(22, Item1.Position.Y, 0.001f);
            Assert.AreEqual(105, Item2.Position.X, 0.001f);
            Assert.AreEqual(28, Item2.Position.Y, 0.001f);
            Assert.AreEqual(105, Item3.Position.X, 0.001f);
            Assert.AreEqual(23, Item3.Position.Y, 0.001f);
            Assert.AreEqual(2, Portable3.CarrierItems.Count);
            Assert.AreEqual(Portable3, Carrier1.CarrierLoad);
        }

        #endregion

        #region Helper

        private void Move(Angle direction1, float speed1, Angle? direction2, float? speed2)
        {
            Moving1.Direction = direction1;
            Moving1.Speed = speed1;

            if (direction2.HasValue)
                Moving2.Direction = direction2.Value;
            if (speed2.HasValue)
                Moving2.Speed = speed2.Value;

            Vector3 pos1 = Item1.Position;
            Vector3 pos2 = (direction2 != null ? Item2.Position : Vector3.Zero);
            Vector3 pos3 = Item3.Position;

            bool second = direction2.HasValue;
            Vector3 velocity = Vector3.Zero;
            float strenght = 0;
            velocity += Vector3.FromAngleXY(Moving1.Direction) * Math.Min(Moving1.Speed, Moving1.MaximumSpeed);
            strenght += Carrier1.CarrierStrength;
            velocity += Vector3.FromAngleXY(Moving3.Direction) * Math.Min(Moving3.Speed, Moving3.MaximumSpeed);
            if (direction2 != null)
            {
                velocity += Vector3.FromAngleXY(Moving2.Direction) * Math.Min(Moving2.Speed, Moving2.MaximumSpeed);
                strenght += Carrier2.CarrierStrength;
            }

            // Neue Positionen berechnen
            float factor = Portable3.PortableWeight > 0 ? strenght / Portable3.PortableWeight : 1f;
            factor = Math.Min(1f, Math.Max(0f, factor));

            velocity = (velocity / (second ? 3 : 2)) * factor;
            pos1 += velocity;
            pos2 += velocity;
            pos3 += velocity;

            Engine.Update();

            Assert.AreEqual(pos1.X, Item1.Position.X, 0.001f);
            Assert.AreEqual(pos1.Y, Item1.Position.Y, 0.001f);
            if (direction2 != null)
            {
                Assert.AreEqual(pos2.X, Item2.Position.X, 0.001f);
                Assert.AreEqual(pos2.Y, Item2.Position.Y, 0.001f);
            }
            Assert.AreEqual(pos3.X, Item3.Position.X, 0.001f);
            Assert.AreEqual(pos3.Y, Item3.Position.Y, 0.001f);
        }

        #endregion
    }
}
