using System;
using System.Collections.Generic;
using System.IO;

namespace StraightForwardWay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var simulator = new MartianRobotSimulator();

            // Example usage with sample data
            string sampleInput = @"5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL";

            Console.WriteLine("=== Sample Input ===");
            Console.WriteLine(sampleInput);
            Console.WriteLine("\n=== Sample Output ===");

            string result = simulator.ProcessInput(sampleInput);
            Console.WriteLine(result);

            Console.WriteLine("\n=== Interactive Mode ===");
            Console.WriteLine("Enter your input (press Enter twice when finished):");

            string userInput = ReadMultiLineInput();
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                string userResult = simulator.ProcessInput(userInput);
                Console.WriteLine("\nYour Output:");
                Console.WriteLine(userResult);
            }
        }

        private static string ReadMultiLineInput()
        {
            var lines = new List<string>();
            string line;
            int emptyLineCount = 0;

            while ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    emptyLineCount++;
                    if (emptyLineCount >= 2) break;
                }
                else
                {
                    emptyLineCount = 0;
                    lines.Add(line);
                }
            }

            return string.Join("\n", lines);
        }
    }
}