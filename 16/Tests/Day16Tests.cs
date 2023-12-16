using Shouldly;

namespace Tests;

public class Day16Tests
{

    public static readonly string SampleInput =
    """
    .|...\....
    |.-.\.....
    .....|-...
    ........|.
    ..........
    .........\
    ..../.\\..
    .-.-/..|..
    .|....-|.\
    ..//.|....
    """.ReplaceLineEndings().Trim();

    public static readonly string PuzzleInput = File.ReadAllText("input.txt").ReplaceLineEndings().Trim();

    [Fact]
    public void test_sample_input_part1()
    {
        Day16.Part1(SampleInput).ShouldBe(46);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day16.Part1(PuzzleInput).ShouldBe(7632L);
    }
}