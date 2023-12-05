// See https://aka.ms/new-console-template for more information

string[] data = File.ReadAllText(args[0]).Split("\n\n", StringSplitOptions.TrimEntries);
// seeds: 79 14 55 13
long[] seeds = [.. data[0].Split(": ")[1].Split(" ").Select(long.Parse)];
List<DataMappingSet> dataMappingSets = [..data[1..].Select(DataMappingSet.Parse)];

// Console.WriteLine(string.Join(", ", seeds));
// Console.WriteLine(string.Join("\n\n", dataMappingSets));
Part1();

void Part1()
{
    List<long> locations = new ();
    foreach (long seed in seeds)
    {
        long transformed = seed;
        foreach (DataMappingSet set in dataMappingSets)
        {
            transformed = set.Transform(transformed);
        }
        locations.Add(transformed);
    }
    long min = locations.Min();
    Console.WriteLine($"Part 1: {min}");
}

record DataMappingSet(string Label, List<DataMapping> Maps)
{
    public long Transform(long source)
    {
        foreach (DataMapping mapping in Maps)
        {
            if (mapping.Contains(source))
            {
                return mapping.Transform(source);
            }
        }
        return source;
    }

    // seed-to-soil map:
    // 50 98 2
    // 52 50 48
    public static DataMappingSet Parse(string data)
    {
        var split = data.Split("\n");
        return new DataMappingSet(split[0], [..split[1..].Select(DataMapping.Parse)]);
    }
        
    public override string ToString()
    {
        return $"DMS ({Label}): \n{string.Join("\n", Maps)}";
    }
}
public record DataMapping(long DestStart, long SourceStart, long Length)
{
    public bool Contains(long source)
    {
        return source >= SourceStart && source < SourceStart + Length;
    }

    public long Transform(long source)
    {
        return source + (DestStart - SourceStart);
    }

    public static DataMapping Parse(string data)
    {
        // 50 98 2
        long[] parts = [.. data.Split(" ").Select(long.Parse)];
        return new DataMapping(parts[0], parts[1], parts[2]);
    }
}