using ToyRobotSimulation.Core.Commands;
using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.CommandTests
{
    public class LeftCommandTests
    {
        private Robot _robot;
        private Table _table;

        public LeftCommandTests()
        {
            _robot = new Robot();
            _table = new Table(5, 5);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, Direction.WEST)]
        [InlineData(1, 1, Direction.EAST, Direction.NORTH)]
        [InlineData(2, 2, Direction.SOUTH, Direction.EAST)]
        [InlineData(3, 3, Direction.WEST, Direction.SOUTH)]
        public void LeftCommand_ValidTurn_ShouldTurnRobotLeft(int startX, int startY, Direction startDirection, Direction expectedDirection)
        {
            // Arrange
            var startPosition = new Position(new Point(startX, startY), startDirection);
            var command = new LeftCommand();
            _robot.Place(startPosition);

            // Act
            command.Execute(_robot, _table);

            // Assert
            var expectedPosition = new Position(new Point(startX, startY), expectedDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Fact]
        public void LeftCommand_RobotNotPlaced_ShouldNotTurnRobot()
        {
            // Arrange
            var command = new LeftCommand();

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }
    }
}
