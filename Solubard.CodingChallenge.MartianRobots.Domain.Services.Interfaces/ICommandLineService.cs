using Solubard.CodingChallenge.MartianRobots.Domain.Model;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces
{
    public interface ICommandLineService
    {
        public (int, int) GetGridDimensions();
        IList<(Rover, IList<Movement>)> GetRoversWithInstructions();
        public string DirectionToString(Direction direction);
    }
}
