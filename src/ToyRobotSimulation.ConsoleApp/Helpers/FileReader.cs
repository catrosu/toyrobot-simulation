using Microsoft.Extensions.Logging;

namespace ToyRobotSimulation.ConsoleApp.Helpers
{
    /// <summary>
    /// Helper to read the commands from a file
    /// </summary>
    public class FileReader
    {
        private readonly CommandProcessor _processor;
        private readonly ILogger<FileReader> _logger;

        public FileReader(CommandProcessor processor, ILogger<FileReader> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public async Task ReadCommandsFromFileAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    _logger.LogInformation("Reading commands from file: {filePath}", filePath);
                    var commands = await File.ReadAllLinesAsync(filePath, cancellationToken);
                    foreach (var command in commands)
                    {
                        _processor.ProcessInput(command, true);
                    }
                }
                else
                {
                    _logger.LogError("File not found: {filePath}", filePath);
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading the file: {filePath}", filePath);
                Console.WriteLine("An error occurred while processing the command. Please check the file path and try again. Refer to the log file for more details.");
            }
        }
    }
}
