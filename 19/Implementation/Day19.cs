using System.Text.RegularExpressions;

public class Day19 {

    public static long Part1(string input)
    {
        (Dictionary<string, Workflow> workflows, IEnumerable<MachinePart> parts) = Parse(input);
        return parts.Where(part => Workflow.Accept(part, workflows)).Select(part => part.Value).Sum();
    }

    public static long Part2(string input)
    {
        return 0;
    }

    public static (Dictionary<string, Workflow> Workflows, IEnumerable<MachinePart> Parts) Parse(string input)
    {
        string[] rows = input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
        Dictionary<string, Workflow> workflows = 
            rows.TakeWhile(row => row != string.Empty)
                .Select(Workflow.Parse)
                .ToDictionary(workflow => workflow.Label);
        IEnumerable<MachinePart> parts = rows.SkipWhile(row => row != string.Empty).Skip(1).Select(MachinePart.Parse);
        return (workflows, parts);
    }

}

public enum Rating : ushort
{
    X = 'x',
    M = 'm',
    A = 'a',
    S = 's',
}

public enum Comparator : ushort
{
    GT = '>',
    LT = '<',
}

public partial record Workflow(string Label, IEnumerable<ICondition> Conditions)
{
    public static Workflow Parse(string input)
    {
        // qs{s>3448:A,lnx}
        var groups = WorkFlowRegex().Match(input).Groups;
        IEnumerable<ICondition> conditions = groups["conditions"].Value.Split(",").Select(ICondition.Parse);
        return new Workflow(groups["label"].Value, conditions);
    }

    public static bool Accept(MachinePart part, Dictionary<string, Workflow> workflows)
    {
        string label = "in";
        HashSet<string> seen = new ();
        while(!seen.Contains(label))
        {
            seen.Add(label);
            Workflow flow = workflows[label];
            label = flow.Conditions.Where(condition => condition.Check(part)).First().Label;
            if (label == "R") { return false; }
            if (label == "A") { return true; }
        }
        throw new Exception($"Infinite loop detected for part: {part}");
    }

    [GeneratedRegex(@"(?<label>\w+){(?<conditions>[^}]+)}")]
    public static partial Regex WorkFlowRegex();
}

public interface ICondition
{
    public string Label { get; }
    public bool Check(MachinePart machinePart);

    public static ICondition Parse(string input)
    {
        if (!input.Contains(':')) { return new ElseCondition(input); }
        return IfCondition.Parse(input);
        
    }
}

public record ElseCondition(string Label) : ICondition
{
    public bool Check(MachinePart machinePart) => true;
}

public partial record IfCondition(Rating Rating, Comparator Comparator, int Value, string Label) : ICondition
{
    public bool Check(MachinePart machinePart)
    {
        return Comparator switch {
            Comparator.GT => machinePart[Rating] > Value,
            Comparator.LT => machinePart[Rating] < Value,
            _ => throw new Exception($"Invalid comparator {Comparator}")
        };
    }

    public static IfCondition Parse(string input)
    {
        var groups = IfConditionRegex().Match(input).Groups;
        Rating rating = (Rating)groups["rating"].Value[0];
        Comparator condition = (Comparator)groups["condition"].Value[0];
        int value = groups["value"].ToInt();
        string label = groups["label"].Value;
        return new IfCondition(rating, condition, value, label);
    }

    [GeneratedRegex(@"(?<rating>[xmas])(?<condition>[<>])(?<value>\d+):(?<label>\w+)")]
    public static partial Regex IfConditionRegex();

    
}

public partial record MachinePart(int X, int M, int A, int S)
{
    public long Value { get; } = X + M + A + S;
    public int this[Rating ix] => ix switch
    {
        Rating.X => X,
        Rating.M => M,
        Rating.A => A,
        Rating.S => S,
        _ => throw new IndexOutOfRangeException($"Invalid rating {ix}")
    };

    public static MachinePart Parse(string input)
    {
        // {x=787,m=2655,a=1222,s=2876}
        var groups = MachinePartRegex().Match(input).Groups;
        var x = groups["x"];
        return new MachinePart(groups["x"].ToInt(), groups["m"].ToInt(), groups["a"].ToInt(), groups["s"].ToInt());

    }
    [GeneratedRegex(@"{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)}")]
    public static partial Regex MachinePartRegex();
}

public static class Helpers
{
    public static int ToInt(this Group input) => int.Parse(input.Value);
}