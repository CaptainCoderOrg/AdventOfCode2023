namespace AoCHelpers;

public enum Direction
{
    North,
    East,
    South,
    West
}

public static class DirectionExtensions
{
    public static IEnumerable<Direction> Cardinals => [ Direction.North, Direction.East, Direction.South, Direction.West ];
    public static Position Step(this Position position, Direction dir)
    {
        return dir switch
        {
            Direction.North => position + (-1, 0),
            Direction.East => position + (0, 1),
            Direction.West => position + (0, -1),
            Direction.South => position + (1, 0),
            _ => throw new Exception($"Invalid direction {dir}"),            
        };
    }
}