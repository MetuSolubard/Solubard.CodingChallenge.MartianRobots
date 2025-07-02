using Solubard.CodingChallenge.MartianRobots.Domain.Model;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;

namespace Solubard.CodingChallenge.MartianRobots.Tests.Domain.Services
{
    public class Rover
    {
        public class Constructor
        {
            [Fact]
            public void DefaultConstruction_HasExpectedInitialState()
            {
                var rover = new MartianRobots.Domain.Model.Rover();

                Assert.Equal(Direction.None, rover.CurrentDirection);
                Assert.Equal(0, rover.CurrentXPosition);
                Assert.Equal(0, rover.CurrentYPosition);
                Assert.False(rover.IsLost);
                Assert.Null(rover.InstructionsProcessed);
                Assert.Equal(0, rover.Odometer);
            }

            [Fact]
            public void PropertyAssignment_SetsValuesCorrectly()
            {
                var rover = new MartianRobots.Domain.Model.Rover
                {
                    CurrentDirection = Direction.North,
                    CurrentXPosition = 5,
                    CurrentYPosition = 3,
                    IsLost = true,
                    InstructionsProcessed = new List<Movement> { Movement.Forward },
                    Odometer = 100
                };

                Assert.Equal(Direction.North, rover.CurrentDirection);
                Assert.Equal(5, rover.CurrentXPosition);
                Assert.Equal(3, rover.CurrentYPosition);
                Assert.True(rover.IsLost);
                Assert.Single(rover.InstructionsProcessed);
                Assert.Equal(Movement.Forward, rover.InstructionsProcessed[0]);
                Assert.Equal(100, rover.Odometer);
            }
        }
    }
}