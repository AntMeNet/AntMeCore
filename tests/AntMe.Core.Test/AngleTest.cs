using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet die Angle-Klasse
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class AngleTest
    {
        private float[,] inverts;
        private float[,] values;
        private int[,] degreeDiffs;

        /// <summary>
        /// Initialisiert einen Puffer von vorberechneten Winkel-Werten und deren Spiegelungen.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            // Winkel a, Winkel b, Erwarteter Diff
            values = new float[,]
                {
                    {0, Angle.Right, 0*Angle.PiQuarter},
                    {0, Angle.LowerRight, 1*Angle.PiQuarter},
                    {0, Angle.Down, 2*Angle.PiQuarter},
                    {0, Angle.LowerLeft, 3*Angle.PiQuarter},
                    {0, Angle.Left, 4*Angle.PiQuarter},
                    {0, Angle.UpperLeft, -3*Angle.PiQuarter},
                    {0, Angle.Up, -2*Angle.PiQuarter},
                    {0, Angle.UpperRight, -1*Angle.PiQuarter},
                    {Angle.Down, Angle.Right, -2*Angle.PiQuarter},
                    {Angle.Down, Angle.LowerRight, -1*Angle.PiQuarter},
                    {Angle.Down, Angle.Down, 0*Angle.PiQuarter},
                    {Angle.Down, Angle.LowerLeft, 1*Angle.PiQuarter},
                    {Angle.Down, Angle.Left, 2*Angle.PiQuarter},
                    {Angle.Down, Angle.UpperLeft, 3*Angle.PiQuarter},
                    {Angle.Down, Angle.Up, 4*Angle.PiQuarter},
                    {Angle.Down, Angle.UpperRight, -3*Angle.PiQuarter},
                    {Angle.Left, Angle.Right, -4*Angle.PiQuarter},
                    {Angle.Left, Angle.LowerRight, -3*Angle.PiQuarter},
                    {Angle.Left, Angle.Down, -2*Angle.PiQuarter},
                    {Angle.Left, Angle.LowerLeft, -1*Angle.PiQuarter},
                    {Angle.Left, Angle.Left, 0*Angle.PiQuarter},
                    {Angle.Left, Angle.UpperLeft, 1*Angle.PiQuarter},
                    {Angle.Left, Angle.Up, 2*Angle.PiQuarter},
                    {Angle.Left, Angle.UpperRight, 3*Angle.PiQuarter},
                    {Angle.Up, Angle.Right, 2*Angle.PiQuarter},
                    {Angle.Up, Angle.LowerRight, 3*Angle.PiQuarter},
                    {Angle.Up, Angle.Down, -4*Angle.PiQuarter},
                    {Angle.Up, Angle.LowerLeft, -3*Angle.PiQuarter},
                    {Angle.Up, Angle.Left, -2*Angle.PiQuarter},
                    {Angle.Up, Angle.UpperLeft, -1*Angle.PiQuarter},
                    {Angle.Up, Angle.Up, 0*Angle.PiQuarter},
                    {Angle.Up, Angle.UpperRight, 1*Angle.PiQuarter},
                };

            // Winkel, Invert X, Invert Y
            inverts = new float[,]
                {
                    {Angle.Right, Angle.Left, Angle.Right},
                    {Angle.LowerRight, Angle.LowerLeft, Angle.UpperRight},
                    {Angle.Down, Angle.Down, Angle.Up},
                    {Angle.LowerLeft, Angle.LowerRight, Angle.UpperLeft},
                    {Angle.Left, Angle.Right, Angle.Left},
                    {Angle.UpperLeft, Angle.UpperRight, Angle.LowerLeft},
                    {Angle.Up, Angle.Up, Angle.Down},
                    {Angle.UpperRight, Angle.UpperLeft, Angle.LowerRight},
                };

            // Degree Diffs
            // Winkel a, Winkel b, Erwarteter Diff
            degreeDiffs = new int[,]
            {
                {-360, -360, 0},
                {-360, -270, 90},
                {-360, -180, 180},
                {-360, -90, -90},
                {-360, 0, 0},
                {-360, 90, 90},
                {-360, 180, 180},
                {-360, 270, -90},
                {-360, 360, 0},

                {-180, -360, -180},
                {-180, -270, -90},
                {-180, -180, 0},
                {-180, -90, 90},
                {-180, 0, -180},
                {-180, 90, -90},
                {-180, 180, 0},
                {-180, 270, 90},
                {-180, 360, -180},

                {0, -360, 0},
                {0, -270, 90},
                {0, -180, 180},
                {0, -90, -90},
                {0, 0, 0},
                {0, 90, 90},
                {0, 180, 180},
                {0, 270, -90},
                {0, 360, 0},

                {144, -360, -144},
                {144, -270, -54},
                {144, -180, 36},
                {144, -90, 126},
                {144, 0, -144},
                {144, 90, -54},
                {144, 180, 36},
                {144, 270, 126},
                {144, 360, -144}
            };
        }

        #region Init

        /// <summary>
        /// Testet Konstruktor mit Float als Initialwert (muss als Bogenmaß interpretiert werden).
        /// </summary>
        [TestMethod]
        public void InitMitfloat()
        {
            var a = new Angle(1f);
            float b = a;

            Assert.AreEqual(a.Radian, 1f);
            Assert.AreEqual(b, 1f);
        }

        /// <summary>
        /// Initialisierung ohne Parameter (muss als 0 (Osten) interpretiert werden).
        /// </summary>
        [TestMethod]
        public void InitOhneParameter()
        {
            var a = new Angle();
            float b = a;

            Assert.AreEqual(a.Radian, 0);
            Assert.AreEqual(a.Degree, 0);
            Assert.AreEqual(a, 0);
            Assert.AreEqual(b, 0);
        }

        #endregion

        #region Statische Methoden

        [TestMethod]
        public void ConvertToRadian()
        {
            for (int i = -1000; i < 1000; i++)
            {
                float b = Angle.ConvertToRadian(i);
                Assert.AreEqual(b, (float) i/360*Angle.TwoPi);
            }
        }

        [TestMethod]
        public void ConvertToDegree()
        {
            for (int i = -1000; i < 1000; i++)
            {
                float a = (float) i/10;
                int b = Angle.ConvertToDegree(a);
                Assert.AreEqual(b, (int) Math.Round(a / Angle.TwoPi * 360));
            }
        }

        [TestMethod]
        public void NormalizeRadian()
        {
            for (int i = -1000; i < 1000; i++)
            {
                float a = (float) i/10;
                float b = Angle.NormalizeRadian(a);

                int c = (int) (-a/Angle.TwoPi) + 1;
                float d = a + (Angle.TwoPi*c);

                Assert.AreEqual(Math.Round(b, 2), Math.Round(d%Angle.TwoPi, 2));
            }
        }

        [TestMethod]
        public void NormalizeDegree()
        {
            for (int i = -1000; i < 1000; i++)
            {
                int a = Angle.NormalizeDegree(i);
                int b = (i + 3600)%360;

                Assert.AreEqual(a, b);
            }
        }

        #endregion

        #region Set Radian

        [TestMethod]
        public void SetRadianPositiveInRange()
        {
            float input = 1f;

            var a = new Angle();
            a.Radian = input;

            var b = (int) (input*(180/Math.PI));

            Assert.AreEqual(a.Radian, input);
            Assert.AreEqual(a.Degree, b);
            Assert.AreEqual(a, input);
        }

        [TestMethod]
        public void SetRadianPositiveOutOfRange()
        {
            float input = 17f;

            var a = new Angle();
            a.Radian = input;

            var b = (int) ((input)*(180/Math.PI));
            b %= 360;

            Assert.AreEqual(a.Radian, input%Angle.TwoPi);
            Assert.AreEqual(a.Degree, b);
            Assert.AreEqual(a, input);
        }

        [TestMethod]
        public void SetRadianNegativeInRange()
        {
            float input = -1f;

            var a = new Angle();
            a.Radian = input;

            float correction = input + Angle.TwoPi;
            var b = (int)Math.Round((correction) * (180 / Math.PI));

            Assert.AreEqual(a.Radian, correction);
            Assert.AreEqual(a.Degree, b);
            Assert.AreEqual(a, correction);
        }

        [TestMethod]
        public void SetRadianNegativeOutOfRange()
        {
            float input = -17f;

            var a = new Angle();
            a.Radian = input;

            float correction = input + (3*Angle.TwoPi);
            var b = (int) Math.Round((correction)*(180/Math.PI));

            Assert.AreEqual(a.Radian, correction);
            Assert.AreEqual(a.Degree, b);
            Assert.AreEqual(a, correction);
        }

        #endregion

        #region Set Degree

        [TestMethod]
        public void SetDegreeZero()
        {
            int input = 0;
            int output = 0;

            var a = new Angle();
            a.Degree = input;

            Assert.AreEqual(a.Degree, output);
        }

        [TestMethod]
        public void SetDegreePositiveInRange()
        {
            int input = 127;
            int output = 127;

            var a = new Angle();
            a.Degree = input;

            Assert.AreEqual(a.Degree, output);
        }

        [TestMethod]
        public void SetDegreePositiveOutOfRange()
        {
            int input = 500;
            int output = 140;

            var a = new Angle();
            a.Degree = input;

            Assert.AreEqual(a.Degree, output);
        }

        [TestMethod]
        public void SetDegreeNegativeInRange()
        {
            int input = -109;
            int output = 251;

            var a = new Angle();
            a.Degree = input;

            Assert.AreEqual(a.Degree, output);
        }

        [TestMethod]
        public void SetDegreeNegativeOutOfRange()
        {
            int input = -1000;
            int output = 80;

            var a = new Angle();
            a.Degree = input;

            Assert.AreEqual(a.Degree, output);
        }

        #endregion

        #region Set Compass

        [TestMethod]
        public void SetFromCompass()
        {
            Assert.AreEqual(Angle.Right, Angle.FromCompass(Compass.East));
            Assert.AreEqual(Angle.LowerRight, Angle.FromCompass(Compass.SouthEast));
            Assert.AreEqual(Angle.Down, Angle.FromCompass(Compass.South));
            Assert.AreEqual(Angle.LowerLeft, Angle.FromCompass(Compass.SouthWest));
            Assert.AreEqual(Angle.Left, Angle.FromCompass(Compass.West));
            Assert.AreEqual(Angle.UpperLeft, Angle.FromCompass(Compass.NorthWest));
            Assert.AreEqual(Angle.Up, Angle.FromCompass(Compass.North));
            Assert.AreEqual(Angle.UpperRight, Angle.FromCompass(Compass.NorthEast));
        }

        [TestMethod]
        public void GetCompass()
        {
            for (int i = 0; i < 360; i++)
            {
                var angle = Angle.FromDegree(i);
                
                Compass c = Compass.East;
                if (i >= 0 && i < 23)
                {
                    c = Compass.East;
                }
                else if (i >= 23 && i < 68)
                {
                    c = Compass.SouthEast;
                }
                else if (i >= 68 && i < 113)
                {
                    c = Compass.South;
                }
                else if (i >= 113 && i < 158)
                {
                    c = Compass.SouthWest;
                }
                else if (i >= 158 && i < 203)
                {
                    c = Compass.West;
                }
                else if (i >= 203 && i < 248)
                {
                    c = Compass.NorthWest;
                }
                else if (i >= 248 && i < 293)
                {
                    c = Compass.North;
                }
                else if (i >= 293 && i < 338)
                {
                    c = Compass.NorthEast;
                }
                else if (i >= 338 && i < 360)
                {
                    c = Compass.East;
                }

                Assert.AreEqual(c, angle.Compass);
            }
        }

        #endregion

        #region Calculations

        /// <summary>
        /// This test is used to check whether the conversion algorithms from radian to degree and back use Math.Round.
        /// </summary>
        [TestMethod]
        public void CheckMathRoundIsUsed()
        {
            Assert.AreEqual(123, Angle.ConvertToDegree(Angle.ConvertToRadian(123)));
            Assert.AreEqual(125, Angle.ConvertToDegree(Angle.ConvertToRadian(125)));
        }

        [TestMethod]
        public void AddPositiveFloats()
        {
            for (int i = -100; i < 100; i++)
            {
                for (int j = -100; j < 100; j++)
                {
                    float a = (float) i/10;
                    float b = (float) j/10;

                    var c = new Angle(a);
                    Angle d = c + b;

                    float e = Angle.NormalizeRadian(Angle.NormalizeRadian(a) + b);

                    Assert.AreEqual(d, e);
                }
            }
        }

        [TestMethod]
        public void AddNegativeFloats()
        {
            for (int i = -100; i < 100; i++)
            {
                for (int j = -100; j < 100; j++)
                {
                    float a = (float) i/10;
                    float b = (float) j/10;

                    var c = new Angle(a);
                    Angle d = c - b;

                    float e = Angle.NormalizeRadian(Angle.NormalizeRadian(a) - b);

                    Assert.AreEqual(d, e);
                }
            }
        }

        [TestMethod]
        public void FindDiff()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                float e = Angle.Diff(c, d);
                Assert.AreEqual(x, e, 0.01f);
            }
        }

        [TestMethod]
        public void FindDiffDegrees()
        {
            for (int i = 0; i < degreeDiffs.GetLength(0); i++)
            {
                int a = degreeDiffs[i, 0];
                int b = degreeDiffs[i, 1];
                int x = degreeDiffs[i, 2];
                int e = Angle.Diff(a, b);
                Assert.AreEqual(x, e);
            }
        }

        [TestMethod]
        public void CamparerGreater()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c > d;
                Assert.AreEqual(e, x > 0);
            }
        }

        [TestMethod]
        public void CamparerGreaterOrEqual()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c >= d;
                Assert.AreEqual(e, x >= 0);
            }
        }

        [TestMethod]
        public void CamparerSmaller()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c < d;
                Assert.AreEqual(e, x < 0);
            }
        }

        [TestMethod]
        public void CamparerSmallerOrEqual()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c <= d;
                Assert.AreEqual(e, x <= 0);
            }
        }

        [TestMethod]
        public void InvertX()
        {
            for (int i = 0; i < inverts.GetLength(0); i++)
            {
                float a = inverts[i, 0];
                float b = inverts[i, 1];
                Angle c = new Angle(a).InvertX();
                Assert.AreEqual(Math.Round(c.Radian, 2), Math.Round(b, 2));
            }
        }

        [TestMethod]
        public void InvertY()
        {
            for (int i = 0; i < inverts.GetLength(0); i++)
            {
                float a = inverts[i, 0];
                float b = inverts[i, 2];
                Angle c = new Angle(a).InvertY();
                Assert.AreEqual(Math.Round(c.Radian, 2), Math.Round(b, 2));
            }
        }

        #endregion
    }
}