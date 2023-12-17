using System.Data;
using System.Diagnostics;
using AoCHelpers;

public class Day17
{
    public static long Part1(string input)
    {
        Grid grid = Grid.Parse(input);
        return grid.Traverse((grid.Rows - 1, grid.Columns - 1));
    }

    public static long Part2(string input)
    {
        return 0;
    }
}

public record Grid(int Rows, int Columns)
{
    private int[,] _positions = null!;
    // Define the indexer to allow client code to use [] notation.
    public int this[int row, int col] => _positions[row, col];
    public int this[Position position] => _positions[position.Row, position.Col];
    public static Grid Parse(string input)
    {
        string[] rows = input.Split(Environment.NewLine);
        int[,] positions = new int[rows.Length, rows[0].Length];
        for (int row = 0; row < rows.Length; row++)
        {
            for (int col = 0; col < rows[0].Length; col++)
            {
                positions[row, col] = rows[row][col] - '0';
            }
        }
        Grid grid = new Grid(positions.GetLength(0), positions.GetLength(1));
        grid._positions = positions;
        return grid;
    }

    // 1  function Dijkstra(Graph, source):
    // 2      
    // 3      for each vertex v in Graph.Vertices:
    // 4          dist[v] ← INFINITY
    // 5          prev[v] ← UNDEFINED
    // 6          add v to Q
    // 7      dist[source] ← 0
    // 8      
    // 9      while Q is not empty:
    // 10          u ← vertex in Q with min dist[u]
    // 11          remove u from Q
    // 12          
    // 13          for each neighbor v of u still in Q:
    // 14              alt ← dist[u] + Graph.Edges(u, v)
    // 15              if alt < dist[v]:
    // 16                  dist[v] ← alt
    // 17                  prev[v] ← u
    // 18
    // 19      return dist[], prev[]

    
    public long Traverse(Position end)
    {
        Position start = (0, 0);
        // (Position current, Direction direction, int stepsInDirection, int heatLevel)
        PriorityQueue<Node, int> queue = new();
        queue.Enqueue(new Node(start, Direction.East, 0), 0);
        // queue.Enqueue(new Node(start, Direction.South, 1), this[1, 0]);
        HashSet<Node> visited = new ();
        int loops = 0;
        while (queue.TryDequeue(out Node? current, out int heatLevel) && loops++ < 1_000_000)
        {
            if (current.Position == end) { return heatLevel; }
            if (visited.Contains(current)) { continue; }
            visited.Add(current);

            IEnumerable<Node> neighbors = Neighbors(current);
            foreach (Node neighbor in neighbors)
            {
                if (!IsInGrid(neighbor.Position)) { continue; }
                queue.Enqueue(neighbor, heatLevel + this[neighbor.Position]);
            }
        }
        

        throw new Exception($"No valid solution? Looped {loops}");
    }

    public bool IsInGrid(Position position) => position.Row >= 0 && position.Row < Rows && position.Col >= 0 && position.Col < Columns;

    public IEnumerable<Node> Neighbors(Node current) {

        // If we have not taken 3 steps, we can continue in the same direction.
        if (current.StepsInDirection < 3)
        {
            yield return new Node(current.Position.Step(current.Movement), current.Movement, current.StepsInDirection + 1);
        }

        Direction rotated = current.Movement.Rotate();
        yield return new Node(current.Position.Step(rotated), rotated, 1);
        Direction rotatedCounterClockwise = current.Movement.RotateCounterClockwise();
        yield return new Node(current.Position.Step(rotatedCounterClockwise), rotatedCounterClockwise, 1);        
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}

public static class DirectionExtensions
{
    public static Direction Rotate(this Direction direction)
    {
        return direction switch 
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new Exception($"Invalid direction {direction}"),
        };
    }

    public static Direction RotateCounterClockwise(this Direction direction)
    {
        return direction switch 
        {
            Direction.North => Direction.West,
            Direction.West => Direction.South,
            Direction.South => Direction.East,
            Direction.East => Direction.North,
            _ => throw new Exception($"Invalid direction {direction}"),
        };
    }
    public static Position Step(this Position position, Direction direction)
    {
        return direction switch {
            Direction.North => position + (-1,  0),
            Direction.South => position + ( 1,  0),
            Direction.East  => position + ( 0,  1),
            Direction.West  => position + ( 0, -1),
            _ => throw new Exception($"Cannot take step in {direction}"),
        };
    }
}

public record Node(Position Position, Direction Movement, int StepsInDirection);