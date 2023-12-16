using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using AoCHelpers;
public class Day16
{
    public static long Part1(string input)
    {
        Grid grid = Grid.Parse(input);
        return grid.Energized(((0, 0), Direction.East)).Count;
    }

    public static long Part2(string input)
    {
        Grid grid = Grid.Parse(input);
        (Position, Direction)[] possibleStarts = [
        .. Enumerable.Range(0, grid.Columns).Select(col => ((0, col), Direction.South)),
        .. Enumerable.Range(0, grid.Columns).Select(col => ((grid.Rows - 1, col), Direction.North)),
        .. Enumerable.Range(0, grid.Rows).Select(row => ((row, 0), Direction.East)),
        .. Enumerable.Range(0, grid.Rows).Select(row => ((row, grid.Columns - 1), Direction.West))];

        return possibleStarts.Select(grid.Energized).Select(h => h.Count).Max();
    }
}

public record Grid(IReadOnlyDictionary<Position, Tile> Tiles, int Rows, int Columns)
{
    public static Grid Parse(string input)
    {
        string[] raw = input.Split(Environment.NewLine);
        Dictionary<Position, Tile> tiles = new();
        int rows = raw.Length;
        int columns = raw[0].Length;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                char ch = raw[row][col];
                tiles[(row, col)] = (Tile)ch;
            }
        }
        return new Grid(new ReadOnlyDictionary<Position, Tile>(tiles), rows, columns);
    }

    public HashSet<Position> Energized((Position pos, Direction dir) start)
    {
            //  1  procedure BFS(G, root) is
        //  2      let Q be a queue
        //  3      label root as explored
        //  4      Q.enqueue(root)
        HashSet<Position> energized = new();
        HashSet<(Position pos, Direction dir)> visited = new();
        Queue<(Position pos, Direction dir)> queue = new();
        queue.Enqueue(start);
        visited.Add(start);
        energized.Add(start.pos);

        //  5      while Q is not empty do  
        //  6          v := Q.dequeue()
        while (queue.TryDequeue(out (Position pos, Direction dir) node))
        {
            //  7          if v is the goal then
            //  8              return v
            //  9          for all edges from v to w in G.adjacentEdges(v) do
            foreach ((Position pos, Direction dir) neighbor in Neighbors(node))
            {
                // 10              if w is not labeled as explored then
                if (visited.Contains(neighbor)) { continue; }
                // 11                  label w as explored
                visited.Add(neighbor);
                // 12                  w.parent := v
                // 13                  Q.enqueue(w)
                energized.Add(neighbor.pos);
                queue.Enqueue(neighbor);
            }
        }
        return energized;
    }

    public IEnumerable<(Position pos, Direction dir)> Neighbors((Position pos, Direction dir) node)
    {
        return neighbors(node).Where(n => n.pos.Row >= 0 && n.pos.Row < Rows && n.pos.Col >= 0 && n.pos.Col < Columns);
    }

    private IEnumerable<(Position pos, Direction dir)> neighbors((Position pos, Direction dir) node)
    {
        Tile tile = Tiles[node.pos];
        if (
            // If we are on an empty cell, keep going in the direction we are going
            tile is Tile.None ||
            // If we are going north or south AND we hit a vertical pipe, keep going
            (tile is Tile.VerticalPipe && (node.dir == Direction.North || node.dir == Direction.South)) ||
            // If we are going east or west AND we hit a horizontal pipe, keep going
            (tile is Tile.HorizontalPipe && (node.dir == Direction.East || node.dir == Direction.West))
            )
        {
            Position nextPosition = node.pos + (node.dir.Row, node.dir.Col);
            return [(nextPosition, node.dir)];
        }

        // Split North / South
        if (tile is Tile.VerticalPipe && (node.dir == Direction.East || node.dir == Direction.West))
        {
            return [
                (node.pos + (Direction.North.Row, Direction.North.Col), Direction.North),
                (node.pos + (Direction.South.Row, Direction.South.Col), Direction.South)
            ];
        }

        // Split East / West
        if (tile is Tile.HorizontalPipe && (node.dir == Direction.North || node.dir == Direction.South))
        {
            return [
                (node.pos + (Direction.East.Row, Direction.East.Col), Direction.East),
                (node.pos + (Direction.West.Row, Direction.West.Col), Direction.West)
            ];
        }


        Direction nextDirection = (char)tile switch 
        {
            '/' => new Direction(-node.dir.Col, -node.dir.Row),
            '\\' => new Direction(node.dir.Col, node.dir.Row),
            _ => throw new Exception("Could not determine new direction")

        };

        return [(node.pos + (nextDirection.Row, nextDirection.Col), nextDirection)];

    }
}

public record Direction(int Row, int Col)
{
    public static Direction North { get; } = new Direction(-1, 0);
    public static Direction East { get; } = new Direction(0, 1);
    public static Direction South { get; } = new Direction(1, 0);
    public static Direction West { get; } = new Direction(0, -1);
}

public enum Tile : ushort
{
    None = '.',
    VerticalPipe = '|',
    HorizontalPipe = '-',
    ForwardMirror = '/',
    BackwordMirror = '\\',
}