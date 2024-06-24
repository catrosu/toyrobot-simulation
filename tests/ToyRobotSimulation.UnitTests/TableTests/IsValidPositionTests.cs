using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.TableTests
{
    public class IsValidPositionTests
    {
        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(4, 4, true)]
        [InlineData(1, 3, true)]
        [InlineData(5, 5, false)]
        [InlineData(-1, -1, false)]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        [InlineData(5, 0, false)]
        [InlineData(0, 5, false)]
        public void IsValidPosition_ShouldReturnCorrectly(int x, int y, bool expected)
        {
            // Arrange
            var table = new Table(5, 5);
            var point = new Point(x, y);

            // Act
            var result = table.IsValidPosition(point);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
