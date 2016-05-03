using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Prüft, ob die Zuweisung zwischen Int (Gradangabe) und dem Compass-Enum stimmt
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class CompassTest
    {
        /// <summary>
        /// Prüft die Konvertierung von Compass zu Int (Gradangabe)
        /// </summary>
        [TestMethod]
        public void DirectionToInt()
        {
            Assert.AreEqual((int)Compass.East, 0);
            Assert.AreEqual((int)Compass.SouthEast, 45);
            Assert.AreEqual((int)Compass.South, 90);
            Assert.AreEqual((int)Compass.SouthWest, 135);
            Assert.AreEqual((int)Compass.West, 180);
            Assert.AreEqual((int)Compass.NorthWest, 225);
            Assert.AreEqual((int)Compass.North, 270);
            Assert.AreEqual((int)Compass.NorthEast, 315);
        }

        /// <summary>
        /// Prüft die Konvertierung von exakten (!) Gradangaben zu Compass
        /// </summary>
        [TestMethod]
        public void IntToDirection()
        {
            Assert.AreEqual((Compass)0, Compass.East);
            Assert.AreEqual((Compass)45, Compass.SouthEast);
            Assert.AreEqual((Compass)90, Compass.South);
            Assert.AreEqual((Compass)135, Compass.SouthWest);
            Assert.AreEqual((Compass)180, Compass.West);
            Assert.AreEqual((Compass)225, Compass.NorthWest);
            Assert.AreEqual((Compass)270, Compass.North);
            Assert.AreEqual((Compass)315, Compass.NorthEast);
        }
    }
}
