using ToyRobotSimulation.Domain;
using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Commands
{
    /// <summary>
    /// The command to place the toy robot on the surface at a specified position and direction.
    /// The robot is being placed only if the required position is a valid one.
    /// </summary>
    public class PlaceCommand : ICommand
    {
        public Position Position { get; private set; }

        public PlaceCommand(Position position)
        {
            Position = position;
        }

        public void Execute(IToy toy, ISurface surface)
        {
            if (surface.IsValidPosition(Position.Point))
            {
                toy.Place(Position);
            }
        }
    }
}
