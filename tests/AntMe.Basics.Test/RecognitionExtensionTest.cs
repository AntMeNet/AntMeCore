using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Core.Extensions;
using AntMe.Core.Debug;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Test für alle Funktionalitäten der Recognition Extension (ehemals PassiveInteraction)
    /// Autor: Tom Wendel
    /// Status: TODOs
    /// </summary>
    [TestClass]
    public class RecognitionExtensionTest
    {
        private Engine Engine;
        private Map Map;
        private Vector2 Pos = new Vector2(100, 100);
        private DebugVisibleItem VisibleItem;
        private DebugSightingItem SightingItem;
        private DebugSmellableItem SmellableItem;
        private DebugSnifferItem SnifferItem;
        private VisibleProperty Visible;
        private SightingProperty Sighting;
        private SmellableProperty Smellable;
        private SnifferProperty Sniffer;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine();
            Engine.RegisterExtension(new RecognitionExtension(), 1);
            Map = Map.CreateMap(MapPreset.Small, true);
            Engine.Init(Map);
        }

        private void InitVisibleItem(Vector2 pos)
        {
            VisibleItem = new DebugVisibleItem(pos);
            Visible = VisibleItem.GetProperty<VisibleProperty>();
            Engine.InsertItem(VisibleItem);
        }

        private void InitSightingItem(Vector2 pos)
        {
            SightingItem = new DebugSightingItem(pos);
            Sighting = SightingItem.GetProperty<SightingProperty>();
            Engine.InsertItem(SightingItem);
        }

        private void InitSmellableItem(Vector2 pos)
        {
            SmellableItem = new DebugSmellableItem(pos);
            Smellable = SmellableItem.GetProperty<SmellableProperty>();
            Engine.InsertItem(SmellableItem);
        }

        private void InitSnifferItem(Vector2 pos)
        {
            SnifferItem = new DebugSnifferItem(pos);
            Sniffer = SnifferItem.GetProperty<SnifferProperty>();
            Engine.InsertItem(SnifferItem);
        }

        [TestCleanup]
        public void CleanupEngine()
        {
            VisibleItem = null;
            SightingItem = null;
            SmellableItem = null;
            SnifferItem = null;
            Visible = null;
            Sighting = null;
            Smellable = null;
            Sniffer = null;
            Map = null;
            Engine = null;
        }

        #endregion

        #region Property Events

        #region VisibleProperty

        /// <summary>
        /// Testet die Funktionalität des Change Events
        /// </summary>
        [TestMethod]
        public void VisibleRadiusChanged()
        {
            InitVisibleItem(Pos);

            int count = 0;
            float expected = 0f;

            Visible.OnVisibilityRadiusChanged += (i, v) =>
            {
                Assert.AreEqual(VisibleItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal value
            expected = 10;
            Visible.VisibilityRadius = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Big value
            expected = float.MaxValue;
            Visible.VisibilityRadius = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative value
            expected = 0;
            Visible.VisibilityRadius = -10;
            Assert.AreEqual(1, count);
            count = 0;

        }

        #endregion

        #region SightingProperty

        /// <summary>
        /// Testet die Funktionalität des Change Events
        /// </summary>
        [TestMethod]
        public void SightingViewAngleChanged()
        {
            InitSightingItem(Pos);
            int count = 0;
            float expected = 0;
            Sighting.OnViewAngleChanged += (i, v) =>
            {
                Assert.AreEqual(SightingItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normaler Wert
            expected = 10;
            Sighting.ViewAngle = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Wert größer 360
            expected = 360;
            Sighting.ViewAngle = 700;
            Assert.AreEqual(1, count);
            count = 0;

            // Negativer Wert
            expected = 0;
            Sighting.ViewAngle = -100;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Testet die Funktionalität des Change Events
        /// </summary>
        [TestMethod]
        public void SightingViewRangeChanged()
        {
            InitSightingItem(Pos);
            int count = 0;
            float expected = 0;
            Sighting.OnViewRangeChanged += (i, v) =>
            {
                Assert.AreEqual(SightingItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Sighting.ViewRange = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Max Value
            expected = float.MaxValue;
            Sighting.ViewRange = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;
            
            // Negative Value
            expected = 0;
            Sighting.ViewRange = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Testet die Funktionalität des Change Events
        /// </summary>
        [TestMethod]
        public void SightingViewDirectionChanged()
        {
            InitSightingItem(Pos);
            int count = 0;
            Angle expected = Angle.Left;
            Sighting.OnViewDirectionChanged += (i, v) =>
            {
                Assert.AreEqual(SightingItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Value 1
            expected = Angle.FromDegree(10);
            Sighting.ViewDirection = Angle.FromDegree(10);
            Assert.AreEqual(1, count);
            count = 0;

            // Value 2
            expected = Angle.FromDegree(133);
            Sighting.ViewDirection = Angle.FromDegree(133);
            Assert.AreEqual(1, count);
            count = 0;

            // Value 3
            expected = Angle.FromDegree(-30);
            Sighting.ViewDirection = Angle.FromDegree(-30);
            Assert.AreEqual(1, count);
            count = 0;

            // Value 1
            expected = Angle.FromDegree(710);
            Sighting.ViewDirection = Angle.FromDegree(710);
            Assert.AreEqual(1, count);
            count = 0;
        }

        #endregion

        #region SmellableProperty

        /// <summary>
        /// Testet die Funktionalität des Change Events
        /// </summary>
        [TestMethod]
        public void SmellableRadiusChanged()
        {
            InitSmellableItem(Pos);
            int count = 0;
            float expected = 0;
            Smellable.OnSmellableRadiusChanged += (i, v) =>
            {
                Assert.AreEqual(SmellableItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Smellable.SmellableRadius = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = float.MaxValue;
            Smellable.SmellableRadius = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            Smellable.SmellableRadius = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        #endregion

        #region SnifferProperty

        // No Property Events
        
        #endregion

        #endregion

        #region Sighting

        /// <summary>
        /// Prüft, ob bei einem Zellenwechsel auch ein Environment Change 
        /// stattfindet. Das darf aber wirklich bei einem Zellenwechsel 
        /// passieren.
        /// </summary>
        [TestMethod]
        public void EnvironmentChange()
        {
            // Startpunkt ist Zelle (1/1)
            InitSightingItem(new Vector2(30, 30));
            Assert.AreEqual(new Index2(1, 1), SightingItem.Cell);
            Assert.IsNotNull(Sighting.Environment);

            int count = 0;
            VisibleEnvironment env = null;
            Sighting.OnEnvironmentChanged += (i, v) =>
            {
                count++;
                env = v;
            };

            // Erster Place in die selbe Zelle
            SightingItem.Position = new Vector3(35, 35, 0);
            Assert.AreEqual(new Index2(1, 1), SightingItem.Cell);
            Assert.AreEqual(0, count);
            Assert.AreEqual(null, env);

            // Zweiter Place in die Nachbarzelle 2/1
            SightingItem.Position = new Vector3(50, 30, 0);
            Assert.AreEqual(new Index2(2, 1), SightingItem.Cell);
            Assert.AreEqual(1, count);

            // Dritter Place in die linke, obere Zelle (0/0)
            SightingItem.Position = new Vector3(10, 10, 0);
            Assert.AreEqual(new Index2(0, 0), SightingItem.Cell);
            Assert.AreEqual(2, count);

            // Vierter Place außerhalb des Spielfeldes (0/-1).
            // Da Zelle 0/0 referenziert wird, sollte kein Event passieren
            SightingItem.Position = new Vector3(10, -10, 0);
            Assert.AreEqual(new Index2(0, 0), SightingItem.Cell);
            Assert.AreEqual(2, count);
        }

        /// <summary>
        /// Prüft die Richtigkeit des Environment Inhaltes
        /// </summary>
        [TestMethod]
        public void EnvironmentContent()
        {
            // TODO: fix this!

            //Map.SpeedMap[2, 2] = 90;
            //Map.SpeedMap[2, 3] = 100;
            //Map.SpeedMap[2, 4] = 110;
            //Map.SpeedMap[3, 2] = 93;
            //Map.SpeedMap[3, 3] = 103;
            //Map.SpeedMap[3, 4] = 113;
            //Map.SpeedMap[4, 2] = 96;
            //Map.SpeedMap[4, 3] = 106;
            //Map.SpeedMap[4, 4] = 116;

            //Map.HeightMap[2, 2] = 90;
            //Map.HeightMap[2, 3] = 100;
            //Map.HeightMap[2, 4] = 110;
            //Map.HeightMap[3, 2] = 93;
            //Map.HeightMap[3, 3] = 103;
            //Map.HeightMap[3, 4] = 113;
            //Map.HeightMap[4, 2] = 96;
            //Map.HeightMap[4, 3] = 106;
            //Map.HeightMap[4, 4] = 116;

            //// [0,0]
            //InitSightingItem(new Vector2(1, 1));
            //Assert.AreEqual(new Index2(0, 0), SightingItem.Cell);

            //Assert.AreEqual(false, Sighting.Environment.NorthWest.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.North.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.NorthEast.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.West.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.Center.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.East.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.SouthWest.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.South.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.SouthEast.HasValue);

            //Assert.AreEqual(1, Sighting.Environment.Center.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.East.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.South.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.SouthEast.Value.Speed, 0.01f);

            //Assert.AreEqual(0, Sighting.Environment.Center.Value.Height);
            //Assert.AreEqual(0, Sighting.Environment.East.Value.Height);
            //Assert.AreEqual(0, Sighting.Environment.South.Value.Height);
            //Assert.AreEqual(0, Sighting.Environment.SouthEast.Value.Height);

            //// [9,9]
            //InitSightingItem(new Vector2(199, 199));
            //Assert.AreEqual(new Index2(9, 9), SightingItem.Cell);

            //Assert.AreEqual(true, Sighting.Environment.NorthWest.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.North.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.NorthEast.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.West.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.Center.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.East.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.SouthWest.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.South.HasValue);
            //Assert.AreEqual(false, Sighting.Environment.SouthEast.HasValue);

            //Assert.AreEqual(1, Sighting.Environment.NorthWest.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.North.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.West.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.Center.Value.Speed, 0.01f);

            //Assert.AreEqual(0, Sighting.Environment.NorthWest.Value.Height);
            //Assert.AreEqual(0, Sighting.Environment.North.Value.Height);
            //Assert.AreEqual(0, Sighting.Environment.West.Value.Height);
            //Assert.AreEqual(0, Sighting.Environment.Center.Value.Height);

            //// [3, 3]
            //InitSightingItem(new Vector2(70, 70));
            //Assert.AreEqual(new Index2(3, 3), SightingItem.Cell);

            //Assert.AreEqual(true, Sighting.Environment.NorthWest.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.North.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.NorthEast.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.West.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.Center.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.East.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.SouthWest.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.South.HasValue);
            //Assert.AreEqual(true, Sighting.Environment.SouthEast.HasValue);

            //Assert.AreEqual(0.9, Sighting.Environment.NorthWest.Value.Speed, 0.01f);
            //Assert.AreEqual(0.93, Sighting.Environment.North.Value.Speed, 0.01f);
            //Assert.AreEqual(0.96, Sighting.Environment.NorthEast.Value.Speed, 0.01f);
            //Assert.AreEqual(1, Sighting.Environment.West.Value.Speed, 0.01f);
            //Assert.AreEqual(1.03, Sighting.Environment.Center.Value.Speed, 0.01f);
            //Assert.AreEqual(1.06, Sighting.Environment.East.Value.Speed, 0.01f);
            //Assert.AreEqual(1.1, Sighting.Environment.SouthWest.Value.Speed, 0.01f);
            //Assert.AreEqual(1.13, Sighting.Environment.South.Value.Speed, 0.01f);
            //Assert.AreEqual(1.16, Sighting.Environment.SouthEast.Value.Speed, 0.01f);

            //Assert.AreEqual(90 - 103, Sighting.Environment.NorthWest.Value.Height);
            //Assert.AreEqual(93 - 103, Sighting.Environment.North.Value.Height);
            //Assert.AreEqual(96 - 103, Sighting.Environment.NorthEast.Value.Height);
            //Assert.AreEqual(100 - 103, Sighting.Environment.West.Value.Height);
            //Assert.AreEqual(103 - 103, Sighting.Environment.Center.Value.Height);
            //Assert.AreEqual(106 - 103, Sighting.Environment.East.Value.Height);
            //Assert.AreEqual(110 - 103, Sighting.Environment.SouthWest.Value.Height);
            //Assert.AreEqual(113 - 103, Sighting.Environment.South.Value.Height);
            //Assert.AreEqual(116 - 103, Sighting.Environment.SouthEast.Value.Height);
        }

        /// <summary>
        /// Prüft den Sichtmechanismus mit verschiedenen Sichtradien aber 
        /// ohne Sichtkegel.
        /// </summary>
        [TestMethod]
        public void CheckSighting()
        {
            InitSightingItem(new Vector2(50, 100));
            InitVisibleItem(new Vector2(150, 100));
            Sighting.ViewAngle = 360;
            Sighting.ViewDirection = 180;
            int newItemCount = 0;
            int lostItemCount = 0;
            int ItemCount = 0;
            Sighting.OnNewVisibleItem += (v) =>
            {
                Assert.AreEqual(Visible, v);
                newItemCount++;
            };
            Sighting.OnLostVisibleItem += (v) =>
            {
                Assert.AreEqual(Visible, v);
                lostItemCount++;
            };
            Sighting.OnVisibleItem += (v) =>
            {
                Assert.AreEqual(Visible, v);
                ItemCount++;
            };

            // Test 1: Blind Items
            Sighting.ViewRange = 0;
            Visible.VisibilityRadius = 0;
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(0, ItemCount);
            Assert.AreEqual(0, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 2: Hohe Sichtweite
            Sighting.ViewRange = 120;
            Visible.VisibilityRadius = 0;
            Engine.Update();
            Assert.AreEqual(1, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(1, ItemCount);
            Assert.AreEqual(1, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 3: Verbleibend
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(1, ItemCount);
            Assert.AreEqual(1, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 4: Keine Sichtweite, Hohe Sichtbarkeit
            Sighting.ViewRange = 0;
            Visible.VisibilityRadius = 120;
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(1, ItemCount);
            Assert.AreEqual(1, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 5: Sichtbarkeit verloren
            Sighting.ViewRange = 0;
            Visible.VisibilityRadius = 99;
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(1, lostItemCount);
            Assert.AreEqual(0, ItemCount);
            Assert.AreEqual(0, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 6: Knappe Überschneidung in der Mitte
            Sighting.ViewRange = 50;
            Visible.VisibilityRadius = 50;
            Engine.Update();
            Assert.AreEqual(1, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(1, ItemCount);
            Assert.AreEqual(1, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 7: Delete Visible Item
            Engine.RemoveItem(VisibleItem);
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(1, lostItemCount);
            Assert.AreEqual(0, ItemCount);
            Assert.AreEqual(0, Sighting.VisibleItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;
        }

        /// <summary>
        /// Prüft die Funktionalität des Sichtradius
        /// </summary>
        [TestMethod]
        public void CheckViewAngle()
        {
            InitSightingItem(new Vector2(50, 100));
            InitVisibleItem(new Vector2(60, 100));

            Visible.VisibilityRadius = 0;

            // Test 1: Blind
            Sighting.ViewRange = 20;
            Sighting.ViewAngle = 0;
            Sighting.ViewDirection = 0;
            Engine.Update();
            Assert.AreEqual(0, Sighting.VisibleItems.Count);

            // Test 2: Small Range
            Sighting.ViewRange = 20;
            Sighting.ViewAngle = 10;
            for (int i = 0; i < 360; i++)
            {
                Sighting.ViewDirection = Angle.FromDegree(i);
                Engine.Update();
                if (i < 5 || i > 355)
                    Assert.AreEqual(1, Sighting.VisibleItems.Count);                    
                else
                    Assert.AreEqual(0, Sighting.VisibleItems.Count);
            }

            // Test 3: Bigger Visibility Radius
            Visible.VisibilityRadius = 10;
            for (int i = 0; i < 360; i++)
            {
                Sighting.ViewDirection = Angle.FromDegree(i);
                Engine.Update();
                // TODO: Genaue Formel ermitteln
                if (i < 5 || i > 355)
                    Assert.AreEqual(1, Sighting.VisibleItems.Count);
                else
                    Assert.AreEqual(0, Sighting.VisibleItems.Count);
            }
        }

        #endregion

        #region Sniffing

        /// <summary>
        /// Prüft das Verhalten zwischen riechender- und riechbarer Einheit.
        /// </summary>
        [TestMethod]
        public void CheckSniffing()
        {
            InitSnifferItem(new Vector2(50, 100));
            InitSmellableItem(new Vector2(150, 100));

            int newItemCount = 0;
            int lostItemCount = 0;
            int ItemCount = 0;
            Sniffer.OnNewSmellableItem += (v) =>
            {
                Assert.AreEqual(Smellable, v);
                newItemCount++;
            };
            Sniffer.OnLostSmellableItem += (v) =>
            {
                Assert.AreEqual(Smellable, v);
                lostItemCount++;
            };
            Sniffer.OnSmellableItem += (v) =>
            {
                Assert.AreEqual(Smellable, v);
                ItemCount++;
            };

            // Test 1: No Contact
            Smellable.SmellableRadius = 0;
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(0, ItemCount);
            Assert.AreEqual(0, Sniffer.SmellableItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 2: Wachsendes Signal
            for (int i = 0; i < 15; i++)
            {
                float size = 10 * i;
                Smellable.SmellableRadius = size;
                Engine.Update();

                if (i < 10)
                {
                    Assert.AreEqual(0, newItemCount);
                    Assert.AreEqual(0, lostItemCount);
                    Assert.AreEqual(0, ItemCount);
                    Assert.AreEqual(0, Sniffer.SmellableItems.Count);
                }
                else if (i == 10)
                {
                    Assert.AreEqual(1, newItemCount);
                    Assert.AreEqual(0, lostItemCount);
                    Assert.AreEqual(1, ItemCount);
                    Assert.AreEqual(1, Sniffer.SmellableItems.Count);
                }
                else
                {
                    Assert.AreEqual(0, newItemCount);
                    Assert.AreEqual(0, lostItemCount);
                    Assert.AreEqual(1, ItemCount);
                    Assert.AreEqual(1, Sniffer.SmellableItems.Count);
                }
                newItemCount = lostItemCount = ItemCount = 0;
            }

            // Test 4: Element bewegt sich außerhalb des Bereichs
            Smellable.SmellableRadius = 70;
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(1, lostItemCount);
            Assert.AreEqual(0, ItemCount);
            Assert.AreEqual(0, Sniffer.SmellableItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 5: In den Bereich laufen
            SnifferItem.Position = new Vector3(100, 100, 0);
            Engine.Update();
            Assert.AreEqual(1, newItemCount);
            Assert.AreEqual(0, lostItemCount);
            Assert.AreEqual(1, ItemCount);
            Assert.AreEqual(1, Sniffer.SmellableItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;

            // Test 6: Delete Item
            Engine.RemoveItem(SmellableItem);
            Engine.Update();
            Assert.AreEqual(0, newItemCount);
            Assert.AreEqual(1, lostItemCount);
            Assert.AreEqual(0, ItemCount);
            Assert.AreEqual(0, Sniffer.SmellableItems.Count);
            newItemCount = lostItemCount = ItemCount = 0;
        }

        #endregion
    }
}
