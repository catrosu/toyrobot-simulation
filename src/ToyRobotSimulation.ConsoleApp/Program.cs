using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ToyRobotSimulation.ConsoleApp.Helpers;
using ToyRobotSimulation.Core;
using ToyRobotSimulation.Domain.Entities;
using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.ConsoleApp
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Logging.ConfigureLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                var processor = host.Services.GetRequiredService<CommandProcessor>();

                Console.WriteLine(ConsoleConstants.WelcomeMessage);

                if (args.Length > 0)
                {
                    // Read commands from the file
                    var filePath = args[0];
                    var fileReader = host.Services.GetRequiredService<FileReader>();
                    await fileReader.ReadCommandsFromFileAsync(filePath, cancellationToken);
                    return 0;
                }
                else
                {
                    Console.WriteLine(string.Format(ConsoleConstants.Instructions, Environment.GetLogicalDrives()[0]));

                    // Read commands from standard input
                    while (true)
                    {
                        var input = Console.ReadLine();

                        if (input is null) break;

                        if (input.Trim().Equals(ConsoleConstants.ExitCommand, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Exiting the simulation.");
                            break;
                        }

                        processor.ProcessInput(input);
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred");
                Console.WriteLine("An unexpected error occurred. Please check the log for details.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostContext, loggerConfiguration) => Logging.ConfigureSerilog(loggerConfiguration))
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions<SurfaceConfiguration>()
                        .Bind(context.Configuration.GetSection("SurfaceConfiguration"))
                        .ValidateOnStart();

                    services.AddScoped<IToy, Robot>();

                    services.AddSingleton<ISurfaceFactory, SurfaceFactory>();
                    services.AddScoped(provider => provider.GetRequiredService<ISurfaceFactory>().New());

                    services.AddSingleton<Simulation>();
                    services.AddSingleton<CommandProcessor>();
                    services.AddSingleton<FileReader>();
                });
    }
}
