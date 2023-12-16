using Shouldly;

namespace Tests;

public class Day15Tests
{

    public static readonly string SampleInput = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7".Trim();
    public static readonly string PuzzleInput = File.ReadAllText("input.txt").Trim();
    [Fact]
    public void test_sample_input_part1()
    {
        Day15.Part1(SampleInput).ShouldBe(1320);
    }

    [Fact]
    public void test_sample_input_part2()
    {
        Day15.Part2(SampleInput).ShouldBe(145);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day15.Part1(PuzzleInput).ShouldBe(514394L);
    }

    [Fact]
    public void test_puzzle_input_part2()
    {
        Day15.Part2(PuzzleInput).ShouldBe(514394L);
    }

    [Theory]
    [InlineData("rn=1", 30)]
    [InlineData("cm-", 253)]
    [InlineData("qp=3", 97)]
    [InlineData("cm=2", 47)]
    [InlineData("qp-", 14)]
    [InlineData("pc=4", 180)]
    [InlineData("ot=9", 9)]
    [InlineData("ab=5", 197)]
    [InlineData("pc-", 48)]
    [InlineData("pc=6", 214)]
    [InlineData("ot=7", 231)]
    public void test_hash(string toHash, int expected)
    {
        Day15.Hash(toHash).ShouldBe(expected);
    }

    [Theory]
    [InlineData("rn=1", "rn", 1)]
    [InlineData("qp=3", "qp", 3)]
    [InlineData("ot=9", "ot", 9)]
    public void test_parse_insert(string input, string key, int expected)
    {
        Instruction.Parse(input).ShouldBe(new Insert(key, expected));
    }

    [Theory]
    [InlineData("rn-", "rn")]
    [InlineData("qp-", "qp")]
    [InlineData("ot-", "ot")]
    public void test_parse_remove(string input, string key)
    {
        Instruction.Parse(input).ShouldBe(new Remove(key));
    }




}