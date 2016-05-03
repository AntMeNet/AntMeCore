using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Core.Extensions;
using AntMe.Core.Debug;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet das Verhalten bei Kollisionen
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class PhysicsExtensionCollisionTest
    {
        private Engine Engine;
        private Map Map;
        private DebugWalkingItem Item1;
        private WalkingProperty Moving1;
        private CollidableProperty Collidable1;
        private DebugWalkingItem Item2;
        private WalkingProperty Moving2;
        private CollidableProperty Collidable2;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine();
            Engine.RegisterExtension(new PhysicsExtension(), 1);
        }

        private void InitItem(Vector2 pos1, Vector2? pos2)
        {
            Item1 = new DebugCollisionItem(new Vector2(pos1.X, pos1.Y));
            Moving1 = Item1.GetProperty<WalkingProperty>();
            Collidable1 = Item1.GetProperty<CollidableProperty>();

            if (pos2.HasValue)
            {
                Item2 = new DebugCollisionItem(new Vector2(pos2.Value.X, pos2.Value.Y));
                Moving2 = Item2.GetProperty<WalkingProperty>();
                Collidable2 = Item2.GetProperty<CollidableProperty>();
            }
        }

        private void InitFlat(bool blockBorder)
        {
            Map = Map.CreateMap(MapPreset.Small, true);
            Engine.Init(Map);

            Engine.InsertItem(Item1);
            if (Item2 != null)
                Engine.InsertItem(Item2);
        }

        [TestCleanup]
        public void CleanupEngine()
        {
            Item1 = null;
            Moving1 = null;
            Collidable1 = null;
            Item2 = null;
            Moving2 = null;
            Collidable2 = null;
            Map = null;
            Engine = null;
        }

        #endregion

        #region Events

        /// <summary>
        /// Prüft, ob das Fix Changed Event funktioniert.
        /// </summary>
        [TestMethod]
        public void CollisionPropertyEvents()
        {
            InitItem(new Vector2(100, 100), null);

            bool Fixed = false;
            bool Mass = false;
            bool Radius = false;

            Collidable1.OnCollisionFixedChanged += (i, v) =>
            {
                Fixed = true;
                Assert.AreEqual(true, v);
            };

            Collidable1.OnCollisionMassChanged += (i, v) =>
            {
                Mass = true;
                Assert.AreEqual(7, v);
            };

            Collidable1.OnCollisionRadiusChanged += (i, v) =>
            {
                Radius = true;
                Assert.AreEqual(5, v);
            };

            Collidable1.CollisionFixed = true;
            Collidable1.CollisionMass = 7;
            Collidable1.CollisionRadius = 5;

            Assert.AreEqual(true, Fixed);
            Assert.AreEqual(true, Mass);
            Assert.AreEqual(true, Radius);
        }

        #endregion

        #region Border Bahavior

        private int BorderCount = 0;
        private Compass BorderDirection = Compass.West;

        // Testet das Verhalten am Rand im Block Mode
        [TestMethod]
        public void BorderBlock()
        {
            InitItem(new Vector2(100, 100), null);
            InitFlat(true);

            Moving1.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // Obere Kante
            Move(new Vector2(100, 3), new Vector2(102, 2), Angle.UpperRight);
            CheckBorderEvent(1, Compass.North);
            Move(new Vector2(100, 3), new Vector2(98, 2), Angle.UpperLeft);
            CheckBorderEvent(1, Compass.North);

            // untere Kante
            Move(new Vector2(100, 197), new Vector2(102, 198), Angle.LowerRight);
            CheckBorderEvent(1, Compass.South);
            Move(new Vector2(100, 197), new Vector2(98, 198), Angle.LowerLeft);
            CheckBorderEvent(1, Compass.South);

            // linke Kante
            Move(new Vector2(3, 100), new Vector2(2, 102), Angle.LowerLeft);
            CheckBorderEvent(1, Compass.West);
            Move(new Vector2(3, 100), new Vector2(2, 98), Angle.UpperLeft);
            CheckBorderEvent(1, Compass.West);

            // rechte Kante
            Move(new Vector2(197, 100), new Vector2(198, 102), Angle.LowerRight);
            CheckBorderEvent(1, Compass.East);
            Move(new Vector2(197, 100), new Vector2(198, 98), Angle.UpperRight);
            CheckBorderEvent(1, Compass.East);
        }

        // Testet das Drop Verhalten mit Collisionsradius
        [TestMethod]
        public void BorderDropWest()
        {
            InitItem(new Vector2(100, 100), null);
            InitFlat(false);

            Moving1.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // linke Kante
            Move(new Vector2(1, 100), new Vector2(-1, 102), Angle.LowerLeft);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item1.Id);
        }

        // Testet das Drop Verhalten mit Collisionsradius
        [TestMethod]
        public void BorderDropNorth()
        {
            InitItem(new Vector2(100, 100), null);
            InitFlat(false);

            Moving1.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // Obere Kante
            Move(new Vector2(100, 1), new Vector2(102, -1), Angle.UpperRight);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item1.Id);
        }

        // Testet das Drop Verhalten mit Collisionsradius
        [TestMethod]
        public void BorderDropEast()
        {
            InitItem(new Vector2(100, 100), null);
            InitFlat(false);

            Moving1.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // rechte Kante
            Move(new Vector2(199, 100), new Vector2(201, 102), Angle.LowerRight);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item1.Id);
        }
        
        // Testet das Drop Verhalten mit Collisionsradius
        [TestMethod]
        public void BorderDropSouth()
        {
            InitItem(new Vector2(100, 100), null);
            InitFlat(false);

            Moving1.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // untere Kante
            Move(new Vector2(100, 199), new Vector2(102, 201), Angle.LowerRight);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item1.Id);
        }

        private void CheckBorderEvent(int count)
        {
            Assert.AreEqual(count, BorderCount);
            BorderCount = 0;
        }

        private void CheckBorderEvent(int count, Compass direction)
        {
            CheckBorderEvent(count);
            Assert.AreEqual(direction, BorderDirection);
        }

        #endregion

        #region Cell Behavior

        // Testet das Verhalten beim Zellenwechsel mit zu großer Höhendifferenz
        [TestMethod]
        public void CellBlock()
        {
            InitItem(new Vector2(30, 30), null);
            InitFlat(true);
            Map.Tiles[1, 1].Height = TileHeight.High;

            Moving1.OnHitWall += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            float distance = new Vector2(2, 2).Length();

            // Zellenwechsel (hoch->tief) nach links
            Move(new Vector2(21, 30), new Vector2(21 - distance, 30), Angle.Left);
            CheckBorderEvent(0);

            // Zellenwechsel (hoch->tief) nach rechts
            Move(new Vector2(39, 30), new Vector2(39 + distance, 30), Angle.Right);
            CheckBorderEvent(0);

            // Zellenwechsel (hoch->tief) nach oben
            Move(new Vector2(30, 21), new Vector2(30, 21 - distance), Angle.Up);
            CheckBorderEvent(0);

            // Zellenwechsel (hoch->tief) nach unten
            Move(new Vector2(30, 39), new Vector2(30, 39 + distance), Angle.Down);
            CheckBorderEvent(0);

            // Zellenwechsel (tief->hoch) nach links
            Move(new Vector2(43, 30), new Vector2(42, 30), Angle.Left);
            CheckBorderEvent(1, Compass.West);

            // Zellenwechsel (tief->hoch) nach rechts
            Move(new Vector2(17, 30), new Vector2(18, 30), Angle.Right);
            CheckBorderEvent(1, Compass.East);

            // Zellenwechsel (tief->hoch) nach oben
            Move(new Vector2(30, 43), new Vector2(30, 42), Angle.Up);
            CheckBorderEvent(1, Compass.North);

            // Zellenwechsel (tief->hoch) nach unten
            Move(new Vector2(30, 17), new Vector2(30, 18), Angle.Down);
            CheckBorderEvent(1, Compass.South);
        }

        #endregion

        #region Regular Object vs. Object Collision

        private int collision1 = 0;
        private int collision2 = 0;
        private int collision3 = 0;

        // Testet eine Kollision mit einem Item ohne Collision Prop
        [TestMethod]
        public void MovingVsNonCollision()
        {
            InitItem(new Vector2(50, 100), null);
            Item2 = new DebugWalkingItem(new Vector2(150, 100));
            Moving2 = Item2.GetProperty<WalkingProperty>();
            InitFlat(true);

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            float distance = new Vector2(2, 2).Length();

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(97 + distance, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(103 - distance, 100), Angle.Left);
            CheckCollisionEvents(0, 0, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 97 + distance), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 103 - distance), Angle.Up);
            CheckCollisionEvents(0, 0, 0);
        }

        // Testet die Kollision mit einem fixierten Gegenstand aus 
        // unterschiedlichsten Winkeln
        [TestMethod]
        public void MovingVsFixed()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable2.CollisionFixed = true;

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            float distance = new Vector2(2, 2).Length();

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(103 - distance - 4, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(103 - distance, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 103 - distance - 4), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 103 - distance), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        // Testet die Kollision mit einem bweglichen Gegenstand gleicher Masse 
        // aus unterschiedlichsten Winkeln
        [TestMethod]
        public void MovingVsMovingSameMasses()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(98, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(102, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 98), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 102), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        // Testet die Kollision mit einem beweglichen Gegenstand abweichender 
        // Masse aus unterschiedlichsten Winkeln
        [TestMethod]
        public void MovingVsMovingDifferentMasses()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable1.CollisionMass = 3;
            Collidable2.CollisionMass = 1;

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            float distance = new Vector2(2, 2).Length();
            float diff = 4 - (6 - distance - distance);

            float massSum = Collidable1.CollisionMass + Collidable2.CollisionMass;
            float dist1 = diff * (Collidable2.CollisionMass / massSum);
            float dist2 = diff * (Collidable1.CollisionMass / massSum);

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(97 + distance - dist1, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(103 - distance + dist2, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 97 + distance - dist1), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 103 - distance + dist2), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        private void CheckCollisionEvents(int counter1, int counter2, int counter3)
        {
            Assert.AreEqual(counter1, collision1);
            Assert.AreEqual(counter2, collision2);
            Assert.AreEqual(counter3, collision3);
            collision1 = 0;
            collision2 = 0;
            collision3 = 0;
        }

        #endregion

        #region Multi Collision

        /// <summary>
        /// Kollision von 3 gleichwertigen Items
        /// </summary>
        [TestMethod]
        public void ItemVsItemVsItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kollision von 3 Items, wovon eines fixiert ist
        /// </summary>
        [TestMethod]
        public void ItemVsItemVsFixedItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Testet die Kollision zwischen zwei Items deren Auflösung zur 
        /// Kollision mit dem blockierenden Spielfeldrand führt.
        /// </summary>
        [TestMethod]
        public void ItemVsItemVsBlockBorder()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            // Kollision Links (Item1 blocked)
            Item2.Position = new Vector3(2, 100, 0);
            Moving2.Speed = 0;

            Move(new Vector2(7, 100), new Vector2(6, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);


            // Kollision Rechts (Item1 blocked)
            Item2.Position = new Vector3(197, 100, 0);
            Moving2.Speed = 0;

            Move(new Vector2(192, 100), new Vector2(193.914f, 100), Angle.Right);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Oben (Item1 blocked)
            Item2.Position = new Vector3(100, 3, 0);
            Moving2.Speed = 0;

            Move(new Vector2(100, 6), new Vector2(100, 6), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        /// <summary>
        /// Testet die Kollision zwischen zwei Items deren Auflösung zur 
        /// Kollision mit dem reflektierenden Spielfeldrand führt.
        /// </summary>
        [TestMethod]
        public void ItemVsItemVsReflectBorder()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            // Kollision Links (Item1 blocked)
            Item2.Position = new Vector3(2, 100, 0);
            Moving2.Speed = 0;

            Move(new Vector2(7, 100), new Vector2(6, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);


            // Kollision Rechts (Item1 blocked)
            Item2.Position = new Vector3(197, 100, 0);
            Moving2.Speed = 0;

            Move(new Vector2(192, 100), new Vector2(193.914f, 100), Angle.Right);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Oben (Item1 blocked)
            Item2.Position = new Vector3(100, 3, 0);
            Moving2.Speed = 0;

            Move(new Vector2(100, 6), new Vector2(100, 6), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        /// <summary>
        /// Testet die Kollision zwischen zwei Items deren Auflösung zur 
        /// Kollision mit einer unüberwindbaren Zellengrenze führt.
        /// </summary>
        [TestMethod]
        public void ItemVsItemVsCell()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            // Felder 3-7 auf x und y Achse anheben
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 5; y++)
                    Map.Tiles[x + 3, y + 3].Height = TileHeight.High;

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            // Kollision Links (Item1 blocked)
            Item2.Position = new Vector3(162, 100, 0);
            Moving2.Speed = 0;

            Move(new Vector2(167, 100), new Vector2(166, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);


            // Kollision Rechts (Item1 blocked)
            Item2.Position = new Vector3(57, 100, 0);
            Moving2.Speed = 0;

            Move(new Vector2(52, 100), new Vector2(53.914f, 100), Angle.Right);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Oben (Item1 blocked)
            Item2.Position = new Vector3(100, 162, 0);
            Moving2.Speed = 0;

            Move(new Vector2(167, 6), new Vector2(166, 6), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        #endregion

        #region Overflows

        /// <summary>
        /// Testet das ordnungsgemäße Verhalten bei einem Radius von 0
        /// </summary>
        [TestMethod]
        public void CollisionRadiusNull()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable2.CollisionRadius = 0;

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(99, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(101, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 99), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 101), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        /// <summary>
        /// Testet das Verhalten bei einer Masse von 0
        /// </summary>
        [TestMethod]
        public void CollisionMassNull()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable2.CollisionMass = 0;

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            float distance = new Vector2(2, 2).Length();

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(97 + distance, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(97 + distance + 4, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 97 + distance), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 97 + distance + 4), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        /// <summary>
        /// Testet das Verhalten bei einer Masse von float.Max
        /// </summary>
        [TestMethod]
        public void CollisionMassOversized()
        {
            InitItem(new Vector2(50, 100), new Vector2(150, 100));
            InitFlat(true);

            Collidable1.CollisionMass = 3;
            Collidable2.CollisionMass = float.MaxValue;

            Collidable1.OnCollision += (i, v) =>
            {
                collision1++;
            };

            Collidable2.OnCollision += (i, v) =>
            {
                collision2++;
            };

            float distance = new Vector2(2, 2).Length();
            float diff = 4 - (6 - distance - distance);

            float massSum = Collidable1.CollisionMass + Collidable2.CollisionMass;
            float dist1 = diff * (Collidable2.CollisionMass / massSum);
            float dist2 = diff * (Collidable1.CollisionMass / massSum);

            // Kollision X-Achse
            Move(new Vector2(97, 100), new Vector2(97 + distance - dist1, 100), Angle.Right,
                new Vector2(103, 100), new Vector2(103 - distance + dist2, 100), Angle.Left);
            CheckCollisionEvents(1, 1, 0);

            // Kollision Y-Achse
            Move(new Vector2(100, 97), new Vector2(100, 97 + distance - dist1), Angle.Down,
                new Vector2(100, 103), new Vector2(100, 103 - distance + dist2), Angle.Up);
            CheckCollisionEvents(1, 1, 0);
        }

        #endregion

        #region Helper

        private void Move(Vector2 start, Vector2 result, Angle direction)
        {
            Item1.Position = new Vector3(start.X, start.Y, 0);
            Item1.MoveSpeed = new Vector2(2, 2).Length();
            Item1.MoveDirection = direction;
            Engine.Update();

            Assert.AreEqual(result.X, Item1.Position.X, 0.01f);
            Assert.AreEqual(result.Y, Item1.Position.Y, 0.01f);
        }

        private void Move(Vector2 start1, Vector2 result1, Angle direction1, Vector2 start2, Vector2 result2, Angle direction2)
        {
            Item1.Position = new Vector3(start1.X, start1.Y, 0);
            Item1.MoveSpeed = new Vector2(2, 2).Length();
            Item1.MoveDirection = direction1;

            Item2.Position = new Vector3(start2.X, start2.Y, 0);
            Item2.MoveSpeed = new Vector2(2, 2).Length();
            Item2.MoveDirection = direction2;

            Engine.Update();

            Assert.AreEqual(result1.X, Item1.Position.X, 0.01f);
            Assert.AreEqual(result1.Y, Item1.Position.Y, 0.01f);
            Assert.AreEqual(result2.X, Item2.Position.X, 0.01f);
            Assert.AreEqual(result2.Y, Item2.Position.Y, 0.01f);
        }

        #endregion
    }
}
