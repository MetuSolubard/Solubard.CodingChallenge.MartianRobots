namespace Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces
{
    public interface IRobot
    {
        public char CurrentDirection { get; set; }
        public int CurrentPosition { get; set; }
    }
}
