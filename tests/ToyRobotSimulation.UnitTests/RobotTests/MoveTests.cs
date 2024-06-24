using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.RobotTests
{

    public class MoveTests
    {
        private Robot _robot;

        public MoveTests()
        {
            _robot = new Robot();
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, 0, 1)]
        [InlineData(1, 1, Direction.EAST, 2, 1)]
        [InlineData(2, 2, Direction.SOUTH, 2, 1)]
        [InlineData(3, 3, Direction.WEST, 2, 3)]
        public void Move_ShouldMoveRobot(int startX, int startY, Direction startDirection, int expectedX, int expectedY)
        {
            var startPosition = new Position(new Point(startX, startY), startDirection);
            _robot.Place(startPosition);

            _robot.Move();

            var expectedPosition = new Position(new Point(expectedX, expectedY), startDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Fact]
        public void Move_NotPlaced_ShouldNotMoveRobot()
        {
            _robot.Move();

            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }
    }
}
