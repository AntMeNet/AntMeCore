using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Überprüft die richtige Arbeitsweise von Vektor2.
    /// Autor: Tom Wendel
    /// Status: Implemented
    /// </summary>
    [TestClass]
    public class Vector2Test
    {
        #region Static Stuff

        /// <summary>
        /// Überprüft die Rückgabewerte der statischen Werte.
        /// </summary>
        [TestMethod]
        public void StaticValues()
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = (i * ((float)Math.PI / 4));
                
                Vector2 value = new Vector2();
                switch (i)
                {
                    case 0: value = Vector2.Right; break;
                    case 1: value = Vector2.LowerRight; break;
                    case 2: value = Vector2.Down; break;
                    case 3: value = Vector2.LowerLeft; break;
                    case 4: value = Vector2.Left; break;
                    case 5: value = Vector2.UpperLeft; break;
                    case 6: value = Vector2.Up; break;
                    case 7: value = Vector2.UpperRight; break;
                    default: throw new Exception("Error in Test-Code");
                }

                Assert.AreEqual(Math.Cos(angle), value.X, Vector2.EPS_MIN);
                Assert.AreEqual(Math.Sin(angle), value.Y, Vector2.EPS_MIN);
            }

            Vector2 zero = Vector2.Zero;
            Assert.AreEqual(0f, zero.X, Vector2.EPS_MIN);
            Assert.AreEqual(0f, zero.Y, Vector2.EPS_MIN);
        }

        /// <summary>
        /// Überprüft die Erstellung eines Vektors aus einem Winkel heraus.
        /// </summary>
        [TestMethod]
        public void StaticFromAngle()
        {
            for (int i = 0; i < 360; i++)
            {
                Angle a = Angle.FromDegree(i);
                Vector2 v = Vector2.FromAngle(a);

                float angle = ((float)i / 360) * ((float)Math.PI * 2);
                Assert.AreEqual(Math.Cos(angle), v.X, Vector2.EPS_MIN);
                Assert.AreEqual(Math.Sin(angle), v.Y, Vector2.EPS_MIN);                
            }
        }

        #endregion

        #region Basics

        /// <summary>
        /// Überprüft die verschiedenen Konstruktoren.
        /// </summary>
        [TestMethod]
        public void ConstructorCheck()
        {
            // Empty Construktor
            Vector2 v = new Vector2();
            Assert.AreEqual(0, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(0, v.Y, Vector2.EPS_MIN);

            // Zero
            v = new Vector2(0, 0);
            Assert.AreEqual(0, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(0, v.Y, Vector2.EPS_MIN);

            // Positive low
            v = new Vector2(10, 21);
            Assert.AreEqual(10, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(21, v.Y, Vector2.EPS_MIN);

            // Negative low
            v = new Vector2(-20, -0.000003f);
            Assert.AreEqual(-20, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(-0.000003f, v.Y, Vector2.EPS_MIN);

            // Positive max
            v = new Vector2(float.MaxValue, float.MaxValue);
            Assert.AreEqual(float.MaxValue, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(float.MaxValue, v.Y, Vector2.EPS_MIN);

            // Negative Max
            v = new Vector2(float.MinValue, 0);
            Assert.AreEqual(float.MinValue, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(0, v.Y, Vector2.EPS_MIN);
        }

        /// <summary>
        /// Überprüft, ob Werte nachträglich geändert werden können.
        /// </summary>
        [TestMethod]
        public void ChangeValue()
        {
            Vector2 v = new Vector2(1, 1);
            v.X = 29;
            v.Y = 11;
            Assert.AreEqual(29, v.X, Vector2.EPS_MIN);
            Assert.AreEqual(11, v.Y, Vector2.EPS_MIN);
        }

        #endregion

        #region Operator

        [TestMethod]
        public void AddOperator()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SubOperator()
        {
            throw new NotImplementedException();
        }

        public void MultiplyOperator()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DivideOperator()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Comparer()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Check Methods

        /// <summary>
        /// Überprüft die Invertierungsfunktionen.
        /// </summary>
        [TestMethod]
        public void CheckInverter()
        {
            for (int x = -10000; x < 10000; x += 100)
            {
                for (int y = -10000; y < 10000; y += 100)
                {
                    Vector2 v = new Vector2(x, y);

                    Vector2 ix = v.InvertX();
                    Assert.AreEqual(-x, ix.X, Vector2.EPS_MIN);
                    Assert.AreEqual(y, ix.Y, Vector2.EPS_MIN);
                    
                    Vector2 iy = v.InvertY();
                    Assert.AreEqual(x, iy.X, Vector2.EPS_MIN);
                    Assert.AreEqual(-y, iy.Y, Vector2.EPS_MIN);

                    Vector2 ixy = v.InvertXY();
                    Assert.AreEqual(-x, ixy.X, Vector2.EPS_MIN);
                    Assert.AreEqual(-y, ixy.Y, Vector2.EPS_MIN);
                }                
            }
        }

        /// <summary>
        /// Überprüft die Längenberechnung.
        /// </summary>
        [TestMethod]
        public void CheckLength()
        {
            for (int x = -10000; x < 10000; x += 100)
            {
                for (int y = -10000; y < 10000; y += 100)
                {
                    Vector2 v = new Vector2(x, y);
                    float len = v.Length();
                    float lenSqr = v.LengthSquared();

                    float checkSqr = (x * x) + (y * y);
                    float check = (float)Math.Sqrt(checkSqr);

                    Assert.AreEqual(checkSqr, lenSqr, Vector2.EPS_MIN);
                    Assert.AreEqual(check, len, Vector2.EPS_MIN);
                }
            }
        }

        /// <summary>
        /// Überprüft die Umwandlung von Vector zu Angle.
        /// </summary>
        [TestMethod]
        public void ToAngle()
        {
            for (int x = -10000; x < 10000; x += 100)
            {
                for (int y = -10000; y < 10000; y += 100)
                {
                    Vector2 v = new Vector2(x, y);
                    Angle a = v.ToAngle();

                    float result = (float)Math.Atan2(v.Y, v.X);
                    if (result < 0)
                        result += (float)Math.PI * 2;

                    Assert.AreEqual(result, a.Radian, Vector2.EPS_MIN);
                }
            }
        }

        /// <summary>
        /// Überprüft die Normalisierungsfunktion.
        /// </summary>
        [TestMethod]
        public void CheckNormalizer()
        {
            for (int x = -10000; x < 10000; x += 100)
            {
                for (int y = -10000; y < 10000; y += 100)
                {
                    Vector2 v = new Vector2(x, y);
                    Vector2 norm = v.Normalize();

                    Angle a = v.ToAngle();
                    Angle aNorm = norm.ToAngle();

                    Assert.AreEqual(a.Radian, aNorm.Radian, Vector2.EPS_MIN);

                    // Spezialfall Nullvektor
                    if (x == 0 && y == 0)
                        Assert.AreEqual(0f, norm.Length(), Vector2.EPS_MIN);
                    else
                        Assert.AreEqual(1f, norm.Length(), Vector2.EPS_MIN);
                }
            }
        }


        #endregion
    }
}