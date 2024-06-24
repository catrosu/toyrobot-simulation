using Moq;
using Microsoft.Extensions.Logging;
using ToyRobotSimulation.Core;
using ToyRobotSimulation.Domain.Interfaces;
using ToyRobotSimulation.Domain;

namespace ToyRobotSimulation.Tests
{
    public class ProcessCommandTests
    {
        private readonly Mock<IToy> _toyMock;
        private readonly Mock<ISurface> _surfaceMock;
        private readonly Mock<ILogger<Simulation>> _loggerMock;
        private readonly Simulation _simulation;

        public ProcessCommandTests()
        {
            _toyMock = new Mock<IToy>();
            _surfaceMock = new Mock<ISurface>();
            _loggerMock = new Mock<ILogger<Simulation>>();
            _simulation = new Simulation(_toyMock.Object, _surfaceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ProcessCommand_ShouldReturnSuccess_WhenPlaceCommandIsValid()
        {
            // Arrange
            var commandString = "PLACE 1,2,NORTH";
            _surfaceMock.Setup(s => s.IsValidPosition(It.IsAny<Point>())).Returns(true);

            // Act
            var result = _simulation.ProcessCommand(commandString);

            // Assert
            Assert.True(result.IsSuccess);
            _toyMock.Verify(t => t.Place(It.IsAny<Position>()), Times.Once);
        }

        [Fact]
        public void ProcessCommand_ShouldReturnSuccess_WhenMoveCommandIsValid()
        {
            // Arrange
            var placeCommand = "PLACE 1,2,NORTH";
            _surfaceMock.Setup(s => s.IsValidPosition(It.IsAny<Point>())).Returns(true);
            _simulation.ProcessCommand(placeCommand);

            var commandString = "MOVE";
            _toyMock.Setup(t => t.Position).Returns(new Position(new Point(1, 2), Direction.NORTH));
            _toyMock.Setup(t => t.IsPlaced).Returns(true);
            _surfaceMock.Setup(s => s.IsValidPosition(It.IsAny<Point>())).Returns(true);

            // Act
            var result = _simulation.ProcessCommand(commandString);

            // Assert
            Assert.True(result.IsSuccess);
            _toyMock.Verify(t => t.Move(), Times.Once);
        }

        [Fact]
        public void ProcessCommand_ShouldReturnInvalid_WhenFirstCommandIsNotPlace()
        {
            // Arrange
            var commandString = "MOVE";

            // Act
            var result = _simulation.ProcessCommand(commandString);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.ValidationErrors, v => v.ErrorMessage == "The first command must be a PLACE command.");
        }

        [Fact]
        public void ProcessCommand_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            var commandString = "PLACE 1,2,NORTH";
            _surfaceMock.Setup(s => s.IsValidPosition(It.IsAny<Point>())).Throws(new Exception("Surface error"));

            // Act
            var result = _simulation.ProcessCommand(commandString);

            // Assert
            Assert.False(result.IsSuccess);
            _loggerMock.Verify(
             m => m.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((o, t) => o.ToString().Equals($"An error occurred while processing command: {commandString}")),
                 It.Is<Exception>(x => x.Message == "Surface error"),
                 It.IsAny<Func<It.IsAnyType, Exception, string>>()),
             Times.AtLeastOnce);
        }
    }
}
