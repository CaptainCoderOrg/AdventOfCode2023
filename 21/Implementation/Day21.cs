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
    /// <summary>
    /// Given a position, returns if we can move into that space
    /// </summary>
    public bool this[Position ix]
    {
        get
        {
            if (ix.Row < 0 || ix.Row >= Rows || ix.Col < 0 || ix.Col >= Columns) { return false; }
            return Grid[ix.Row][ix.Col] != '#';
        }
    }

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

    public long FindValidPositions(Position startingPosition, int totalSteps)
    {
        Queue<(Position pos, int steps)> queue = new();
        queue.Enqueue((Start, 0));
        HashSet<Position> visited = new() { startingPosition };
        long count = 0;
        while (queue.TryDequeue(out (Position pos, int steps) current))
        {
            if (current.steps > totalSteps) { continue; }
            if (current.steps % 2 == 0)
            {
                count++;
            }
            foreach (Position neighbor in Neighbors(current.pos))
            {
                // Not convinced this check is valid
                if (visited.Contains(neighbor)) { continue; }
                visited.Add(neighbor);
                queue.Enqueue((neighbor, current.steps + 1));
            }
        }

        return count;
    }

    public bool IsValidPosition(Position position) => this[position];

    public IEnumerable<Position> Neighbors(Position position) =>
        DirectionHelper.Cardinals
                       .Select(dir => position.Step(dir))
                       .Where(pos => this[pos]);
}