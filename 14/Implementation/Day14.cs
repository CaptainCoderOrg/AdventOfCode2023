using System.Text;

public class Day14
{
    public const char LooseRock = 'O';
    public const char StiffRock = '#';
    public const char NoRock    = '.';
    public const int Cycles = 1_000_000_000;
    public static long Part1(string[] input) => CalculateLoad(ShiftNorth(input));

    public static long Part2(string[] grid)
    {
        string[] original = grid.ToArray();
        Dictionary<string, int> map = new Dictionary<string, int>();
        int cycleCount = 0;
        do
        {
            map[ToKey(grid)] = cycleCount++;
            grid = Cycle(grid);            
        } while ( ! map.ContainsKey(ToKey(grid)));
        
        string key = ToKey(grid);
        int cyclesAt = map[key];
        int cycleLength = cycleCount - cyclesAt;
        int numCycles = (Cycles - cyclesAt) % cycleLength + cyclesAt;
        string[] lastGrid = Cycle(original, numCycles);

        return CalculateLoad(lastGrid);
    }

    public static string ToKey(string[] grid) => string.Join("", grid);
    public static string[] Cycle(string[] grid, int count)
    {
        for (int ix = 0; ix < count; ix++)
        {
            grid = Cycle(grid);
        }
        return grid;
    }
    public static string[] Cycle(string[] grid) => ShiftEast(ShiftSouth(ShiftWest(ShiftNorth(grid))));
    public static string[] Shift(string[] grid, Func<string[], string[]> transpose, Func<string, int, string> paddingFunction)
    {
        string[] transposed = transpose(grid);
        List<string> newRows = new ();
        foreach (string row in transposed)
        {
            string[] groups = row.Split(StiffRock);
            List<string> shiftedGroups = new();
            foreach (string group in groups)
            {
                int looseRocks = group.Count(ch => ch == LooseRock);
                shiftedGroups.Add(paddingFunction("".PadLeft(looseRocks, LooseRock), group.Length));
            }
            newRows.Add(string.Join(StiffRock, shiftedGroups));
        }
        return transpose(newRows.ToArray());
    }

    public static string[] ShiftNorth(string[] grid) => Shift(grid, AoCHelpers.Transpose, (s, l) => s.PadRight(l, NoRock));
    public static string[] ShiftEast(string[] grid) => Shift(grid, x => x, (s, l) => s.PadLeft(l, NoRock));
    public static string[] ShiftSouth(string[] grid) => Shift(grid, AoCHelpers.Transpose, (s, l) => s.PadLeft(l, NoRock));
    public static string[] ShiftWest(string[] grid) => Shift(grid, x => x, (s, l) => s.PadRight(l, NoRock));
    
    public static long CalculateLoad(string[] grid)
    {
        long total = 0;
        for (int row = 0; row < grid.Length; row++)
        {
            long baseLoad = grid[row].Count(ch => ch == LooseRock);
            int multiplier = grid.Length - row;
            total += baseLoad * multiplier;
        }
        return total;
    }
}



public static class AoCHelpers 
{
    public static string[] Transpose(this string[] input)
    {
        List<string> newRows = new();
        for (int col = 0; col < input[0].Length; col++)
        {
            StringBuilder builder = new();
            for (int row = 0; row < input.Length; row++)
            {
                builder.Append(input[row][col]);
            }
            newRows.Add(builder.ToString());
            builder.Clear();
        }
        return newRows.ToArray();
    }

    public static string[] SplitAoCInput(this string input)
    {
        return input.Trim()
                    .ReplaceLineEndings()
                    .Split(Environment.NewLine, StringSplitOptions.TrimEntries);
    }
}