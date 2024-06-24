namespace ToyRobotSimulation.Domain
{
    public record Position(Point Point, Direction Facing)
    {
        public static readonly Position Default = new Position(new Point(-1, -1), Direction.NORTH);

        public Position Move() =>
            this with { Point = Point.Move(Facing) };

        public Position TurnLeft() =>
            this with
            {
                Facing = Facing switch
                {
                    Direction.NORTH => Direction.WEST,
                    Direction.WEST => Direction.SOUTH,
                    Direction.SOUTH => Direction.EAST,
                    Direction.EAST => Direction.NORTH,
                    _ => Facing
                }
            };

        public Position TurnRight() =>
            this with
            {
                Facing = Facing switch
                {
                    Direction.NORTH => Direction.EAST,
                    Direction.EAST => Direction.SOUTH,
                    Direction.SOUTH => Direction.WEST,
                    Direction.WEST => Direction.NORTH,
                    _ => Facing
                }
            };

        public override string ToString() => $"{Point.X},{Point.Y},{Facing}";
    }
}
