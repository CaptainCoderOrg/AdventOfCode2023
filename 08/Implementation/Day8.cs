using System.Numerics;
using System.Text.RegularExpressions;
using System.Transactions;

public partial class Day8
{

    public static long Part1(string input)
    {
        // List<string> is similar to C++ std::vector and Java's ArrayList
        // LLR
        string[] parts = input.Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string steps = parts[0];
        Dictionary<string, (string Left, string Right)> mapping = ParseMapping(parts[1]);
        long stepCount = StepsToEnd("AAA", steps, 0, mapping, (current) => current is "ZZZ").Steps;
        return stepCount;
    }

    public static long Part2(string input)
    {
        string[] parts = input.Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string steps = parts[0];
        Dictionary<string, (string Left, string Right)> mapping = ParseMapping(parts[1]);
        string[] starts = [.. mapping.Keys.Where(key => key.EndsWith("A"))];
        Dictionary<string, List<StepEntry>> entries = new();
        List<long> cycleSize = new();
        foreach (string start in starts)
        {
            cycleSize.Add(GetStepsToEnds(steps, start, mapping)[0].Steps);
        }
        
        return cycleSize.Aggregate(LCM);
    }

    public static long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);
    public static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

    public static List<StepEntry> GetStepsToEnds(
        string steps, 
        string start,  
        Dictionary<string, (string Left, string Right)> mapping)
    {
        long totalSteps = 0;
        List<StepEntry> PossibleEnds = new List<StepEntry>();
        
        while (true && PossibleEnds.Count < 1_000)
        {
            StepEntry entry = StepsToEnd(start, steps, totalSteps, mapping, (current) => current.EndsWith('Z'));
            if (PossibleEnds.Contains(entry)) { return PossibleEnds; }
            PossibleEnds.Add(entry);
            start = entry.End;
            totalSteps += entry.Steps;

            // After we get to the end. We need to take 1 step.
            // Then start from there.
        }
        throw new Exception("Oh dear goodness... someone save me.");
    }

    public static StepEntry StepsToEnd(
        string start, 
        string steps, 
        long totalSteps, 
        Dictionary<string, (string Left, string Right)> mapping,
        Predicate<string> finished
        )
    {
        long stepCount = 0;
        string currentLocation = start;
        
        do
        {
            char direction = steps[(int)totalSteps % steps.Length];
            stepCount++;
            totalSteps++;
            (string left, string right) = mapping[currentLocation];
            if (direction == 'L')
            {
                currentLocation = left;
            }
            else
            {
                currentLocation = right;
            }
        }
        while ((!(finished(currentLocation)) && stepCount < 1_000_000));

        return new StepEntry(start, currentLocation, stepCount);
    }

    // AAA = (BBB, BBB)
    // 
    public static Dictionary<string, (string Left, string Right)> ParseMapping(string mappings)
    {
        string[] lines = mappings.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, (string Left, string Right)> mapping = new();
        foreach (string line in lines)
        {
            Match matches = EntryRegex().Match(line);
            string key = matches.Groups["key"].Value;
            string left = matches.Groups["left"].Value;
            string right = matches.Groups["right"].Value;
            mapping[key] = (left, right);
        }
        return mapping;
    }

    //(?<key>[A-Z]{3}) = \((?<from>[A-Z]{3}), (?<to>[A-Z]{3})\)
    [GeneratedRegex(@"(?<key>.{3}) = \((?<left>.{3}), (?<right>.{3})\)")]
    private static partial Regex EntryRegex();

    public record StepEntry(string Start, string End, long Steps);

}

// Credit: steve7411
internal static class MathUtils {
    public static T LCM<T>(T a, T b) where T : INumber<T> => T.Abs(a * b) / GCD(a, b);

    public static T GCD<T>(T a, T b) where T : INumber<T> =>
        b == T.Zero ? a : GCD(b, a % b);
}