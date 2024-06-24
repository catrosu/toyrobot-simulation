using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Commands
{
    public interface ICommand
    {
        void Execute(IToy toy, ISurface surface);
    }
}
