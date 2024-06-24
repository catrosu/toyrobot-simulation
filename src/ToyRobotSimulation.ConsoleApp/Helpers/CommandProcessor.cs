using ToyRobotSimulation.Core;
using ToyRobotSimulation.Domain.Entities;
using ToyRobotSimulation.Domain;
using Microsoft.Extensions.Logging;

namespace ToyRobotSimulation.ConsoleApp.Helpers
{
    /// <summary>
    /// Processes the commands received either from the file or direct input in the console app
    /// </summary>
    public class CommandProcessor
    {
        private readonly Simulation _simulation;
        private readonly ILogger<CommandProcessor> _logger;

        public CommandProcessor(Simulation simulation, ILogger<CommandProcessor> logger)
        {
            _simulation = simulation;
            _logger = logger;
        }

        /// <summary>
        /// Processes the input data, and prints out the outcome
        /// </summary>
        /// <param name="input">The command</param>
        /// <param name="fromFile">Flag to help outputting the commands from the file</param>
        public void ProcessInput(string input, bool fromFile = false)
        {
            try
            {
                if (input.Equals("error", StringComparison.CurrentCultureIgnoreCase)) throw new ArgumentException($"The {input} input throws an exception for testing purposes.");

                if (fromFile)
                {
                    Console.WriteLine(input); // Print the command text only if it is from a file
                }

                // 'REPORT' handled as a Query
                if (input.Trim().ToUpper() == CommandConstants.ReportCommand)
                {
                    var reportResult = _simulation.ProcessQuery(input);
                    if (reportResult.IsSuccess)
                    {
                        Console.WriteLine(reportResult.Value);
                    }
                    else
                    {
                        Console.WriteLine(string.Join(", ", reportResult.ValidationErrors.Select(err => err.ErrorMessage)));
                    }
                }
                else // everything else is handled as a Command
                {
                    var result = _simulation.ProcessCommand(input);
                    if (!result.IsSuccess)
                    {
                        Console.WriteLine(string.Join(", ", result.ValidationErrors.Select(err => err.ErrorMessage)));
                    }
                }

                // update the grid
                DisplayGrid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing input: {input}", input);
                Console.WriteLine("An error occurred while processing the command. Please try again. Refer to the log file for more details.");
            }
        }

        /// <summary>
        /// Displays the current position on a grid
        /// </summary>
        private void DisplayGrid()
        {
            var robot = (Robot)_simulation.GetToy();
            var table = (Table)_simulation.GetSurface();

            int width = table.Width;
            int height = table.Height;

            char[,] grid = new char[height, width];

            // Initialize the grid with empty spaces
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = '.';
                }
            }

            // Place the robot on the grid
            if (robot.IsPlaced)
            {
                grid[height - 1 - robot.Position.Point.Y, robot.Position.Point.X] = GetDirectionChar(robot.Position.Facing);
            }

            // Display the grid
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(grid[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private char GetDirectionChar(Direction direction)
        {
            return direction switch
            {
                Direction.NORTH => '^',
                Direction.EAST => '>',
                Direction.SOUTH => 'v',
                Direction.WEST => '<',
                _ => ' '
            };
        }
    }
}
