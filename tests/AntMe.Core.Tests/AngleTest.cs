using System;
using System.Collections.Generic;
using Xunit;

namespace AntMe.Core
{
    /// <summary>
    /// Tests for the basic Angle Struct.
    /// </summary>
    public class AngleTest
    {
        #region Data Rows

        /// <summary>
        /// Returns a set of Test Data for Radian Calculations.
        /// 1) Radian (Unnormalized)
        /// 2) Radian (Normalized)
        /// 3) Degrees (Unnormalized)
        /// 4) Degrees (Normalized)
        /// 5) Compass
        /// </summary>
        public static IEnumerable<object[]> RadianValues
        {
            get
            {
                return new[]
                {
                    // Radian, Normalized Radian, Degrees, Normalized Degrees, Compass
                    new object[] { -12f, 0.566370f, -688, 32, Compass.SouthEast }, // Far negative
                    new object[] { -(2 * Math.PI) - 0.001f, (2 * Math.PI) - 0.001f, 0, 0, Compass.East }, // Close to the negative border
                    new object[] { -(2 * Math.PI), 0f, 0, 0, Compass.East },
                    new object[] { -(2 * Math.PI) + 0.001f, 0.001f, 0, 0, Compass.East }, // In range negative
                    new object[] { -3f, 3.283185f, -172, 188, Compass.West },
                    new object[] { -1f, 5.283185f, -64, 303, Compass.NorthEast },
                    new object[] { 0f, 0f, 0, 0, Compass.East  },// Zero
                    new object[] { 1f, 1f, 57, 57, Compass.SouthEast  },// In Range
                    new object[] { 3f, 3f, 172, 172, Compass.West },
                    new object[] { (2 * Math.PI) - 0.001f, (2 * Math.PI) - 0.001f, 0, 0, Compass.East, },
                    new object[] { (2 * Math.PI), 0f, 0, 0, Compass.East  }, // Overflow
                    new object[] { (2 * Math.PI) + 0.001f, 0.001f, 0, 0, Compass.East },
                    new object[] { 12f, 5.716814f, 688, 328, Compass.NorthEast  },
                };
            }
        }

        /// <summary>
        /// Returns a set of relevant Angles for Degree Calculations.
        /// 1) Degrees (Unnormalized)
        /// 2) Degrees (Normalized)
        /// 3) Radian (Unnormalized)
        /// 4) Radian (Normalized)
        /// 5) Compass
        /// </summary>
        public static IEnumerable<object[]> DegreeValues
        {
            get
            {
                return new[]
                {
                    // Degrees, Normalized Degrees, Radian, Normalzed Radian, Compass
                    new object[] { -1000, 80, -17.45329f, 1.396263f, Compass.South },
                    new object[] { -360, 0, -6.283185f, 0f, Compass.East },
                    new object[] { -359, 1, -6.265732f, 0.017453f, Compass.East },
                    new object[] { -100, 260, -1.745329f, 4.537856f, Compass.North  },
                    new object[] { -1, 359, -0.017453f, 6.265732f, Compass.East },
                    new object[] { 0, 0, 0f, 0f, Compass.East },
                    new object[] { 1, 1, 0.017453f, 0.017453f, Compass.East },
                    new object[] { 100, 100, 1.745329f, 1.745329f, Compass.South  },
                    new object[] { 359, 359, 6.265732f, 6.265732f, Compass.East },
                    new object[] { 360, 0, 0f, 0f, Compass.East },
                    new object[] { 1000, 280, 17.45329f, 4.886922f, Compass.North },
                };
            }
        }

