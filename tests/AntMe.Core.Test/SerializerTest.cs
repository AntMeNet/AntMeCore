//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace AntMe.Simulation.Test
//{
//    /// <summary>
//    /// Testet die beiden Klassen StateSerializer und StateDeserializer
//    /// </summary>
//    [TestClass]
//    public class SerializerTest
//    {
//        #region Init

//        private StateSerializer _serializer;
//        private StateDeserializer _deserializer;

//        [TestInitialize]
//        public void InitSerializer()
//        {
//            _serializer = new StateSerializer();
//            _deserializer = new StateDeserializer();
//        }

//        [TestCleanup]
//        public void CleanupSerializer()
//        {
//            _serializer.Dispose();
//            _serializer = null;

//            _deserializer.Dispose();
//            _deserializer = null;
//        }

//        public SerializerTest()
//        {
//        }

//        private MainState GetBasicState()
//        {
//            MainState state = new MainState();
//            state.Mode = LevelMode.Running;
//            state.Map = new MapState();
//            state.Map.Tiles = new MapTile[1, 1];

//            return state;
//        }

//        private void CompareStates(MainState input, MainState output)
//        {
//            Assert.IsNotNull(input);
//            Assert.IsNotNull(output);

//            // Lists
//            Assert.IsNotNull(output.Factions);
//            Assert.IsNotNull(output.Items);
//            Assert.IsNotNull(output.ScreenHighlights);

//            // MainState
//            Assert.AreEqual(input.Date.ToString("u"), output.Date.ToString("u"));
//            Assert.AreEqual(input.Mode, output.Mode);
//            Assert.AreEqual(input.Round, output.Round);

//            // Map
//            Assert.IsNotNull(output.Map);
//            for (int x = 0; x < input.Map.Tiles.GetLength(0); x++)
//            {
//                for (int y = 0; y < input.Map.Tiles.GetLength(1); y++)
//                {
//                    Assert.AreEqual(input.Map.Tiles[x, y], output.Map.Tiles[x, y]);
//                    Assert.AreEqual(input.Map.Tiles[x, y], output.Map.Tiles[x, y]);
//                }
//            }

//            // Factions
//            Assert.AreEqual(input.Factions.Count, output.Factions.Count);
//            for (int i = 0; i < input.Factions.Count; i++)
//            {
//                Assert.AreEqual(input.Factions[i].Name, output.Factions[i].Name);
//                Assert.AreEqual(input.Factions[i].PlayerIndex, output.Factions[i].PlayerIndex);
//                Assert.AreEqual(input.Factions[i].Points, output.Factions[i].Points);
//            }

//            // Items
//            Assert.AreEqual(input.Items.Count, output.Items.Count);
//            for (int i = 0; i < input.Items.Count; i++)
//            {
//                Assert.AreEqual(input.Items[i].Id, output.Items[i].Id);
//                Assert.AreEqual(input.Items[i].Position, output.Items[i].Position);
//            }

//            // TODO: ScreenHighlights
//        }

//        private void TestFrame(Action<MainState> fillMethod)
//        {
//            TestFrames(fillMethod);
//        }

//        private void TestFrame2(Action<MainState> fillMethod, Action<MainState> updateMethod)
//        {
//            TestFrames(fillMethod, updateMethod);
//        }

//        private void TestFrames(params Action<MainState>[] updates)
//        {
//            MainState input = GetBasicState();
//            MainState output = null;

//            foreach (var item in updates)
//            {
//                item(input);

//                byte[] data = _serializer.Serialize(input);
//                output = _deserializer.Deserialize(data);
//            }

//            CompareStates(input, output);
//        }

//        #endregion

//        [TestMethod]
//        public void EmptyStateFirst()
//        {
//            TestFrame((state) => 
//            {
//            });
//        }

//        [TestMethod]
//        public void EmptyStateUpdate()
//        {
//            TestFrame2((state) => 
//            { 
//            }, (state) =>
//            {
//                state.Round++;
//                state.Date = DateTimeOffset.Now;
//                state.Mode = LevelMode.FinishedPlayer1;
//            });
//        }

//        [TestMethod]
//        public void FactionStateFirst()
//        {
//            AntFactionState antFaction = new AntFactionState() {
//                Name = "Howard",
//                PlayerIndex = 1,
//                Points = 1000
//            };

//            TestFrame((state) =>
//            {
//                state.Factions.Add(antFaction);
//            });
//        }

//        [TestMethod]
//        public void FactionStateUpdate()
//        {
//            AntFactionState antFaction = new AntFactionState()
//            {
//                Name = "Howard",
//                PlayerIndex = 1,
//                Points = 1000
//            };

