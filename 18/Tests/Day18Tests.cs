using AoCHelpers;
using Shouldly;

namespace Tests;

public class Day18Tests
{
    public static readonly string SampleInput =
    """
    R 6 (#70c710)
    D 5 (#0dc571)
    L 2 (#5713f0)
    D 2 (#d2c081)
    R 2 (#59c680)
    D 2 (#411b91)
    L 5 (#8ceee2)
    U 2 (#caa173)
    L 1 (#1b58a2)
    U 2 (#caa171)
    R 2 (#7807d2)
    U 3 (#a77fa3)
    L 2 (#015232)
    U 2 (#7a21e3)
    """.ReplaceLineEndings().Trim();

    public static readonly string PuzzleInput = File.ReadAllText("input.txt").ReplaceLineEndings().Trim();

    [Fact]
    public void test_sample_input_part1()
    {
        Day18.Part1(SampleInput).ShouldBe(62);
    }

    [Fact]
    public void test_sample_input_part2()
    {
        Day18.Part2(SampleInput).ShouldBe(952408144115L);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day18.Part1(PuzzleInput).ShouldBe(108909L);
    }

    [Fact]
    public void test_puzzle_input_part2()
    {
        Day18.Part2(PuzzleInput).ShouldBe(133125706867777L);
    }


    
    [Fact]
    public void test_parse_outline()
    {
        Outline outline = Outline.Parse(SampleInput);
        // 0123456
        // ####### 0
        // #.....# 1
        // ###...# 2
        // ..#...# 3
        // ..#...# 4
        // ###.### 5
        // #...#.. 6
        // ##..### 7
        // .#....# 8
        // .###### 9
        outline.Holes.ShouldContain(new Position(0, 0));
        outline.Holes.ShouldContain(new Position(0, 6));
        outline.Holes.ShouldContain(new Position(5, 6));
        outline.Holes.ShouldContain(new Position(5, 4));
    }

    [Theory]
    [InlineData("U 2 (#7a21e3)", Direction.Up, 2, "7a21e3")]
    [InlineData("R 13 (#7807d2)", Direction.Right, 13, "7807d2")]
    [InlineData("L 1 (#1b58a2)", Direction.Left, 1, "1b58a2")]
    [InlineData("D 5 (#0dc571)", Direction.Down, 5, "0dc571")]
    public void test_parse_instruction(string input, Direction expectedDir, int expectedStep, string expectedHash)
    {
        Instruction.Parse(input).ShouldBe(new Instruction(expectedDir, expectedStep, expectedHash));
    }

    [Theory]
    [InlineData("U 2 (#70c710)", Direction.Right, 461937, "70c710")]
    [InlineData("R 13 (#0dc571)", Direction.Down, 56407, "0dc571")]
    [InlineData("L 1 (#8ceee2)", Direction.Left, 577262, "8ceee2")]
    [InlineData("D 5 (#a77fa3)", Direction.Up, 686074, "a77fa3")]
    public void test_parse_true_instruction(string input, Direction expectedDir, int expectedStep, string expectedHash)
    {
        Instruction.ParseTrueInstruction(input).ShouldBe(new Instruction(expectedDir, expectedStep, expectedHash));
    }
}