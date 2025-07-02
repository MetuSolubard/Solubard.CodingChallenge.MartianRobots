using Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums;
using Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Model
{
    public class Rover : IRobot, IRover
    {
        public Direction CurrentDirection { get; set; }
        public int Odometer { get; set; }
        public int CurrentXPosition { get; set; }
        public int CurrentYPosition { get; set; }
        public bool IsLost { get; set; }
        public List<Movement> InstructionsProcessed { get; set; }
    }
}
