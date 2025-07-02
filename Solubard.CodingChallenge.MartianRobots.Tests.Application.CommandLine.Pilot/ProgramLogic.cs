using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Solubard.CodingChallenge.MartianRobots.Domain.Model;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces;
using Solubard.CodingChallenge.MartianRobots.Application.CommandLine.Pilot;

namespace Solubard.CodingChallenge.MartianRobots.Tests.Application.CommandLine.Pilot
{
    public class ProgramLogic
    {
        [Fact]
        public async Task ExecuteAsync_WithSampleData_ProcessesCorrectly()
        {
            var fakeCommandLineService = A.Fake<ICommandLineService>();
            var fakeMovementService = A.Fake<IMovementService>();
            var programLogic = new MartianRobots.Application.CommandLine.Pilot.ProgramLogic(fakeCommandLineService, fakeMovementService);

            var gridDimensions = (5, 3);
            var rover1 = new Rover
            {
                CurrentDirection = Direction.East,
                CurrentXPosition = 1,
                CurrentYPosition = 1,
                InstructionsProcessed = new List<Movement>(),
                IsLost = false
            };
            var rover2 = new Rover
            {
                CurrentDirection = Direction.North,
                CurrentXPosition = 3,
                CurrentYPosition = 2,
                InstructionsProcessed = new List<Movement>(),
                IsLost = false
            };

            var roversWithInstructions = new List<(Rover, IList<Movement>)>
            {
                (rover1, new List<Movement> { Movement.TurnRight, Movement.Forward }),
                (rover2, new List<Movement> { Movement.Forward, Movement.TurnLeft })
            };

            var processedRover1 = new Rover
            {
                CurrentDirection = Direction.East,
                CurrentXPosition = 1,
                CurrentYPosition = 1,
                InstructionsProcessed = new List<Movement> { Movement.TurnRight, Movement.Forward },
                IsLost = false
            };
            var processedRover2 = new Rover
            {
                CurrentDirection = Direction.North,
                CurrentXPosition = 3,
                CurrentYPosition = 3,
                InstructionsProcessed = new List<Movement> { Movement.Forward, Movement.TurnLeft },
                IsLost = true
            };

            A.CallTo(() => fakeCommandLineService.GetGridDimensions()).Returns(gridDimensions);
            A.CallTo(() => fakeCommandLineService.GetRoversWithInstructions()).Returns(roversWithInstructions);
            A.CallTo(() => fakeMovementService.ExecuteMovementInstructions(5, 3, rover1, roversWithInstructions[0].Item2))
                .Returns(processedRover1);
            A.CallTo(() => fakeMovementService.ExecuteMovementInstructions(5, 3, rover2, roversWithInstructions[1].Item2))
                .Returns(processedRover2);
            A.CallTo(() => fakeCommandLineService.DirectionToString(Direction.East)).Returns("E");
            A.CallTo(() => fakeCommandLineService.DirectionToString(Direction.North)).Returns("N");

            var originalConsoleOut = Console.Out;

            try
            {
                using var consoleOutput = new StringWriter();
                Console.SetOut(consoleOutput);

                var result = await programLogic.ExecuteAsync();

                Assert.Contains("1 1 E", result);
                Assert.Contains("3 3 N LOST", result);

                var output = consoleOutput.ToString();
                Assert.Contains("=== Sample Input ===", output);
                Assert.Contains("=== Sample Output ===", output);
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }

            A.CallTo(() => fakeCommandLineService.GetGridDimensions()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCommandLineService.GetRoversWithInstructions()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMovementService.ExecuteMovementInstructions(A<int>._, A<int>._, A<Rover>._, A<IList<Movement>>._))
                .MustHaveHappened(2, Times.Exactly);
        }

