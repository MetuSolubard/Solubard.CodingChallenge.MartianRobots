using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Services;

namespace Solubard.CodingChallenge.MartianRobots.Tests.Domain.Services
{
    public class CommandLineService
    {
        public class DirectionToString
        {
            [Fact]
            public void AllDirections_ReturnsCorrectStrings()
            {
                var service = new MartianRobots.Domain.Services.CommandLineService();

                Assert.Equal("N", service.DirectionToString(Direction.North));
                Assert.Equal("S", service.DirectionToString(Direction.South));
                Assert.Equal("E", service.DirectionToString(Direction.East));
                Assert.Equal("W", service.DirectionToString(Direction.West));
            }

            [Fact]
            public void InvalidDirection_ThrowsArgumentException()
            {
                var service = new MartianRobots.Domain.Services.CommandLineService();

                Assert.Throws<ArgumentException>(() => service.DirectionToString(Direction.None));
            }
        }

        public class MovementToString
        {
            [Fact]
            public void AllMovements_ReturnsCorrectStrings()
            {
                var service = new MartianRobots.Domain.Services.CommandLineService();

                Assert.Equal("L", service.MovementToString(Movement.TurnLeft));
                Assert.Equal("R", service.MovementToString(Movement.TurnRight));
                Assert.Equal("F", service.MovementToString(Movement.Forward));
            }

            [Fact]
            public void InvalidMovement_ThrowsArgumentException()
            {
                var service = new MartianRobots.Domain.Services.CommandLineService();

                Assert.Throws<ArgumentException>(() => service.MovementToString(Movement.None));
            }
        }
    }
}
