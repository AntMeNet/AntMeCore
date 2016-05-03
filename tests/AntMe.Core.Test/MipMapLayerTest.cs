using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AntMe.Core.Test
{
    [TestClass]
    public class MipMapLayerTest
    {
        private const float maxRadius = 10.0f;
        private const float height = 50.5f;
        private const float width = 100.0f;

        private MipMapLayer<int> layer;

        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>
        [TestInitialize]
        public void Initialize()
        {
            layer = new MipMapLayer<int>(width, height, maxRadius);
        }

        /// <summary>
        /// Tests whether the layer cells all get a clean empty list and that the cell widths etc are all computed correctly.
        /// </summary>
        [TestMethod]
        public void InitializeMipMapLayer()
        {
            Assert.AreEqual(width, layer.Width, "Width is not correct.");
            Assert.AreEqual(height, layer.Height, "Height is not correct.");

            Assert.AreEqual(maxRadius, layer.MaxRadius, "Rejected radius is not correct.");

            Assert.AreEqual((int)(width / maxRadius), layer.WidthCells, "WidthCells is not correct.");
            Assert.AreEqual((int)(height / maxRadius), layer.HeightCells, "HeightCells is not correct.");

            Assert.AreEqual(height / (int)(height / Math.Max(maxRadius, 1.0)), layer.CellHeight, "Cell height is not correct.");
            Assert.AreEqual(width / (int)(width / Math.Max(maxRadius, 1.0)), layer.CellWidth, "Cell width is not correct.");
        }

        /// <summary>
        /// Tests whether the add method really adds objects by adding two and trying to retrieve them again.
        /// </summary>
        [TestMethod]
        public void AddObjectToMipMapLayer()
        {
            int int1 = 1;
            int int2 = 2;

            layer.Add(int1, new Vector3(10,10, 0), 10);
            layer.Add(int2, new Vector3(5, 10, 0), 10);

            HashSet<int> items = layer.FindAll(new Vector3(10, 10, 0), 10);
            Assert.AreEqual(2, items.Count, "Could not retrieve two items");
            Assert.IsTrue(items.Contains(int1), "Could not retrieve the first item");
            Assert.IsTrue(items.Contains(int2), "Could not retrieve the second item");
        }
    }
}
