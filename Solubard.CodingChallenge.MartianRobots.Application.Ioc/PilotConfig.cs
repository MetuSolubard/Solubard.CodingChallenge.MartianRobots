using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solubard.CodingChallenge.MartianRobots.Domain.Services;
using Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces;

namespace Solubard.CodingChallenges.MartianRobots.Applications.Ioc
{
    public static class PilotConfig
    {
        public static IHost ConfigureContainer(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton(new HashSet<(int x, int y)>());
            builder.Services.AddScoped<ICommandLineService, CommandLineService>();
            builder.Services.AddScoped<IMovementService, MovementService>();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            return builder.Build();

        }
    }
}
