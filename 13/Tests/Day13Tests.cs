using Shouldly;

namespace Tests;

public class Day13Tests
{

    public static readonly string[] SampleInput =
    """
    #.##..##.
    ..#.##.#.
    ##......#
    ##......#
    ..#.##.#.
    ..##..##.
    #.#.##.#.

    #...##..#
    #....#..#
    ..##..###
    #####.##.
    #####.##.
    ..##..###
    #....#..#
    """.Trim().Split(Environment.NewLine, StringSplitOptions.TrimEntries);

    public static readonly string[] PuzzleInput = File.ReadAllLines("input.txt");

    [Fact]
    public void test_sample_input_part1()
    {
        Day13.Part1(SampleInput).ShouldBe(405);
    }

    [Fact]
    public void test_sample_input_part2()
    {
        Day13.Part2(SampleInput).ShouldBe(400);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day13.Part1(PuzzleInput).ShouldBe(33780L);
    }

    [Fact]
    public void test_puzzle_input_part2()
    {
        Day13.Part2(PuzzleInput).ShouldBe(23479L);
    }

    [Fact]
    public void test_find_mirror_row()
    {
        string rawGrid =
        """
        #...##..#
        #....#..#
        ..##..###
        #####.##.
        #####.##.
        ..##..###
        #....#..#
        """.Trim();
        Grid grid = new Grid(rawGrid);

        grid.FindMirrorRow().ShouldBe(4);
    }

    [Fact]
    public void test_find_mirror_column()
    {
        string rawGrid =
        """
        #.##..##.
        ..#.##.#.
        ##......#
        ##......#
        ..#.##.#.
        ..##..##.
        #.#.##.#.
        """.Trim();
        Grid grid = new Grid(rawGrid);

        grid.FindMirrorColumn().ShouldBe(5);
    }

    [Fact]
    public void test_mirror_rows()
    {
        string rawGrid =
        """
        #...##..#
        #....#..#
        ..##..###
        #####.##.
        #####.##.
        ..##..###
        #....#..#
        """.Trim();
        Grid grid = new Grid(rawGrid);

        (Grid top, Grid bottom) = grid.MirrorRows(3);

        top.RawGrid.ShouldBe(bottom.RawGrid);
    }

    [Fact]
    public void test_mirror_columns()
    {
        string rawGrid =
        """
        #.##..##.
        ..#.##.#.
        ##......#
        ##......#
        ..#.##.#.
        ..##..##.
        #.#.##.#.
        """.Trim();
        Grid grid = new Grid(rawGrid);

        (Grid left, Grid right) = grid.MirrorColumns(4);

        left.RawGrid.ShouldBe(right.RawGrid);

    }

    [Fact]
    public void test_transpose_grid()
    {
        string rawGrid =
        """
        ABCD
        EFGH
        IJKL
        """.Trim();
        Grid grid = new Grid(rawGrid);
        Grid transposed = grid.Transpose();
        string transposedGrid =
        """
        AEI
        BFJ
        CGK
        DHL
        """.Trim();
        transposed.RawGrid.ShouldBe(transposedGrid);
    }

    // [Fact]
    // public void test_grid_equality()
    // {
    //     Grid g0 = new Grid("ABC\nDEF");
    //     Grid g1 = new Grid("ABC\nDEF");
    //     g0.ShouldBe(g1);
    // }
}
