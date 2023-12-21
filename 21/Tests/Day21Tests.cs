using AoCHelpers;
using Shouldly;

namespace Tests;

public class Day21Tests
{
    public const string SampleInput =
    """
    ...........
    .....###.#.
    .###.##..#.
    ..#.#...#..
    ....#.#....
    .##..S####.
    .##..#...#.
    .......##..
    .##.#.####.
    .##..##.##.
    ...........
    """;

    public readonly string PuzzleInput = File.ReadAllText("input.txt");

    [Fact]
    public void test_sample_input_part1()
    {
        Day21.Part1(SampleInput, 6).ShouldBe(16);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day21.Part1(PuzzleInput, 64).ShouldBe(3632L);
    }

    [Fact]
    public void test_parse()
    {
        Map map = Map.Parse(SampleInput);
        
        map.Rows.ShouldBe(11);
        map.Columns.ShouldBe(11);
        map.Start.ShouldBe(new AoCHelpers.Position(5, 5));
        Position[] neighbors = [..map.Neighbors(map.Start)];
        neighbors.Length.ShouldBe(2);
        neighbors.ShouldBeSubsetOf([new Position(4, 5), new Position(5, 4)]);
    }
}