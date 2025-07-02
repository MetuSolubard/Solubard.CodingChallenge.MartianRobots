namespace StraightForwardWay
{
    public class MartianRobotSimulator
    {
        private int _maxX;
        private int _maxY;
        private HashSet<(int x, int y)> _scents;

        public MartianRobotSimulator()
        {
            _scents = new HashSet<(int, int)>();
        }

        public string ProcessInput(string input)
        {
            var lines = input.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var results = new List<string>();

            // Parse grid dimensions
            var gridDimensions = lines[0].Split(' ');
            _maxX = int.Parse(gridDimensions[0]);
            _maxY = int.Parse(gridDimensions[1]);

            // Reset scents for new simulation
            _scents.Clear();

            // Process each robot
            for (int i = 1; i < lines.Length; i += 2)
            {
                if (i + 1 >= lines.Length) break;

                var positionLine = lines[i];
                var instructionLine = lines[i + 1];

                var robot = ParseRobot(positionLine);
                var result = ProcessRobot(robot, instructionLine);
                results.Add(result);
            }

            return string.Join("\n", results);
        }

        private Robot ParseRobot(string positionLine)
        {
            var parts = positionLine.Split(' ');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            Direction direction = ParseDirection(parts[2]);

            return new Robot(x, y, direction);
        }

        private Direction ParseDirection(string directionStr)
        {
            return directionStr switch
            {
                "N" => Direction.North,
                "S" => Direction.South,
                "E" => Direction.East,
                "W" => Direction.West,
                _ => throw new ArgumentException($"Invalid direction: {directionStr}")
            };
        }

        private string ProcessRobot(Robot robot, string instructions)
        {
            bool isLost = false;

            foreach (char instruction in instructions)
            {
                if (isLost) break;

                switch (instruction)
                {
                    case 'L':
                        robot.TurnLeft();
                        break;
                    case 'R':
                        robot.TurnRight();
                        break;
                    case 'F':
                        var newPosition = robot.GetNextPosition();

                        if (IsOutOfBounds(newPosition.x, newPosition.y))
                        {
                            // Check if there's a scent at current position
                            if (!_scents.Contains((robot.X, robot.Y)))
                            {
                                // Robot falls off and leaves scent
                                _scents.Add((robot.X, robot.Y));
                                isLost = true;
                            }
                            // If there's a scent, ignore the forward command
                        }
                        else
                        {
                            // Move forward
                            robot.MoveForward();
                        }
                        break;
                    default:
                        // Future-proofing: ignore unknown commands
                        break;
                }
            }

            string result = $"{robot.X} {robot.Y} {DirectionToString(robot.Direction)}";
            if (isLost)
            {
                result += " LOST";
            }

            return result;
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x > _maxX || y < 0 || y > _maxY;
        }

        private string DirectionToString(Direction direction)
        {
            return direction switch
            {
                Direction.North => "N",
                Direction.South => "S",
                Direction.East => "E",
                Direction.West => "W",
                _ => throw new ArgumentException($"Invalid direction: {direction}")
            };
        }
    }
}
