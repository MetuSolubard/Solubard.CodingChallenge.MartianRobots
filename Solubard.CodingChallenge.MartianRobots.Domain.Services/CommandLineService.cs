using Solubard.CodingChallenge.MartianRobots.Domain.Model;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Services
{
    public class CommandLineService : ICommandLineService
    {
        private List<string> _lines;

        public CommandLineService()
        {
            _lines = null;
        }

        public (int, int) GetGridDimensions()
        {
            EnsureInit();

            _lines = ReadMultiLineInput();

            // Parse grid dimensions
            var gridDimensions = _lines[0].Split(' ');
            return (int.Parse(gridDimensions[0]), int.Parse(gridDimensions[1]));
        }

        public IList<(Rover, IList<Movement>)> GetRoversWithInstructions()
        {
            EnsureInit();

            var roversWithInstructions = new List<(Rover, IList<Movement>)>();

            for (int i = 1; i < _lines.Count; i += 2)
            {
                if (i + 1 >= _lines.Count) break;

                var positionLine = _lines[i];
                var instructionLine = _lines[i + 1];

                roversWithInstructions.Add((ParseRover(positionLine), ParseMovement(instructionLine)));
            }

            return roversWithInstructions;
        }

        public string MovementToString(Movement movement)
        {
            return movement switch
            {
                Movement.TurnLeft => "L",
                Movement.TurnRight => "R",
                Movement.Forward => "F",
                _ => throw new ArgumentException($"Invalid movement: {movement}")
            };
        }

        public string DirectionToString(Direction direction)
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

        private List<string> ReadMultiLineInput()
        {
            var lines = new List<string>();
            string line;
            int emptyLineCount = 0;

            while ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    emptyLineCount++;
                    if (emptyLineCount >= 2) break;
                }
                else
                {
                    emptyLineCount = 0;
                    lines.Add(line);
                }
            }

            return lines;
        }

        private void EnsureInit()
        {
            if (_lines == null)
            {
                _lines = new List<string>();
            }
        }

        private Rover ParseRover(string positionLine)
        {
            var parts = positionLine.Split(' ');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            return new Rover
            {
                CurrentDirection = ParseDirection(parts[2]),
                CurrentXPosition = x,
                CurrentYPosition = y,
                InstructionsProcessed = new List<Movement>(),
                IsLost = false,
            };
        }

        private Direction ParseDirection(string direction)
        {
            return direction switch
            {
                "N" => Direction.North,
                "S" => Direction.South,
                "E" => Direction.East,
                "W" => Direction.West,
                _ => throw new ArgumentException($"Invalid direction: {direction}")
            };
        }

        private IList<Movement> ParseMovement(string instructions)
        {
            var movementInstructions = new List<Movement>();

            foreach (char instruction in instructions)
            {
                switch (instruction)
                {
                    case 'L':
                        movementInstructions.Add(Movement.TurnLeft);
                        break;
                    case 'R':
                        movementInstructions.Add(Movement.TurnRight);
                        break;
                    case 'F':
                        movementInstructions.Add(Movement.Forward);
                        break;
                    default:
                        break;
                }
            }

            return movementInstructions;
        }
    }
}
