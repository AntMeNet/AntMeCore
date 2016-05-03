using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Core.Extensions;
using System.Linq;
using AntMe.Core.Debug;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Tests für alle Funktionalitäten der InteractionExtension
    /// Autor: Tom Wendel
    /// Status: TODOs
    /// </summary>
    [TestClass]
    public class InteractionExtensionTest
    {
        private Engine Engine;
        private Map Map;
        private Vector2 Pos = new Vector2(100, 100);
        private DebugAttackerItem AttackerItem;
        private DebugAttackableItem AttackableItem;
        private DebugCollectorItem CollectorItem;
        private DebugCollectableItem CollectableItem;
        private AttackerProperty Attacker;
        private AttackableProperty Attackable;
        private CollectorProperty Collector;
        private CollectableProperty Collectable;

        #region Init

        [TestInitialize]
        public void InitEngine()
        {
            Engine = new Engine();
            Engine.RegisterExtension(new InteractionExtension(), 1);
            Map = Map.CreateMap(MapPreset.Small, true);
            Engine.Init(Map);
        }

        private void InitAttackerItem(Vector2 pos)
        {
            AttackerItem = new DebugAttackerItem(pos);
            Attacker = AttackerItem.GetProperty<AttackerProperty>();
            Engine.InsertItem(AttackerItem);
        }

        private void InitAttackableItem(Vector2 pos)
        {
            AttackableItem = new DebugAttackableItem(pos);
            Attackable = AttackableItem.GetProperty<AttackableProperty>();
            Engine.InsertItem(AttackableItem);
        }

        private void InitCollectorItem(Vector2 pos)
        {
            CollectorItem = new DebugCollectorItem(pos);
            Collector = CollectorItem.GetProperty<CollectorProperty>();
            Engine.InsertItem(CollectorItem);
        }

        private void InitCollectableItem(Vector2 pos)
        {
            CollectableItem = new DebugCollectableItem(pos);
            Collectable = CollectableItem.GetProperty<CollectableProperty>();
            Engine.InsertItem(CollectableItem);
        }

        [TestCleanup]
        public void CleanupEngine()
        {
            AttackerItem = null;
            AttackableItem = null;
            CollectorItem = null;
            CollectableItem = null;
            Attacker = null;
            Attackable = null;
            Collector = null;
            Collectable = null;
            Map = null;
            Engine = null;
        }

        #endregion

        #region Property Events

        #region Attacker Property

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void AttackRangeChanged()
        {
            InitAttackerItem(Pos);

            int count = 0;
            float expected = 0;
            Attacker.OnAttackRangeChanged += (g, v) =>
            {
                Assert.AreEqual(AttackerItem, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Attacker.AttackRange = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = float.MaxValue;
            Attacker.AttackRange = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            Attacker.AttackRange = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void AttackRecoveryTimeChanged()
        {
            InitAttackerItem(Pos);

            int count = 0;
            int expected = 0;
            Attacker.OnAttackRecoveryTimeChanged += (g, v) =>
            {
                Assert.AreEqual(AttackerItem, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Attacker.AttackRecoveryTime = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = int.MaxValue;
            Attacker.AttackRecoveryTime = int.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            Attacker.AttackRecoveryTime = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void AttackStrengthChanged()
        {
            InitAttackerItem(Pos);

            int count = 0;
            int expected = 0;
            Attacker.OnAttackStrengthChanged += (g, v) =>
            {
                Assert.AreEqual(AttackerItem, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Attacker.AttackStrength = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = int.MaxValue;
            Attacker.AttackStrength = int.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;
            
            // Negative Value
            expected = 0;
            Attacker.AttackStrength = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        #endregion

        #region Attackable Property

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void AttackableHealthChanged()
        {
            InitAttackableItem(Pos);

            int count = 0;
            int expected = 0;
            Attackable.OnAttackableHealthChanged += (g, v) =>
            {
                Assert.AreEqual(AttackableItem, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Attackable.AttackableHealth = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = int.MaxValue;
            Attackable.AttackableHealth = int.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;
            
            // Negative Value
            expected = 0;
            Attackable.AttackableHealth = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void AttackableRadiusChanged()
        {
            InitAttackableItem(Pos);

            int count = 0;
            float expected = 0f;
            Attackable.OnAttackableRadiusChanged += (g, v) =>
            {
                Assert.AreEqual(AttackableItem, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Attackable.AttackableRadius = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = float.MaxValue;
            Attackable.AttackableRadius = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            Attackable.AttackableRadius = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        #endregion

        #region Collectable Property

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void CollectableRadiusChanged()
        {
            InitCollectableItem(Pos);
            int count = 0;
            float expected = 0;
            Collectable.OnCollectableRadiusChanged += (i, v) =>
            {
                Assert.AreEqual(CollectableItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Collectable.CollectableRadius = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Overseized Value
            expected = float.MaxValue;
            Collectable.CollectableRadius = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            Collectable.CollectableRadius = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }
        
        /// <summary>
        /// Prüft das Good Management
        /// </summary>
        [TestMethod]
        public void CollectableGoodsChanged()
        {
            InitCollectableItem(Pos);
            DebugGood1 good1 = new DebugGood1(100, 0);
            DebugGood2 good2 = new DebugGood2(100, 0);

            int newGoodCount = 0;
            int lostGoodCount = 0;
            Collectable.OnNewCollectableGood += (g, v) =>
            {
                newGoodCount++;
            };
            Collectable.OnLostCollectableGood += (g, v) =>
            {
                lostGoodCount++;
            };

            // Check Init
            Assert.AreEqual(0, Collectable.CollectableGoods.Count());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert Good1
            Collectable.AddCollectableGood(good1);
            Assert.AreEqual(1, Collectable.CollectableGoods.Count());
            Assert.AreEqual(true, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(1, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert Good1 again
            try
            {
                Collectable.AddCollectableGood(good1);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }
            Assert.AreEqual(1, Collectable.CollectableGoods.Count());
            Assert.AreEqual(true, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert another instance of Good1
            DebugGood1 good12 = new DebugGood1(100, 0);
            try
            {
                Collectable.AddCollectableGood(good12);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }
            Assert.AreEqual(1, Collectable.CollectableGoods.Count());
            Assert.AreEqual(true, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert an instance of good2
            Collectable.AddCollectableGood(good2);
            Assert.AreEqual(2, Collectable.CollectableGoods.Count());
            Assert.AreEqual(true, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(true, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(good2, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(1, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Remove Good1
            Collectable.RemoveCollectableGood<DebugGood1>();
            Assert.AreEqual(1, Collectable.CollectableGoods.Count());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(true, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(good2, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(1, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Remove Good2
            Collectable.RemoveCollectableGood<DebugGood2>();
            Assert.AreEqual(0, Collectable.CollectableGoods.Count());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collectable.ContainsGood<DebugGood2>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collectable.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(1, lostGoodCount);
            newGoodCount = lostGoodCount = 0;
        }

        #endregion

        #region Collector Property

        /// <summary>
        /// Prüft die Events der Property Eigenschaften
        /// </summary>
        [TestMethod]
        public void OnCollectorRangeChanged()
        {
            InitCollectorItem(Pos);
            int count = 0;
            float expected = 0;
            Collector.OnCollectorRangeChanged += (i, v) =>
            {
                Assert.AreEqual(CollectorItem, i);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            Collector.CollectorRange = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = float.MaxValue;
            Collector.CollectorRange = float.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            Collector.CollectorRange = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Prüft das Good Management
        /// </summary>
        [TestMethod]
        public void CollectorGoodsChanged()
        {
            InitCollectorItem(Pos);
            DebugGood1 good1 = new DebugGood1(100, 0);
            DebugGood2 good2 = new DebugGood2(100, 0);

            int newGoodCount = 0;
            int lostGoodCount = 0;
            Collector.OnNewCollectableGood += (g, v) =>
            {
                newGoodCount++;
            };
            Collector.OnLostCollectableGood += (g, v) =>
            {
                lostGoodCount++;
            };

            // Check Init
            Assert.AreEqual(0, Collector.CollectableGoods.Count());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert Good1
            Collector.AddCollectableGood(good1);
            Assert.AreEqual(1, Collector.CollectableGoods.Count());
            Assert.AreEqual(true, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(1, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert Good1 again
            try
            {
                Collector.AddCollectableGood(good1);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }
            Assert.AreEqual(1, Collector.CollectableGoods.Count());
            Assert.AreEqual(true, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert another instance of Good1
            DebugGood1 good12 = new DebugGood1(100, 0);
            try
            {
                Collector.AddCollectableGood(good12);
                throw new Exception("Should throw an Exception");
            }
            catch (InvalidOperationException) { }
            Assert.AreEqual(1, Collector.CollectableGoods.Count());
            Assert.AreEqual(true, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Insert an instance of good2
            Collector.AddCollectableGood(good2);
            Assert.AreEqual(2, Collector.CollectableGoods.Count());
            Assert.AreEqual(true, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(true, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(good1, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(good2, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(1, newGoodCount);
            Assert.AreEqual(0, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Remove Good1
            Collector.RemoveCollectableGood<DebugGood1>();
            Assert.AreEqual(1, Collector.CollectableGoods.Count());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(true, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(good2, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(1, lostGoodCount);
            newGoodCount = lostGoodCount = 0;

            // Remove Good2
            Collector.RemoveCollectableGood<DebugGood2>();
            Assert.AreEqual(0, Collector.CollectableGoods.Count());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood1>());
            Assert.AreEqual(false, Collector.ContainsGood<DebugGood2>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood1>());
            Assert.AreEqual(null, Collector.GetCollectableGood<DebugGood2>());
            Assert.AreEqual(0, newGoodCount);
            Assert.AreEqual(1, lostGoodCount);
            newGoodCount = lostGoodCount = 0;
        }

        #endregion

        #region Collectable Good

        /// <summary>
        /// Testet die Events der Ressourcen
        /// </summary>
        [TestMethod]
        public void AmountChanged()
        {
            DebugGood1 good = new DebugGood1(100, 0);
            good.Capacity = int.MaxValue;

            int count = 0;
            int expected = 0;
            good.OnAmountChanged += (g, v) =>
            {
                Assert.AreEqual(good, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 10;
            good.Amount = 10;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = int.MaxValue;
            good.Amount = int.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Normal Value
            expected = 0;
            good.Amount = -10;
            Assert.AreEqual(1, count);
            count = 0;
        }

        /// <summary>
        /// Testet die Events der Ressourcen
        /// </summary>
        [TestMethod]
        public void CapacityChanged()
        {
            DebugGood1 good = new DebugGood1(100, 0);

            int count = 0;
            int expected = 0;
            good.OnCapacityChanged += (g, v) =>
            {
                Assert.AreEqual(good, g);
                Assert.AreEqual(expected, v);
                count++;
            };

            // Normal Value
            expected = 100;
            good.Capacity = 100;
            Assert.AreEqual(1, count);
            count = 0;

            // Oversized Value
            expected = int.MaxValue;
            good.Capacity = int.MaxValue;
            Assert.AreEqual(1, count);
            count = 0;

            // Negative Value
            expected = 0;
            good.Capacity = -100;
            Assert.AreEqual(1, count);
            count = 0;
        }

        #endregion

        #endregion

        #region Attacking

        /// <summary>
        /// Prüft die Angriffsmechanismen im Bezug auf Range.
        /// </summary>
        [TestMethod]
        public void CheckAttackRange()
        {
            InitAttackerItem(new Vector2(50, 100));
            InitAttackableItem(new Vector2(150, 100));

            Attackable.AttackableHealth = 100;
            Attackable.AttackableRadius = 20;
            Attacker.AttackRange = 20;
            Attacker.AttackRecoveryTime = 0;
            Attacker.AttackStrength = 5;

            AttackableProperty expectedTarget = null;
            int targetChanged = 0;
            int attackerHit = 0;
            int expectedAttackerHit = 0;
            Attacker.OnAttackTargetChanged += (s, v) => 
            {
                Assert.AreEqual(expectedTarget, v);
                targetChanged++;
            };
            Attacker.OnAttackHit += (s, v) =>
            {
                Assert.AreEqual(Attackable.Item, s);
                Assert.AreEqual(expectedAttackerHit, v);
                attackerHit++;
            };

            int newAttackerCount = 0;
            int lostAttackerCount = 0;
            int attackerCount = 0;
            int healthChangedCount = 0;
            int attackableHit = 0;
            int expectedHealth = 0;
            int expectedAttackableHit = 0;
            Attackable.OnNewAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                newAttackerCount++;
            };
            Attackable.OnLostAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                lostAttackerCount++;
            };
            Attackable.OnAttackerItem += (v) => 
            {
                Assert.AreEqual(Attacker, v);
                attackerCount++;
            };
            Attackable.OnAttackableHealthChanged += (s, v) => 
            {
                Assert.AreEqual(expectedHealth, v);
                healthChangedCount++;
            };
            Attackable.OnAttackerHit += (s, v) =>
            {
                Assert.AreEqual(Attacker.Item, s);
                Assert.AreEqual(expectedAttackableHit, v);
                attackableHit++;
            };

            // Angriff auf hohe Distanz (out of range)
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 100;

            Attacker.Attack(Attackable);
            Engine.Update();
            
            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount = 
                attackerCount = healthChangedCount = attackerHit = 
                attackableHit = 0;

            // Angriff auf kurze Distanz (in range)
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 95;

            AttackerItem.Position = new Vector3(130, 100, 0);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;
            
            // Angriff auf kurze Distanz (in range)
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 90;

            AttackerItem.Position = new Vector3(140, 100, 0);
            Engine.Update();
            
            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // Bewegung außerhalb des Angriffsradius
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 90;

            AttackerItem.Position = new Vector3(100, 100, 0);
            Engine.Update();
            
            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(1, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // Bewegung zurück in Range
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 85;

            AttackerItem.Position = new Vector3(140, 100, 0);
            Engine.Update();
            
            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // Stop
            expectedTarget = null;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 85;

            Attacker.StopAttack();
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(1, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;
        }

        /// <summary>
        /// Bildet den Ablauf eines Kampfes ab bei dem der Angegriffene 
        /// plötzlich stirbt.
        /// </summary>
        [TestMethod]
        public void AttackFlowAttackableDeath()
        {
            InitAttackerItem(new Vector2(140, 100));
            InitAttackableItem(new Vector2(150, 100));

            Attackable.AttackableHealth = 10;
            Attackable.AttackableRadius = 20;
            Attacker.AttackRange = 20;
            Attacker.AttackRecoveryTime = 0;
            Attacker.AttackStrength = 5;

            AttackableProperty expectedTarget = null;
            int targetChanged = 0;
            int attackerHit = 0;
            int expectedAttackerHit = 0;
            Attacker.OnAttackTargetChanged += (s, v) =>
            {
                Assert.AreEqual(expectedTarget, v);
                targetChanged++;
            };
            Attacker.OnAttackHit += (s, v) =>
            {
                Assert.AreEqual(Attackable.Item, s);
                Assert.AreEqual(expectedAttackerHit, v);
                attackerHit++;
            };

            int newAttackerCount = 0;
            int lostAttackerCount = 0;
            int attackerCount = 0;
            int healthChangedCount = 0;
            int attackableHit = 0;
            int expectedHealth = 0;
            int expectedAttackableHit = 0;
            int killCount = 0;
            Attackable.OnNewAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                newAttackerCount++;
            };
            Attackable.OnLostAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                lostAttackerCount++;
            };
            Attackable.OnAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                attackerCount++;
            };
            Attackable.OnAttackableHealthChanged += (s, v) =>
            {
                Assert.AreEqual(expectedHealth, v);
                healthChangedCount++;
            };
            Attackable.OnAttackerHit += (s, v) =>
            {
                Assert.AreEqual(Attacker.Item, s);
                Assert.AreEqual(expectedAttackableHit, v);
                attackableHit++;
            };
            Attackable.OnKill += (s) =>
            {
                killCount++;
            };

            // Erste Runde (5 Hitpoints übrig)
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 5;

            Attacker.Attack(Attackable);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(0, killCount);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = killCount = 0;

            // Zweite Runde (0 Hitpoints übrig -> Death)
            expectedTarget = null;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 0;

            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(1, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(1, killCount);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(null, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;
        }

        /// <summary>
        /// Bildet den Ablauf eines Kampfes an bei dem der Angreifer plötzlich 
        /// stirbt.
        /// </summary>
        [TestMethod]
        public void AttackFlowAttackerDeath()
        {
            InitAttackerItem(new Vector2(140, 100));
            InitAttackableItem(new Vector2(150, 100));

            Attackable.AttackableHealth = 10;
            Attackable.AttackableRadius = 20;
            Attacker.AttackRange = 20;
            Attacker.AttackRecoveryTime = 0;
            Attacker.AttackStrength = 5;

            AttackableProperty expectedTarget = null;
            int targetChanged = 0;
            int attackerHit = 0;
            int expectedAttackerHit = 0;
            Attacker.OnAttackTargetChanged += (s, v) =>
            {
                Assert.AreEqual(expectedTarget, v);
                targetChanged++;
            };
            Attacker.OnAttackHit += (s, v) =>
            {
                Assert.AreEqual(Attackable.Item, s);
                Assert.AreEqual(expectedAttackerHit, v);
                attackerHit++;
            };

            int newAttackerCount = 0;
            int lostAttackerCount = 0;
            int attackerCount = 0;
            int healthChangedCount = 0;
            int attackableHit = 0;
            int expectedHealth = 0;
            int expectedAttackableHit = 0;
            Attackable.OnNewAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                newAttackerCount++;
            };
            Attackable.OnLostAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                lostAttackerCount++;
            };
            Attackable.OnAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                attackerCount++;
            };
            Attackable.OnAttackableHealthChanged += (s, v) =>
            {
                Assert.AreEqual(expectedHealth, v);
                healthChangedCount++;
            };
            Attackable.OnAttackerHit += (s, v) =>
            {
                Assert.AreEqual(Attacker.Item, s);
                Assert.AreEqual(expectedAttackableHit, v);
                attackableHit++;
            };

            // Erste Runde (5 Hitpoints übrig)
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 5;

            Attacker.Attack(Attackable);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // Zweite Runde (Attacker entfernt)
            expectedTarget = null;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 5;

            Engine.RemoveItem(Attacker.Item);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(1, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(null, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;
        }

        /// <summary>
        /// Testet die korrekte Arbeitsweise der Angriffsrecovery.
        /// </summary>
        [TestMethod]
        public void AttackRecoveryCheck()
        {
            InitAttackerItem(new Vector2(100, 100));
            InitAttackableItem(new Vector2(150, 100));

            Attackable.AttackableHealth = 100;
            Attackable.AttackableRadius = 20;
            Attacker.AttackRange = 20;
            Attacker.AttackRecoveryTime = 2;
            Attacker.AttackStrength = 5;

            AttackableProperty expectedTarget = null;
            int targetChanged = 0;
            int attackerHit = 0;
            int expectedAttackerHit = 0;
            Attacker.OnAttackTargetChanged += (s, v) =>
            {
                Assert.AreEqual(expectedTarget, v);
                targetChanged++;
            };
            Attacker.OnAttackHit += (s, v) =>
            {
                Assert.AreEqual(Attackable.Item, s);
                Assert.AreEqual(expectedAttackerHit, v);
                attackerHit++;
            };

            int newAttackerCount = 0;
            int lostAttackerCount = 0;
            int attackerCount = 0;
            int healthChangedCount = 0;
            int attackableHit = 0;
            int expectedHealth = 0;
            int expectedAttackableHit = 0;
            Attackable.OnNewAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                newAttackerCount++;
            };
            Attackable.OnLostAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                lostAttackerCount++;
            };
            Attackable.OnAttackerItem += (v) =>
            {
                Assert.AreEqual(Attacker, v);
                attackerCount++;
            };
            Attackable.OnAttackableHealthChanged += (s, v) =>
            {
                Assert.AreEqual(expectedHealth, v);
                healthChangedCount++;
            };
            Attackable.OnAttackerHit += (s, v) =>
            {
                Assert.AreEqual(Attacker.Item, s);
                Assert.AreEqual(expectedAttackableHit, v);
                attackableHit++;
            };

            // Attacker out of range
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 100;

            Attacker.Attack(Attackable);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // move into range
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 100;

            Attacker.Item.Position = new Vector3(140, 100, 0);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(1, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // wait for 9 Rounds
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 100;
            
            Engine.Update(); // zweite Warterunde -> kein Hit

            expectedHealth = 95;
            Engine.Update(); // Hit!
            Engine.Update(); // erste Warterunde -> kein Hit
            Engine.Update(); // zweite Warterunde -> kein Hit

            expectedHealth = 90;
            Engine.Update(); // Hit!
            Engine.Update(); // erste Warterunde -> kein Hit
            Engine.Update(); // zweite Warterunde -> kein Hit

            expectedHealth = 85;
            Engine.Update(); // Hit!
            Engine.Update(); // erste Warterunde -> kein Hit

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(3, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(9, attackerCount);
            Assert.AreEqual(3, healthChangedCount);
            Assert.AreEqual(3, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // cancel attack
            expectedTarget = null;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 85;

            Attacker.StopAttack();
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(1, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // attack again (-> reset)
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 85;

            Attacker.Attack(Attackable);
            Engine.Update();
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(1, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(2, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // wait for recovery-1 and Attack (without stop) (-> no reset)
            expectedTarget = Attackable;
            expectedAttackerHit = 5;
            expectedAttackableHit = 5;
            expectedHealth = 80;

            Attacker.Attack(Attackable);
            Engine.Update(); // Hit, da schon in der zweiten aus vorherigem Test

            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            Engine.Update(); // Erste Warterunde

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(1, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(2, attackerCount);
            Assert.AreEqual(1, healthChangedCount);
            Assert.AreEqual(1, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // move out
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 80;

            Attacker.Item.Position = new Vector3(100, 100, 0);
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(0, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(0, newAttackerCount);
            Assert.AreEqual(1, lostAttackerCount);
            Assert.AreEqual(0, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;

            // back again (-> reset)
            expectedTarget = Attackable;
            expectedAttackerHit = 0;
            expectedAttackableHit = 0;
            expectedHealth = 80;

            Attacker.Item.Position = new Vector3(140, 100, 0);
            Engine.Update();
            Engine.Update();

            Assert.AreEqual(expectedTarget, Attacker.AttackTarget);
            Assert.AreEqual(expectedHealth, Attackable.AttackableHealth);
            Assert.AreEqual(1, Attackable.AttackerItems.Count());
            Assert.AreEqual(0, targetChanged);
            Assert.AreEqual(0, attackerHit);
            Assert.AreEqual(1, newAttackerCount);
            Assert.AreEqual(0, lostAttackerCount);
            Assert.AreEqual(2, attackerCount);
            Assert.AreEqual(0, healthChangedCount);
            Assert.AreEqual(0, attackableHit);
            Assert.AreEqual(Engine, Attacker.Item.Engine);
            Assert.AreEqual(Engine, Attackable.Item.Engine);
            targetChanged = newAttackerCount = lostAttackerCount =
                attackerCount = healthChangedCount = attackerHit =
                attackableHit = 0;            
        }

        #endregion

        #region Collecting

        /// <summary>
        /// Prüft die Reichweite zwischen Collector und Collectable.
        /// </summary>
        [TestMethod]
        public void CollectableRangeCheck()
        {
            InitCollectorItem(new Vector2(50, 100));
            InitCollectableItem(new Vector2(150, 100));

            DebugGood1 collectableGood = new DebugGood1(200, 200);

            Assert.AreEqual(null, collectableGood.Property);
            Collectable.CollectableRadius = 20;
            Collectable.AddCollectableGood(collectableGood);
            Assert.AreEqual(Collectable, collectableGood.Property);

            DebugGood1 collectorGood = new DebugGood1(100, 0);

            Collector.CollectorRange = 20;
            Collector.AddCollectableGood(collectorGood);
            int result = 0;

            // Collect from out of range
            result = Collector.Collect<DebugGood1>(Collectable, 10);
            Assert.AreEqual(0, result);
            Assert.AreEqual(200, collectableGood.Amount);
            Assert.AreEqual(0, collectorGood.Amount);

            // Collect within range
            Collector.Item.Position = new Vector3(110, 100, 0);
            result = Collector.Collect<DebugGood1>(Collectable, 10);
            Assert.AreEqual(10, result);
            Assert.AreEqual(190, collectableGood.Amount);
            Assert.AreEqual(10, collectorGood.Amount);

            // Collect too much
            result = Collector.Collect<DebugGood1>(Collectable, 100);
            Assert.AreEqual(90, result);
            Assert.AreEqual(100, collectableGood.Amount);
            Assert.AreEqual(100, collectorGood.Amount);

            // Give something
            result = Collector.Give<DebugGood1>(Collectable, 20);
            Assert.AreEqual(20, result);
            Assert.AreEqual(120, collectableGood.Amount);
            Assert.AreEqual(80, collectorGood.Amount);

            // Give too much (less than available)
            result = Collector.Give<DebugGood1>(Collectable, 200);
            Assert.AreEqual(80, result);
            Assert.AreEqual(200, collectableGood.Amount);
            Assert.AreEqual(0, collectorGood.Amount);

            // Give too much (more than acceptable)
            collectorGood.Capacity = 200;
            collectorGood.Amount = 200;
            collectableGood.Amount = 150;
            result = Collector.Give<DebugGood1>(Collectable, 200);
            Assert.AreEqual(50, result);
            Assert.AreEqual(200, collectableGood.Amount);
            Assert.AreEqual(150, collectorGood.Amount);

            // Give from out of range
            Collector.Item.Position = new Vector3(50, 100, 0);
            collectableGood.Amount = 100;
            result = Collector.Give<DebugGood1>(Collectable, 10);
            Assert.AreEqual(0, result);
            Assert.AreEqual(100, collectableGood.Amount);
            Assert.AreEqual(150, collectorGood.Amount);
        }

        /// <summary>
        /// Prüft das Verhalten bei nicht passenden Gütern
        /// </summary>
        [TestMethod]
        public void GoodMatching()
        {
            // TODO:
            throw new NotImplementedException();
        }

        public void TransferCheck()
        {
            // TODO
            throw new NotImplementedException();

            // Transfer ohne Match
            // Transder mit einem Match aber 0 Amount
            // Transfer mit einem match aber zu wenig kapazität
            // Transfer mit mehreren Matches die alle passen
            // Transfer mit mehreren Matches bei Defiziten im Ziel
            // Transfer mit mehreren Matches mit Defiziten in der QUelle
        }

        [TestMethod]
        public void TransferGoodCheck()
        {
            // TODO
            throw new NotImplementedException();

            // Transfer mit falschen Typen
            // Transfer mit 0 Amount
            // Transfer mit zu wenig Zielkapazität
        }

        /// <summary>
        /// Testet die Events der Ressourcen
        /// </summary>
        [TestMethod]
        public void AmountCapacityInteraction()
        {
            DebugGood1 good = new DebugGood1(100, 0);
            good.Capacity = int.MaxValue;

            int amountCount = 0;
            int capacityCount = 0;
            int amountExpected = 0;
            int capacityExcepted = 0;
            good.OnAmountChanged += (g, v) =>
            {
                Assert.AreEqual(good, g);
                Assert.AreEqual(amountExpected, v);
                amountCount++;
            };

            good.OnCapacityChanged += (g, v) =>
            {
                Assert.AreEqual(good, g);
                Assert.AreEqual(capacityExcepted, v);
                capacityCount++;
            };

            // Initialwert Value
            capacityExcepted = 0;
            amountExpected = 0;
            good.Amount = 0;
            good.Capacity = 0;
            Assert.AreEqual(1, amountCount);
            Assert.AreEqual(1, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;

            // raise Capacity (to 100)
            capacityExcepted = 100;
            amountExpected = 0;
            good.Capacity = 100;
            Assert.AreEqual(0, amountCount);
            Assert.AreEqual(1, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;

            // raise Amount within range (50)
            capacityExcepted = 100;
            amountExpected = 50;
            good.Amount = 50;
            Assert.AreEqual(1, amountCount);
            Assert.AreEqual(0, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
            
            // raise Amount to range (100)
            capacityExcepted = 100;
            amountExpected = 100;
            good.Amount = 100;
            Assert.AreEqual(1, amountCount);
            Assert.AreEqual(0, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
            
            // raise Amount over range (150)
            capacityExcepted = 100;
            amountExpected = 100;
            good.Amount = 150;
            Assert.AreEqual(1, amountCount);
            Assert.AreEqual(0, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
            
            // raise Capacity (to 150)
            capacityExcepted = 150;
            amountExpected = 100;
            good.Capacity = 150;
            Assert.AreEqual(0, amountCount);
            Assert.AreEqual(1, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
            
            // lower capacity (to 50)
            capacityExcepted = 50;
            amountExpected = 50;
            good.Capacity = 50;
            Assert.AreEqual(1, amountCount);
            Assert.AreEqual(1, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
            
            // lower Amount to Zero
            capacityExcepted = 50;
            amountExpected = 0;
            good.Amount = 0;
            Assert.AreEqual(1, amountCount);
            Assert.AreEqual(0, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
            
            // lower Capacity to Zero
            capacityExcepted = 0;
            amountExpected = 0;
            good.Capacity = 0;
            Assert.AreEqual(0, amountCount);
            Assert.AreEqual(1, capacityCount);
            Assert.AreEqual(amountExpected, good.Amount);
            Assert.AreEqual(capacityExcepted, good.Capacity);
            amountCount = capacityCount = 0;
        }

        #endregion
    }
}
