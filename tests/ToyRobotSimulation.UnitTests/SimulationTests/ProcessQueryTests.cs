using Moq;
using Microsoft.Extensions.Logging;
using ToyRobotSimulation.Core;
using ToyRobotSimulation.Domain.Interfaces;
using ToyRobotSimulation.Domain;

namespace ToyRobotSimulation.Tests
{
    public class ProcessQueryTests
    {
        private readonly Mock<IToy> _toyMock;
        private readonly Mock<ISurface> _surfaceMock;
        private readonly Mock<ILogger<Simulation>> _loggerMock;
        private readonly Simulation _simulation;

        public ProcessQueryTests()
        {
            _toyMock = new Mock<IToy>();
            _surfaceMock = new Mock<ISurface>();
            _loggerMock = new Mock<ILogger<Simulation>>();
            _simulation = new Simulation(_toyMock.Object, _surfaceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public void ProcessQuery_ShouldReturnSuccess_WithValidReport()
        {
            // Arrange
            var queryString = "REPORT";
            _toyMock.Setup(t => t.IsPlaced).Returns(true);
            _toyMock.Setup(t => t.Report()).Returns("1,2,NORTH");

            // Act
            var result = _simulation.ProcessQuery(queryString);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("1,2,NORTH", result.Value);
        }

        [Fact]
        public void ProcessQuery_ShouldReturnInvalid_WhenRobotIsNotPlaced()
        {
            // Arrange
            var queryString = "REPORT";

            // Act
            var result = _simulation.ProcessQuery(queryString);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.ValidationErrors, v => v.ErrorMessage == "The toy robot is not placed on the table.");
        }

        [Fact]
        public void ProcessQuery_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            var queryString = "REPORT";
            _toyMock.Setup(t => t.IsPlaced).Returns(true);
            _toyMock.Setup(t => t.Report()).Throws(new Exception("Toy error"));
            _surfaceMock.Setup(s => s.IsValidPosition(It.IsAny<Point>())).Returns(true);
            _simulation.ProcessCommand("PLACE 1,2,NORTH");

            // Act
            var result = _simulation.ProcessQuery(queryString);

            // Assert
            Assert.False(result.IsSuccess);

            _loggerMock.Verify(
              m => m.Log(
                  LogLevel.Error,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((o, t) => o.ToString().Equals($"An error occurred while processing the query: {queryString}")),
                  It.Is<Exception>(x => x.Message == "Toy error"),
                  It.IsAny<Func<It.IsAnyType, Exception, string>>()),
              Times.AtLeastOnce);
        }
    }
}
