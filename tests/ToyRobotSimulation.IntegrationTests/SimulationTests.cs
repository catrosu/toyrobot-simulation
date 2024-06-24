using ToyRobotSimulation.Core;
using ToyRobotSimulation.Domain.Interfaces;
using ToyRobotSimulation.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RobotSimulation.ConsoleApp;

namespace ToyRobotSimulation.IntegrationTests
{
    public class SimulationTests
    {
        private readonly IHost _host;
        private readonly Simulation _simulation;

        public SimulationTests()
        {
            _host = CreateHostBuilder().Build();
            _simulation = _host.Services.GetRequiredService<Simulation>();
        }

        private IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var tableSettings = context.Configuration.GetSection("SurfaceConfiguration").Get<SurfaceConfiguration>();

                    services.AddScoped<ISurface>(provider => new Table(tableSettings.Width, tableSettings.Height));
                    services.AddScoped<IToy, Robot>();
                    services.AddSingleton<Simulation>();
                });

        [Fact]
        public void Place_Move_And_Report_Should_Return_Correct_Position()
        {
            // Arrange
            var placeCommand = "PLACE 0,0,NORTH";
            var moveCommand = CommandConstants.MoveCommand;
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var placeResult = _simulation.ProcessCommand(placeCommand);
            var moveResult = _simulation.ProcessCommand(moveCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.True(placeResult.IsSuccess);
            Assert.True(moveResult.IsSuccess);
            Assert.True(reportResult.IsSuccess);
            Assert.Equal("0,1,NORTH", reportResult.Value);
        }

        [Fact]
        public void Turn_Left_And_Report_Should_Return_Correct_Direction()
        {
            // Arrange
            var placeCommand = "PLACE 0,0,NORTH";
            var leftCommand = CommandConstants.LeftCommand;
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var placeResult = _simulation.ProcessCommand(placeCommand);
            var leftResult = _simulation.ProcessCommand(leftCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.True(placeResult.IsSuccess);
            Assert.True(leftResult.IsSuccess);
            Assert.True(reportResult.IsSuccess);
            Assert.Equal("0,0,WEST", reportResult.Value);
        }

        [Fact]
        public void Complex_Sequence_Should_Return_Correct_Final_Position()
        {
            // Arrange
            var placeCommand = "PLACE 1,2,EAST";
            var moveCommand = CommandConstants.MoveCommand;
            var leftCommand = CommandConstants.LeftCommand;
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var placeResult = _simulation.ProcessCommand(placeCommand);
            var moveResult1 = _simulation.ProcessCommand(moveCommand);
            var moveResult2 = _simulation.ProcessCommand(moveCommand);
            var leftResult = _simulation.ProcessCommand(leftCommand);
            var moveResult3 = _simulation.ProcessCommand(moveCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.True(placeResult.IsSuccess);
            Assert.True(moveResult1.IsSuccess);
            Assert.True(moveResult2.IsSuccess);
            Assert.True(leftResult.IsSuccess);
            Assert.True(moveResult3.IsSuccess);
            Assert.True(reportResult.IsSuccess);
            Assert.Equal("3,3,NORTH", reportResult.Value);
        }

        [Theory]
        [InlineData("PLACE a,0,NORTH")]
        [InlineData("PLACE 5,5,NORTH,EAST")]
        [InlineData("PLACE 0,0,NWSE")]
        [InlineData("PLACE 0,0,INVALID")]
        public void Invalid_PlaceCommand_Should_Return_Invalid(string placeCommand)
        {
            // Act
            var result = _simulation.ProcessCommand(placeCommand);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid PLACE command", result.ValidationErrors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Move_Without_Placing_Should_Return_Invalid()
        {
            // Arrange
            var moveCommand = CommandConstants.MoveCommand;
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var moveResult = _simulation.ProcessCommand(moveCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.False(moveResult.IsSuccess);
            Assert.Contains("The first command must be a PLACE command.", moveResult.ValidationErrors.Select(e => e.ErrorMessage));
            Assert.False(reportResult.IsSuccess);
            Assert.Contains("The toy robot is not placed on the table.", reportResult.ValidationErrors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Left_Without_Placing_Should_Return_Invalid()
        {
            // Arrange
            var leftCommand = CommandConstants.LeftCommand;
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var leftResult = _simulation.ProcessCommand(leftCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.False(leftResult.IsSuccess);
            Assert.Contains("The first command must be a PLACE command.", leftResult.ValidationErrors.Select(e => e.ErrorMessage));
            Assert.False(reportResult.IsSuccess);
            Assert.Contains("The toy robot is not placed on the table.", reportResult.ValidationErrors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Right_Without_Placing_Should_Return_Invalid()
        {
            // Arrange
            var rightCommand = CommandConstants.RightCommand;
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var rightResult = _simulation.ProcessCommand(rightCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.False(rightResult.IsSuccess);
            Assert.Contains("The first command must be a PLACE command.", rightResult.ValidationErrors.Select(e => e.ErrorMessage));
            Assert.False(reportResult.IsSuccess);
            Assert.Contains("The toy robot is not placed on the table.", reportResult.ValidationErrors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Second_PlaceCommand_Should_Override_First_Position()
        {
            // Arrange
            var placeCommand1 = "PLACE 0,0,NORTH";
            var moveCommand = CommandConstants.MoveCommand;
            var placeCommand2 = "PLACE 1,1,EAST";
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var placeResult1 = _simulation.ProcessCommand(placeCommand1);
            var moveResult = _simulation.ProcessCommand(moveCommand);
            var placeResult2 = _simulation.ProcessCommand(placeCommand2);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.True(placeResult1.IsSuccess);
            Assert.True(moveResult.IsSuccess);
            Assert.True(placeResult2.IsSuccess);
            Assert.True(reportResult.IsSuccess);
            Assert.Equal("1,1,EAST", reportResult.Value);
        }

        [Theory]
        [InlineData("PLACE 0,0,NORTH", "0,0,NORTH")]
        [InlineData("PLACE 1,2,EAST", "1,2,EAST")]
        [InlineData("PLACE 3,4,SOUTH", "3,4,SOUTH")]
        [InlineData("PLACE 2,3,WEST", "2,3,WEST")]
        [InlineData("PLACE 4,4,NORTH", "4,4,NORTH")]
        public void PlaceAndReport_Should_Return_Expected_Position(string placeCommand, string expectedReport)
        {
            // Arrange
            var reportCommand = CommandConstants.ReportCommand;

            // Act
            var placeResult = _simulation.ProcessCommand(placeCommand);
            var reportResult = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.True(placeResult.IsSuccess);
            Assert.True(reportResult.IsSuccess);
            Assert.Equal(expectedReport, reportResult.Value);
        }

        [Fact]
        public void Invalid_Commands_Should_Be_Ignored_Until_Place()
        {
            // Arrange
            var moveCommand = CommandConstants.MoveCommand;
            var leftCommand = CommandConstants.LeftCommand;
            var rightCommand = CommandConstants.RightCommand;
            var reportCommand = CommandConstants.ReportCommand;
            var placeCommand = "PLACE 0,0,NORTH";

            // Act
            var moveResult = _simulation.ProcessCommand(moveCommand);
            var leftResult = _simulation.ProcessCommand(leftCommand);
            var rightResult = _simulation.ProcessCommand(rightCommand);
            var reportResultBeforePlace = _simulation.ProcessQuery(reportCommand);
            var placeResult = _simulation.ProcessCommand(placeCommand);
            var reportResultAfterPlace = _simulation.ProcessQuery(reportCommand);

            // Assert
            Assert.False(moveResult.IsSuccess);
            Assert.False(leftResult.IsSuccess);
            Assert.False(rightResult.IsSuccess);
            Assert.False(reportResultBeforePlace.IsSuccess);
            Assert.True(placeResult.IsSuccess);
            Assert.True(reportResultAfterPlace.IsSuccess);
            Assert.Equal("0,0,NORTH", reportResultAfterPlace.Value);
        }
    }
}
