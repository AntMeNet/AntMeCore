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
                    new object[] { -(2 * Math.PI) - 0.001f, (2 * Math.PI) - 0.001f, -360, 0, Compass.East }, // Close to the negative border
                    new object[] { -(2 * Math.PI), 0f, -360, 0, Compass.East },
                    new object[] { -(2 * Math.PI) + 0.001f, 0.001f, -360, 0, Compass.East }, // In range negative
                    new object[] { -3f, 3.283185f, -172, 188, Compass.West },
                    new object[] { -1f, 5.283185f, -57, 303, Compass.NorthEast },
                    new object[] { 0f, 0f, 0, 0, Compass.East  },// Zero
                    new object[] { 1f, 1f, 57, 57, Compass.SouthEast  },// In Range
                    new object[] { 3f, 3f, 172, 172, Compass.West },
                    new object[] { (2 * Math.PI) - 0.001f, (2 * Math.PI) - 0.001f, 360, 0, Compass.East, },
                    new object[] { (2 * Math.PI), 0f, 360, 0, Compass.East  }, // Overflow
                    new object[] { (2 * Math.PI) + 0.001f, 0.001f, 360, 0, Compass.East },
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
                    new object[] { 360, 0, 6.283185f, 0f, Compass.East },
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

        #region Calculations

        /// <summary>
        /// Tests the AddRadian Method to add two Angles.
        /// </summary>
        [Theory]
        [InlineData(3f, -10f, -7f + (4 * Math.PI))]
        [InlineData(3f, -(2 * Math.PI), 3f)]
        [InlineData(3f, -3f, 0f)]
        [InlineData(3f, 0f, 3f)]
        [InlineData(3f, 3f, 6f)]
        [InlineData(3f, 2 * Math.PI, 3f)]
        [InlineData(3f, 10f, 13f - (4 * Math.PI))]
        public void AddRadianByMethod(float init, float add, float expectation)
        {
            // Arrange
            Angle value = new Angle(init);

            // Act
            Angle result = value.AddRadian(add);

            // Assert
            Assert.Equal(expectation, result.Radian, 5);
        }

        /// <summary>
        /// Tests the Add Operator.
        /// </summary>
        [Theory]
        [InlineData(3f, -10f, -7f + (4 * Math.PI))]
        [InlineData(3f, -(2 * Math.PI), 3f)]
        [InlineData(3f, -3f, 0f)]
        [InlineData(3f, 0f, 3f)]
        [InlineData(3f, 3f, 6f)]
        [InlineData(3f, 2 * Math.PI, 3f)]
        [InlineData(3f, 10f, 13f - (4 * Math.PI))]
        public void AddRadianByOperator(float init, float add, float expectation)
        {
            // Arrange
            Angle value = new Angle(init);

            // Act
            Angle result = value + add;

            // Assert
            Assert.Equal(expectation, result.Radian, 5);
        }

        /// <summary>
        /// Tests the AddDegree Method to add two Angles.
        /// </summary>
        [Theory]
        [InlineData(150, -600, 270)]
        [InlineData(150, -360, 150)]
        [InlineData(150, -150, 0)]
        [InlineData(150, 0f, 150)]
        [InlineData(150, 150, 300)]
        [InlineData(150, 360, 150)]
        [InlineData(150, 600, 30)]
        public void AddDegreeByMethod(int init, int add, int expectation)
        {
            // Arrange
            Angle value = Angle.FromDegree(init);

            // Act
            Angle result = value.AddDegree(add);

            // Assert
            Assert.Equal(expectation, result.Degree);
        }

        /// <summary>
        /// Tests the SubstractRadian Method to substract an Angle from anonther.
        /// </summary>
        [Theory]
        [InlineData(3f, -10f, 13f - (4 * Math.PI))]
        [InlineData(3f, -(2 * Math.PI), 3f)]
        [InlineData(3f, -3f, 6f)]
        [InlineData(3f, 0f, 3f)]
        [InlineData(3f, 3f, 0f)]
        [InlineData(3f, 2 * Math.PI, 3f)]
        [InlineData(3f, 10f, -7f + (4 * Math.PI))]
        public void SubstractRadianByMethod(float init, float substract, float expectation)
        {
            // Arrange
            Angle value = new Angle(init);

            // Act
            Angle result = value.SubstractRadian(substract);

            // Assert
            Assert.Equal(expectation, result.Radian, 5);
        }

        /// <summary>
        /// Tests the Substract Operator.
        /// </summary>
        [Theory]
        [InlineData(3f, -10f, 13f - (4 * Math.PI))]
        [InlineData(3f, -(2 * Math.PI), 3f)]
        [InlineData(3f, -3f, 6f)]
        [InlineData(3f, 0f, 3f)]
        [InlineData(3f, 3f, 0f)]
        [InlineData(3f, 2 * Math.PI, 3f)]
        [InlineData(3f, 10f, -7f + (4 * Math.PI))]
        public void SubstractRadianByOperator(float init, float substract, float expectation)
        {
            // Arrange
            Angle value = new Angle(init);

            // Act
            Angle result = value - substract;

            // Assert
            Assert.Equal(expectation, result.Radian, 5);
        }

        /// <summary>
        /// Tests the SubstractDegree Method to substract an Angle from anonther.
        /// </summary>
        [Theory]
        [InlineData(150, -600, 30)]
        [InlineData(150, -360, 150)]
        [InlineData(150, -150, 300)]
        [InlineData(150, 0, 150)]
        [InlineData(150, 150, 0)]
        [InlineData(150, 360, 150)]
        [InlineData(150, 600, 270)]
        public void SubstractDegreeByMethod(int init, int substract, float expectation)
        {
            // Arrange
            Angle value = Angle.FromDegree(init);

            // Act
            Angle result = value.SubstractDegree(substract);

            // Assert
            Assert.Equal(expectation, result.Degree);
        }

        #endregion

        #region Casts

        public void CastFromAngleToRadian()
        {
            // Arrange
            // Act
            // Assert
        }

        public void CastFromRadianToAngle()
        {
            // Arrange
            // Act
            // Assert
        }

        #endregion

        #region Diff

        public void CalculateDiffByRadian()
        {
            // Arrange
            // Act
            // Assert
        }

        public void CalculateDiffByDegree()
        {
            // Arrange
            // Act
            // Assert
        }

        #endregion

        #region Inverter

        public void InvertX()
        {
            // Arrange
            // Act
            // Assert
        }

        public void InvertY()
        {
            // Arrange
            // Act
            // Assert
        }

        #endregion
    }
}
