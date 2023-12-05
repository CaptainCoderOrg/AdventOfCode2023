// See https://aka.ms/new-console-template for more information

using System.Globalization;

string[] data = GetData().Split("\n\n", StringSplitOptions.TrimEntries);
// seeds: 79 14 55 13
long[] seeds = [.. data[0].Split(": ")[1].Split(" ").Select(long.Parse)];
List<NumberRange> seedRanges = NumberRange.Parse(data[0].Split(":")[1]);
List<DataMappingSet> dataMappingSets = [.. data[1..].Select(DataMappingSet.Parse)];
// Console.WriteLine(string.Join("\n", seedRanges));
// Console.WriteLine(string.Join(", ", seeds));
// Console.WriteLine(string.Join("\n\n", dataMappingSets));
// Part1();
Part2();

string GetData()
{
    string sampleInput = 
    @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4".Trim();
    // return sampleInput;
    return File.ReadAllText(args[0]);
}

void Part1()
{
    List<long> locations = new();
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

void Part2()
{
    List<NumberRange> ranges = seedRanges.ToList();
    foreach (DataMappingSet set in dataMappingSets)
    {
        List<NumberRange> nextRanges = new ();
        foreach (NumberRange range in ranges)
        {
            nextRanges.AddRange(set.Transform(range));
        }
        ranges = nextRanges;
    }
    long minimum = ranges.Select(range => range.Start).Min();
    Console.WriteLine($"Part 2: {minimum}");
}

public record DataMappingSet(string Label, List<DataMapping> Maps)
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

    public List<NumberRange> Transform(NumberRange toTransform)
    {
        List<NumberRange> notTransformed = new() { toTransform };
        List<NumberRange> transformed = new();
        foreach (DataMapping map in Maps)
        {
            List<NumberRange> skipped = new();
            foreach (NumberRange range in notTransformed)
            {
                PartitionResult result = map.Partition(range);
                if (result.Contains is not null) { transformed.Add(map.Transform(result.Contains)); }

                if (result.LessThan is not null) { skipped.Add(result.LessThan); }

                if (result.GreaterThan is not null) { skipped.Add(result.GreaterThan); }
            }
            notTransformed = skipped;
        }
        transformed.AddRange(notTransformed);
        return transformed;
    }

    // seed-to-soil map:
    // 50 98 2
    // 52 50 48
    public static DataMappingSet Parse(string data)
    {
        var split = data.Split("\n");
        return new DataMappingSet(split[0], [.. split[1..].Select(DataMapping.Parse)]);
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

    // Assume contained CAN be transformed
    public NumberRange Transform(NumberRange contained) => new NumberRange(Transform(contained.Start), contained.Length);

    public PartitionResult Partition(NumberRange range)
    {
        long rangeEnd = range.Start + range.Length - 1; // 100
        long mappingEnd = SourceStart + Length - 1; // 95

        if (rangeEnd < SourceStart)
        {
            return new PartitionResult(range, null, null);
        }
        if (range.Start > mappingEnd)
        {
            return new PartitionResult(null, null, range);
        }
        if (range.Start >= SourceStart && rangeEnd <= mappingEnd)
        {
            return new PartitionResult(null, range, null);
        }

        // Case with multiple partitions
        NumberRange? LessThan = null;
        if (range.Start < SourceStart)
        {
            LessThan = new NumberRange(range.Start, SourceStart - range.Start);
        }
        // |81 ----------- 100| <- source
        //      Length 20
        //     |91 --- 95| <- DataMapping GreaterThan: | 96 -- 100 |
        //       Length 5
        //         |92-----93|

        NumberRange? GreaterThan = null;
        if (rangeEnd > mappingEnd)
        {
            GreaterThan = new NumberRange(mappingEnd + 1, rangeEnd - mappingEnd);
        }

        // Overlapping Range
        NumberRange? Contained = null;
        long containedStart = SourceStart;
        if (range.Start > SourceStart)
        {
            containedStart = range.Start;
        }
        long containedEnd = mappingEnd;
        if (rangeEnd < mappingEnd)
        {
            containedEnd = rangeEnd;
        }
        long containedLength = containedEnd - containedStart + 1;
        if (containedLength > 0)
        {
            Contained = new NumberRange(containedStart, containedLength);
        }

        return new PartitionResult(LessThan, Contained, GreaterThan);
    }

    public static DataMapping Parse(string data)
    {
        // 50 98 2
        long[] parts = [.. data.Split(" ").Select(long.Parse)];
        return new DataMapping(parts[0], parts[1], parts[2]);
    }
}

public record PartitionResult(NumberRange? LessThan, NumberRange? Contains, NumberRange? GreaterThan);

public record NumberRange(long Start, long Length)
{
    public static List<NumberRange> Parse(string data)
    {
        // 79 14 55 13
        // Queue<long> nums = new Queue<long>(data.Split(" ").Select(long.Parse));
        // List<NumberRange> ranges = new();
        // while (nums.Count > 0)
        // {
        //     long start = nums.Dequeue();
        //     long length = nums.Dequeue();
        //     ranges.Add(new NumberRange(start, length));
        // }
        // return ranges;
        return [.. data.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).Chunk(2).Select(nums => new NumberRange(nums[0], nums[1]))];
    }

    public override string ToString()
    {
        return $"NumberRange (Start: {Start}, Length: {Length})";
    }
}