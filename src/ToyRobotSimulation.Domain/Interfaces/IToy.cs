namespace ToyRobotSimulation.Domain.Interfaces
{
    public interface IToy
    {
        Position Position { get; }

        bool IsPlaced { get; }
        void Place(Position position);
        void Move();
        void Left();
        void Right();
        string Report();
    }
}
