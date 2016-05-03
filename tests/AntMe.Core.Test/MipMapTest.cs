using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AntMe.Core.Test
{
    /// <summary>
    /// TODO!
    /// </summary>
    [TestClass]
    public class MipMapTest
    {
        private MipMap<int> mipmap;
        private const float height = 50.5f;
        private const float width = 100.0f;

        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>
        [TestInitialize]
        public void Initialize()
        {
            mipmap = new MipMap<int>(width, height);
        }

        /// <summary>
        /// This test adds two layers to the mipmap and checks the order of the layers afterwards.
        /// </summary>
        [TestMethod]
        public void AddMipMapLayer()
        {
            float maxrad1 = 1.0f;
            float maxrad2 = 10.0f;

            // add the layers in normal order
            mipmap.AddLayer(maxrad1);
            mipmap.AddLayer(maxrad2);

            // check whether the mipmap ordered them by radius
            Assert.AreEqual(float.PositiveInfinity, mipmap.Layers[0].MaxRadius);
            Assert.AreEqual(maxrad1, mipmap.Layers[2].MaxRadius);
            Assert.AreEqual(maxrad2, mipmap.Layers[1].MaxRadius);
        }

        /// <summary>
        /// Tests whether the add method really adds objects by adding two and trying to retrieve them again.
        /// </summary>
        [TestMethod]
        public void AddObjectToMipMapLayer()
        {
            int int1 = 1;
            int int2 = 2;

            mipmap.Add(int1, new Vector3(10, 10, 0), 10);
            mipmap.Add(int2, new Vector3(5, 10, 0), 10);

            HashSet<int> items = mipmap.FindAll(new Vector3(10, 10, 0), 10);
            Assert.AreEqual(2, items.Count, "Could not retrieve two items");
            Assert.IsTrue(items.Contains(int1), "Could not retrieve the first item");
            Assert.IsTrue(items.Contains(int2), "Could not retrieve the second item");
        }

        /// <summary>
        /// Tests whether the add method really adds objects to different layers by adding two objects with different sizes and trying to retrieve them again.
        /// </summary>
        [TestMethod]
        public void AddObjectToMipMapLayers()
        {
            // add a layer first
            float maxRadius = 10.0f;
            mipmap.AddLayer(maxRadius);

            int int1 = 1;
            int int2 = 2;

            mipmap.Add(int1, new Vector3(10, 10, 0), maxRadius*2);
            mipmap.Add(int2, new Vector3(5, 10, 0), maxRadius/2);

            HashSet<int> items = mipmap.FindAll(new Vector3(10, 10, 0), 10);
            Assert.AreEqual(2, items.Count, "Could not retrieve two items");
            Assert.IsTrue(items.Contains(int1), "Could not retrieve the first item");
            Assert.IsTrue(items.Contains(int2), "Could not retrieve the second item");
        }
    }
}
