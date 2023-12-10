// See https://aka.ms/new-console-template for more information
string SampleInputSimple = 
    """
    .....
    .S-7.
    .|.|.
    .L-J.
    .....
    """.Trim().ReplaceLineEndings("\n");

Console.WriteLine(SampleInputSimple.PadGrid('.'));
// return;
Maze maze = Maze.Parse(SampleInputSimple);

string expanded = string.Join("\n", maze.ExpandMap()).Trim();

char[][] toFill = [.. maze.ExpandMap().Select(row => row.ToCharArray())];
Day10.FloodFill(toFill, 0, 0);
string filled = string.Join("\n", toFill.Select(row => new string(row)));
Console.WriteLine(filled);

// string expected =
// """
// .........
// .........            
// ..S - 7..
            
// . | . | .
            
// . L - J .
            
// . . . . .
// """.Trim();
// Console.WriteLine("Expanded:");
// Console.WriteLine(expanded);
// Console.WriteLine("Expected:");
// Console.WriteLine(expected);