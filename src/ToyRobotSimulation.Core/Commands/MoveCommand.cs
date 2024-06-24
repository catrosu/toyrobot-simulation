using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Commands
{
    /// <summary>
    /// The commmand to move the toy robot, only if the robot is placed.
    /// If the robot is not placed, the command is being ignored.
    /// </summary>
    public class MoveCommand : ICommand
    {
        public void Execute(IToy toy, ISurface surface)
        {
            if (!toy.IsPlaced) return;

            var newPosition = toy.Position.Move();
            if (surface.IsValidPosition(newPosition.Point))
            {
                toy.Move();
            }
        }
    }
}
