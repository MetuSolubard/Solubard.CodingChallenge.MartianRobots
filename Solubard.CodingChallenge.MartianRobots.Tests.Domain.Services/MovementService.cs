using Solubard.CodingChallenge.MartianRobots.Domain.Model;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces;
using Solubard.CodingChallenge.MartianRobots.Domain.Services;

namespace Solubard.CodingChallenge.MartianRobots.Tests.Domain.Services
{
    public class MovementService
    {
        public class Constructor
        {
            [Fact]
            public void Scents_IsNull_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() =>
                    new MartianRobots.Domain.Services.MovementService(null));
            }

            [Fact]
            public void Scents_NotNull_DoesNot_ThrowException()
            {
                var scents = new HashSet<(int x, int y)> { (1, 1), (2, 2) };
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                Assert.NotNull(movementService);
            }

        }

        public class ExecuteMovementInstructions
        {
            [Fact]
            public void TurnLeft_ChangesDirectionCorrectly()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.TurnLeft });

                Assert.Equal(Direction.West, result.CurrentDirection);
                Assert.Equal(1, result.CurrentXPosition);
                Assert.Equal(1, result.CurrentYPosition);
                Assert.False(result.IsLost);
                Assert.Single(result.InstructionsProcessed);
                Assert.Equal(Movement.TurnLeft, result.InstructionsProcessed[0]);
            }

            [Fact]
            public void TurnRight_ChangesDirectionCorrectly()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.TurnRight });

                Assert.Equal(Direction.East, result.CurrentDirection);
                Assert.Equal(1, result.CurrentXPosition);
                Assert.Equal(1, result.CurrentYPosition);
            }

            [Theory]
            [InlineData(Direction.North, Direction.West)]
            [InlineData(Direction.West, Direction.South)]
            [InlineData(Direction.South, Direction.East)]
            [InlineData(Direction.East, Direction.North)]
            public void TurnLeft_AllDirections(Direction initial, Direction expected)
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = initial,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.TurnLeft });

                Assert.Equal(expected, result.CurrentDirection);
            }

            [Theory]
            [InlineData(Direction.North, Direction.East)]
            [InlineData(Direction.East, Direction.South)]
            [InlineData(Direction.South, Direction.West)]
            [InlineData(Direction.West, Direction.North)]
            public void TurnRight_AllDirections(Direction initial, Direction expected)
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = initial,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.TurnRight });

                Assert.Equal(expected, result.CurrentDirection);
            }

            [Theory]
            [InlineData(Direction.North, 1, 2)]
            [InlineData(Direction.South, 1, 0)]
            [InlineData(Direction.East, 2, 1)]
            [InlineData(Direction.West, 0, 1)]
            public void MoveForward_UpdatesPositionCorrectly(Direction direction, int expectedX, int expectedY)
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = direction,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.Forward });

                Assert.Equal(expectedX, result.CurrentXPosition);
                Assert.Equal(expectedY, result.CurrentYPosition);
                Assert.Equal(direction, result.CurrentDirection);
                Assert.False(result.IsLost);
            }

            [Fact]
            public void RobotFallsOffNorthEdge_MarkedAsLost()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 2,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(2, 2, rover, new List<Movement> { Movement.Forward });

                Assert.Equal(1, result.CurrentXPosition);
                Assert.Equal(2, result.CurrentYPosition);
                Assert.True(result.IsLost);
                Assert.Contains((1, 2), scents);
            }

            [Fact]
            public void RobotFallsOffSouthEdge_MarkedAsLost()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.South,
                    CurrentXPosition = 1,
                    CurrentYPosition = 0,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(2, 2, rover, new List<Movement> { Movement.Forward });

                Assert.Equal(1, result.CurrentXPosition);
                Assert.Equal(0, result.CurrentYPosition);
                Assert.True(result.IsLost);
            }

            [Fact]
            public void RobotFallsOffEastEdge_MarkedAsLost()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.East,
                    CurrentXPosition = 2,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(2, 2, rover, new List<Movement> { Movement.Forward });

                Assert.Equal(2, result.CurrentXPosition);
                Assert.Equal(1, result.CurrentYPosition);
                Assert.True(result.IsLost);
            }

            [Fact]
            public void RobotFallsOffWestEdge_MarkedAsLost()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.West,
                    CurrentXPosition = 0,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(2, 2, rover, new List<Movement> { Movement.Forward });

                Assert.Equal(0, result.CurrentXPosition);
                Assert.Equal(1, result.CurrentYPosition);
                Assert.True(result.IsLost);
            }

            [Fact]
            public void RobotSavedByScent_DoesNotFallOff()
            {
                var scents = new HashSet<(int x, int y)> { (1, 2) };
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 2,
                    InstructionsProcessed = new List<Movement>()
                };

                var result = movementService.ExecuteMovementInstructions(2, 2, rover, new List<Movement> { Movement.Forward });

                Assert.Equal(1, result.CurrentXPosition);
                Assert.Equal(2, result.CurrentYPosition);
                Assert.False(result.IsLost);
                Assert.Single(result.InstructionsProcessed);
                Assert.Equal(Movement.None, result.InstructionsProcessed[0]);
            }

            [Fact]
            public void LostRobotStopsProcessing_IgnoresRemainingInstructions()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 2,
                    InstructionsProcessed = new List<Movement>()
                };

                var instructions = new List<Movement> { Movement.Forward, Movement.TurnLeft, Movement.Forward };
                var result = movementService.ExecuteMovementInstructions(2, 2, rover, instructions);

                Assert.True(result.IsLost);
                Assert.Single(result.InstructionsProcessed);
                Assert.Equal(Movement.Forward, result.InstructionsProcessed[0]);
            }

            [Fact]
            public void ComplexMovementSequence_ExecutesCorrectly()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.East,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                var instructions = new List<Movement>
            {
                Movement.TurnRight, Movement.Forward, Movement.TurnRight, Movement.Forward,
                Movement.TurnRight, Movement.Forward, Movement.TurnRight, Movement.Forward
            };
                var result = movementService.ExecuteMovementInstructions(5, 5, rover, instructions);

                Assert.Equal(1, result.CurrentXPosition);
                Assert.Equal(1, result.CurrentYPosition);
                Assert.Equal(Direction.East, result.CurrentDirection);
                Assert.False(result.IsLost);
                Assert.Equal(8, result.InstructionsProcessed.Count);
            }

            [Fact]
            public void WithInvalidDirection_ThrowsInvalidOperationExceptionn()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.None,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                Assert.Throws<InvalidOperationException>(() =>
                    movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.TurnLeft }));
            }

            [Fact]
            public void WithInvalidMovement_ThrowsInvalidOperationExceptionn()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.None,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };

                Assert.Throws<InvalidOperationException>(() =>
                    movementService.ExecuteMovementInstructions(5, 5, rover, new List<Movement> { Movement.Forward }));
            }

            [Fact]
            public void WithGivenSampleData_ProducesExpectedResults()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);

                var rover1 = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.East,
                    CurrentXPosition = 1,
                    CurrentYPosition = 1,
                    InstructionsProcessed = new List<Movement>()
                };
                var instructions1 = new List<Movement>
            {
                Movement.TurnRight, Movement.Forward, Movement.TurnRight, Movement.Forward,
                Movement.TurnRight, Movement.Forward, Movement.TurnRight, Movement.Forward
            };

                var rover2 = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 3,
                    CurrentYPosition = 2,
                    InstructionsProcessed = new List<Movement>()
                };
                var instructions2 = new List<Movement>
            {
                Movement.Forward, Movement.TurnRight, Movement.TurnRight, Movement.Forward,
                Movement.TurnLeft, Movement.TurnLeft, Movement.Forward, Movement.Forward,
                Movement.TurnRight, Movement.TurnRight, Movement.Forward, Movement.TurnLeft, Movement.TurnLeft
            };

                var rover3 = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.West,
                    CurrentXPosition = 0,
                    CurrentYPosition = 3,
                    InstructionsProcessed = new List<Movement>()
                };
                var instructions3 = new List<Movement>
            {
                Movement.TurnLeft, Movement.TurnLeft, Movement.Forward, Movement.Forward,
                Movement.Forward, Movement.TurnLeft, Movement.Forward, Movement.TurnLeft,
                Movement.Forward, Movement.TurnLeft
            };

                var result1 = movementService.ExecuteMovementInstructions(5, 3, rover1, instructions1);
                var result2 = movementService.ExecuteMovementInstructions(5, 3, rover2, instructions2);
                var result3 = movementService.ExecuteMovementInstructions(5, 3, rover3, instructions3);

                Assert.Equal(1, result1.CurrentXPosition);
                Assert.Equal(1, result1.CurrentYPosition);
                Assert.Equal(Direction.East, result1.CurrentDirection);
                Assert.False(result1.IsLost);

                Assert.Equal(3, result2.CurrentXPosition);
                Assert.Equal(3, result2.CurrentYPosition);
                Assert.Equal(Direction.North, result2.CurrentDirection);
                Assert.True(result2.IsLost);

                Assert.Equal(2, result3.CurrentXPosition);
                Assert.Equal(3, result3.CurrentYPosition);
                Assert.Equal(Direction.South, result3.CurrentDirection);
                Assert.False(result3.IsLost);
            }

            [Fact]
            public void WithMultipleRobots_ScentSystem_PreventsCertainForwardMovements()
            {
                var scents = new HashSet<(int x, int y)>();
                var movementService = new MartianRobots.Domain.Services.MovementService(scents);

                var rover1 = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 2,
                    InstructionsProcessed = new List<Movement>()
                };

                var rover2 = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 1,
                    CurrentYPosition = 2,
                    InstructionsProcessed = new List<Movement>()
                };

                var result1 = movementService.ExecuteMovementInstructions(2, 2, rover1, new List<Movement> { Movement.Forward, Movement.Forward });
                var result2 = movementService.ExecuteMovementInstructions(2, 2, rover2, new List<Movement> { Movement.Forward, Movement.Forward });

                Assert.True(result1.IsLost);
                Assert.False(result2.IsLost);
                Assert.Contains((1, 2), scents);
            }
        }
    }
}
