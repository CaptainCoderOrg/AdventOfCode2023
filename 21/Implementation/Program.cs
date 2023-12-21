// #r "21.sln"
using AoCHelpers;

// Map map = Map.Parse(Day21.SampleInput);
Map map = Map.Parse(Day21.PuzzleInput);
// map.MarkAndPrintPositions(map.Start, 66);
// map.MarkAndPrintPositions(map.Start, 30, 3);
int radius = 5;
int steps = 500;
HashSet<Position> marks = map.MarkPositions(map.Start, steps, radius);
map.PrintMarks(marks, radius);
// map.PrintSubGrid(1, 0, marks);


Dictionary<string, List<Position>> patterns = new ();

for (int row = -radius + 1; row < radius; row++ )
{
    for (int col = -radius + 1; col < radius; col++)
    {
        Position subGrid = (row, col);
        string subGridPattern = map.SubGridToString(row, col, marks);
        if (!patterns.TryGetValue(subGridPattern, out List<Position>? found))
        {
            found = new List<Position>();
            patterns[subGridPattern] = found;
        }
        found.Add(subGrid);
    }
}

foreach ((string pattern, List<Position> subGrid) in patterns)
{
    Console.WriteLine($"Match: {string.Join(", ", subGrid.Select(r => $"({r.Row}, {r.Col})"))}");
}