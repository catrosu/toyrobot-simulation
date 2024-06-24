using ToyRobotSimulation.Core.Commands;
using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.CommandTests
{
    public class RightCommandTests
    {
        private Robot _robot;
        private Table _table;

        public RightCommandTests()
        {
            _robot = new Robot();
            _table = new Table(5, 5);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, Direction.EAST)]
        [InlineData(1, 1, Direction.EAST, Direction.SOUTH)]
        [InlineData(2, 2, Direction.SOUTH, Direction.WEST)]
        [InlineData(3, 3, Direction.WEST, Direction.NORTH)]
        public void RightCommand_ValidTurn_ShouldTurnRobotRight(int startX, int startY, Direction startDirection, Direction expectedDirection)
        {
            // Arrange
            var startPosition = new Position(new Point(startX, startY), startDirection);
            var command = new RightCommand();
            _robot.Place(startPosition);

            // Act
            command.Execute(_robot, _table);

            // Assert
            var expectedPosition = new Position(new Point(startX, startY), expectedDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Fact]
        public void RightCommand_RobotNotPlaced_ShouldNotTurnRobot()
        {
            // Arrange
            var command = new RightCommand();

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }
    }
}
