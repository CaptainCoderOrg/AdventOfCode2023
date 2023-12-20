using System.Reflection.Metadata;
using Shouldly;

namespace Tests;

public class Day20Tests
{

    public const string SampleInput1 =
    """""
    broadcaster -> a, b, c
    %a -> b
    %b -> c
    %c -> inv
    &inv -> a
    """""; //.ReplaceLineEndings().Trim();

    public const string SampleInput2 =
    """
    broadcaster -> a
    %a -> inv, con
    &inv -> b
    %b -> con
    &con -> output
    """; //.ReplaceLineEndings().Trim();

    public readonly string PuzzleInput = File.ReadAllText("input.txt");

    [Fact]
    public void test_push_button()
    {
        ModuleNetwork network = ModuleNetwork.Parse(SampleInput1);
        network.PushButton().ShouldBe((8, 4));
        network.PushButton().ShouldBe((8, 4));
    }

    [Theory]
    [InlineData(SampleInput1, 32_000_000)]
    [InlineData(SampleInput2, 11_687_500)]
    public void test_sample_input_part1(string input, long expected)
    {
        Day20.Part1(input).ShouldBe(expected);
    }

    [Fact]
    public void test_puzzle_input_part1()
    {
        Day20.Part1(PuzzleInput).ShouldBe(841763884L);
    }

    [Fact]
    public void test_parse_sample_input_1()
    {
        // broadcaster -> a, b, c
        // %a -> b
        // %b -> c
        // %c -> inv
        // &inv -> a
        ModuleNetwork network = ModuleNetwork.Parse(SampleInput1);
        network.Nodes.Count.ShouldBe(5);
        network.Nodes.ShouldContainKey("broadcaster");
        network.Nodes["broadcaster"].MType.ShouldBe(ModuleType.Broadcaster);
        network.Nodes.ShouldContainKey("a");
        network.Nodes["a"].MType.ShouldBe(ModuleType.FlipFlop);
        network.Nodes.ShouldContainKey("b");
        network.Nodes["b"].MType.ShouldBe(ModuleType.FlipFlop);
        network.Nodes.ShouldContainKey("c");
        network.Nodes["c"].MType.ShouldBe(ModuleType.FlipFlop);
        network.Nodes.ShouldContainKey("inv");
        network.Nodes["inv"].MType.ShouldBe(ModuleType.Conjunction);
        
        network.IncomingEdges["a"].Count.ShouldBe(2);
        network.IncomingEdges["a"].ShouldContain("broadcaster");
        network.IncomingEdges["a"].ShouldContain("inv");

        network.IncomingEdges["b"].Count.ShouldBe(2);
        network.IncomingEdges["b"].ShouldContain("broadcaster");
        network.IncomingEdges["b"].ShouldContain("a");

        network.IncomingEdges["c"].Count.ShouldBe(2);
        network.IncomingEdges["c"].ShouldContain("broadcaster");
        network.IncomingEdges["c"].ShouldContain("b");

        network.IncomingEdges["inv"].Count.ShouldBe(1);
        network.IncomingEdges["inv"].ShouldContain("c");
    }
}