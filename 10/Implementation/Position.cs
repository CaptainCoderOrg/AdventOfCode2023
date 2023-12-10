
/// <summary>
/// A discrete readonly position defined by a row and column. For convenience,
/// this struct supports an implicit cast from (int, int) tuples and the <see
/// cref="MutablePosition"/> struct.
/// </summary>
[Serializable]
public readonly record struct Position(int Row, int Col)
{
    /// <summary>
    /// Allows (int, int) tuples to be used anywhere a Position can be used. Be careful not to 
    /// do this when using a position as a key in a HashSet or Dictionary.
    /// </summary>
    public static implicit operator Position((int row, int col) pair) => new(pair.row, pair.col);


    /// <summary>
    /// Scales both the row and column by the specified amount
    /// </summary>
    public static Position operator *(Position a, int b) => new(a.Row * b, a.Col * b);
    /// <summary>
    /// Scales both the row and column by the specified amount
    /// </summary>
    public static Position operator *(int a, Position b) => new(a * b.Row, a * b.Col);

    /// <summary>
    /// Sums the row and column values together
    /// </summary>
    public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Col + b.Col);

    /// <summary>
    /// Calculates the simple difference in row and column values
    /// </summary>
    public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Col - b.Col);

    /// <summary>
    /// Takes the smaller row and column of each position. For example: Position.Min((0, 5), (-1, 6)) => (-1, 5);
    /// </summary>
    public static Position Min(Position a, Position b) => new(Math.Min(a.Row, b.Row), Math.Min(a.Col, b.Col));

    /// <summary>
    /// Takes the larger row and column of each position. For example: Position.Max((0, 5), (-1, 6)) => (0, 6);
    /// </summary>
    public static Position Max(Position a, Position b) => new(Math.Max(a.Row, b.Row), Math.Max(a.Col, b.Col));

    /// <summary>
    /// Finds the smallest row and column of all position.
    /// </summary>
    public static Position Min(IEnumerable<Position> positions) => positions.Aggregate(Min);

    /// <summary>
    /// Finds the largest row and column of all positions
    /// </summary>
    public static Position Max(IEnumerable<Position> positions) => positions.Aggregate(Max);

    /// <summary>
    /// Finds both the min and max position from all provided positions
    /// </summary>
    public static (Position Min, Position Max) FindMinMax((Position, Position) seed, IEnumerable<Position> positions)
    {
        var step = ((Position min, Position max) acc, Position pos) => (Min(pos, acc.min), Max(pos, acc.max));
        var minMax = positions.Aggregate(seed, step);
        return minMax;
    }

    /// <summary>
    /// Iterates over all specified positions and returns a pair of Positions containing the minimum and maximum positions
    /// </summary>
    public static (Position Min, Position Max) FindMinMax(IEnumerable<Position> positions) =>
    FindMinMax(((int.MaxValue, int.MaxValue), (int.MinValue, int.MinValue)), positions);

    /// <summary>
    /// Finds both the min and max position from all provided positions
    /// </summary>
    public static (Position Min, Position Max) FindMinMax(params IEnumerable<Position>[] enumerables)
    {
        Position min = (int.MaxValue, int.MaxValue);
        Position max = (int.MinValue, int.MinValue);
        foreach (IEnumerable<Position> positions in enumerables)
        {
            (min, max) = FindMinMax((min, max), positions);
        }
        return (min, max);
    }
}