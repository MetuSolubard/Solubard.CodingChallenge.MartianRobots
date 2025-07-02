using Microsoft.Extensions.DependencyInjection;
using Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces;
using Solubard.CodingChallenges.MartianRobots.Applications.Ioc;

namespace Solubard.CodingChallenge.MartianRobots.Application.CommandLine.Pilot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = PilotConfig.ConfigureContainer(args);
            var commandLineService = host.Services.GetRequiredService<ICommandLineService>();
            var movementService = host.Services.GetRequiredService<IMovementService>();

            var programLogic = new ProgramLogic(commandLineService, movementService);
            await programLogic.ExecuteAsync();
        }
    }
}