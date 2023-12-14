using Shouldly;

namespace Tests;

public class Day14Tests
{
    public static readonly string[] SampleInput =
    """
    O....#....
    O.OO#....#
    .....##...
    OO.#O....O
    .O.....O#.
    O.#..O.#.#
    ..O..#O..O
    .......O..
    #....###..
    #OO..#....
    """.SplitAoCInput();

    public static readonly string[] PuzzleInput = File.ReadAllText("input.txt").SplitAoCInput();

    [Fact]
    public void test_sample_input_part1()
    {
        Day14.Part1(SampleInput).ShouldBe(136);
    }

    [Fact]
    public void test_sample_input_part2()
    {
        Day14.Part2(SampleInput).ShouldBe(64);
    }

        [Fact]
    public void test_puzzle_input_part2()
    {
        Day14.Part2(PuzzleInput).ShouldBe(112452L);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day14.Part1(PuzzleInput).ShouldBe(109345L);
    }

    [Fact]
    public void test_shift_north()
    {

        string[] shifted = Day14.ShiftNorth(SampleInput);
        
        string newGrid = string.Join(Environment.NewLine, shifted);

        string expected = 
        """
        OOOO.#.O..
        OO..#....#
        OO..O##..O
        O..#.OO...
        ........#.
        ..#....#.#
        ..O..#.O.O
        ..O.......
        #....###..
        #....#....
        """.Trim().ReplaceLineEndings();

        newGrid.ShouldBe(expected);

    }

    [Fact]
    public void test_shift_south()
    {

        string[] shifted = Day14.ShiftSouth(SampleInput);
        
        string newGrid = string.Join(Environment.NewLine, shifted);

        string expected = 
        """
        .....#....
        ....#....#
        ...O.##...
        ...#......
        O.O....O#O
        O.#..O.#.#
        O....#....
        OO....OO..
        #OO..###..
        #OO.O#...O
        """.Trim().ReplaceLineEndings();

        newGrid.ShouldBe(expected);
    }

    [Fact]
    public void test_shift_east()
    {

        string[] shifted = Day14.ShiftEast(SampleInput);
        
        string newGrid = string.Join(Environment.NewLine, shifted);

        string expected = 
        """
        ....O#....
        .OOO#....#
        .....##...
        .OO#....OO
        ......OO#.
        .O#...O#.#
        ....O#..OO
        .........O
        #....###..
        #..OO#....
        """.Trim().ReplaceLineEndings();

        newGrid.ShouldBe(expected);
    }

    [Fact]
    public void test_shift_west()
    {

        string[] shifted = Day14.ShiftWest(SampleInput);
        
        string newGrid = string.Join(Environment.NewLine, shifted);

        string expected = 
        """
        O....#....
        OOO.#....#
        .....##...
        OO.#OO....
        OO......#.
        O.#O...#.#
        O....#OO..
        O.........
        #....###..
        #OO..#....
        """.Trim().ReplaceLineEndings();

        newGrid.ShouldBe(expected);
    }

    [Fact]
    public void test_cycle()
    {
        string[] oneCycle = Day14.Cycle(SampleInput, 1);
        string[] twoCycles = Day14.Cycle(SampleInput, 2);
        string[] threeCycles = Day14.Cycle(SampleInput, 3);

        string expectedOne = """
        .....#....
        ....#...O#
        ...OO##...
        .OO#......
        .....OOO#.
        .O#...O#.#
        ....O#....
        ......OOOO
        #...O###..
        #..OO#....
        """.Trim();

        string.Join(Environment.NewLine, oneCycle).ShouldBe(expectedOne);


        string expectedTwo = """
        .....#....
        ....#...O#
        .....##...
        ..O#......
        .....OOO#.
        .O#...O#.#
        ....O#...O
        .......OOO
        #..OO###..
        #.OOO#...O
        """.Trim();

        string.Join(Environment.NewLine, twoCycles).ShouldBe(expectedTwo);

        string expectedThree = """
        .....#....
        ....#...O#
        .....##...
        ..O#......
        .....OOO#.
        .O#...O#.#
        ....O#...O
        .......OOO
        #...O###.O
        #.OOO#...O
        """.Trim(); 

        string.Join(Environment.NewLine, threeCycles).ShouldBe(expectedThree);
    }
}