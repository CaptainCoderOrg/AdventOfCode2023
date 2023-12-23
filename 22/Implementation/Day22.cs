using System.Diagnostics;

public class Day22
{
    public static long Part1(string input) => Tower.BuildTower(Brick.ParseAll(input)).Removeable().Count();

    public static long Part2(string input)
    {
        Tower tower = Tower.BuildTower(Brick.ParseAll(input));
        HashSet<Brick> removeable = [..tower.Removeable()];
        return tower.Bricks.Where(b => !removeable.Contains(b)).Select(tower.SimulateRemoval).Sum();;
    }
}

[DebuggerDisplay("({X}, {Y}, {Z})")]
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
    public HashSet<Position3D> Occupied() => Position3D.Occupied(Lower, Upper).ToHashSet();
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

    private Dictionary<Brick, HashSet<Brick>> _supports = new ();
    private Dictionary<Brick, HashSet<Brick>> _supporting = new ();
    private Dictionary<Position3D, Brick> _positionToBrick = new ();
    private List<Brick> _bricks = new ();
    private int _drops = 0;
    public IEnumerable<Brick> Bricks => [.._bricks];

    public static Tower BuildTower(IEnumerable<Brick> bricks)
    {
        Tower tower = new ();
        IEnumerable<Brick> sorted = bricks.Order();

        foreach (Brick brick in sorted)
        {
            Brick dropped = tower.SimulateDrop(brick);
            // During construction, if we dropped this brick count it
            if (brick != dropped) { tower._drops++; }
            tower.Add(dropped);
        }
        return tower;
    }

    public long SimulateRemoval(Brick toRemove)
    {
        HashSet<Brick> copy = _bricks.ToHashSet();
        copy.Remove(toRemove);
         // Build a new tower with the remaining bricks
        Tower newTower = BuildTower(copy);
        return newTower._drops;
    }

    public IEnumerable<Brick> Removeable()
    {
        // A brick can be safely removed IF the it does not support any bricks
        // OR if ALL of the bricks that it supports have at least 2 supporting bricks
        foreach (Brick brick in _bricks)
        {
            HashSet<Brick> supporting = Supporting(brick);
            if (supporting.Count == 0 || supporting.All(other => Supports(other).Count >= 2))
            {
                yield return brick;
            }
        }
    }

    public void Add(Brick toAdd)
    {
        HashSet<Position3D> newPositions = toAdd.Occupied();
        foreach (Position3D position in newPositions)
        {
            _positionToBrick[position] = toAdd;
        }
        _bricks.Add(toAdd);
    }

    public Brick SimulateDrop(Brick toDrop)
    {
        Brick dropped = toDrop.Drop(1);
        foreach (Position3D below in dropped.Occupied())
        {
            // If any position below is occupied, we can't drop
            if (IsOccupied(below))
            {
                return toDrop;
            }
        }
        // Otherwise we keep dropping
        return SimulateDrop(dropped);
    }

    /// <summary>
    /// Given a brick, which bricks support it?
    /// </summary>
    public HashSet<Brick> Supports(Brick brick)
    {
        if (!_supports.TryGetValue(brick, out HashSet<Brick>? supports))
        {
            supports = new ();
            foreach(Position3D below in brick.Drop(1).Occupied())
            {
                if (!_positionToBrick.ContainsKey(below)) { continue; }
                Brick other = _positionToBrick[below];
                if (other == brick) { continue; }
                supports.Add(other);
            }
            _supports[brick] = supports;
        }
        return supports;
    }

    /// <summary>
    /// Given a brick, which bricks does it support?
    /// </summary>
    public HashSet<Brick> Supporting(Brick brick)
    {
        if (!_supporting.TryGetValue(brick, out HashSet<Brick>? supporting))
        {
            supporting = new ();
            var aboves = brick.Lift(1).Occupied();
            foreach(Position3D above in aboves) //brick.Lift(1).Occupied())
            {
                if (!_positionToBrick.ContainsKey(above)) { continue; }
                Brick other = _positionToBrick[above];
                if (other == brick) { continue; }
                supporting.Add(other);
            }
            _supporting[brick] = supporting;
        }
        return supporting;
    }

    /// <summary>
    /// A position is occupied if there is a block there OR if the position is under ground.
    /// </summary>
    public bool IsOccupied(Position3D position) => position.Z <= 0 || _positionToBrick.ContainsKey(position);

}