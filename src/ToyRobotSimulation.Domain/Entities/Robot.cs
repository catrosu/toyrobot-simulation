using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Domain.Entities
{
    /// <summary>
    /// The Robot domain entity
    /// </summary>
    public class Robot : IToy
    {
        public Position Position { get; private set; } = Position.Default;
        public bool IsPlaced { get; private set; } = false;

        public void Place(Position position)
        {
            Position = position;
            IsPlaced = true;
        }

        public void Move()
        {
            if (!IsPlaced) return;

            Position = Position.Move();
        }

        public void Left()
        {
            if (!IsPlaced) return;

            Position = Position.TurnLeft();
        }

        public void Right()
        {
            if (!IsPlaced) return;

            Position = Position.TurnRight();
        }

        public string Report() => Position.ToString();
    }
}
