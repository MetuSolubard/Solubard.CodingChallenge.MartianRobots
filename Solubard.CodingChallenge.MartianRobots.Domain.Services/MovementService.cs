using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces;
using Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Services
{
    public class MovementService : IMovementService
    {
        private HashSet<(int x, int y)> _scents;

        public MovementService(
            HashSet<(int x, int y)> scents
            )
        {
            if (scents == null)
            {
                throw new ArgumentNullException($"{nameof(scents)} must be initialized in Dependency Injection configuration.");
            }
            else
            {
                _scents = scents;
            }
        }

        public IRobot ExecuteMovementInstructions(
            int gridMaxX,
            int gridMaxY,
            IRobot robot,
            IList<Movement> movementInstructions
            )
        {
            foreach (Movement movementInstruction in movementInstructions)
            {
                if (robot.IsLost) break;

                switch (movementInstruction)
                {
                    case Movement.TurnLeft:
                        robot.CurrentDirection = TurnLeft(robot.CurrentDirection);
                        robot.InstructionsProcessed.Add(movementInstruction);
                        break;
                    case Movement.TurnRight:
                        robot.CurrentDirection = TurnRight(robot.CurrentDirection);
                        robot.InstructionsProcessed.Add(movementInstruction);
                        break;
                    case Movement.Forward:
                        (int x, int y) newPosition = MoveForward(robot);

                        if (IsOutOfBounds(gridMaxX, gridMaxY, newPosition.x, newPosition.y))
                        {
                            // Check if there's a scent at current position
                            if (_scents.Contains((robot.CurrentXPosition, robot.CurrentYPosition)))
                            {
                                robot.InstructionsProcessed.Add(Movement.None);
                                break;
                            }
                            else
                            {
                                // Robot falls off and leaves scent
                                _scents.Add((robot.CurrentXPosition, robot.CurrentYPosition));
                                robot.IsLost = true;
                                robot.InstructionsProcessed.Add(movementInstruction);
                            }
                        }
                        else
                        {
                            // Move forward
                            robot.CurrentXPosition = newPosition.x;
                            robot.CurrentYPosition = newPosition.y;
                            robot.InstructionsProcessed.Add(movementInstruction);
                        }
                        break;
                    default:
                        // Future-proofing: ignore unknown commands
                        break;
                }
            }

            return robot;
        }

        private Direction TurnLeft(Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.West,
                Direction.West => Direction.South,
                Direction.South => Direction.East,
                Direction.East => Direction.North,
                _ => throw new InvalidOperationException($"Unknown direction: {direction}")
            };
        }

        private Direction TurnRight(Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new InvalidOperationException($"Unknown direction: {direction}")
            };
        }

        private (int x, int y) MoveForward(IRobot robot)
        {
            return robot.CurrentDirection switch
            {
                Direction.North => (robot.CurrentXPosition, robot.CurrentYPosition + 1),
                Direction.South => (robot.CurrentXPosition, robot.CurrentYPosition - 1),
                Direction.East => (robot.CurrentXPosition + 1, robot.CurrentYPosition),
                Direction.West => (robot.CurrentXPosition - 1, robot.CurrentYPosition),
                _ => throw new InvalidOperationException($"Unknown movement: {robot.CurrentDirection}")
            };
        }

        private bool IsOutOfBounds(
            int gridMaxX,
            int gridMaxY,
            int currentX,
            int currentY
            )
        {
            return currentX < 0 || currentX > gridMaxX || currentY < 0 || currentY > gridMaxY;
        }
    }
}
