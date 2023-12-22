using System.Diagnostics;
using Shouldly;

namespace Tests;

public class Day22Tests
{
    public const string SampleInput =
    """
    1,0,1~1,2,1
    0,0,2~2,0,2
    0,2,3~2,2,3
    0,0,4~0,2,4
    2,0,5~2,2,5
    0,1,6~2,1,6
    1,1,8~1,1,9
    """;

    public static readonly string PuzzleInput = File.ReadAllText("input.txt");
     
    [Fact]
    public void test_sample_input_part1()
    {
        Day22.Part1(SampleInput).ShouldBe(5);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day22.Part1(PuzzleInput).ShouldBe(5);
    }

    [Fact]
    public void test_parse_all()
    {
        Brick[] bricks = [..Brick.ParseAll(SampleInput)];
        bricks.Length.ShouldBe(7);
        bricks.ShouldBeSubsetOf(new Brick[]
        {
            new Brick((1,0,1), (1,2,1)),
            new Brick((0,0,2), (2,0,2)),
            new Brick((0,2,3), (2,2,3)),
            new Brick((0,0,4), (0,2,4)),
            new Brick((2,0,5), (2,2,5)),
            new Brick((0,1,6), (2,1,6)),
            new Brick((1,1,8), (1,1,9)),
        });
    }

    [Fact]
    public void test_sort_bricks()
    {
        Brick[] bricks = [
                            new Brick((0,0,2), (2,0,2)),
                            new Brick((0,2,3), (2,2,3)),
                            new Brick((2,0,5), (2,2,5)),
                            new Brick((1,0,1), (1,2,1)),
                            new Brick((0,0,4), (0,2,4)),
                            new Brick((1,1,8), (1,1,9)),
                            new Brick((0,1,6), (2,1,6)),
                            ];
        // Random.Shared.Shuffle(bricks);
        Brick[] sorted = [.. bricks.Order()];
        sorted[0].ShouldBe(new Brick((1,0,1), (1,2,1)));
        sorted[1].ShouldBe(new Brick((0,0,2), (2,0,2)));
        sorted[2].ShouldBe(new Brick((0,2,3), (2,2,3)));
        sorted[3].ShouldBe(new Brick((0,0,4), (0,2,4)));
        sorted[4].ShouldBe(new Brick((2,0,5), (2,2,5)));
        sorted[5].ShouldBe(new Brick((0,1,6), (2,1,6)));
        sorted[6].ShouldBe(new Brick((1,1,8), (1,1,9)));   
    }

    //  y
    // 012
    // .G. 6
    // .G. 5
    // .F. 4
    // ??? 3 z
    // B.C 2
    // AAA 1
    // --- 0
    [Fact]
    public void test_fall()
    {
        List<Brick> bricks = [
                            new Brick((0,0,2), (2,0,2)),
                            new Brick((0,2,3), (2,2,3)),
                            new Brick((2,0,5), (2,2,5)),
                            new Brick((1,0,1), (1,2,1)),
                            new Brick((0,0,4), (0,2,4)),
                            new Brick((1,1,8), (1,1,9)),
                            new Brick((0,1,6), (2,1,6)),
                            ];

        // Brick[] tower = Tower.Fall(bricks).ToArray();
        
    }
}