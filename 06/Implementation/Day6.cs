using System.Runtime.CompilerServices;

namespace Implementation;

public static class Day6
{
    private static StringSplitOptions TrimAndRemove = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
    public static string SolvePart1(string input)
    {
        // Time:      7  15   30
        // Split(":")
        // "Time", "      7  15   30"
        // Trim() => "7  15   30"
        // Distance:  9  40  200 
        string[] lines = input.Split("\n", TrimAndRemove);
        int[] times = ParseLine(lines[0]);
        int[] distances = ParseLine(lines[1]);
        List<int> allSolutions = new();
        foreach ((int time, int distance) in times.Zip(distances))
        {
            int solutions = 0;
            for (int x = 0; x <= time; x++)
            {
                if (CalculateDistance(x, time, distance) < 0)
                {
                    solutions++;
                }
            }
            allSolutions.Add(solutions);
        }
        int solution = allSolutions.Aggregate(1, (int x, int y) => x * y);
        return solution.ToString();
    }

    public static string SolvePart2(string input)
    {
        string[] lines = input.Split("\n", TrimAndRemove);
        long time = ParseLinePart2(lines[0]);
        long distance = ParseLinePart2(lines[1]);
        int solutions = 0;
        for (long x = 0; x <= time; x++)
        {
            if (CalculateDistance(x, time, distance) < 0)
            {
                solutions++;
            }
        }
        return solutions.ToString();
    }

    public static int CalculateDistance(int x, int time, int distance) => x * x - time * x + distance;
    public static long CalculateDistance(long x, long time, long distance) => x * x - time * x + distance;

    public static long ParseLinePart2(string line) => long.Parse(string.Join("", ParseLine(line)));

    public static int[] ParseLine(string line) =>
        // line.Split(":", TrimAndRemove)[1].Split(new char[0], TrimAndRemove).Select(int.Parse).ToArray();
        [.. line.Split(":", TrimAndRemove)[1].Split(new char[0], TrimAndRemove).Select(int.Parse)];
}
