namespace Tests;
using Shouldly;

public class DataMappingSetTest
{
    
    
    // seeds: 79 14 55 13

    // seed-to-soil map:
    // 50 98 2
    // 52 50 48
    [Fact]
    public void test_transform_seed_to_soil()
    {
        //   | 79 --- 92 |
        // |50     --     98|
        //       14
        NumberRange range = new NumberRange(79, 14);
        DataMappingSet set = new DataMappingSet
        (
            "seed-to-soil",
            new List<DataMapping>
            {
                new DataMapping(50, 98, 2),
                new DataMapping(52, 50, 48),
            }
        );

        List<NumberRange> ranges = set.Transform(range);
        ranges.Count.ShouldBe(1);
    }

    //    |1 --------------------------------------------- 100|
    //        Length: 100
    //             | 11 -- 20 |
    //    |1 --- 10| 51 -- 60 |31 ------------------------ 100| 
    [Fact]
    public void test_transform_one_mapping()
    {
        NumberRange range = new NumberRange(1, 100);
        DataMappingSet set = new DataMappingSet
        (
            "transform",
            new List<DataMapping>
            {
                new DataMapping(51, 11, 10),
            }
        );

        List<NumberRange> ranges = set.Transform(range);
        ranges.Count.ShouldBe(3);
        ranges.ShouldContain(new NumberRange(1, 10)); // 1 - 10
        ranges.ShouldContain(new NumberRange(51, 10)); // 51 - 60
        ranges.ShouldContain(new NumberRange(21, 80)); // 21 - 100
    }

    //    |1 --------------------------------------------- 100|
    //        Length: 100
    //             | 11 -- 20 | <- Mapping
    //    |1 --- 10| 51 -- 60 |21 ------------------------ 100| 
    //                                | 31 - 50 | <- Mapping
    //    |1 --- 10| 11 -- 20 |21 - 30| 31 - 50 | 51 -- 100   |
    //             | 51 -- 60 |       | 71 - 90 |
    [Fact]
    public void test_transform_many()
    {
        NumberRange range = new NumberRange(1, 100);
        DataMappingSet set = new DataMappingSet
        (
            "transform",
            new List<DataMapping>
            {
                new DataMapping(51, 11, 10),
                new DataMapping(71, 31, 20),
            }
        );

        List<NumberRange> ranges = set.Transform(range);
        ranges.Count.ShouldBe(5);
        ranges.ShouldContain(new NumberRange(1, 10)); // 1 - 10
        ranges.ShouldContain(new NumberRange(51, 10)); // 51 - 60
        ranges.ShouldContain(new NumberRange(21, 10)); // 21 - 30
        ranges.ShouldContain(new NumberRange(71, 20)); // 71 - 90
        ranges.ShouldContain(new NumberRange(51, 50)); // 51 - 100

    }

    [Fact]
    public void test_soil_bug()
    {
        DataMappingSet mapping = new DataMappingSet(
            "seed-to-soil",
            new List<DataMapping>()
            {
                new DataMapping(50, 98, 2),
                new DataMapping(52, 50, 48),
            }
        );
        NumberRange range = new NumberRange(79, 14);

        List<NumberRange> ranges = mapping.Transform(range);

        ranges.Count.ShouldBe(1);
        ranges.ShouldContain(new NumberRange(81, 14));
        
        // {DMS (seed-to-soil map:): 
        // DataMapping { DestStart = 50, SourceStart = 98, Length = 2 }
        // DataMapping { DestStart = 52, SourceStart = 50, Length = 48 }}
        // {NumberRange (Start: 79, Length: 14)}
    }
}