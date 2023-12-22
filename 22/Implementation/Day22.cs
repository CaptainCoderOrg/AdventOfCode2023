using System.Diagnostics;
using AoCHelpers;
using CaptainCoder.MathUtils;

public class Day22
{
    public static long Part1(string input)
    {
        (List<Brick> bricks, Dictionary<int, HashSet<Position3D>> tower) = Tower.Fall([..Brick.ParseAll(input)]);
        return Tower.CheckDisintegrate(bricks, tower);
    }

    public static long Part2(string input)
    {
        return 0;
    }
}

public record struct Position3D(int X, int Y, int Z)
{
    public static implicit operator Position3D((int x, int y, int z) tuple) => new(tuple.x, tuple.y, tuple.z);

    public Position3D Drop(int n) => (X, Y, Z - n);
    public Position3D Lift(int n) => (X, Y, Z + n);

    public static IEnumerable<Position3D> Occupied(Position3D Lower, Position3D Upper)
    {
        for (int x = Lower.X; x <= Upper.X; x++)
        {
            for (int y = Lower.Y; y <= Upper.Y; y++)
            {
                for (int z = Lower.Z; z <= Upper.Z; z++)
                {
                    yield return new Position3D(x, y, z);
                }
            }
        }
    }
}

[DebuggerDisplay("{Lower.X}, {Lower.Y}, {Lower.Z} ~ {Upper.X}, {Upper.Y}, {Upper.Z}")]
public record Brick(Position3D Lower, Position3D Upper) : IComparable<Brick>
{
    // Terribly inefficient
    public HashSet<Position3D> Occupied() => [.. Position3D.Occupied(Lower, Upper)]; //_occupied;
    // private HashSet<Position3D> _occupied = [.. Position3D.Occupied(Lower, Upper)];
    public Brick Drop(int n) => new Brick(Lower.Drop(n), Upper.Drop(n));
    public Brick Lift(int n) => new Brick(Lower.Lift(n), Upper.Lift(n));
    public static IEnumerable<Brick> ParseAll(string input) =>
        input.ReplaceLineEndings().Split(Environment.NewLine).Select(Parse);
    public static Brick Parse(string row)
    {
        string[] parts = row.Split("~");
        int[] left = [.. parts[0].Split(",").Select(x => int.Parse(x))];
        int[] right = [.. parts[1].Split(",").Select(x => int.Parse(x))];
        return new Brick((left[0], left[1], left[2]), (right[0], right[1], right[2]));
    }
    public int CompareTo(Brick? other) => Lower.Z.CompareTo(other?.Lower.Z);
}

public class Tower
{
    public static int CheckDisintegrate(List<Brick> bricks, Dictionary<int, HashSet<Position3D>> tower)
    {
        bool IsStableWithBrickRemoved(Brick toCheck, Brick removed)
        {
            
            Brick ifMoved = toCheck.Drop(1);
            if (ifMoved.Lower.Z <= 0) { return true; }
            foreach (Position3D pos in ifMoved.Occupied())
            {
                HashSet<Position3D> inZPosition = tower.GetValueOrDefault(pos.Z, []).ToHashSet();
                inZPosition.ExceptWith(removed.Occupied());
                if (inZPosition.Contains(pos)) { return true; }
            }
            return false;
        }
        string labels = string.Join("", Enumerable.Range((int)'A', bricks.Count).Select(x => (char)x));
        HashSet<Brick> removable = [.. bricks];
        foreach (Brick toCheck in bricks)
        {
            foreach (Brick toRemove in bricks)
            {
                if (toCheck == toRemove) { continue; }
                if (!IsStableWithBrickRemoved(toCheck, toRemove))
                {
                    removable.Remove(toRemove);
                }
            }
        }
        return removable.Count;
    }

    public static (List<Brick> bricks, Dictionary<int, HashSet<Position3D>> tower) Fall(List<Brick> bricks)
    {
        bricks = [.. bricks.Order()];
        List<Brick> newBricks = new();
        // Given a z index, which positions are occupied there
        Dictionary<int, HashSet<Position3D>> occupied = new();

        // Because the bricks are sorted, we know we are moving the bottom
        // most brick
        foreach (Brick brick in bricks)
        {
            Brick falling = brick;
            while (falling.Lower.Z > 0 && !CollidesAt(falling.Lower.Z, falling)) // While brick can fall
            {
                falling = falling.Drop(1);
            }
            falling = falling.Drop(-1);
            SetBrick(falling);
            // Set Block in occupied
            newBricks.Add(falling);
        }

        return (newBricks, occupied);

        void SetBrick(Brick toSet)
        {
            foreach (Position3D position in toSet.Occupied())
            {
                if (!occupied.TryGetValue(position.Z, out HashSet<Position3D>? inZ))
                {
                    inZ = new HashSet<Position3D>();
                    occupied[position.Z] = inZ;
                }
                inZ.Add(position);
            }
        }

        bool CollidesAt(int z, Brick toCheck)
        {
            HashSet<Position3D> positionsAtHeight = occupied.GetValueOrDefault(z, []);
            return toCheck.Occupied().Intersect(positionsAtHeight).Any();
        }
    }

}