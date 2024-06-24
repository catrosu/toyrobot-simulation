using Ardalis.Result;
using ToyRobotSimulation.Core;
using ToyRobotSimulation.Core.Commands;
using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Entities;

namespace ToyRobotSimulation.UnitTests.CommandTests
{
    public class CommandParserTests
    {
        private readonly Robot _robot;
        private readonly Table _table;

        public CommandParserTests()
        {
            _robot = new Robot();
            _table = new Table(5, 5);
        }

        [Theory]
        [InlineData("PLACE 0,0,NORTH", 0, 0, Direction.NORTH)]
        [InlineData("PLACE 1,2,EAST", 1, 2, Direction.EAST)]
        [InlineData("PLACE 3,4,SOUTH", 3, 4, Direction.SOUTH)]
        [InlineData("PLACE 2,3,WEST", 2, 3, Direction.WEST)]
        public void Parse_ValidPlaceCommand_ShouldPlaceRobotCorrectly(string input, int expectedX, int expectedY, Direction expectedDirection)
        {
            // Act
            var result = CommandParser.ParseCommand(input);

            // Assert
            Assert.True(result.IsSuccess);
            var command = result.Value;
            command.Execute(_robot, _table);

            var expectedPosition = new Position(new Point(expectedX, expectedY), expectedDirection);
            Assert.True(_robot.IsPlaced);
            Assert.Equal(expectedPosition, _robot.Position);
        }

        [Theory]
        [InlineData(CommandConstants.MoveCommand)]
        [InlineData(CommandConstants.LeftCommand)]
        [InlineData(CommandConstants.RightCommand)]
        public void Parse_ValidSimpleCommands_ShouldReturnCorrectCommand(string input)
        {
            // Act
            var result = CommandParser.ParseCommand(input);

            // Assert
            Assert.True(result.IsSuccess);
            var command = result.Value;
            Assert.NotNull(command);

            switch (input)
            {
                case CommandConstants.MoveCommand:
                    Assert.IsType<MoveCommand>(command);
                    break;
                case CommandConstants.LeftCommand:
                    Assert.IsType<LeftCommand>(command);
                    break;
                case CommandConstants.RightCommand:
                    Assert.IsType<RightCommand>(command);
                    break;
            }
        }

        [Theory]
        [InlineData("PLACE")]
        [InlineData("PLACE 1,2")]
        [InlineData("PLACE 1,2,NORTH,EAST")]
        [InlineData("PLACE X,Y,NORTH")]
        [InlineData("MOVE FORWARD")]
        [InlineData("TURN LEFT")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("UNKNOWN COMMAND")]
        public void Parse_InvalidCommands_ShouldReturnInvalidResult(string input)
        {
            // Act
            var result = CommandParser.ParseCommand(input);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }
    }
}
