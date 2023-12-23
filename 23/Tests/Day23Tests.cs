using AoCHelpers;
using Shouldly;

namespace Tests;

public class Day23Tests
{
    [Fact]
    public void test_sample_input_part1()
    {
        Day23.Part1(Day23.SampleInput).ShouldBe(94);
    }
    [Fact]
    public void test_puzzle_input_part1()
    {
        Day23.Part1(Day23.PuzzleInput).ShouldBe(2170L);
    }

    [Fact]
    public void test_all_path_lengths()
    {
        long[] lengths = [..Graph.Parse(Day23.SampleInput).AllPathLengths()];
        lengths.Length.ShouldBe(6);
        lengths.ShouldBeSubsetOf(new long[]{
            94,
            90,
            86,
            82,
            82,
            74
        });
    }
    
    [Fact]
    public void test_build_graph_sample_input()
    {

        Grid grid = Grid.Parse(Day23.SampleInput);
        Graph graph = Graph.Build(grid);
        // 15 steps from Start to First Conjunction
        graph.Edges[grid.Start].Count.ShouldBe(1);
        Edge firstConjunction = graph.Edges[grid.Start][0];
        firstConjunction.Length.ShouldBe(15);
        Position secondNode = firstConjunction.Destination;
        graph.Edges[secondNode].Count.ShouldBe(2);
        // 22 to both
        graph.Edges[secondNode][0].Length.ShouldBe(22);
        graph.Edges[secondNode][1].Length.ShouldBe(22);

        graph.Edges[new Position(13, 5)].Count.ShouldBe(2);

        graph.Edges[grid.End].Count.ShouldBe(0);

        // There should be exactly one edge to the end
        graph.Edges
             .Values
             .Where(edges => edges.Any(e => e.Destination == grid.End))
             .Count().ShouldBe(1);
    }

    [Fact]
    public void test_build_graph_puzzle_input_finishes()
    {
        Graph g = Graph.Build(Grid.Parse(Day23.PuzzleInput));
    }

    [Fact]
    public void validate_nodes()
    {
        Grid sampleGrid = Grid.Parse(Day23.SampleInput);
        Graph sampleGraph = Graph.Build(sampleGrid);
        sampleGraph.Nodes.Count.ShouldBe(9);

        Grid puzzleGrid = Grid.Parse(Day23.PuzzleInput);
        Graph puzzleGraph = Graph.Build(puzzleGrid);
        puzzleGraph.Nodes.Count.ShouldBe(36);
    }

    // [Fact]
    // public void test_all_paths_are_maze()
    // {
    //     Grid grid = Grid.Parse(Day23.PuzzleInput);
        
    //     foreach (Position p in Position.Iterate((0, 0), (grid.Rows -1, grid.Columns - 1)))
    //     {
    //         Position[] neighbors = [..grid.Neighbors(p)];
    //         int count = neighbors.Length;
    //         count.ShouldBeLessThan(3, $"{p} has {count} neighbors: {string.Join(",", neighbors)}");
    //     }
    // }
}