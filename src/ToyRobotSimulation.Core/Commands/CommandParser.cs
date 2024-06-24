using Ardalis.Result;
using ToyRobotSimulation.Domain;

namespace ToyRobotSimulation.Core.Commands
{
    /// <summary>
    /// Parses command strings and creates corresponding command.
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// Parses a command string and creates a corresponding <see cref="ICommand"/> object.
        /// </summary>
        /// <param name="commandString">The command string to be parsed.</param>
        /// <returns>A <see cref="Result{ICommand}"/> containing the command object or validation errors.</returns>
        public static Result<ICommand> ParseCommand(string commandString)
        {
            var parts = commandString.Trim().ToUpper().Split(' ');
            var command = parts[0];

            return command switch
            {
                CommandConstants.PlaceCommand => ParsePlaceCommand(parts),
                CommandConstants.MoveCommand when parts.Length == 1 => Result<ICommand>.Success(new MoveCommand()),
                CommandConstants.LeftCommand when parts.Length == 1 => Result<ICommand>.Success(new LeftCommand()),
                CommandConstants.RightCommand when parts.Length == 1 => Result<ICommand>.Success(new RightCommand()),
                _ => Result<ICommand>.Invalid(new ValidationError("Invalid command"))
            };
        }


        /// <summary>
        /// Parses the arguments for the PLACE command and creates a <see cref="PlaceCommand"/> object.
        /// </summary>
        /// <param name="parts">The array of command parts.</param>
        /// <returns>A <see cref="Result{ICommand}"/> containing the <see cref="PlaceCommand"/> object or validation errors.</returns>
        private static Result<ICommand> ParsePlaceCommand(string[] parts)
        {
            if (parts.Length != 2) return Result<ICommand>.Invalid(new ValidationError("Invalid PLACE command"));

            var placeArgs = parts[1].Split(',');
            if (placeArgs.Length != 3) return Result<ICommand>.Invalid(new ValidationError("Invalid PLACE command"));

            if (!int.TryParse(placeArgs[0], out int x) || !int.TryParse(placeArgs[1], out int y) ||
                !Enum.TryParse<Direction>(placeArgs[2], true, out Direction direction))
            {
                return Result<ICommand>.Invalid(new ValidationError("Invalid PLACE command"));
            }

            return Result<ICommand>.Success(new PlaceCommand(new Position(new Point(x, y), direction)));
        }
    }
}