using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet die komplette Funktion des Index2-Structs
    /// Author: Tom Wendel
    /// </summary>
    [TestClass]
    public class Index2Test
    {
        #region Konstruktor

        [TestMethod]
        public void EmptyContructor()
        {
            Index2 x = new Index2();

            Assert.AreEqual(0, x.X);
            Assert.AreEqual(0, x.Y);
        }

        [TestMethod]
        public void ContructorParameter()
        {
            for (int x = -1000; x < 1000; x++)
            {
                for (int y = -1000; y < 1000; y++)
                {
                    Index2 value = new Index2(x, y);
                    Assert.AreEqual(x, value.X);
                    Assert.AreEqual(y, value.Y);
                }
            }
        }

        #endregion

        #region Operator

        [TestMethod]
        public void AddOperator()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Index2 value1 = new Index2(x1, y1);
                            Index2 value2 = new Index2(x2, y2);
                            Index2 result = value1 + value2;

                            Assert.AreEqual(x1 + x2, result.X);
                            Assert.AreEqual(y1 + y2, result.Y);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SubtractOperator()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Index2 value1 = new Index2(x1, y1);
                            Index2 value2 = new Index2(x2, y2);
                            Index2 result = value1 - value2;

                            Assert.AreEqual(x1 - x2, result.X);
                            Assert.AreEqual(y1 - y2, result.Y);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void MultipyOperator()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int scale = -10; scale < 10; scale++)
                    {
                        Index2 value1 = new Index2(x1, y1);
                        Index2 result = value1 * scale;

                        Assert.AreEqual(x1 * scale, result.X);
                        Assert.AreEqual(y1 * scale, result.Y);
                    }
                }
            }
        }

        [TestMethod]
        public void DivideOperator()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int scale = -10; scale < 10; scale++)
                    {
                        if (scale == 0) continue;

                        Index2 value1 = new Index2(x1, y1);
                        Index2 result = value1 / scale;

                        Assert.AreEqual(x1 / scale, result.X);
                        Assert.AreEqual(y1 / scale, result.Y);
                    }
                }
            }
        }

        [TestMethod]
        public void Comparer()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Index2 value1 = new Index2(x1, y1);
                            Index2 value2 = new Index2(x2, y2);
                            bool result = value1 == value2;

                            Assert.AreEqual(x1 == x2 && y1 == y2, result);
                        }
                    }
                }
            }
        }

        #endregion

        #region Consts

        [TestMethod]
        public void CheckConsts()
        {
            Assert.AreEqual(-1, Index2.UpperLeft.X);
            Assert.AreEqual(-1, Index2.UpperLeft.Y);

            Assert.AreEqual(0, Index2.Up.X);
            Assert.AreEqual(-1, Index2.Up.Y);

            Assert.AreEqual(1, Index2.UpperRight.X);
            Assert.AreEqual(-1, Index2.UpperRight.Y);

            Assert.AreEqual(-1, Index2.Left.X);
            Assert.AreEqual(0, Index2.Left.Y);

            Assert.AreEqual(0, Index2.Zero.X);
            Assert.AreEqual(0, Index2.Zero.Y);

            Assert.AreEqual(1, Index2.Right.X);
            Assert.AreEqual(0, Index2.Right.Y);

            Assert.AreEqual(-1, Index2.LowerLeft.X);
            Assert.AreEqual(1, Index2.LowerLeft.Y);

            Assert.AreEqual(0, Index2.Down.X);
            Assert.AreEqual(1, Index2.Down.Y);

            Assert.AreEqual(1, Index2.LowerRight.X);
            Assert.AreEqual(1, Index2.LowerRight.Y);
        }

        #endregion
    }
}
