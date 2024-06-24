using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Commands
{
    /// <summary>
    /// The commmand to turn the toy robot 90 degrees to the left.
    /// </summary>
    public class LeftCommand : ICommand
    {
        public void Execute(IToy toy, ISurface surface)
        {
            toy.Left();
        }
    }
}
