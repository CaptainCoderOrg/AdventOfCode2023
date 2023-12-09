public class Day9
{
    public static long Part1(string input)
    {
        long result = input
                        .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(ParseLine)
                        .Select(Extrapolate)
                        .Select(ls => ls[^1])
                        .Sum();
        return result;
    }

    public static long Part2(string input)
    {
        long result = input
                        .Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(ParseLine)
                        .Select(Extrapolate)
                        .Select(ls => ls[0])
                        .Sum();
        return result;
    }

    // Takes a list and adds the next element in the sequence to the end and beginning.
    // For convenience, returns the modified list
    public static List<long> Extrapolate(List<long> sequence)
    {
        if (sequence.All(x => x == 0)) 
        { 
            sequence.Insert(0, 0);
            sequence.Add(0);
            return sequence; 
        }

        List<long> nextSequence = new ();
        // IEnumerable<(long first, long second)> zipped = 
        foreach((long first, long second) in sequence.Zip(sequence[1..]))
        {
            nextSequence.Add(second - first);
        }
        
        Extrapolate(nextSequence);
        long deltaFirst = nextSequence[0];
        sequence.Insert(0, sequence[0] - deltaFirst);
        long deltaLast = nextSequence[^1];
        sequence.Add(sequence[^1] + deltaLast);
        return sequence;
    }

    public static List<long> ParseLine(string line) => [.. line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse)];
}