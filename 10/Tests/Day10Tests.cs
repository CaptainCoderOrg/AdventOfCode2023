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