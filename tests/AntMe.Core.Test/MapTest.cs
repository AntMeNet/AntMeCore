using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Prüft die Funktionalität der Map-Klasse und deren Caching-Mechanismen 
    /// richtig funktionieren.
    /// Autor: Tom Wendel
    /// Status: Implemented
    /// </summary>
    [TestClass]
    public class MapTest
    {
        private Map Map;

        #region Init

        [TestInitialize]
        public void InitMap()
        {
            Map = Map.CreateMap(20, 20, true);
        }

        [TestCleanup]
        public void CleanupMap()
        {
            Map = null;
        }

        #endregion

        #region Validation

        [TestMethod]
        public void ValidationClean()
        {
            Map.CheckMap();
        }

        [TestMethod]
        public void ValidatePlayerCountMin()
        {
            Map.StartPoints = new Index2[0];
            Validate();
        }

        [TestMethod]
        public void ValidatePlayerCountMax()
        {
            Map.StartPoints = new Index2[9];
            Validate();
        }

        [TestMethod]
        public void ValidatePlayerStartpointRange()
        {
            // Player zu weit unten
            Map.StartPoints[0] = new Index2(0, (Map.GetCellCount().Y) + 1);
            Validate();

            // Player zu weit rechts
            Map.StartPoints[0] = new Index2((Map.GetCellCount().X) + 1, 0);
            Validate();

            // Player zu weit oben
            Map.StartPoints[0] = new Index2(0, -3);
            Validate();

            // Player zu weit links
            Map.StartPoints[0] = new Index2(-5, 0);
            Validate();
        }

        /// <summary>
        /// Überprüft die minimal- und maximal-Grenzen für die Anzahl Zellen
        /// </summary>
        [TestMethod]
        public void CheckCellDimensions()
        {
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    Map.Tiles = new MapTile[x, y];
                    if (x < Map.MIN_WIDTH || x > Map.MAX_WIDTH || y < Map.MIN_HEIGHT || y > Map.MAX_HEIGHT)
                    {
                        Validate();
                    }
                }
            }
        }

        /// <summary>
        /// Prüft die Validierung kollidierender Startpoints.
        /// </summary>
        [TestMethod]
        public void ValidateCollidingStartpoints()
        {
            Map.StartPoints[0] = new Index2(0, 0);
            Map.StartPoints[1] = new Index2(0, 0);
            Validate();

            Map.StartPoints[0] = new Index2(4, 6);
            Map.StartPoints[5] = new Index2(4, 6);
            Validate();
        }

        private void Validate()
        {
            try
            {
                Map.CheckMap();
                Assert.Fail("Map should throw an Exception");
            }
            catch (InvalidMapException) { }
        }

        #endregion

        #region Static Stuff

        [TestMethod]
        public void MapStaticGetHeight()
        {
            // TODO

            MapTile tile = new MapTile();
            float max = Map.CELLSIZE - 0.001f;

            // Plane
            tile.Height = TileHeight.Low;
            tile.Shape = TileShape.Flat;
            Assert.AreEqual(MapTile.HEIGHT_LOW, Map.GetHeight(tile, new Vector2(0, 0)), 0.001f);
            Assert.AreEqual(MapTile.HEIGHT_LOW, Map.GetHeight(tile, new Vector2(max, 0)), 0.001f);
            Assert.AreEqual(MapTile.HEIGHT_LOW, Map.GetHeight(tile, new Vector2(0, max)), 0.001f);
            Assert.AreEqual(MapTile.HEIGHT_LOW, Map.GetHeight(tile, new Vector2(max, max)), 0.001f);

            tile.Height = TileHeight.Medium;
            throw new NotImplementedException();

        }

        [TestMethod]
        public void CheckSerialization()
        {
            // TODO: Random karte einmal serializieren und zurück
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CheckCreateMapMethods()
        {
            // TODO: ein paar Maps erzeugen und schauen, ob alles gut ist
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CheckGetCellIndex()
        {
            // TODO: Vector2, 3, Out of Bounds
            throw new NotImplementedException();
        }

        /// <summary>
        /// Überprüft, ob die richtige Anzahl Zellen zurückgeliefert wird.
        /// </summary>
        [TestMethod]
        public void CheckCellCount()
        {
            for (int x = Map.MIN_WIDTH; x <= Map.MAX_WIDTH; x++)
            {
                for (int y = Map.MIN_HEIGHT; y <= Map.MAX_HEIGHT; y++)
                {
                    Map.Tiles = new MapTile[x, y];
                    Index2 cells = Map.GetCellCount();
                    Assert.AreEqual(x, cells.X);
                    Assert.AreEqual(y, cells.Y);
                }
            }
        }

        /// <summary>
        /// Prüft die automatische Berechnung 
        /// </summary>
        [TestMethod]
        public void CheckCellSze()
        {
            for (int x = Map.MIN_WIDTH; x <= Map.MAX_WIDTH; x++)
            {
                for (int y = Map.MIN_HEIGHT; y <= Map.MAX_HEIGHT; y++)
                {
                    Map.Tiles = new MapTile[x, y];
                    Vector2 size = Map.GetSize();
                    Assert.AreEqual(x * Map.CELLSIZE, size.X, Vector2.EPS_MIN);
                    Assert.AreEqual(y * Map.CELLSIZE, size.Y, Vector2.EPS_MIN);
                }
            }

        }

        #endregion
    }
}
