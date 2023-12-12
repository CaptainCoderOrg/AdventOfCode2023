namespace Tests;
using Shouldly;
//        ^-- Shouldly has 2 els
public class Day12Tests
{

    public static readonly string SampleInput =
    """
    ???.### 1,1,3
    .??..??...?##. 1,1,3
    ?#?#?#?#?#?#?#? 1,3,1,6
    ????.#...#... 4,1,1
    ????.######..#####. 1,6,5
    ?###???????? 3,2,1
    """.Trim();

    public static readonly string PuzzleInput = string.Join(Environment.NewLine, File.ReadAllLines("input.txt"));

    [Fact]
    public void test_sample_input_part1()
    {
        Day12.Part1(SampleInput).ShouldBe(21);
    }
    
    [Fact]
    public void test_sample_input_part2()
    {
        Day12.Part2(SampleInput).ShouldBe(525152);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day12.Part1(PuzzleInput).ShouldBe(7732L);
    }

    [Fact]
    public void test_puzzle_input_part2()
    {
        Day12.Part2(PuzzleInput).ShouldBe(4500070301581L);
    }

    [Theory]
    [InlineData("???.### 1,1,3", 1)]
    [InlineData(".??..??...?##. 1,1,3", 4)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [InlineData("????.#...#... 4,1,1", 1)]
    [InlineData("????.######..#####. 1,6,5", 4)]
    [InlineData("?###???????? 3,2,1", 10)]
    public void test_process_springs(string line, long expected)
    {
        Day12.ProcessSprings(line).ShouldBe(expected);
    }

    [Fact]
    public void test_process_springs_first()
    {
        Day12.ProcessSprings("???.### 1,1,3").ShouldBe(1);
    }

    [Theory]
    [InlineData(".# 1", ".#?.#?.#?.#?.# 1,1,1,1,1")]
    [InlineData("???.### 1,1,3", "???.###????.###????.###????.###????.### 1,1,3,1,1,3,1,1,3,1,1,3,1,1,3")]
    public void test_expand_input(string input, string output)
    {
        Day12.ExpandInput(input).ShouldBe(output);
    }
}