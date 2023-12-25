public class Day24
{

    public const string SampleInput =
    """
    19, 13, 30 @ -2,  1, -2
    18, 19, 22 @ -1, -1, -2
    20, 25, 34 @ -2, -2, -4
    12, 31, 28 @ -1, -2, -1
    20, 19, 15 @  1, -5, -3
    """;

    public static readonly string PuzzleInput = File.ReadAllText("input.txt");
    public static long Part1(string input, long lowerBound, long upperBound)
    {
        Hailstone[] stones = [..input.ReplaceLineEndings().Split(Environment.NewLine).Select(Hailstone.Parse)];
        long count = 0;
        for (int ix = 0; ix < stones.Length; ix++)
        {
            for (int jx = ix + 1; jx < stones.Length; jx++)
            {
                if(stones[ix].PathsIntersect(stones[jx], lowerBound, upperBound))
                {
                    count++;
                }
            }
        }
        return count;
    }

    public static long Part2(string input)
    {
        return 0;
    }
}

public record Vector3D(long X, long Y, long Z)
{
    public static Vector3D Parse(string input)
    {
        long[] pos = [..input.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)];
        return new Vector3D(pos[0], pos[1], pos[2]);
    }
}

public record Hailstone(Vector3D Start, Vector3D Velocity)
{
    public long A { get; } = Velocity.Y;
    public long B { get; } = -Velocity.X;
    public long C { get; } = Start.X * Velocity.Y - Start.Y * Velocity.X; 
    public static Hailstone Parse(string input)
    {
        // 19, 13, 30 @ -2,  1, -2
        string[] parts = input.Split(" @ ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return new Hailstone(Vector3D.Parse(parts[0]), Vector3D.Parse(parts[1]));
    }

    public (decimal X, decimal Y) IntersectsAt(Hailstone other)
    {
        decimal X = (C*other.B - other.C*B) / (decimal)(A*other.B - other.A*B);
        decimal Y = (other.C*A - C * other.A) / (decimal)(A*other.B - other.A*B);
        return (X, Y);
    }

    public bool PathsIntersect(Hailstone other, long lowerBound, long upperBound)
    {
        // a1*b2 == a2 * b1 (means parallel)
        if (A*other.B == other.A * B) 
        {   
            Console.WriteLine($"{this} is parallel to {other}");
            return false; 
        }
        (decimal X, decimal Y) = IntersectsAt(other);
        Console.WriteLine($"{this} intersects {other} @ {X:.###}, {Y:.###}");
        // Check bounds
        if (X < lowerBound || Y < lowerBound || X > upperBound || Y > upperBound) 
        { 
            Console.WriteLine($"{X:.###}, {Y:.##} is not within {lowerBound}, {upperBound}");
            return false; 
        }
        Console.WriteLine($"{X:.###}, {Y:.##} is within {lowerBound}, {upperBound}");
        // Intersects in the future?
        bool interesectsInFuture = IsFuture(X, Y) && other.IsFuture(X, Y);
        Console.WriteLine($"Intersects in future: {interesectsInFuture}");
        return interesectsInFuture;
    }

    protected bool IsFuture(decimal x, decimal y)
    {
        if (Velocity.X < 0 && Start.X < x) return false;
        if (Velocity.X > 0 && Start.X > x) return false;
        if (Velocity.Y < 0 && Start.Y < y) return false;
        if (Velocity.Y > 0 && Start.Y > y) return false;
        return true;
    }

    public bool IntersectsInFuture(decimal X, decimal Y)
    {
        return (X - Start.X)*Velocity.X >= 0 &&
               (Y - Start.Y)*Velocity.Y >= 0;
    }

    public override string ToString()
    {
        return $"Hailstone ({Start.X}, {Start.Y}) ~ ({Velocity.X}, {Velocity.Y})";
    }
}