//            TestFrame2((state) =>
//            {
//                state.Factions.Add(antFaction);
//            }, (state) => 
//            {
//                antFaction.Points = 1200;
//            });
//        }

//        [TestMethod]
//        public void FactionStatesFirst()
//        {
//            AntFactionState antFaction = new AntFactionState()
//            {
//                Name = "Howard",
//                PlayerIndex = 1,
//                Points = 1000
//            };
            
//            BugFactionState bugFaction = new BugFactionState()
//            {
//                Name = "Buggies",
//                PlayerIndex = 4,
//                Points = 522
//            };

//            TestFrame((state) =>
//            {
//                state.Factions.Add(antFaction);
//                state.Factions.Add(bugFaction);
//            });
//        }

//        [TestMethod]
//        public void FractionStatesUpdate()
//        {
//            AntFactionState antFaction = new AntFactionState()
//            {
//                Name = "Howard",
//                PlayerIndex = 1,
//                Points = 1000
//            };

//            BugFactionState bugFaction = new BugFactionState()
//            {
//                Name = "Buggies",
//                PlayerIndex = 4,
//                Points = 522
//            };

//            TestFrame2((state) =>
//            {
//                state.Factions.Add(antFaction);
//                state.Factions.Add(bugFaction);
//            }, (state) =>
//            {
//                state.Round++;
//                state.Date = DateTimeOffset.Now;

//                antFaction.Points = 1111;
//                bugFaction.Points = 732;
//            });
//        }

//        [TestMethod]
//        public void ItemStateFirst()
//        {
//            SugarState sugarState = new SugarState()
//            {
//                Id = 22,
//                Position = new Core.Vector3(10, 10, 0),
//                Radius = 2f
//            };

//            TestFrame((state) =>
//            {
//                state.Items.Add(sugarState);
//            });
//        }

//        [TestMethod]
//        public void ItemStatesFirst()
//        {
//            SugarState sugarState = new SugarState()
//            {
//                Id = 22,
//                Position = new Core.Vector3(10, 10, 0),
//                Radius = 2f
//            };

//            AppleState appleState = new AppleState()
//            {
//                Id = 23,
//                Position = new Core.Vector3(30, 30, 0),
//                Radius = 2f
//            };

//            TestFrame((state) =>
//            {
//                state.Items.Add(sugarState);
//                state.Items.Add(appleState);
//            });
//        }

//        [TestMethod]
//        public void ItemStateUpdate()
//        {
//            SugarState sugarState = new SugarState()
//            {
//                Id = 22,
//                Position = new Core.Vector3(10, 10, 0),
//                Radius = 2f
//            };

//            TestFrame2((state) =>
//            {
//                state.Items.Add(sugarState);
//            }, (state) =>
//            {
//                sugarState.Position = new Core.Vector3(12, 12, 0);
//            });
//        }

//        [TestMethod]
//        public void ItemStatesUpdate()
//        {
//            SugarState sugarState = new SugarState()
//            {
//                Id = 22,
//                Position = new Core.Vector3(10, 10, 0),
//                Radius = 2f
//            };

//            AppleState appleState = new AppleState()
//            {
//                Id = 23,
//                Position = new Core.Vector3(30, 30, 0),
//                Radius = 2f
//            };

//            TestFrame2((state) =>
//            {
//                state.Items.Add(sugarState);
//            }, (state) =>
//            {
//                sugarState.Position = new Core.Vector3(12, 12, 0);
//                sugarState.Radius = 3f;
//                appleState.Position = new Core.Vector3(28, 28, 0);
//            });
//        }

//        [TestMethod]
//        public void ComplexSeries()
//        {
//            AntState ant = new AntState()
//            {
//                Id = 33,
//                PlayerIndex = 2,
//                Position = new Core.Vector3(100, 100, 0),
//                Radius = 2
//            };

//            MarkerState marker = new MarkerState()
//            {
//                Id = 34,
//                PlayerIndex = 2,
//                Position = new Core.Vector3(102, 102, 0)
//            };

//            TestFrames(
//                (state) => { state.Items.Add(ant); },
//                (state) => { ant.Position = new Core.Vector3(104, 104, 0); },
//                (state) => { ant.Position = new Core.Vector3(104, 104, 0); },
//                (state) => { ant.Position = new Core.Vector3(104, 106, 0); },
//                (state) => { ant.Position = new Core.Vector3(105, 106, 0); },
//                (state) => { ant.Position = new Core.Vector3(90, 106, 0); },
//                (state) => { state.Items.Add(marker); },
//                (state) => { state.Items.Remove(ant); }
//                );

//        }
//    }
//}
