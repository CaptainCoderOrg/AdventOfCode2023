using Shouldly;

namespace Tests;

public class Day9Tests
{
    public static string SampleInput =
    """
    0 3 6 9 12 15
    1 3 6 10 15 21
    10 13 16 21 30 45
    """.Trim();

    public static string Input = File.ReadAllText("input.txt");

    [Fact]
    public void test_sample_input_part1()
    {
        Day9.Part1(SampleInput).ShouldBe(114);
    }

    [Fact]
    public void test_input_part1()
    {
        Day9.Part1(Input).ShouldBe(2008960228L);
    }

    [Theory]
    [InlineData(new long[]{0, 0, 0}, 0)]
    [InlineData(new long[]{3, 3, 3}, 3)]
    [InlineData(new long[]{0, 3, 6, 9, 12, 15}, 18)]
    [InlineData(new long[]{1, 3, 6, 10, 15, 21}, 28)]
    [InlineData(new long[]{10, 13, 16, 21, 30, 45}, 68)]
    public void test_extrapolate(long[] inputs, long lastElement)
    {
        // Arrange
        List<long> toTest = [.. inputs];

        // Act
        List<long> result = Day9.Extrapolate(toTest);
        
        // Assert
        result[^1].ShouldBe(lastElement);
    }
}