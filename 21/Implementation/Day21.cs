using System.Diagnostics;
using AoCHelpers;

public class Day21
{
    public static long Part1(string input, int steps)
    {
        Map map = Map.Parse(input);
        return map.FindValidPositions(map.Start, steps);
    }

    public static long Part2(string input)
    {
        return 0;
    }

    public const string SampleInput =
    """
    ...........
    .....###.#.
    .###.##..#.
    ..#.#...#..
    ....#.#....
    .##..S####.
    .##..#...#.
    .......##..
    .##.#.####.
    .##..##.##.
    ...........
    """;

    public static readonly string PuzzleInput = File.ReadAllText("input.txt");

}

public enum Direction
{
    North,
    East,
    South,
    West,
}

public static class DirectionHelper
{
    public static IEnumerable<Direction> Cardinals = [Direction.North, Direction.East, Direction.South, Direction.West];
    public static Position Step(this Position position, Direction direction) =>
    direction switch
    {
        Direction.North => position + (-1, 0),
        Direction.South => position + (1, 0),
        Direction.East => position + (0, 1),
        Direction.West => position + (0, -1),
        _ => throw new Exception($"Invalid direction {direction}"),
    };


}

public record Map(string[] Grid, int Rows, int Columns, Position Start)
{
    
    public PositionInfo this[Position ix] => GridPositionInfo(ix);
    public PositionInfo this[int row, int col] => this[(row, col)];

    public static Map Parse(string input)
    {
        string[] grid = input.ReplaceLineEndings().Trim().Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        int rows = grid.Length;
        int columns = grid[0].Length;
        Position start = (-1, -1);
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[0].Length; col++)
            {
                char ch = grid[row][col];
                if (ch == 'S')
                {
                    start = (row, col);
                    break;
                }
            }
        }
        Debug.Assert(start != (-1, -1));
        return new Map(grid, rows, columns, start);
    }

    /// <summary>
    /// Given a global position, returns the subgrid position
    /// </summary>
    public PositionInfo GridPositionInfo(Position position)
    {
        int row = ((position.Row % Rows) + Rows) % Rows;
        int col = ((position.Col % Columns) + Columns) % Columns;
        
        int gridRow;
        if (position.Row >= 0)
        {
            gridRow = position.Row / Rows;
        }
        else // (position.Row < 0)
        {
            gridRow = (position.Row - Rows + 1) / Rows;
        }

        int gridColumn;
        if (position.Col >= 0)
        {
            gridColumn = position.Col / Columns;
        }
        else // (position.Row < 0)
        {
            gridColumn = (position.Col - Columns + 1) / Columns;
        }
        
        return new PositionInfo(position, (gridRow, gridColumn), (row, col), this);
    }

    public void Traverse(Position start, int totalSteps, Predicate<Position> inBounds, Action<Position> onCount)
    {
        bool isEven = totalSteps % 2 == 0;
        Queue<(Position pos, int steps)> queue = new();
        queue.Enqueue((Start, 0));
        HashSet<Position> visited = new() { start };
        while (queue.TryDequeue(out (Position pos, int steps) current))
        {
            if (!inBounds(current.pos)) { continue; }
            if (current.steps > totalSteps) { continue; }
            if (current.steps % 2 == 0 && isEven)
            {
                onCount(current.pos);
            }
            else if (!isEven)
            {
                onCount(current.pos);
            }
            foreach (Position neighbor in Neighbors(current.pos))
            {
                // Not convinced this check is valid
                if (visited.Contains(neighbor)) { continue; }
                if (!inBounds(neighbor)) { continue; }
                visited.Add(neighbor);
                queue.Enqueue((neighbor, current.steps + 1));
            }
        }
    }

    public long FindValidPositions(Position startingPosition, int totalSteps) => FindValidPositions(startingPosition, totalSteps, (_) => true);

    public long FindValidPositions(Position startingPosition, int totalSteps, Predicate<Position> inBounds)
    {
        long count = 0;
        Traverse(startingPosition, totalSteps, inBounds, (_) => count++);
        return count;
    }

    public bool IsValidPosition(Position position)
    {
        int row = ((position.Row % Rows) + Rows) % Rows;
        int col = ((position.Col % Columns) + Columns) % Columns;
        return Grid[row][col] != '#';
    }

    public IEnumerable<Position> Neighbors(Position position) =>
        DirectionHelper.Cardinals
                       .Select(dir => position.Step(dir))
                       .Where(pos => IsValidPosition(pos));
}

public record PositionInfo(Position RawPosition, Position GridPosition, Position PositionInGrid, Map Map)
{
    public override string ToString()
    {
        (int gridRow, int gridColumn) = GridPosition;
        (int row, int col) = PositionInGrid;
        (int r, int c) = RawPosition;
        return $"({r},{c}) => Grid[{gridRow}, {gridColumn}][({row}, {col})]";
    }
}