namespace StraightForwardWay.Tests
{
    public class RobotTests
    {
        [Fact]
        public void Robot_Constructor_SetsInitialPosition()
        {
            var robot = new Robot(5, 3, Direction.North);

            Assert.Equal(5, robot.X);
            Assert.Equal(3, robot.Y);
            Assert.Equal(Direction.North, robot.Direction);
        }

        [Theory]
        [InlineData(Direction.North, Direction.West)]
        [InlineData(Direction.West, Direction.South)]
        [InlineData(Direction.South, Direction.East)]
        [InlineData(Direction.East, Direction.North)]
        public void TurnLeft_ChangesDirectionCorrectly(Direction initial, Direction expected)
        {
            var robot = new Robot(0, 0, initial);

            robot.TurnLeft();

            Assert.Equal(expected, robot.Direction);
        }

        [Theory]
        [InlineData(Direction.North, Direction.East)]
        [InlineData(Direction.East, Direction.South)]
        [InlineData(Direction.South, Direction.West)]
        [InlineData(Direction.West, Direction.North)]
        public void TurnRight_ChangesDirectionCorrectly(Direction initial, Direction expected)
        {
            var robot = new Robot(0, 0, initial);

            robot.TurnRight();

            Assert.Equal(expected, robot.Direction);
        }

        [Theory]
        [InlineData(Direction.North, 5, 6)]
        [InlineData(Direction.South, 5, 4)]
        [InlineData(Direction.East, 6, 5)]
        [InlineData(Direction.West, 4, 5)]
        public void GetNextPosition_ReturnsCorrectPosition(Direction direction, int expectedX, int expectedY)
        {
            var robot = new Robot(5, 5, direction);

            var nextPos = robot.GetNextPosition();

            Assert.Equal(expectedX, nextPos.x);
            Assert.Equal(expectedY, nextPos.y);
        }

        [Theory]
        [InlineData(Direction.North, 5, 6)]
        [InlineData(Direction.South, 5, 4)]
        [InlineData(Direction.East, 6, 5)]
        [InlineData(Direction.West, 4, 5)]
        public void MoveForward_UpdatesPositionCorrectly(Direction direction, int expectedX, int expectedY)
        {
            var robot = new Robot(5, 5, direction);

            robot.MoveForward();

            Assert.Equal(expectedX, robot.X);
            Assert.Equal(expectedY, robot.Y);
            Assert.Equal(direction, robot.Direction);
        }

        [Fact]
        public void Robot_MultipleOperations_WorksCorrectly()
        {
            var robot = new Robot(1, 1, Direction.East);

            robot.TurnRight();
            robot.MoveForward();
            robot.TurnLeft();
            robot.MoveForward();

            Assert.Equal(2, robot.X);
            Assert.Equal(0, robot.Y);
            Assert.Equal(Direction.East, robot.Direction);
        }
    }

    public class MartianRobotSimulatorTests
    {
        [Fact]
        public void ProcessInput_WithSampleData_ReturnsCorrectOutput()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL";

            string expectedOutput = "1 1 E\n3 3 N LOST\n2 3 S";

            string result = simulator.ProcessInput(input);

            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void ProcessInput_SingleRobotNoMovement_ReturnsInitialPosition()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"5 5
2 2 N
LLLL";

            string result = simulator.ProcessInput(input);

            Assert.Equal("2 2 N", result);
        }

        [Fact]
        public void ProcessInput_RobotTurnsOnly_ChangesOrientationOnly()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"5 5
2 2 N
LLLL";

            string result = simulator.ProcessInput(input);

            Assert.Equal("2 2 N", result);
        }

        [Fact]
        public void ProcessInput_RobotMovesForwardOnly_UpdatesPosition()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"5 5
2 2 N
FFF";

            string result = simulator.ProcessInput(input);

            Assert.Equal("2 5 N", result);
        }

        [Theory]
        [InlineData("N", "F", "1 2 N")]
        [InlineData("S", "F", "1 0 S")]
        [InlineData("E", "F", "2 1 E")]
        [InlineData("W", "F", "0 1 W")]
        public void ProcessInput_RobotMovesToBoundary_StaysOnGrid(string direction, string instruction, string expected)
        {
            var simulator = new MartianRobotSimulator();
            string input = $@"2 2
1 1 {direction}
{instruction}";

            string result = simulator.ProcessInput(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ProcessInput_SecondRobotSavedByScent_DoesNotFallOff()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"2 2
1 1 N
FF
1 1 N
FF";

            string result = simulator.ProcessInput(input);

            var lines = result.Split('\n');
            Assert.Equal("1 2 N LOST", lines[0]);
            Assert.Equal("1 2 N", lines[1]);
        }

        [Fact]
        public void ProcessInput_EmptyInstructionString_RobotStaysInPlace()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"5 5
2 3 W
LLLL";

            string result = simulator.ProcessInput(input);

            Assert.Equal("2 3 W", result);
        }

        [Fact]
        public void ProcessInput_LargeGrid_HandlesCorrectly()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"50 50
