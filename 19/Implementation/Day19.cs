using System.Text.RegularExpressions;

public class Day19 {

    public static long Part1(string input)
    {
        (Dictionary<string, Workflow> workflows, IEnumerable<MachinePart> parts) = Parse(input);
        return parts.Where(part => Workflow.Accept(part, workflows)).Select(part => part.Value).Sum();
    }

    public static long Part2(string input, int min = 1, int max = 4000)
    {
        (Dictionary<string, Workflow> workflows, IEnumerable<MachinePart> _) = Parse(input);
        PartRange all = new PartRange(
            X: new RatingRange(min, max),
            M: new RatingRange(min, max),
            A: new RatingRange(min, max),
            S: new RatingRange(min, max)
        );
        Queue<(PartRange range, string label)> queue = new ();
        queue.Enqueue((all, "in"));
        HashSet<PartRange> accepted = new ();
        while (queue.TryDequeue(out var next))
        {
            (PartRange range, string label) = next;
            if (label == "A")
            {
                accepted.Add(range);
                continue;
            }
            Workflow workflow = workflows[label];
            var splits = range.Split(workflow).ToArray();
            foreach ((PartRange, string label) split in splits)
            {
                if (split.label == "R") { continue; }
                queue.Enqueue(split);
            }
        }
        return accepted.Sum(r => r.Count);
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

public record RatingRange(long Low, long High)
{
    public static RatingRange Empty { get; } = new RatingRange(long.MaxValue, long.MinValue);
    public long Count => Math.Max(0, High - Low + 1);
}
public record PartRange(RatingRange X, RatingRange M, RatingRange A, RatingRange S)
{
    public static PartRange Empty { get; } = new PartRange(RatingRange.Empty, RatingRange.Empty, RatingRange.Empty, RatingRange.Empty);
    public RatingRange this[Rating ix] => ix switch
    {
        Rating.X => X,
        Rating.M => M,
        Rating.A => A,
        Rating.S => S,
        _ => throw new IndexOutOfRangeException($"Invalid rating {ix}"),
    };

    public long Count => X.Count * M.Count * A.Count * S.Count;

    public PartRange Set(Rating rating, RatingRange range) => rating switch
    {
        Rating.X => this with { X = range },
        Rating.M => this with { M = range },
        Rating.A => this with { A = range },
        Rating.S => this with { S = range },
        _ => throw new IndexOutOfRangeException($"Invalid rating {rating}"),
    };

    public IEnumerable<(PartRange, string NextWorkflowLabel)> Split(Workflow flow)
    {
        PartRange currentRange = this;
        foreach (ICondition condition in flow.Conditions)
        {
            (PartRange included, currentRange) = condition.Split(currentRange);
            yield return (included, condition.Label);
        }
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
    public (PartRange Included, PartRange Excluded) Split(PartRange partRange);

    public static ICondition Parse(string input)
    {
        if (!input.Contains(':')) { return new ElseCondition(input); }
        return IfCondition.Parse(input);
        
    }
}

public record ElseCondition(string Label) : ICondition
{
    public bool Check(MachinePart machinePart) => true;
    public (PartRange Included, PartRange Excluded) Split(PartRange partRange) => (partRange, PartRange.Empty);
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

    public (PartRange Included, PartRange Excluded) Split(PartRange partRange)
    {
        RatingRange range = partRange[Rating];
        if (Comparator is Comparator.LT)
        {   
            long newUpper = Math.Min(Value - 1, range.High);
            PartRange included = partRange.Set(Rating, new RatingRange(range.Low, newUpper));
            long newLower = Math.Max(range.Low, Value);
            PartRange excluded = partRange.Set(Rating, new RatingRange(newLower, range.High));
            return (included, excluded);
        }
        else // is GT
        {
            long newUpper = Math.Min(Value, range.High);
            PartRange excluded = partRange.Set(Rating, new RatingRange(range.Low, newUpper));
            long newLower = Math.Max(range.Low, Value + 1);
            PartRange included = partRange.Set(Rating, new RatingRange(newLower, range.High));
            return (included, excluded);
        }
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