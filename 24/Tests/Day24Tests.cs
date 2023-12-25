using Shouldly;

namespace Tests;

public class Day24Tests
{
    [Fact]
    public void test_sample_input_part()
    {
        Day24.Part1(Day24.SampleInput, 7, 27).ShouldBe(2);
    }

    [Fact]
    public void test_puzzle_input_part()
    {
        Day24.Part1(Day24.PuzzleInput, 200_000_000_000_000, 400_000_000_000_000).ShouldBe(2);
    }

    // [Fact]
    // public void test_crossed_in_past()
    // {
    //     // Hailstone A: 12, 31, 28 @ -1, -2, -1
    //     // Hailstone B: 20, 19, 15 @ 1, -5, -3
    //     // Hailstones' paths crossed in the past for both hailstones.
    //     Hailstone a = new Hailstone(new Vector3D(12, 31, 28), new Vector3D(-1, -2, -1));
    //     Hailstone b = new Hailstone(new Vector3D(20, 19, 15), new Vector3D(1, -5, -3));
    //     (var x, var y) = a.IntersectsAt(b);
    //     a.IntersectsInFuture(x, y).ShouldBeFalse();
    // }
}