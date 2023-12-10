using System.Collections.ObjectModel;

public class Day10
{
    public static long Part1(string input)
    {
        Maze maze = Maze.Parse(input);
        return maze.Walk().Values.Max();
    }



}

// public record Position(int Row, int Col);
// | is a vertical pipe connecting north and south.
// - is a horizontal pipe connecting east and west.
// L is a 90-degree bend connecting north and east.
// J is a 90-degree bend connecting north and west.
// 7 is a 90-degree bend connecting south and west.
// F is a 90-degree bend connecting south and east.
// . is ground; there is no pipe in this tile.
// S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
public enum Pipe
{
    // Ground = '.',   // .
    // Starting = 'S', // S
    Vertical = '|',
    Horizontal = '-',
    BottomLeft = 'L',
    BottomRight = 'J',
    TopRight = '7',
    TopLeft = 'F',
}

public record Maze(IReadOnlyDictionary<Position, Pipe> Pipes, Position Start)
{

    public static Maze Parse(string input)
    {
        HashSet<Pipe> valid = new HashSet<Pipe> {
            Pipe.Vertical, 
            Pipe.Horizontal, 
            Pipe.BottomLeft,
            Pipe.BottomRight,
            Pipe.TopRight,
            Pipe.TopLeft
        };
        Dictionary<Position, Pipe> pipes = new Dictionary<Position, Pipe>();
        Position start = default;
        string[] maze = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        
        for (int row = 0; row < maze.Length; row++)
        {
            for (int col = 0; col < maze[row].Length; col++)
            {
                if (maze[row][col] == 'S') { start = (row, col); }
                Pipe pipe = (Pipe)maze[row][col];
                if (!valid.Contains(pipe)) { continue; }
                pipes[new Position(row, col)] =  pipe;
            }
        }
        
        pipes[start] = DetermineStartShape(maze, start);
        return new Maze(new ReadOnlyDictionary<Position, Pipe>(pipes), start);
    }

    // 1  procedure BFS(G, root) is
    // 2      let Q be a queue
    // 3      label root as explored
    // 4      Q.enqueue(root)
    // 5      while Q is not empty do
    // 6          v := Q.dequeue()
    // 7          if v is the goal then
    // 8              return v
    // 9          for all edges from v to w in G.adjacentEdges(v) do
    // 10              if w is not labeled as explored then
    // 11                  label w as explored
    // 12                  w.parent := v
    // 13                  Q.enqueue(w)

    public Dictionary<Position, long> Walk()
    {
        Dictionary<Position, long> distances = new();
        Queue<Position> queue = new Queue<Position>();
        HashSet<Position> explored = new HashSet<Position>();
        distances[Start] = 0;
        explored.Add(Start);
        queue.Enqueue(Start);
        while (queue.Count > 0)
        {
            Position current = queue.Dequeue();
            // How many steps have we taken?
            foreach (Position neighbor in Neighbors(current))
            {
                if (explored.Contains(neighbor)) { continue; }
                distances[neighbor] = distances[current] + 1;
                queue.Enqueue(neighbor);
                explored.Add(neighbor);
            }
        }
        return distances;
    }

    public IEnumerable<Position> Neighbors(Position position)
    {
        IEnumerable<Position> possible = Pipes[position] switch
        {
            Pipe.Vertical    => [position - (1, 0), position + (1, 0) ],
            Pipe.Horizontal  => [position - (0, 1), position + (0, 1) ],
            Pipe.BottomLeft  => [position - (1, 0), position + (0, 1)],
            Pipe.BottomRight => [position - (1, 0), position - (0, 1)],
            Pipe.TopRight    => [position + (1, 0), position - (0, 1)],
            Pipe.TopLeft     => [position + (1, 0), position + (0, 1)],
            _ => throw new Exception("Could not determine neighbors")
        };
        return possible.Where(Pipes.ContainsKey);
    }

    public static Pipe DetermineStartShape(string[] maze, Position position)
    {
        // Note: There is a hack here that doesn't account for being at the edge
        // of a map. This has been 
        Position above = position - (1, 0);
        Position below = position + (1, 0);
        Position left = position - (0, 1);
        Position right = position + (0, 1);
        
        char spaceAbove = maze[above.Row][above.Col];
        char spaceBelow = maze[below.Row][below.Col];
        char spaceLeft = maze[left.Row][left.Col];
        char spaceRight = maze[right.Row][right.Col];

        // If Above pipes down and below Pipes Up, we are vertical
        if (spaceAbove.PipesDown() && spaceBelow.PipesUp())
        {
            return Pipe.Vertical;
        }
        
        // If Above pipes down and left Pipes right, we are BottomRight
        if (spaceAbove.PipesDown() && spaceLeft.PipesRight())
        {
            return Pipe.BottomRight;
        }

        // If Above pipes down and right Pipes left, we are BottomRight
        if (spaceAbove.PipesDown() && spaceRight.PipesLeft())
        {
            return Pipe.BottomLeft;
        }

        // F  <---
        // ^
        // |
        if (spaceBelow.PipesUp() && spaceRight.PipesLeft())
        {
            return Pipe.TopLeft;
        }

        // --> 7 
        //     ^
        //     |
        if (spaceBelow.PipesUp() && spaceLeft.PipesRight())
        {
            return Pipe.TopRight;
        }

        // --> - <--
        if (spaceLeft.PipesRight() && spaceRight.PipesLeft())
        {
            return Pipe.Horizontal;
        }

        throw new Exception("Could not determine starting pipe");

    }

    
}

public static class Extensions
{
    public static string UpPipes = "|LJ";
    public static string DownPipes = "|F7";
    public static string RightPipes = "-LF";
    public static string LeftPipes = "-J7";
    public static bool PipesRight(this char ch) => RightPipes.Contains(ch);
    public static bool PipesDown(this char ch) => DownPipes.Contains(ch);
    public static bool PipesUp(this char ch) => UpPipes.Contains(ch);
    public static bool PipesLeft(this char ch) => LeftPipes.Contains(ch);
}