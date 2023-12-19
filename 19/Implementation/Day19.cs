using System.Text.RegularExpressions;

public class Day19 {

    public static long Part1(string input)
    {
        return 0;
    }

    public static long Part2(string input)
    {
        return 0;
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