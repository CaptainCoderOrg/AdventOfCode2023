namespace Tests;
using Shouldly;

public class Day11Tests
{

    public static readonly string SampleInput =
    """
    ...#......
    .......#..
    #.........
    ..........
    ......#...
    .#........
    .........#
    ..........
    .......#..
    #...#.....
    """.ReplaceLineEndings().Trim();

    public static readonly string PuzzleInput = File.ReadAllText("input.txt").Trim();

    [Fact]
    public void test_sample_input_part1()
    {
        Day11.Part1(SampleInput).ShouldBe(374L);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day11.Part1(PuzzleInput).ShouldBe(9947476L);
    }

        [Fact]
    public void test_puzzle_input_part2()
    {
        Day11.Part2(PuzzleInput).ShouldBe(519939907614L);
    }

    [Fact]
    public void test_find_empty_0()
    {
        string TestInput =
        """
        ....
        ....
        ....
        ....
        """.ReplaceLineEndings().Trim();
        (HashSet<int> rows, HashSet<int> cols) = Day11.FindEmpty(TestInput.Split(Environment.NewLine));

        rows.Count.ShouldBe(4);
        cols.Count.ShouldBe(4);
    }

    [Fact]
    public void test_find_empty_1()
    {
        string TestInput =
        """
        #...
        ....
        ....
        #...
        """.ReplaceLineEndings().Trim();
        (HashSet<int> rows, HashSet<int> cols) = Day11.FindEmpty(TestInput.Split(Environment.NewLine));

        rows.Count.ShouldBe(2);
        rows.ShouldContain(1);
        rows.ShouldContain(2);

        cols.Count.ShouldBe(3);
        cols.ShouldContain(1);
        cols.ShouldContain(2);
        cols.ShouldContain(3);
    }

    [Fact]
    public void test_find_empty_sample_input()
    {
        string TestInput =
        """
        ...#......
        .......#..
        #.........
        ..........
        ......#...
        .#........
        .........#
        ..........
        .......#..
        #...#.....
        """.ReplaceLineEndings().Trim();
        (HashSet<int> rows, HashSet<int> cols) = Day11.FindEmpty(TestInput.Split(Environment.NewLine));

        rows.Count.ShouldBe(2);
        rows.ShouldContain(3);
        rows.ShouldContain(7);

        cols.Count.ShouldBe(3);
        cols.ShouldContain(2);
        cols.ShouldContain(5);
        cols.ShouldContain(8);
    }

    [Fact]
    public void test_find_positions()
    {
        //   0123456789012
        // 0 ....1........
        // 1 .........2...
        // 2 3............
        // 3 .............
        // 4 .............
        // 5 ........4....
        // 6 .5...........
        // 7 ............6
        // 8 .............
        // 9 .............
        // 0 .........7...
        // 1 8....9.......
        string[] universe = SampleInput.Split(Environment.NewLine);
        (HashSet<int> emptyRows, HashSet<int> emptyCols) = Day11.FindEmpty(universe);


        List<Position> positions = Day11.ExpandUniverse(universe, 2, emptyRows, emptyCols);
        positions.Count.ShouldBe(9);
        positions.ShouldContain(new Position(0, 4));
        positions.ShouldContain(new Position(1, 9));
        positions.ShouldContain(new Position(2, 0));
        positions.ShouldContain(new Position(5, 8));
        positions.ShouldContain(new Position(6, 1));
        positions.ShouldContain(new Position(7, 12));
        positions.ShouldContain(new Position(10, 9));
        positions.ShouldContain(new Position(11, 5));
        positions.ShouldContain(new Position(11, 0));
    }

    [Fact]
    public void test_find_positions_puzzle_input()
    {
        string[] universe = PuzzleInput.Split(Environment.NewLine);
        (HashSet<int> emptyRows, HashSet<int> emptyCols) = Day11.FindEmpty(universe);
        List<Position> positions = Day11.ExpandUniverse(universe, 2, emptyRows, emptyCols);
        positions.Count.ShouldBe(448);
    }

    [Fact]
    public void test_find_pairs_puzzle_input()
    {
        string[] universe = PuzzleInput.Split(Environment.NewLine);
        (HashSet<int> emptyRows, HashSet<int> emptyCols) = Day11.FindEmpty(universe);
        List<Position> positions = Day11.ExpandUniverse(universe, 2, emptyRows, emptyCols);
        IEnumerable<(Position, Position)> pairs = positions.Pairs();
        pairs.Count().ShouldBe(100128);
    }
}