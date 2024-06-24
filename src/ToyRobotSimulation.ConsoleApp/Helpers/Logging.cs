using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;

namespace ToyRobotSimulation.ConsoleApp.Helpers
{
    /// <summary>
    /// Configures the Logging
    /// </summary>
    internal class Logging
    {
        // Configure Serilog
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();
        }

        public static LoggerConfiguration ConfigureSerilog(LoggerConfiguration loggerConfig) =>
            loggerConfig.WriteTo.File(
                       formatter: new RenderedCompactJsonFormatter(),
                       path: $"{Environment.GetLogicalDrives()[0]}\\logs\\toyrobot-simulator.json",
                       restrictedToMinimumLevel: LogEventLevel.Debug,
                       fileSizeLimitBytes: 5 * 1024 * 1024 // limit to 5MB in dev
                       );
    }
}
