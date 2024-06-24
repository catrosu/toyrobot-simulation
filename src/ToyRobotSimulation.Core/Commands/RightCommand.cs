using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Commands
{
    /// <summary>
    /// The commmand to turn the toy robot 90 degrees to the right.
    /// </summary>
    public class RightCommand : ICommand
    {
        public void Execute(IToy toy, ISurface surface)
        {
            toy.Right();
        }
    }
}
