using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Core.Debug;
using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet das Rand- und Zellen-Verhalten der Einheiten.
    /// Autor: Tom Wendel
    /// Status: Implemented
    /// </summary>
    [TestClass]
    public class PhysicsExtensionBorderTest
    {
        private Engine Engine;
        private Map Map;
        private DebugWalkingItem Item;
        private WalkingProperty Moving;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine();
            Engine.RegisterExtension(new PhysicsExtension(), 1);
            BorderCount = 0;
        }

        private void InitItem(Vector2 pos)
        {
            Item = new DebugWalkingItem(new Vector2(pos.X, pos.Y));
            Moving = Item.GetProperty<WalkingProperty>();
        }

        private void InitFlat(bool blockBorder, TileSpeed speed)
        {

            Map = Map.CreateMap(MapPreset.Small, true);
            Index2 cells = Map.GetCellCount();            
            for (int x = 0; x < cells.X; x++)
                for (int y = 0; y < cells.Y; y++)
                    Map.Tiles[x, y].Speed = speed;
            Engine.Init(Map);

            Engine.InsertItem(Item);
        }

        private void InitHeight()
        {
            Map = Map.CreateMap(MapPreset.Small, true);
            Map.Tiles[0, 0].Height = TileHeight.High;
            Map.Tiles[1, 1].Height = TileHeight.High;

            Engine.Init(Map);
            Engine.InsertItem(Item);
        }

        [TestCleanup]
        public void CleanupEngine()
        {
            Item = null;
            Map = null;
            Engine = null;
        }

        #endregion

        #region Border Bahavior

        private int BorderCount = 0;
        private Compass BorderDirection = Compass.West;

        /// <summary>
        /// Testet das Verhalten von Items bei blockierendem Rand
        /// </summary>
        [TestMethod]
        public void BorderBlock()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(true, TileSpeed.Normal);

            Moving.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // Obere Kante
            Move(new Vector2(100, 1), new Vector2(102, 0), Angle.UpperRight);
            CheckBorderEvent(1, Compass.North);
            Move(new Vector2(100, 1), new Vector2(98, 0), Angle.UpperLeft);
            CheckBorderEvent(1, Compass.North);

            // untere Kante
            Move(new Vector2(100, 199), new Vector2(102, 200), Angle.LowerRight);
            CheckBorderEvent(1, Compass.South);
            Move(new Vector2(100, 199), new Vector2(98, 200), Angle.LowerLeft);
            CheckBorderEvent(1, Compass.South);

            // linke Kante
            Move(new Vector2(1, 100), new Vector2(0, 102), Angle.LowerLeft);
            CheckBorderEvent(1, Compass.West);
            Move(new Vector2(1, 100), new Vector2(0, 98), Angle.UpperLeft);
            CheckBorderEvent(1, Compass.West);

            // rechte Kante
            Move(new Vector2(199, 100), new Vector2(200, 102), Angle.LowerRight);
            CheckBorderEvent(1, Compass.East);
            Move(new Vector2(199, 100), new Vector2(200, 98), Angle.UpperRight);
            CheckBorderEvent(1, Compass.East);
        }

        /// <summary>
        /// Testet das Verhalten bei einem Drop-Rand.
        /// </summary>
        [TestMethod]
        public void BorderDropDown()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(false, TileSpeed.Normal);

            Moving.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // untere Kante
            Move(new Vector2(100, 199), new Vector2(102, 201), Angle.LowerRight);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item.Id);
        }

        /// <summary>
        /// Testet das Verhalten bei einem Drop-Rand.
        /// </summary>
        [TestMethod]
        public void BorderDropLeft()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(false, TileSpeed.Normal);

            Moving.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // linke Kante
            Move(new Vector2(1, 100), new Vector2(-1, 102), Angle.LowerLeft);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item.Id);
        }

        /// <summary>
        /// Testet das Verhalten bei einem Drop-Rand.
        /// </summary>
        [TestMethod]
        public void BorderDropTop()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(false, TileSpeed.Normal);

            Moving.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // Obere Kante
            Move(new Vector2(100, 1), new Vector2(102, -1), Angle.UpperRight);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item.Id);
        }

        /// <summary>
        /// Testet das Verhalten bei einem Drop-Rand.
        /// </summary>
        [TestMethod]
        public void BorderDropRight()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(false, TileSpeed.Normal);

            Moving.OnHitBorder += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            // rechte Kante
            Move(new Vector2(199, 100), new Vector2(201, 102), Angle.LowerRight);
            CheckBorderEvent(0);
            Assert.AreEqual(0, Item.Id);
        }

        private void Move(Vector2 start, Vector2 result, Angle direction)
        {
            Item.Position = new Vector3(start.X, start.Y, 0);
            Item.MoveSpeed = new Vector2(2, 2).Length();
            Item.MoveDirection = direction;
            Engine.Update();

            Assert.AreEqual(result.X, Item.Position.X, 0.01f);
            Assert.AreEqual(result.Y, Item.Position.Y, 0.01f);
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

        #region Height Map

        /// <summary>
        /// Prüft die Blockade beim Zellenwechsel über zu große Höhendistanz
        /// </summary>
        [TestMethod]
        public void CheckCellCollision()
        {
            InitItem(new Vector2(30, 30));
            InitHeight();

            Moving.OnHitWall += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            float distance = new Vector2(2, 2).Length();

            // Zellenwechsel (hoch->tief) nach links
            Move(new Vector2(21, 30), new Vector2(21 - distance, 30), Angle.Left);
            CheckBorderEvent(0);

            // Zellenwechsel (hoch->tief) nach rechts
            Move(new Vector2(19, 10), new Vector2(19 + distance, 10), Angle.Right);
            CheckBorderEvent(0);

            // Zellenwechsel (hoch->tief) nach oben
            Move(new Vector2(30, 21), new Vector2(30, 21 - distance), Angle.Up);
            CheckBorderEvent(0);

            // Zellenwechsel (hoch->tief) nach unten
            Move(new Vector2(10, 19), new Vector2(10, 19 + distance), Angle.Down);
            CheckBorderEvent(0);

            // Zellenwechsel (tief->hoch) nach links
            Move(new Vector2(21, 10), new Vector2(20, 10), Angle.Left);
            CheckBorderEvent(1, Compass.West);

            // Zellenwechsel (tief->hoch) nach rechts
            Move(new Vector2(19, 30), new Vector2(20, 30), Angle.Right);
            CheckBorderEvent(1, Compass.East);

            // Zellenwechsel (tief->hoch) nach oben
            Move(new Vector2(10, 21), new Vector2(10, 20), Angle.Up);
            CheckBorderEvent(1, Compass.North);

            // Zellenwechsel (tief->hoch) nach unten
            Move(new Vector2(30, 19), new Vector2(30, 20), Angle.Down);
            CheckBorderEvent(1, Compass.South);
        }

        /// <summary>
        /// Testet die COllision mit der Wand durch unterschiedliche 
        /// Schwellwerte und prüft, ob die Grenze richtig erkannt wird.
        /// </summary>
        [TestMethod]
        public void CheckCellDiff()
        {
            InitItem(new Vector2(30, 30));
            InitHeight();

            Moving.OnHitWall += (i, d) =>
            {
                BorderCount++;
                BorderDirection = d;
            };

            float distance = new Vector2(2, 2).Length();

            for (byte i = 0; i < 255; i++)
            {
                // Zellenwechsel nach links
                if (i >= 100)
                {
                    Move(new Vector2(21, 10), new Vector2(21 - distance, 10), Angle.Left);
                    CheckBorderEvent(0);
                }
                else
                {
                    Move(new Vector2(21, 10), new Vector2(20, 10), Angle.Left);
                    CheckBorderEvent(1, Compass.West);
                }
            }

        }

        #endregion
    }
}
