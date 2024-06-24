using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;


namespace ToyRobotSimulation.UnitTests.RobotTests
{
    public class PlaceTests
    {
        private readonly Robot _robot;

        public PlaceTests()
        {
            _robot = new Robot();
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH)]
        [InlineData(1, 2, Direction.EAST)]
        [InlineData(4, 0, Direction.SOUTH)]
        [InlineData(2, 1, Direction.WEST)]
        [InlineData(-1, -1, Direction.NORTH)]
        [InlineData(5, 5, Direction.NORTH)]
        [InlineData(-1, 0, Direction.EAST)]
        [InlineData(0, -1, Direction.SOUTH)]
        [InlineData(5, 0, Direction.WEST)]
        [InlineData(0, 5, Direction.NORTH)]
        public void Place_ValidPosition_ShouldPlaceRobot(int x, int y, Direction direction)
        {
            var position = new Position(new Point(x, y), direction);
            _robot.Place(position);

            Assert.True(_robot.IsPlaced);
            Assert.Equal(position, _robot.Position);
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, false)]
        [InlineData(1, 1, Direction.EAST, false)]
        [InlineData(2, 2, Direction.SOUTH, false)]
        [InlineData(3, 3, Direction.WEST, false)]
        public void IsPlaced_ShouldBeFalseInitially(int x, int y, Direction direction, bool expectedIsPlaced)
        {
            var position = new Position(new Point(x, y), direction);

            Assert.False(_robot.IsPlaced);
        }
    }
}
