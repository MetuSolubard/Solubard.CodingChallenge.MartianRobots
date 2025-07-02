using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces
{
    public interface IRobot
    {
        public Direction CurrentDirection { get; set; }
        public int CurrentXPosition { get; set; }
        public int CurrentYPosition { get; set; }
        public bool IsLost { get; set; }
        public List<Movement> InstructionsProcessed { get; set; }
    }
}
