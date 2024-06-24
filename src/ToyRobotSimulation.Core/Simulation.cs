using Ardalis.Result;
using Microsoft.Extensions.Logging;
using ToyRobotSimulation.Core.Commands;
using ToyRobotSimulation.Core.Queries;
using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core
{
    /// <summary>
    /// The Simulation class orchestrates the interactions between the toy robot and the table surface it moves on.
    /// </summary>
    public class Simulation
    {
        private readonly IToy _toy;
        private readonly ISurface _surface;
        private readonly ILogger<Simulation> _logger;

        public Simulation(IToy toy, ISurface surface, ILogger<Simulation> logger)
        {
            _toy = toy;
            _surface = surface;
            _logger = logger;
        }

        /// <summary>
        /// Processes a command.
        /// </summary>
        /// <param name="input">The command string to be processed.</param>
        /// <returns>A <see cref="Result"/> indicating the success or failure of the command processing.</returns>
        public Result ProcessCommand(string input)
        {
            try
            {
                // Get the executable command i.e. LeftCommand based on the text input i.e. 'LEFT'
                var result = CommandParser.ParseCommand(input);
                if (!result.IsSuccess)
                {
                    return Result.Invalid(result.ValidationErrors);
                }

                var command = result.Value;

                if (command is PlaceCommand placeCommand)
                {
                    if (!_surface.IsValidPosition(placeCommand.Position.Point))
                    {
                        return Result.Invalid(new ValidationError("Invalid position. The robot cannot be placed outside the surface."));
                    }
                    placeCommand.Execute(_toy, _surface);
                    return Result.Success();
                }

                // Ensure the first command is a PlaceCommand
                if (!_toy.IsPlaced)
                {
                    // Ignore commands until a PLACE command is issued
                    return Result.Invalid(new ValidationError("The first command must be a PLACE command."));
                }

                command.Execute(_toy, _surface);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing command: {input}", input);
                return Result.Error(ex.Message);
            }
        }

        /// <summary>
        /// Processes a query command.
        /// </summary>
        /// <param name="input">The query command string to be processed.</param>
        /// <returns>A <see cref="Result{T}"/> containing the result of the query or validation errors.</returns>
        public Result<string> ProcessQuery(string input)
        {
            try
            {
                var result = QueryParser.ParseQuery(input);
                if (!result.IsSuccess)
                {
                    return Result.Invalid(result.ValidationErrors);
                }

                if (!_toy.IsPlaced)
                {
                    return Result.Invalid(new ValidationError("The toy robot is not placed on the table."));
                }

                var query = result.Value;
                var report = query.Execute(_toy);
                return Result.Success(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the query: {input}", input);
                return Result.Error(ex.Message);
            }
        }

        /// <summary>
        /// Gets the toy robot instance.
        /// </summary>
        /// <returns>The toy robot.</returns>
        public IToy GetToy()
        {
            return _toy;
        }

        /// <summary>
        /// Gets the table surface instance.
        /// </summary>
        /// <returns>The surface on which the toy robot moves.</returns>
        public ISurface GetSurface()
        {
            return _surface;
        }
    }
}
