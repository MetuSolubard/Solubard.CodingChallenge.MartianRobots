using Solubard.CodingChallenge.MartianRobots.Domain.Model;
using Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces;

namespace Solubard.CodingChallenge.MartianRobots.Application.CommandLine.Pilot
{
    public class ProgramLogic
    {
        private readonly ICommandLineService _commandLineService;
        private readonly IMovementService _movementService;

        public ProgramLogic(ICommandLineService commandLineService, IMovementService movementService)
        {
            _commandLineService = commandLineService;
            _movementService = movementService;
        }

        public async Task<string> ExecuteAsync()
        {
            Console.WriteLine("=== Sample Input ===");

            var (x, y) = _commandLineService.GetGridDimensions();
            var roversWithInstructions = _commandLineService.GetRoversWithInstructions();

            var rovers = new List<Rover>();
            foreach (var roverWithInstructions in roversWithInstructions)
            {
                rovers.Add(_movementService.ExecuteMovementInstructions(x, y, roverWithInstructions.Item1, roverWithInstructions.Item2) as Rover);
            }

            var result = new System.Text.StringBuilder();
            foreach (var rover in rovers)
            {
                result.Append($"{rover.CurrentXPosition} {rover.CurrentYPosition} {_commandLineService.DirectionToString(rover.CurrentDirection)}{(rover.IsLost ? " LOST" : null)}\n");
            }

            Console.WriteLine("\n=== Sample Output ===");
            Console.WriteLine(result);

            return result.ToString();
        }
    }
}