        [Fact]
        public async Task ExecuteAsync_WithNoRovers_HandlesGracefully()
        {
            var fakeCommandLineService = A.Fake<ICommandLineService>();
            var fakeMovementService = A.Fake<IMovementService>();
            var programLogic = new MartianRobots.Application.CommandLine.Pilot.ProgramLogic(fakeCommandLineService, fakeMovementService);

            var gridDimensions = (5, 5);
            var roversWithInstructions = new List<(Rover, IList<Movement>)>();

            A.CallTo(() => fakeCommandLineService.GetGridDimensions()).Returns(gridDimensions);
            A.CallTo(() => fakeCommandLineService.GetRoversWithInstructions()).Returns(roversWithInstructions);

            var originalConsoleOut = Console.Out;
            try
            {
                using var consoleOutput = new StringWriter();
                Console.SetOut(consoleOutput);

                var result = await programLogic.ExecuteAsync();

                Assert.Equal("", result);

                var output = consoleOutput.ToString();
                Assert.Contains("=== Sample Input ===", output);
                Assert.Contains("=== Sample Output ===", output);
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }

            A.CallTo(() => fakeMovementService.ExecuteMovementInstructions(A<int>._, A<int>._, A<Rover>._, A<IList<Movement>>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task ExecuteAsync_WithLostRover_DisplaysLostStatus()
        {
            var fakeCommandLineService = A.Fake<ICommandLineService>();
            var fakeMovementService = A.Fake<IMovementService>();
            var programLogic = new MartianRobots.Application.CommandLine.Pilot.ProgramLogic(fakeCommandLineService, fakeMovementService);

            var gridDimensions = (2, 2);
            var rover = new Rover
            {
                CurrentDirection = Direction.North,
                CurrentXPosition = 1,
                CurrentYPosition = 2,
                InstructionsProcessed = new List<Movement>(),
                IsLost = false
            };

            var roversWithInstructions = new List<(Rover, IList<Movement>)>
            {
                (rover, new List<Movement> { Movement.Forward })
            };

            var processedRover = new Rover
            {
                CurrentDirection = Direction.North,
                CurrentXPosition = 1,
                CurrentYPosition = 2,
                InstructionsProcessed = new List<Movement> { Movement.Forward },
                IsLost = true
            };

            A.CallTo(() => fakeCommandLineService.GetGridDimensions()).Returns(gridDimensions);
            A.CallTo(() => fakeCommandLineService.GetRoversWithInstructions()).Returns(roversWithInstructions);
            A.CallTo(() => fakeMovementService.ExecuteMovementInstructions(2, 2, rover, roversWithInstructions[0].Item2))
                .Returns(processedRover);
            A.CallTo(() => fakeCommandLineService.DirectionToString(Direction.North)).Returns("N");

            var originalConsoleOut = Console.Out;
            try
            {
                using var consoleOutput = new StringWriter();
                Console.SetOut(consoleOutput);

                var result = await programLogic.ExecuteAsync();

                Assert.Contains("1 2 N LOST", result);
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }
        }

        [Fact]
        public async Task ExecuteAsync_CallsServicesInCorrectOrder()
        {
            var fakeCommandLineService = A.Fake<ICommandLineService>();
            var fakeMovementService = A.Fake<IMovementService>();
            var programLogic = new MartianRobots.Application.CommandLine.Pilot.ProgramLogic(fakeCommandLineService, fakeMovementService);

            var gridDimensions = (5, 5);
            var roversWithInstructions = new List<(Rover, IList<Movement>)>();

            A.CallTo(() => fakeCommandLineService.GetGridDimensions()).Returns(gridDimensions);
            A.CallTo(() => fakeCommandLineService.GetRoversWithInstructions()).Returns(roversWithInstructions);

            var originalConsoleOut = Console.Out;
            try
            {
                using var consoleOutput = new StringWriter();
                Console.SetOut(consoleOutput);

                await programLogic.ExecuteAsync();
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }

            A.CallTo(() => fakeCommandLineService.GetGridDimensions())
                .MustHaveHappened()
                .Then(A.CallTo(() => fakeCommandLineService.GetRoversWithInstructions())
                .MustHaveHappened());
        }
    }
}