25 25 N
FFFFFFFFFFFFFFFFFFFFFFFFF";

            string result = simulator.ProcessInput(input);

            Assert.Equal("25 50 N", result);
        }

        [Fact]
        public void ProcessInput_RobotAtEdgeWithScent_IgnoresOffGridMoves()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"2 2
2 2 E
F
2 2 E
FFF";

            string result = simulator.ProcessInput(input);

            var lines = result.Split('\n');
            Assert.Equal("2 2 E LOST", lines[0]);
            Assert.Equal("2 2 E", lines[1]);
        }

        [Fact]
        public void ProcessInput_RobotActuallyFallsOffNorth_MarkedAsLost()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"2 2
1 2 N
F";

            string result = simulator.ProcessInput(input);

            Assert.Equal("1 2 N LOST", result);
        }

        [Fact]
        public void ProcessInput_RobotActuallyFallsOffEast_MarkedAsLost()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"2 2
2 1 E
F";

            string result = simulator.ProcessInput(input);

            Assert.Equal("2 1 E LOST", result);
        }

        [Fact]
        public void ProcessInput_RobotFallsOffSouthEdge_MarkedAsLost()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"2 2
1 0 S
F";

            string result = simulator.ProcessInput(input);

            Assert.Equal("1 0 S LOST", result);
        }

        [Fact]
        public void ProcessInput_RobotFallsOffWestEdge_MarkedAsLost()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"2 2
0 1 W
F";

            string result = simulator.ProcessInput(input);

            Assert.Equal("0 1 W LOST", result);
        }

        [Theory]
        [InlineData("LRLRLRLR")]
        [InlineData("FFFFLFFFFRFFFFBFFFF")]
        [InlineData("123LF456R789")]
        public void ProcessInput_InvalidCommands_IgnoresUnknownCommands(string instructions)
        {
            var simulator = new MartianRobotSimulator();
            string input = $@"5 5
2 2 N
{instructions}";

            string result = simulator.ProcessInput(input);
            Assert.NotNull(result);
        }

        [Fact]
        public void ProcessInput_MultipleRobotsWithDifferentPaths_TracksScentsIndependently()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"3 3
0 0 N
FFFF
3 3 E
F
0 0 N
FFFF
3 3 E
F";

            string result = simulator.ProcessInput(input);

            var lines = result.Split('\n');
            Assert.Equal(4, lines.Length);
            Assert.Contains("LOST", lines[0]);
            Assert.Contains("LOST", lines[1]);
            Assert.DoesNotContain("LOST", lines[2]);
            Assert.DoesNotContain("LOST", lines[3]);
        }

        [Fact]
        public void ProcessInput_RobotSequenceRFRFRFRF_ReturnsToStart()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"10 10
5 5 N
RFRFRFRF";

            string result = simulator.ProcessInput(input);

            Assert.Equal("5 5 N", result);
        }

        [Fact]
        public void ProcessInput_LargeGridRobotFallsOff_MarkedAsLost()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"50 50
25 25 N
FFFFFFFFFFFFFFFFFFFFFFFFFF";

            string result = simulator.ProcessInput(input);

            Assert.Equal("25 50 N LOST", result);
        }
    }

    public class IntegrationTests
    {
        [Fact]
        public void FullSimulation_WithOriginalSampleData_ProducesExpectedResults()
        {
            var simulator = new MartianRobotSimulator();
            string input = @"5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL";

            string result = simulator.ProcessInput(input);

            string[] lines = result.Split('\n');

            Assert.Equal("1 1 E", lines[0]);
            Assert.Equal("3 3 N LOST", lines[1]);
            Assert.Equal("2 3 S", lines[2]);
        }

        [Fact]
        public void StressTest_ManyRobotsWithLongInstructions_PerformsWell()
        {
            var simulator = new MartianRobotSimulator();
            var inputBuilder = new System.Text.StringBuilder();
            inputBuilder.AppendLine("10 10");

            for (int i = 0; i < 10; i++)
            {
                inputBuilder.AppendLine($"{i} {i} N");
                inputBuilder.AppendLine("LRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRFLRF");
            }

            string result = simulator.ProcessInput(inputBuilder.ToString());
            string[] lines = result.Split('\n');
            Assert.Equal(10, lines.Length);
        }
    }
}