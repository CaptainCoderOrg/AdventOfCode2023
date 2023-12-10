namespace Tests;
using Shouldly;

public class Day10Tests
{
    public static string PuzzleInput = File.ReadAllText("input.txt");

    public static string SampleInput =
    """
    ...F7.
    ..FJ|.
    .SJ.L7
    .|F--J
    .LJ...
    """.Trim();
    
    public static string SampleInputSimple = 
    """
    .....
    .S-7.
    .|.|.
    .L-J.
    .....
    """.Trim();

    public static string SampleInputPart2 =
    """
    FF7FSF7F7F7F7F7F---7
    L|LJ||||||||||||F--J
    FL-7LJLJ||||||LJL-77
    F--JF--7||LJLJ7F7FJ-
    L---JF-JLJ.||-FJLJJ7
    |F|F-JF---7F7-L7L|7|
    |FFJF7L7F-JF7|JL---7
    7-L-JL7||F7|L7F-7F7|
    L.L7LFJ|||||FJL7||LJ
    L7JLJL-JLJLJL--JLJ.L
    """.Trim();

    [Fact]
    public void test_sample_input_part1()
    {
        Day10.Part1(SampleInput).ShouldBe(8);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day10.Part1(PuzzleInput).ShouldBe(6757L);
    }

    [Fact]
    public void test_sample_input_part2()
    {
        Day10.Part2(SampleInputPart2).ShouldBe(10);
    }

    [Fact]
    public void test_puzzle_input_part2()
    {
        Day10.Part2(PuzzleInput).ShouldBe(523L);
    }

    [Fact]
    public void test_expand_map_does_not_crash()
    {
        // """
        // .....
        // .S-7.
        // .|.|.
        // .L-J.
        // .....
        // """.Trim();
        Maze maze = Maze.Parse(PuzzleInput);

        string expanded = string.Join("\n", maze.ExpandMap()).Trim();

    }

    [Fact]
    public void test_maze_start_shape()
    {
        Maze maze = Maze.Parse(PuzzleInput);
        maze.Pipes[maze.Start].ShouldBe(Pipe.BottomRight);
    }

    [Fact]
    public void test_parse_maze_sample_input()
    {
        // """
        // .....
        // .S-7.
        // .|.|.
        // .L-J.
        // .....
        // """.Trim();
        Maze maze = Maze.Parse(SampleInputSimple);
        maze.Pipes[(1, 1)].ShouldBe(Pipe.TopLeft);
        maze.Pipes[(1, 2)].ShouldBe(Pipe.Horizontal);
        maze.Pipes[(1, 3)].ShouldBe(Pipe.TopRight);
        maze.Pipes[(2, 3)].ShouldBe(Pipe.Vertical);
        maze.Pipes[(3, 3)].ShouldBe(Pipe.BottomRight);
        maze.Pipes[(3, 2)].ShouldBe(Pipe.Horizontal);
        maze.Pipes[(3, 1)].ShouldBe(Pipe.BottomLeft);
        maze.Pipes[(2, 1)].ShouldBe(Pipe.Vertical);
        
    }
}