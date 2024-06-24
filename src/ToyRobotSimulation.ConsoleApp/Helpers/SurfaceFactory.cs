using Microsoft.Extensions.Options;
using ToyRobotSimulation.Domain.Entities;
using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.ConsoleApp.Helpers
{
    public interface ISurfaceFactory
    {
        ISurface New();
    }

    public class SurfaceFactory : ISurfaceFactory
    {
        private readonly SurfaceConfiguration  _surfaceConfig;

        public SurfaceFactory(IOptions<SurfaceConfiguration> surfaceConfig)
        {
            _surfaceConfig = surfaceConfig.Value;
        }

        /// <summary>
        /// Creates an instance of a table with the dimensions from the configuration
        /// </summary>
        /// <returns></returns>
        public ISurface New()
        {
            return new Table(_surfaceConfig.Width, _surfaceConfig.Height);
        }
    }
}
