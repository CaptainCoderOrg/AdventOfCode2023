using AoCHelpers;

public class Day23
{
    public static readonly string PuzzleInput = File.ReadAllText("input.txt");
    public const string SampleInput =
    """
    #.#####################
    #.......#########...###
    #######.#########.#.###
    ###.....#.>.>.###.#.###
    ###v#####.#v#.###.#.###
    ###.>...#.#.#.....#...#
    ###v###.#.#.#########.#
    ###...#.#.#.......#...#
    #####.#.#.#######.#.###
    #.....#.#.#.......#...#
    #.#####.#.#.#########v#
    #.#...#...#...###...>.#
    #.#.#v#######v###.###v#
    #...#.>.#...>.>.#.###.#
    #####v#.#.###v#.#.###.#
    #.....#...#...#.#.#...#
    #.#########.###.#.#.###
    #...###...#...#...#.###
    ###.###.#.###v#####v###
    #...#...#.#.>.>.#.>.###
    #.###.###.#.###.#.#v###
    #.....###...###...#...#
    #####################.#
    """;

    public static long Part1(string input)
    {
        Grid grid = Grid.Parse(input);
        Graph graph = Graph.Build(grid);
        return graph.AllPathLengths().Max();
    }

    public static long Part2(string input)
    {
        return 0;
    }
}

public class Graph
{
    public Position Start { get; init; }
    public Position End { get; init; }
    public IReadOnlyList<Position> Nodes { get; init; } = null!;
    public IReadOnlyDictionary<Position, List<Edge>> Edges { get; init; } = null!;

    public IEnumerable<long> AllPathLengths()
    {
        List<long> paths = new();
        Stack<(Position, long Steps, HashSet<Position>)> toVisit = new ();
        toVisit.Push((Start, 0, [Start]));

        while (toVisit.TryPop(out (Position, long, HashSet<Position>) next))
        {
            (Position current, long steps, HashSet<Position> visited) = next;
            if (current == End) 
            {
                paths.Add(steps);
                // continue;
            }

            List<Edge> edges = Edges[current];
            foreach (Edge edge in edges)
            {
                // // 10              if w is not labeled as explored then
                if (visited.Contains(edge.Destination)) { continue; }
                // // 11                  label w as explored
                // // 12                  w.parent := v
                // // 13                  Q.enqueue(w)
                // visited.Add(pos);
                HashSet<Position> newVisited = visited.ToHashSet();
                _ = newVisited.Add(edge.Destination);
                long newSteps = steps + edge.Length;
                Position newSource = edge.Destination;
                toVisit.Push((newSource, newSteps, newVisited));
            }

        }

        return paths;
    }

    public static Graph Parse(string input) => Build(Grid.Parse(input));

    public static Graph Build(Grid grid)
    {
        HashSet<Position> nodes = [grid.Start, grid.End, .. grid.Conjunctions()];
        Dictionary<Position, List<Edge>> edges = nodes.ToDictionary(n => n, _ => new List<Edge>());

        Queue<(Position Start, int Steps, Position Current)> toVisit = new();
        toVisit.Enqueue((grid.Start, 1, grid.Start.Step(Direction.South)));
        HashSet<(Position source, Position current)> visited = new() { 
            (grid.Start, grid.Start), 
            (grid.Start.Step(Direction.South), grid.Start) 
            };

        // 1  procedure BFS(G, root) is
        // 2      let Q be a queue
        // 3      label root as explored
        // 4      Q.enqueue(root)

        // 5      while Q is not empty do
        while (toVisit.TryDequeue(out (Position, int, Position) next))
        {


            // 6          v := Q.dequeue()
            (Position source, int steps, Position current) = next;
            if (current == new Position(19, 13))
            {
                Console.WriteLine("Here!");
            }

            // If we have found a node, we create an edge to it
            if (nodes.Contains(current) && current != source)
            {
                edges[source].Add(new Edge(source, steps, current));
                source = current;
                steps = 0;
            }

            // 9          for all edges from v to w in G.adjacentEdges(v) do      
            foreach (Position pos in grid.Neighbors(current))
            {
                // 10              if w is not labeled as explored then
                if (visited.Contains((source, pos))) { continue; }
                // 11                  label w as explored
                // 12                  w.parent := v
                // 13                  Q.enqueue(w)
                visited.Add((source, pos));
                toVisit.Enqueue((source, steps + 1, pos));
            }
        }

        return new Graph { Nodes = nodes.ToList(), Edges = edges, Start = grid.Start, End = grid.End };
    }
}

public record Edge(Position Source, long Length, Position Destination);

public record Grid(string[] Raw, int Rows, int Columns)
{
    public const char Wall = '#';
    public char this[Position ix]
    {
        get
        {
            if (ix.Row >= 0 && ix.Col >= 0 && ix.Row < Rows && ix.Col < Columns)
            {
                return Raw[ix.Row][ix.Col];
            }
            return Wall;
        }
    }

    public Position Start { get; } = (0, 1);
    public Position End { get; } = (Rows - 1, Columns - 2);
    public static Grid Parse(string input)
    {
        string[] rows = input.ReplaceLineEndings().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return new(rows, rows.Length, rows[0].Length);
    }

    public bool IsWall(Position position) => this[position] == Wall;

    public IEnumerable<Position> Neighbors(Position position)
    {
        char symbol = this[position];
        IEnumerable<Direction> directions = symbol switch
        {
            '#' => [],
            '^' => [Direction.North],
            '>' => [Direction.East],
            '<' => [Direction.West],
            'v' => [Direction.South],
            '.' => DirectionExtensions.Cardinals,
            _ => throw new Exception($"Invalid symbol detected {symbol} @ {position}"),
        };
        return directions.Select(d => position.Step(d))
                         .Where(pos => !IsWall(pos));
    }

    public IEnumerable<Position> Conjunctions()
    {
        foreach (Position p in Position.Iterate((0, 0), (Rows - 1, Columns - 1)))
        {
            int neighbors = Neighbors(p).Count();
            if (neighbors < 3) { continue; }
            yield return p;
        }
    }
}