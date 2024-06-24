using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.RobotTests
{

    public class ReportPostionTests
    {
        private Robot _robot;

        public ReportPostionTests()
        {
            _robot = new Robot();
        }

        [Theory]
        [InlineData(0, 0, Direction.NORTH, "0,0,NORTH")]
        [InlineData(1, 1, Direction.EAST, "1,1,EAST")]
        [InlineData(2, 2, Direction.SOUTH, "2,2,SOUTH")]
        [InlineData(3, 3, Direction.WEST, "3,3,WEST")]
        public void Report_ShouldReturnPositionAndDirection(int x, int y, Direction direction, string expectedReport)
        {
            var position = new Position(new Point(x, y), direction);
            _robot.Place(position);

            var report = _robot.Report();

            Assert.Equal(expectedReport, report);
        }

        [Fact]
        public void Report_ShouldReturnDefaultPosition()
        {
            var report = _robot.Report();

            Assert.Equal(Position.Default.ToString(), report);
        }
    }
}
