using AoCHelpers;
using Shouldly;

namespace Tests;

public class Day17Tests
{

    public static readonly string SampleInput =
    """
    2413432311323
    3215453535623
    3255245654254
    3446585845452
    4546657867536
    1438598798454
    4457876987766
    3637877979653
    4654967986887
    4564679986453
    1224686865563
    2546548887735
    4322674655533
    """.ReplaceLineEndings().Trim();

    public static readonly string SampleInput2 =
    """
    111111111111
    999999999991
    999999999991
    999999999991
    999999999991
    """.ReplaceLineEndings().Trim();

    public static readonly string PuzzleInput = File.ReadAllText("input.txt").ReplaceLineEndings().Trim();

    [Fact]
    public void test_sample_input_part1()
    {
        Day17.Part1(SampleInput).ShouldBe(102);
    }

    [Fact]
    public void test_sample_input_part2()
    {
        Day17.Part2(SampleInput).ShouldBe(94);
        Day17.Part2(SampleInput2).ShouldBe(71);        
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day17.Part1(PuzzleInput).ShouldBe(1128L);
    }

    [Fact]
    public void test_puzzle_input_part2()
    {
        Day17.Part2(PuzzleInput).ShouldBe(1268L);
    }

    [Fact]
    public void test_parse()
    {
        Grid grid = Grid.Parse(SampleInput);
        grid.Rows.ShouldBe(13);
        grid.Columns.ShouldBe(13);
        grid[0, 0].ShouldBe(2);
        grid[0, 12].ShouldBe(3);
        grid[12, 0].ShouldBe(4);
        grid[12, 12].ShouldBe(3);
    }
}