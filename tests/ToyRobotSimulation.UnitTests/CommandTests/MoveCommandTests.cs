using ToyRobotSimulation.Core.Commands;
using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.CommandTests
{
    public class MoveCommandTests
    {
        private Robot _robot;
        private Table _table;

        public MoveCommandTests()
        {
            _robot = new Robot();
            _table = new Table(5, 5);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, 0, 1)]
        [InlineData(1, 1, Direction.EAST, 2, 1)]
        [InlineData(2, 2, Direction.SOUTH, 2, 1)]
        [InlineData(3, 3, Direction.WEST, 2, 3)]
        [InlineData(1, 4, Direction.SOUTH, 1, 3)]
        [InlineData(4, 1, Direction.WEST, 3, 1)]
        public void MoveCommand_ValidMove_ShouldMoveRobot(int startX, int startY, Direction startDirection, int expectedX, int expectedY)
        {
            // Arrange
            var startPosition = new Position(new Point(startX, startY), startDirection);
            var command = new MoveCommand();
            _robot.Place(startPosition);

            // Act
            command.Execute(_robot, _table);

            // Assert
            var expectedPosition = new Position(new Point(expectedX, expectedY), startDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Theory]
        [InlineData(0, 4, Direction.NORTH)]
        [InlineData(4, 4, Direction.NORTH)]
        [InlineData(0, 0, Direction.SOUTH)]
        [InlineData(4, 0, Direction.SOUTH)]
        [InlineData(0, 0, Direction.WEST)]
        [InlineData(0, 4, Direction.WEST)]
        [InlineData(4, 0, Direction.EAST)]
        [InlineData(4, 4, Direction.EAST)]
        public void MoveCommand_InvalidMove_ShouldNotMoveRobot(int startX, int startY, Direction startDirection)
        {
            // Arrange
            var startPosition = new Position(new Point(startX, startY), startDirection);
            var command = new MoveCommand();
            _robot.Place(startPosition);

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.Equal(startPosition, _robot.Position);
        }

        [Fact]
        public void MoveCommand_RobotNotPlaced_ShouldNotMoveRobot()
        {
            // Arrange
            var command = new MoveCommand();

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }

        [Theory]
        [InlineData(0, 4, Direction.NORTH, 0, 4)]
        [InlineData(4, 4, Direction.NORTH, 4, 4)]
        [InlineData(0, 0, Direction.SOUTH, 0, 0)]
        [InlineData(4, 0, Direction.SOUTH, 4, 0)]
        [InlineData(0, 0, Direction.WEST, 0, 0)]
        [InlineData(0, 4, Direction.WEST, 0, 4)]
        [InlineData(4, 0, Direction.EAST, 4, 0)]
        [InlineData(4, 4, Direction.EAST, 4, 4)]
        public void MoveCommand_EdgeCases_ShouldNotMoveRobot(int startX, int startY, Direction startDirection, int expectedX, int expectedY)
        {
            // Arrange
            var startPosition = new Position(new Point(startX, startY), startDirection);
            var command = new MoveCommand();
            _robot.Place(startPosition);

            // Act
            command.Execute(_robot, _table);

            // Assert
            var expectedPosition = new Position(new Point(expectedX, expectedY), startDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }
    }
}
