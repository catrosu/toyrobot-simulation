namespace ToyRobotSimulation.Domain
{
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point Move(Direction direction)
        {
            return direction switch
            {
                Direction.NORTH => new Point(X, Y + 1),
                Direction.SOUTH => new Point(X, Y - 1),
                Direction.EAST => new Point(X + 1, Y),
                Direction.WEST => new Point(X - 1, Y),
                _ => this
            };
        }

        public override string ToString() => $"{X},{Y}";
    }
}
