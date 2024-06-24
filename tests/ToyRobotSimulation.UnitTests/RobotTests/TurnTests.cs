using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.RobotTests
{

    public class TurnTests
    {
        private Robot _robot;

        public TurnTests()
        {
            _robot = new Robot();
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, Direction.WEST)]
        [InlineData(1, 1, Direction.EAST, Direction.NORTH)]
        [InlineData(2, 2, Direction.SOUTH, Direction.EAST)]
        [InlineData(3, 3, Direction.WEST, Direction.SOUTH)]
        public void TurnLeft_ShouldTurnRobotLeft(int startX, int startY, Direction startDirection, Direction expectedDirection)
        {
            var position = new Position(new Point(startX, startY), startDirection);
            _robot.Place(position);

            _robot.Left();

            var expectedPosition = new Position(new Point(startX, startY), expectedDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Fact]
        public void TurnLeft_NotPlaced_ShouldNotMoveRobot()
        {
            _robot.Left();

            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, Direction.EAST)]
        [InlineData(1, 1, Direction.EAST, Direction.SOUTH)]
        [InlineData(2, 2, Direction.SOUTH, Direction.WEST)]
        [InlineData(3, 3, Direction.WEST, Direction.NORTH)]
        public void TurnRight_ShouldTurnRobotRight(int startX, int startY, Direction startDirection, Direction expectedDirection)
        {
            var position = new Position(new Point(startX, startY), startDirection);
            _robot.Place(position);

            _robot.Right();

            var expectedPosition = new Position(new Point(startX, startY), expectedDirection);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Fact]
        public void TurnRight_NotPlaced_ShouldNotMoveRobot()
        {
            _robot.Right();

            Assert.False(_robot.IsPlaced);
            Assert.Equal(Position.Default, _robot.Position);
        }
    }
}
