using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Domain.Entities
{
    /// <summary>
    /// The Table domain entity
    /// </summary>
    public class Table : ISurface
    {
        public int Width { get; }
        public int Height { get; }

        public Table(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsValidPosition(Point point)
        {
            return point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;
        }
    }
}
