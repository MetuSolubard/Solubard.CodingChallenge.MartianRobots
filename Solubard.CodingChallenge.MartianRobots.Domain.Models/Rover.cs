using Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces;

namespace Solubard.CodingChallenge.MartianRobots.Domain.Model
{
    public class Rover : IRobot, IRover
    {
        public char CurrentDirection { get; set; }
        public int Odometer { get; set; }
        public int CurrentPosition { get; set; }
    }
}
