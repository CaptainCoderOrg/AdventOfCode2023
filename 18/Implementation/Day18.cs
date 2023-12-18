using System.Text.RegularExpressions;
using AoCHelpers;

public partial class Day18
{
    public static long Part1(string input)
    {
        Outline outline = Outline.Parse(input);
        Position start = (1, 1);
        HashSet<Position> filled = outline.FloodFill(start);
        return filled.Count();
    }

    public static long Part2(string input)
    {
        return 0;
    }



}

public enum GridState
{
    Hole,
    NoHole,
}

public record Outline(Position TopLeft, Position BottomRight, HashSet<Position> Holes)
{
    public static Outline Parse(string input)
    {
        HashSet<Position> outline = new();
        Position cursor = (0, 0);
        Position topLeft = cursor;
        Position bottomRight = cursor;
        outline.Add(cursor);
        foreach (string line in input.Split(Environment.NewLine))
        {
            Instruction instruction = Instruction.Parse(line);
            for (int count = 0; count < instruction.Steps; count++)
            {
                cursor = cursor.Step(instruction.Direction);
                outline.Add(cursor);
                topLeft = Position.Min(cursor, topLeft);
                bottomRight = Position.Max(cursor, bottomRight);
            }
        }
        return new Outline(topLeft, bottomRight, outline);
    }

    public void PrettyPrint()
    {
        for (int row = TopLeft.Row; row <= BottomRight.Row; row++)
        {
            for (int col = TopLeft.Col; col <= BottomRight.Col; col++)
            {
                if (Holes.Contains(new Position(row, col)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    public HashSet<Position> FloodFill(Position start)
    {
        HashSet<Position> holes = Holes.ToHashSet();
        Queue<Position> queue = new();
        queue.Enqueue(start);
        holes.Add(start);
        int sanity_stopper = 10_000_000;
        int loops = 0;
        while (queue.TryDequeue(out Position next))
        {
            if  (loops++ > sanity_stopper)
            {
                throw new Exception($"Looped too much: {loops}");
            }
            //  9          for all edges from v to w in G.adjacentEdges(v) do
            // 10              if w is not labeled as explored then
            // 11                  label w as explored
            // 12                  w.parent := v
            // 13                  Q.enqueue(w)
            foreach (Direction dir in (Direction[])[Direction.Up, Direction.Down, Direction.Left, Direction.Right])
            {
                Position step = next.Step(dir);
                if (holes.Contains(step)) { continue; }
                holes.Add(step);
                queue.Enqueue(step);                
            }
        }
        
        return holes;
    }
}

public enum Direction : ushort
{
    Up = 'U',
    Down = 'D',
    Left = 'L',
    Right = 'R'
}

public static class DirectionExtensions
{
    public static Position Step(this Position position, Direction direction)
    {
        return direction switch
        {
            Direction.Up => position + (-1, 0),
            Direction.Down => position + (1, 0),
            Direction.Left => position + (0, -1),
            Direction.Right => position + (0, 1),
            _ => throw new Exception($"Unexpected direction {direction}"),
        };
    }

    public static void PrettyPrintPositionSet(this HashSet<Position> positions)
    {
        (Position topLeft, Position bottomRight) = Position.FindMinMax(positions);
        for (int row = topLeft.Row; row <= bottomRight.Row; row++)
        {
            for (int col = topLeft.Col; col <= bottomRight.Col; col++)
            {
                if (positions.Contains(new Position(row, col)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }
}

public partial record Instruction(Direction Direction, int Steps, string Hash)
{
    public static Instruction Parse(string input)
    {
        var groups = InstructionRegex().Match(input).Groups;
        Direction direction = (Direction)groups["dir"].Value[0];
        int steps = int.Parse(groups["steps"].Value);
        string hash = groups["hash"].Value;
        return new Instruction(direction, steps, hash);
    }

    [GeneratedRegex(@"(?<dir>[DRUL]) (?<steps>\d+) \(#(?<hash>[0-9a-f]+)\)")]
    private static partial Regex InstructionRegex();
}