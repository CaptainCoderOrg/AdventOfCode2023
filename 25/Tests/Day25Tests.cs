using Shouldly;

namespace Tests;

public class Day25Tests
{
    [Fact]
    public void test_sample_input()
    {
        Day25.Part1(Day25.SampleInput).ShouldBe(0);
    }
}