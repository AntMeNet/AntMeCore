using Xunit;

namespace AntMe.Core
{
    /// <summary>
    /// Tests the Values of the Compass Enum.
    /// </summary>
    public class CompassTest
    {
        /// <summary>
        /// Tests the Cast to Int of Compass Values.
        /// </summary>
        [Theory]
        [InlineData(Compass.East, 0)]
        [InlineData(Compass.SouthEast, 45)]
        [InlineData(Compass.South, 90)]
        [InlineData(Compass.SouthWest, 135)]
        [InlineData(Compass.West, 180)]
        [InlineData(Compass.NorthWest, 225)]
        [InlineData(Compass.North, 270)]
        [InlineData(Compass.NorthEast, 315)]
        public void CompassToIntCast(Compass input, int expectation)
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(expectation, (int)input);
        }

        /// <summary>
        /// Tests the Cast to Compass from an Int Value.
        /// </summary>
        [Theory]
        [InlineData(Compass.East, 0)]
        [InlineData(Compass.SouthEast, 45)]
        [InlineData(Compass.South, 90)]
        [InlineData(Compass.SouthWest, 135)]
        [InlineData(Compass.West, 180)]
        [InlineData(Compass.NorthWest, 225)]
        [InlineData(Compass.North, 270)]
        [InlineData(Compass.NorthEast, 315)]
        public void IntToCompassCast(Compass expectation, int input)
        {
            Assert.Equal((Compass)input, expectation);
        }
    }
}
