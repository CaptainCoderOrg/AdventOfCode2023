﻿// See https://aka.ms/new-console-template for more information

using System.Threading.Tasks.Dataflow;
using AoCHelpers;

string Input =
    """
    R 6 (#70c710)
    D 5 (#0dc571)
    L 2 (#5713f0)
    D 2 (#d2c081)
    R 2 (#59c680)
    D 2 (#411b91)
    L 5 (#8ceee2)
    U 2 (#caa173)
    L 1 (#1b58a2)
    U 2 (#caa171)
    R 2 (#7807d2)
    U 3 (#a77fa3)
    L 2 (#015232)
    U 2 (#7a21e3)
    """.ReplaceLineEndings().Trim();

// Input = File.ReadAllText("input.txt").ReplaceLineEndings().Trim();
// DateTime start, end;
// Console.Write("Building Grid: ");
// start = DateTime.Now;
// Outline grid = Outline.ParseTrue(Input);
// end = DateTime.Now;
// Console.WriteLine($"Finished {(end - start).Milliseconds} Milliseconds");
// Console.Write("Flood filling: ");
// start = DateTime.Now;
// HashSet<Position> positions = grid.FloodFill((1, 1));
// end = DateTime.Now;
// Console.WriteLine($"Finished {(end - start).Milliseconds} Milliseconds");
// Console.WriteLine("Counting: ");
// start = DateTime.Now;
// Console.WriteLine(positions.Count);
// end = DateTime.Now;
// Console.WriteLine($"Finished {(end - start).Milliseconds} Milliseconds");
// Console.WriteLine(string.Join(", ", grid.Holes));
// grid.FloodFill((1, 1)).PrettyPrintPositionSet();
