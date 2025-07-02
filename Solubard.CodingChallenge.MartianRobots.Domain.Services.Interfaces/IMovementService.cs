using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces
{
    public interface IMovementService
    {
        public IRobot ExecuteMovementInstructions(
            int gridMaxX,
            int gridMaxY,
            IRobot robot,
            IList<Movement> movementInstructions
            );
    }
}
