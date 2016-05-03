using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Core.Extensions;
using AntMe.Core.Debug;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet das grundsätzliche Verhalten von beweglichen Elementen
    /// Autor: Tom Wendel
    /// Status: Implemented
    /// </summary>
    [TestClass]
    public class PhysicsExtensionBasicTest
    {
        private Engine Engine;
        private Map Map;
        private DebugWalkingItem Item;
        private WalkingProperty Walking;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine();
            Engine.RegisterExtension(new PhysicsExtension(), 1);
        }

        private void InitItem(Vector2 pos)
        {
            Item = new DebugWalkingItem(new Vector2(pos.X, pos.Y));
            Walking = Item.GetProperty<WalkingProperty>();
        }

        private void InitFlat(bool blockBorder, TileSpeed speed)
        {
            Map = Map.CreateMap(MapPreset.Small, true);
            Index2 size = Map.GetCellCount();
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    Map.Tiles[x, y].Speed = speed;

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

        #region Property Events

        [TestMethod]
        public void WalkingPropertyEvents()
        {
            InitItem(new Vector2(100, 100));

            bool MaximumMoveSpeed = false;
            bool MoveDirection = false;
            bool MoveSpeed = false;

            Walking.OnMaximumMoveSpeedChanged += (i, v) =>
            {
                MaximumMoveSpeed = true;
                Assert.AreEqual(6, v);
            };

            Walking.OnMoveDirectionChanged += (i, v) =>
            {
                MoveDirection = true;
                Assert.AreEqual(Angle.FromDegree(10), v);
            };

            Walking.OnMoveSpeedChanged += (i, v) =>
            {
                MoveSpeed = true;
                Assert.AreEqual(3, v);
            };

            Walking.MaximumSpeed = 6;
            Walking.Direction = Angle.FromDegree(10);
            Walking.Speed = 3;

            Assert.AreEqual(true, MaximumMoveSpeed);
            Assert.AreEqual(true, MoveDirection);
            Assert.AreEqual(true, MoveSpeed);
        }

        #endregion

        #region Basic Moving Item

        /// <summary>
        /// Testet simple Bewegungen.
        /// </summary>
        [TestMethod]
        public void BasicMovement()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(true, TileSpeed.Normal);
            MoveToDirection(Angle.UpperRight, 4, 10);
            MoveToDirection(Angle.UpperLeft, 2, 10);
            MoveToDirection(Angle.LowerRight, 20, 10);
            MoveToDirection(Angle.LowerLeft, -3, 10);
            MoveToDirection(Angle.LowerLeft, 3, 8);
        }

        /// <summary>
        /// Testet die Fortbewegung bei verlangsamender Speed Map
        /// </summary>
        [TestMethod]
        public void BasicSlowSpeedMap()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(true, TileSpeed.Slowest);
            MoveToDirection(Angle.UpperRight, 4, 10);
            MoveToDirection(Angle.UpperLeft, 2, 10);
            MoveToDirection(Angle.LowerRight, 20, 10);
            MoveToDirection(Angle.LowerLeft, -3, 10);
            MoveToDirection(Angle.LowerLeft, 3, 8);
        }

        /// <summary>
        /// Testet die Fortbewegung bei beschleunigter Speed Map
        /// </summary>
        [TestMethod]
        public void BasicFastSpeedMap()
        {
            InitItem(new Vector2(100, 100));
            InitFlat(true, TileSpeed.Faster);
            MoveToDirection(Angle.UpperRight, 4, 10);
            MoveToDirection(Angle.UpperLeft, 2, 10);
            MoveToDirection(Angle.LowerRight, 20, 10);
            MoveToDirection(Angle.LowerLeft, -3, 10);
            MoveToDirection(Angle.LowerLeft, 3, 8);
        }

        private void MoveToDirection(Angle angle, float speed, int steps)
        {
            Vector3 pos = Item.Position;
            Vector3 movement = Vector3.FromAngleXY(angle) * Math.Max(Math.Min(Walking.MaximumSpeed, speed), 0);
            Walking.Direction = angle;
            Walking.Speed = speed;

            for (int i = 0; i < steps; i++)
            {
                float speedModificator = Map.Tiles[Item.Cell.X, Item.Cell.Y].GetSpeedMultiplicator();

                Assert.AreEqual(pos.X, Item.Position.X, 0.0001f);
                Assert.AreEqual(pos.Y, Item.Position.Y, 0.0001f);
                Assert.AreEqual(pos.Z, Item.Position.Z, 0.0001f);
                Assert.AreEqual(Walking.Direction.Degree, angle.Degree, 0.0001f);

                pos += movement * speedModificator;
                pos.Z = Map.GetHeight(pos.ToVector2XY());
                Engine.Update();
            }
        }

        #endregion

        #region Speed Map

        /// <summary>
        /// Testet die Fortbewegung auf einer komplexeren Karte
        /// TODO: fix this
        /// </summary>
        [TestMethod]
        public void ComplexSpeedMap()
        {
            //InitItem(new Vector2(100, 100));
            //InitPlain(true, 200);
            //for (int x = 0; x < Map.SpeedMap.GetLength(0); x++)
            //    for (int y = 0; y < Map.SpeedMap.GetLength(1); y++)
            //        if (x % 2 != y % 2)
            //            Map.SpeedMap[x, y] = 50;

            //int cellcount = 0;
            //Item.CellChanged += (i, v) =>
            //{
            //    cellcount++;
            //};

            //MoveToDirection(Angle.UpperRight, 4, 10);
            //MoveToDirection(Angle.UpperLeft, 2, 10);
            //MoveToDirection(Angle.LowerRight, 20, 10);
            //MoveToDirection(Angle.LowerLeft, -3, 10);
            //MoveToDirection(Angle.LowerLeft, 3, 8);
        }

        #endregion
    }
}
