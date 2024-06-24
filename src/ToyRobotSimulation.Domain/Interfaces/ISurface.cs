namespace ToyRobotSimulation.Domain.Interfaces
{
    public interface ISurface
    {
        int Width { get; }
        int Height { get; }

        bool IsValidPosition(Point point);
    }
}

