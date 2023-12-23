// #r "21.sln"
using AoCHelpers;

// Map map = Map.Parse(Day21.SampleInput);
Map map = Map.Parse(Day21.PuzzleInput);
// map.MarkAndPrintPositions(map.Start, 66);
// map.MarkAndPrintPositions(map.Start, 30, 3);

long elfSteps = 26_501_365;
// Total Radius is 202_300 (even, therefore there are 14 possible grids)
long elfRadius = (elfSteps - (map.Rows / 2)) / map.Rows;
Console.WriteLine($"Elf Radius: {elfRadius}");

//          v  v  v  v  v   v
// Radius   4  6  8  10 12  14
// 0 [919]. 3, 5, 7, 9, 11, 13 Occurrences
//          4  6  8  10 12  14
// 2 [930]. 3, 5, 7, 9, 11, 13
//          4  6  8  10 12  14
// 9 [940]. 3, 5, 7, 9, 11, 13
//           4  6  8  10 12  14
// 12 [933]. 3, 5, 7, 9, 11, 13`
long sum = 0;

sum += 919 * (elfRadius - 1 );
sum += 930 * (elfRadius - 1);
sum += 940 * (elfRadius - 1);
sum += 933 * (elfRadius - 1);

// 1 [5513].  1, 1, 1, 1, 1, 1
// 7 [5511].  1, 1, 1, 1, 1, 1
// 8 [5505].  1, 1, 1, 1, 1, 1
// 13 [5503]. 1, 1, 1, 1, 1, 1
sum += 5513 + 5511 + 5505 + 5503;

//           4  6  8  10 12  14
// 3 [6422]. 2, 4, 6, 8, 10, 12
//           4  6  8  10 12  14
// 5 [6406]. 2, 4, 6, 8, 10, 12
//            4  6  8  10 12  14
// 10 [6404]. 2, 4, 6, 8, 10, 12
//            4  6  8  10 12  14
// 11 [6414]. 2, 4, 6, 8, 10, 12
sum += 6422 * (elfRadius - 2);
sum += 6406 * (elfRadius - 2);;
sum += 6404 * (elfRadius - 2);;
sum += 6414 * (elfRadius - 2);;

//           4   6   8  10   12   14
// 4 [7354]. 9, 25, 49, 81, 121, 169
sum += 7354 * (elfRadius - 1) * (elfRadius - 1);
//           4  6    8  10   12   14
// 6 [7315]. 4, 16, 36, 64, 100, 144
sum += 7315 * (elfRadius - 2) * (elfRadius - 2);
Console.WriteLine(sum);
Environment.Exit(0);


Dictionary<int, List<PatternEntry>> entries = new();
Dictionary<string, int> ids = new();
Dictionary<int, int> counts = new ();

for (int radius = 4; radius <= 16; radius += 2)
// for (int radius = 4; radius <= 14; radius += 2)
{
    Dictionary<string, List<Position>> patterns = FindPatterns(radius);
    foreach ((string pattern, List<Position> positions) in patterns)
    {
        int id = ID(pattern);
        int count = positions.Count;
        List<PatternEntry> entryList = Entries(id);
        entryList.Add(new PatternEntry(radius, count));
    }
}

foreach ((int id, List<PatternEntry> entry) in entries)
{
    Console.WriteLine($"{id} [{counts[id]}]. {string.Join(", ", entry.Select(e => e.Count))}");
}


List<PatternEntry> Entries(int id)
{
    if (!entries.TryGetValue(id, out List<PatternEntry>? entryList))
    {
        entryList = new List<PatternEntry>();
        entries[id] = entryList;
    }
    return entryList;
}

int ID(string pattern)
{
    if (!ids.TryGetValue(pattern, out int id))
    {
        id = ids.Count;
        ids[pattern] = id;
        counts[id] = pattern.Where(ch => ch == 'O').Count();
    }
    return id;
}

// Dictionary<string, List<PatternEntry>> patternCounts = new();

// Console.WriteLine($"Total Patterns: {patterns.Keys.Count}");
// foreach ((string pattern, List<Position> subGrid) in patterns)
// {
//     Console.WriteLine($"Match: {subGrid.Count}"); // {string.Join(", ", subGrid.Select(r => $"({r.Row}, {r.Col})"))}");
// }

// Console.WriteLine();

Dictionary<string, List<Position>> FindPatterns(int radius)
{
    int steps = (map.Rows / 2) + map.Rows * (radius - 1);
    HashSet<Position> marks = map.MarkPositions(map.Start, steps, radius);
    // Console.WriteLine(map.SubGridToString(-1, -1, marks));
    string emptyPattern = map.SubGridToString(0, 0, new());

    Dictionary<string, List<Position>> patterns = new();

    for (int row = -radius + 1; row < radius; row++)
    {
        for (int col = -radius + 1; col < radius; col++)
        {
            Position subGrid = (row, col);
            string subGridPattern = map.SubGridToString(row, col, marks);
            if (subGridPattern == emptyPattern) { continue; } // Skip grids we did not enter
            if (!patterns.TryGetValue(subGridPattern, out List<Position>? found))
            {
                found = new List<Position>();
                patterns[subGridPattern] = found;
            }
            found.Add(subGrid);
        }
    }
    return patterns;
}

public record PatternEntry(int Radius, int Count)
{
    public string Pretty()
    {
        return $"R: {Radius}, C: {Count}";
    }
}