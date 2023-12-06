using Implementation;
using Shouldly;

namespace Tests;

public class Day6Tests
{
    private static readonly string SampleInput =
    """
    Time:      7  15   30
    Distance:  9  40  200
    """;

    private static readonly string PuzzleInput =
    """
    Time:        40     92     97     90
    Distance:   215   1064   1505   1100
    """;

    [Fact]
    public void test_sample_input_part1()
    {
        // Triple A Testing: Arrange, Act, Assert

        // Arrange
        string result = Day6.SolvePart1(SampleInput);
        result.ShouldBe("288");
    }

    [Fact]
    public void test_sample_input_part2()
    {
        string result = Day6.SolvePart2(SampleInput);
        result.ShouldBe("71503");
    }

        [Fact]
    public void test_input_part2()
    {
        string result = Day6.SolvePart2(PuzzleInput);
        result.ShouldBe("28545089");
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        // Triple A Testing: Arrange, Act, Assert

        // Arrange
        string result = Day6.SolvePart1(PuzzleInput);
        result.ShouldBe("6209190");
    }

    [Theory]
    [InlineData("Time:      7  15   30", new int[] { 7, 15, 30})]
    [InlineData("Distance:  9  40  200", new int[] { 9, 40, 200})]
    public void test_parse_line(string line, int[] result)
    {
        Day6.ParseLine(line).ShouldBe(result);
    }
}