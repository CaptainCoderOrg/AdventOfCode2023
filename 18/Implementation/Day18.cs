using System.Runtime.InteropServices;
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
        Polygon outline = Polygon.Parse(input);
        // Pick's theorem ( https://en.wikipedia.org/wiki/Pick%27s_theorem )
        long area = Shoelace(outline.Positions);
        long totalArea = area + outline.Perimeter / 2 + 1;
        return totalArea;
    }

    public static long Shoelace(IEnumerable<Position> vertices)
    {
        return vertices.Zip([.. vertices.Skip(1), vertices.First()])
                .Select(((Position First, Position Second) points) => ((long)points.Second.Col + points.First.Col) * ((long)points.Second.Row - points.First.Row))
                .Sum() / 2;
    }


}

public enum GridState
{
    Hole,
    NoHole,
}

public record Polygon(IEnumerable<Position> Positions, long Perimeter)
{
    public static Polygon Parse(string input)
    {
        List<Position> outline = new();
        Position cursor = (0, 0);
        long perimeter = 0;
        // outline.Add(cursor);
        foreach (string line in input.Split(Environment.NewLine))
        {
            Instruction instruction = Instruction.ParseTrueInstruction(line);
            perimeter += instruction.Steps;
            cursor = cursor.StepN(instruction.Direction, instruction.Steps);
            outline.Add(cursor);
        }
        return new Polygon(outline, perimeter);
    }
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
        long loops = 0;
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
            if (++loops % 1_000_000_000 == 0)
            {
                Console.WriteLine($"Found {loops} positions");
            }
            foreach (Direction dir in (Direction[])[Direction.Up, Direction.Down, Direction.Left, Direction.Right])
            {
                Position step = next.Step(dir);
                if (holes.Contains(step)) { continue; }
                holes.Add(step);
                queue.Enqueue(step);                
            }
        }
        Console.WriteLine($"Found {loops} positions");
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

    public static Position StepN(this Position position, Direction direction, int steps)
    {
        return direction switch
        {
            Direction.Up => position + (-steps, 0),
            Direction.Down => position + (steps, 0),
            Direction.Left => position + (0, -steps),
            Direction.Right => position + (0, steps),
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

    public static Instruction ParseTrueInstruction(string input)
    {
        var groups = InstructionRegex().Match(input).Groups;
        string hash = groups["hash"].Value;
        int steps = Convert.ToInt32(hash[0..5], 16);
        Direction direction = hash[5] switch
        {
            '0' => Direction.Right,
            '1' => Direction.Down,
            '2' => Direction.Left,
            '3' => Direction.Up,
            _ => throw new Exception("This problem is dumb. I really wanted to use hex color transformations"),
        };
        return new Instruction(direction, steps, hash);
    }

    [GeneratedRegex(@"(?<dir>[DRUL]) (?<steps>\d+) \(#(?<hash>[0-9a-f]+)\)")]
    private static partial Regex InstructionRegex();
}