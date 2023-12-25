public class Day25
{
    public const string SampleInput = """
    jqt: rhn xhk nvd
    rsh: frs pzl lsr
    xhk: hfx
    cmg: qnr nvd lhk bvb
    rhn: xhk bvb hfx
    bvb: xhk hfx
    pzl: lsr hfx nvd
    qnr: nvd
    ntq: jqt hfx bvb xhk
    nvd: lhk
    lsr: lhk
    rzs: qnr cmg lsr rsh
    frs: qnr lhk lsr
    """;
    public readonly string PuzzleInput = File.ReadAllText("input.txt");
    public static long Part1(string input)
    {
        
        return 0;
    }
}

public class Graph
{
    public static StringSplitOptions TrimAndRemove = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
    private Dictionary<string, HashSet<string>> _edges = new ();

    public void AddEdge(string source, string destination)
    {
        _addEdge(source, destination);
        _addEdge(destination, source);
    }

    public void RemoveEdge(string source, string destination)
    {
        _edges[source].Remove(destination);
        _edges[destination].Remove(source);
    }

    private void _addEdge(string source, string destination)
    {
        if (!_edges.TryGetValue(source, out HashSet<string>? destinations))
            {
                destinations = new HashSet<string>();
                _edges[source] = destinations;
            }
            destinations.Add(destination);
    }

    public string ToDot() => $"graph G {{ { string.Join("\n", _edges.Select(k => $"{k.Key} -- {{ {string.Join(", ", k.Value)} }}")) } }}";

    public static Graph Parse(string input)
    {
        Graph graph = new ();
        var edges = input.ReplaceLineEndings().Split(Environment.NewLine, TrimAndRemove).Select(ParseEdgeList);
        foreach ((string source, IEnumerable<string> destinations) in edges)
        {
            foreach (string destination in destinations)
            {
                graph.AddEdge(source, destination);
            }
        }
        return graph;
    }

    public static (string, IEnumerable<string>) ParseEdgeList(string input) {
        string[] parts = input.Split(": ");
        return (parts[0], parts[1].Split(" ", TrimAndRemove));
    }
}