public class Day11
{
    public static long Part1(string input)
    {
        string[] universe = input.Split(Environment.NewLine);
        (HashSet<int> emptyRows, HashSet<int> emptyCols) = FindEmpty(universe);
        int scale = 2;
        List<Position> positions = ExpandUniverse(universe, scale, emptyRows, emptyCols);
        IEnumerable<(Position first, Position second)> pairs = positions.Pairs();
        return pairs.Select(ManhattanDistance).Sum();
    }

    public static long Part2(string input)
    {
        string[] universe = input.Split(Environment.NewLine);
        (HashSet<int> emptyRows, HashSet<int> emptyCols) = FindEmpty(universe);
        int scale = 1_000_000;
        List<Position> positions = ExpandUniverse(universe, scale, emptyRows, emptyCols);
        IEnumerable<(Position first, Position second)> pairs = positions.Pairs();
        return pairs.Select(ManhattanDistance).Sum();
    }

    public static (HashSet<int> EmptyRows, HashSet<int> EmptyColumns) FindEmpty(string[] universe)
    {
        HashSet<int> emptyRows = [.. universe.Select((string row, int ix) => (row, ix))
                                            .Where(entry => entry.row.All(ch => ch == '.'))
                                            .Select(entry => entry.ix) ];
        HashSet<int> emptyCols = new();


        for (int col = 0; col < universe[0].Length; col++)
        {
            bool isEmpty = true;
            for (int row = 0; row < universe.Length; row++)
            {
                if (universe[row][col] != '.')
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty) { emptyCols.Add(col); }
        }
        return (emptyRows, emptyCols);
    }

    public static long ManhattanDistance((Position first, Position second) pair) =>
        Math.Abs(pair.first.Row - pair.second.Row) + Math.Abs(pair.first.Col - pair.second.Col);

    public static List<Position> ExpandUniverse(string[] universe, int scale, HashSet<int> emptyRows, HashSet<int> emptyCols)
    {
        List<Position> positions = new();
        int rowOffset = 0;
        for (int row = 0; row < universe.Length; row++)
        {
            if (emptyRows.Contains(row))
            {
                rowOffset += scale - 1;
                continue;
            }
            int colOffset = 0;
            for (int col = 0; col < universe[row].Length; col++)
            {
                if (emptyCols.Contains(col))
                {
                    colOffset += scale - 1;
                    continue;
                }
                if (universe[row][col] == '#')
                {
                    positions.Add(new Position(row + rowOffset, col + colOffset));
                }
            }
        }
        return positions;
    }


}

public static class HelperExtensions
{
    public static IEnumerable<(T First, T Second)> Pairs<T>(this List<T> list)
    {
        for (int ix = 0; ix < list.Count; ix++)
        {
            for (int jx = ix + 1; jx < list.Count; jx++)
            {
                yield return (list[ix], list[jx]);
            }
        }
    }
}