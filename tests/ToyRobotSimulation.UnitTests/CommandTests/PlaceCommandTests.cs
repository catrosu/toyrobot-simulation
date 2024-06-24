using ToyRobotSimulation.Core.Commands;
using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.CommandTests
{
    public class PlaceCommandTests
    {
        private Robot _robot;
        private Table _table;

        public PlaceCommandTests()
        {
            _robot = new Robot();
            _table = new Table(5, 5);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH)]
        [InlineData(1, 1, Direction.EAST)]
        [InlineData(2, 2, Direction.SOUTH)]
        [InlineData(3, 3, Direction.WEST)]
        [InlineData(4, 4, Direction.NORTH)]
        public void PlaceCommand_ValidPosition_ShouldPlaceRobot(int x, int y, Direction direction)
        {
            // Arrange
            var position = new Position(new Point(x, y), direction);
            var command = new PlaceCommand(position);

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.True(_robot.IsPlaced);
            Assert.Equal(position, _robot.Position);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH)]
        [InlineData(4, 0, Direction.EAST)]
        [InlineData(0, 4, Direction.SOUTH)]
        [InlineData(4, 4, Direction.WEST)]
        public void PlaceCommand_EdgeCases_ShouldPlaceRobot(int x, int y, Direction direction)
        {
            // Arrange
            var position = new Position(new Point(x, y), direction);
            var command = new PlaceCommand(position);

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.True(_robot.IsPlaced);
            Assert.Equal(position, _robot.Position);
        }


        [Theory]
        [InlineData(5, 5, Direction.NORTH)]
        [InlineData(-1, 0, Direction.EAST)]
        [InlineData(0, -1, Direction.SOUTH)]
        [InlineData(6, 0, Direction.WEST)]
        [InlineData(0, 6, Direction.NORTH)]
        [InlineData(-1, -1, Direction.WEST)]
        public void PlaceCommand_InvalidPosition_ShouldNotPlaceRobot(int x, int y, Direction direction)
        {
            // Arrange
            var position = new Position(new Point(x, y), direction);
            var command = new PlaceCommand(position);

            // Act
            command.Execute(_robot, _table);

            // Assert
            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }
    }
}
