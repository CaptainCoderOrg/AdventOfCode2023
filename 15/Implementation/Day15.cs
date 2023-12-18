public class Day15
{
    public static long Part1(string input)
    {
        return input.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(Hash).Sum();
    }

    public static long Part2(string input)
    {
        List<LensEntry>[] HASHMAP = new List<LensEntry>[256];
        IEnumerable<Instruction> instructions = input.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                                     .Select(Instruction.Parse);
        foreach (Instruction instruction in instructions)
        {
            instruction.Apply(HASHMAP);
        }
        IEnumerable<(List<LensEntry> box, int boxNumber)> boxes = HASHMAP
                                                            .Select((entry, ix) => (entry, ix + 1))
                                                            .Where(pair => pair.entry is not null);
        long result = 0;
        foreach (var (box, boxNumber) in boxes)
        {
            for (int jx = 0; jx < box.Count; jx++)
            {
                result += boxNumber * (jx + 1) * box[jx].FocalLength;
            }
        }

        return result;
    }

    public static void ProcessInstruction(List<LensEntry>[] hashMap, Instruction instruction)
    {
        
    }

    /*
    Determine the ASCII code for the current character of the string.
    Increase the current value by the ASCII code you just determined.
    Set the current value to itself multiplied by 17.
    Set the current value to the remainder of dividing itself by 256.
    */
    public static int Hash(string input)
    {
        int hash = 0;
        foreach (char ch in input)
        {
            hash += ch;
            hash *= 17;
            hash %= 256;
        }
        return hash;
    }
}

public abstract record Instruction
{
    public abstract int Hash { get; }
    public abstract void Apply(List<LensEntry>[] hashMap);
    public static Instruction Parse(string input)
    {
        return input switch
        {
            [.. string label, '-'] => new Remove(label),
            [.. string label, '=', char digit] => new Insert(label, digit - '0'),
            _ => throw new Exception($"Could not parse input '{input}'"),
        };
    }
}
public record Remove(string Label) : Instruction
{
    public override int Hash { get; } = Day15.Hash(Label);
    public override void Apply(List<LensEntry>[] hashMap) => hashMap[Hash]?.RemoveAll(entry => entry.Label == Label);
}
public record Insert(string Label, int FocalLength) : Instruction
{
    public override int Hash { get; } = Day15.Hash(Label);
    public override void Apply(List<LensEntry>[] hashMap)
    {
        LensEntry newEntry = new LensEntry(Label, FocalLength);
        // hashMap[Hash] = hashMap[Hash] != null ? hashMap[Hash] : new List<LensEntry>();
        // hashMap[Hash] = hashMap[Hash] ?? new List<LensEntry>();
        var bucket = hashMap[Hash] ??= new List<LensEntry>();
        int ix = bucket.FindIndex(entry => entry.Label == Label);
        if (ix == -1) {
            bucket.Add(newEntry);
        }
        else
        {
            bucket[ix] = newEntry;
        }        
    }
}
public record LensEntry(string Label, int FocalLength);