        /// <summary>
        /// Returns a set of Angle Values for Compass Calculations.
        /// 1) Compass
        /// 2) Degrees (Normalized)
        /// 3) Radian (Normalized)
        /// </summary>
        public static IEnumerable<object[]> CompassValues
        {
            get
            {
                return new[]
                {
                    // Compass, Degrees, Radian
                    new object[] { Compass.East, 0, 0f },
                    new object[] { Compass.SouthEast, 45, 1 * (Math.PI / 4f) },
                    new object[] { Compass.South, 90, 2 * (Math.PI / 4f) },
                    new object[] { Compass.SouthWest, 135, 3 * (Math.PI / 4f) },
                    new object[] { Compass.West, 180, 4 * (Math.PI / 4f) },
                    new object[] { Compass.NorthWest, 225, 5 * (Math.PI / 4f) },
                    new object[] { Compass.North, 270, 6 * (Math.PI / 4f) },
                    new object[] { Compass.NorthEast, 315, 7 * (Math.PI / 4f) },
                };
            }
        }

        #endregion

        // TODO
        // - Casts to float and int
        // - Check static Values

        private float[,] inverts;
        private float[,] values;
        private int[,] degreeDiffs;

        public AngleTest()
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

        #region Constructors

        /// <summary>
        /// Tests the Constructor Call with a float Parameter. Should be interpreted as Radian.
        /// </summary>
        [Theory, MemberData("RadianValues")]
        public void InitWithFloat(float radian, float normalizedRadian, int degrees, int normalizedDegrees, Compass compass)
        {
            // Arrange

            // Act
            var result = new Angle(radian);

            // Assert
            Assert.Equal(normalizedRadian, result.Radian, 5);
            Assert.Equal(normalizedDegrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        /// <summary>
        /// Tests Constructor without any Paramter
        /// </summary>
        [Fact]
        public void InitWithoutParameter()
        {
            // Arrange

            // Act
            var a = new Angle();

            // Assert
            Assert.Equal(a.Radian, 0, 5);
            Assert.Equal(a.Degree, 0);
            Assert.Equal(a.Compass, Compass.East);
        }

        /// <summary>
        /// Tests the static Method to generate an Angle from a given Radian.
        /// </summary>
        [Theory, MemberData("RadianValues")]
        public void StaticAngleFromRadian(float radian, float normalizedRadian, int degrees, int normalizedDegrees, Compass compass)
        {
            // Arrange

            // Act
            Angle result = Angle.FromRadian(radian);

            // Assert
            Assert.Equal(normalizedRadian, result.Radian, 5);
            Assert.Equal(normalizedDegrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        /// <summary>
        /// Tests the static Method to generate an Angle from a given Degree.
        /// </summary>
        [Theory, MemberData("DegreeValues")]
        public void StaticAngleFromDegree(int degrees, int normalizedDegrees, float radian, float normalizedRadian, Compass compass)
        {
            // Arrange

            // Act
            Angle result = Angle.FromDegree(degrees);

            // Assert
            Assert.Equal(normalizedRadian, result.Radian, 5);
            Assert.Equal(normalizedDegrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        /// <summary>
        /// Tests the static Method to generate an Angle from a given Compass Value.
        /// </summary>
        [Theory, MemberData("CompassValues")]
        public void StaticAngleFromCompass(Compass compass, int degrees, float radian)
        {
            // Arrange

            // Act
            Angle result = Angle.FromCompass(compass);

            // Assert
            Assert.Equal(radian, result.Radian, 5);
            Assert.Equal(degrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tests the static Method to convert from Degrees to Radian.
        /// </summary>
        [Theory, MemberData("DegreeValues")]
        public void StaticConvertFromDegreeToRadian(int degrees, int normalizedDegrees, float radian, float normalizedRadian, Compass compass)
        {
            // Arrange

            // Act
            float result = Angle.ConvertToRadian(degrees);

            // Assert
            Assert.Equal(radian, result, 5);
        }

        /// <summary>
        /// Tests the static Method to convert Radian to Degrees.
        /// </summary>
        [Theory, MemberData("RadianValues")]
        public void StaticConvertFromRadianToDegree(float radian, float normalizedRadian, int degrees, int normalizedDegrees, Compass compass)
        {
            // Arrange

            // Act
            int result = Angle.ConvertToDegree(radian);

            // Assert
            Assert.Equal(degrees, result);
        }

        /// <summary>
        /// Tests the static Method to convert Degrees to Compass.
        /// </summary>
        [Theory]
        [InlineData(-180, Compass.West)]
        [InlineData(0, Compass.East)]
        [InlineData(22, Compass.East)]
        [InlineData(23, Compass.SouthEast)]
        [InlineData(45, Compass.SouthEast)]
        [InlineData(67, Compass.SouthEast)]
        [InlineData(68, Compass.South)]
        [InlineData(90, Compass.South)]
        [InlineData(112, Compass.South)]
        [InlineData(113, Compass.SouthWest)]
        [InlineData(135, Compass.SouthWest)]
        [InlineData(157, Compass.SouthWest)]
        [InlineData(158, Compass.West)]
        [InlineData(180, Compass.West)]
        [InlineData(202, Compass.West)]
        [InlineData(203, Compass.NorthWest)]
        [InlineData(225, Compass.NorthWest)]
        [InlineData(247, Compass.NorthWest)]
        [InlineData(248, Compass.North)]
        [InlineData(270, Compass.North)]
        [InlineData(292, Compass.North)]
        [InlineData(293, Compass.NorthEast)]
        [InlineData(315, Compass.NorthEast)]
        [InlineData(337, Compass.NorthEast)]
        [InlineData(338, Compass.East)]
        [InlineData(359, Compass.East)]
        [InlineData(540, Compass.West)]
        public void StaticConvertFromDegreeToCompass(int input, Compass expectation)
        {
            // Arrange

            // Act
            Compass result = Angle.ConvertToCompass(input);

            // Assert
            Assert.Equal(expectation, result);
        }

        /// <summary>
        /// Tests the static Mathod to normalize a Radian Value to the Range of [0..2Pi)
        /// </summary>
        [Theory, MemberData("RadianValues")]
        public void StaticNormalizeRadian(float radian, float normalizedRadian, int degrees, int normalizedDegrees, Compass compass)
        {
            // Arrange

            // Act
            float result = Angle.NormalizeRadian(radian);

            // Assert
            Assert.Equal(normalizedRadian, result, 5);
        }

        /// <summary>
        /// Tests the static Method to normalize a Degree-Value to the range of [0..359]
        /// </summary>
        [Theory, MemberData("DegreeValues")]
        public void StaticNormalizeDegree(int degrees, int normalizedDegrees, float radian, float normalizedRadian, Compass compass)
        {
            // Arrange

            // Act
            int result = Angle.NormalizeDegree(degrees);

            // Assert
            Assert.Equal(normalizedDegrees, result);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static Direction Properties.
        /// </summary>
        [Fact]
        public void StaticDirectionProperties()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(Angle.FromDegree(0), Angle.Right);
            Assert.Equal(Angle.FromDegree(45), Angle.LowerRight);
            Assert.Equal(Angle.FromDegree(90), Angle.Down);
            Assert.Equal(Angle.FromDegree(135), Angle.LowerLeft);
            Assert.Equal(Angle.FromDegree(180), Angle.Left);
            Assert.Equal(Angle.FromDegree(225), Angle.UpperLeft);
            Assert.Equal(Angle.FromDegree(270), Angle.Up);
            Assert.Equal(Angle.FromDegree(315), Angle.UpperRight);
        }

        /// <summary>
        /// Tests the static Pi Values.
        /// </summary>
        [Fact]
        public void StaticPiConstants()
        {
            Assert.Equal(Math.PI, Angle.Pi, 5);
            Assert.Equal(Math.PI / 2, Angle.PiHalf, 5);
            Assert.Equal(Math.PI / 4, Angle.PiQuarter, 5);
            Assert.Equal(2 * Math.PI, Angle.TwoPi, 5);
        }

        #endregion

        #region Property Setter

        /// <summary>
        /// Tests the Setter of the Radian Property.
        /// </summary>
        [Theory, MemberData("RadianValues")]
        public void SetRadianByProperty(float radian, float normalizedRadian, int degrees, int normalizedDegrees, Compass compass)
        {
            // Arrange
            Angle result = new Angle();

            // Act
            result.Radian = radian;

            // Assert
            Assert.Equal(normalizedRadian, result.Radian);
            Assert.Equal(normalizedDegrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        [Theory, MemberData("DegreeValues")]
        public void SetDegreeByProperty(int degrees, int normalizedDegrees, float radian, float normalizedRadian, Compass compass)
        {
            // Arrange
            Angle result = new Angle();

            // Act
            result.Degree = degrees;

            // Assert
            Assert.Equal(normalizedRadian, result.Radian);
            Assert.Equal(normalizedDegrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        [Theory, MemberData("CompassValues")]
        public void SetCompassByProperty(Compass compass, int degrees, float radian)
        {
            // Arrange
            Angle result = new Angle();

            // Act
            result.Compass = compass;

            // Assert
            Assert.Equal(radian, result.Radian, 5);
            Assert.Equal(degrees, result.Degree);
            Assert.Equal(compass, result.Compass);
        }

        #endregion

        #region Calculations

        [Fact]
        public void AddPositiveFloats()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = -100; i < 100; i++)
            {
                for (int j = -100; j < 100; j++)
                {
                    float a = (float)i / 10;
                    float b = (float)j / 10;

                    var c = new Angle(a);
                    Angle d = c + b;

                    float e = Angle.NormalizeRadian(Angle.NormalizeRadian(a) + b);

                    Assert.Equal((float)d, e);
                }
            }
        }

        [Fact]
        public void AddNegativeFloats()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = -100; i < 100; i++)
            {
                for (int j = -100; j < 100; j++)
                {
                    float a = (float)i / 10;
                    float b = (float)j / 10;

                    var c = new Angle(a);
                    Angle d = c - b;

                    float e = Angle.NormalizeRadian(Angle.NormalizeRadian(a) - b);

                    Assert.Equal((float)d, e);
                }
            }
        }

        [Fact]
        public void FindDiff()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                float e = Angle.Diff(c, d);
                Assert.Equal(x, e, 5);
            }
        }

        [Fact]
        public void FindDiffDegrees()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < degreeDiffs.GetLength(0); i++)
            {
                int a = degreeDiffs[i, 0];
                int b = degreeDiffs[i, 1];
                int x = degreeDiffs[i, 2];
                int e = Angle.Diff(a, b);
                Assert.Equal(x, e);
            }
        }

        [Fact]
        public void CamparerGreater()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c > d;
                Assert.Equal(e, x > 0);
            }
        }

        [Fact]
        public void CamparerGreaterOrEqual()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c >= d;
                Assert.Equal(e, x >= 0);
            }
        }

        [Fact]
        public void CamparerSmaller()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c < d;
                Assert.Equal(e, x < 0);
            }
        }

        [Fact]
        public void CamparerSmallerOrEqual()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < values.GetLength(0); i++)
            {
                float a = values[i, 0];
                float b = values[i, 1];
                float x = values[i, 2];
                var c = new Angle(a);
                var d = new Angle(b);
                bool e = c <= d;
                Assert.Equal(e, x <= 0);
            }
        }

        [Fact]
        public void InvertX()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < inverts.GetLength(0); i++)
            {
                float a = inverts[i, 0];
                float b = inverts[i, 1];
                Angle c = new Angle(a).InvertX();
                Assert.Equal(Math.Round(c.Radian, 2), Math.Round(b, 2));
            }
        }

        [Fact]
        public void InvertY()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(true);

            for (int i = 0; i < inverts.GetLength(0); i++)
            {
                float a = inverts[i, 0];
                float b = inverts[i, 2];
                Angle c = new Angle(a).InvertY();
                Assert.Equal(Math.Round(c.Radian, 2), Math.Round(b, 2));
            }
        }

        #endregion
    }
}
