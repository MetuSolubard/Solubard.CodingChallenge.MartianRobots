namespace StraightForwardWay
{
    public class Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; private set; }

        public Robot(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public void TurnLeft()
        {
            Direction = Direction switch
            {
                Direction.North => Direction.West,
                Direction.West => Direction.South,
                Direction.South => Direction.East,
                Direction.East => Direction.North,
                _ => throw new InvalidOperationException($"Unknown direction: {Direction}")
            };
        }

        public void TurnRight()
        {
            Direction = Direction switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new InvalidOperationException($"Unknown direction: {Direction}")
            };
        }

        public (int x, int y) GetNextPosition()
        {
            return Direction switch
            {
                Direction.North => (X, Y + 1),
                Direction.South => (X, Y - 1),
                Direction.East => (X + 1, Y),
                Direction.West => (X - 1, Y),
                _ => throw new InvalidOperationException($"Unknown direction: {Direction}")
            };
        }

        public void MoveForward()
        {
            var nextPos = GetNextPosition();
            X = nextPos.x;
            Y = nextPos.y;
        }
    }
}
