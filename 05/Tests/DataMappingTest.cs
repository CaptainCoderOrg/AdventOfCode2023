namespace Tests;
using Shouldly;

public class DataMappingTest
{
    
    
    // seeds: 79 14 55 13

    // seed-to-soil map:
    // 50 98 2
    // 52 50 48
    [Theory]
    [InlineData(50, 98, 2, 98)]
    [InlineData(50, 98, 2, 99)]
    [InlineData(52, 50, 48, 50)]
    [InlineData(52, 50, 48, 51)]
    [InlineData(52, 50, 48, 96)]
    [InlineData(52, 50, 48, 97)]
    public void test_should_contain(long destStart, long sourceStart, long length, long toCheck)
    {
        DataMapping mapping = new DataMapping(destStart, sourceStart, length);

        mapping.Contains(toCheck).ShouldBeTrue();
    }

    [Theory]
    [InlineData(50, 98, 2, 97)]
    [InlineData(50, 98, 2, 100)]
    [InlineData(52, 50, 48, 49)]
    [InlineData(52, 50, 48, 48)]
    [InlineData(52, 50, 48, 98)]
    [InlineData(52, 50, 48, 99)]
    public void test_should_not_contain(long destStart, long sourceStart, long length, long toCheck)
    {
        DataMapping mapping = new DataMapping(destStart, sourceStart, length);

        mapping.Contains(toCheck).ShouldBeFalse();
    }

    [Fact]
    public void test_partition_lt_ct_gt()
    {
        // |81 ----------- 100| <- source
        //      Length 20
        //     |91 --- 95| <- DataMapping GreaterThan: | 96 -- 100 |
        //       Length 5

        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(81, 20);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBe(new NumberRange(81, 10));
        result.GreaterThan.ShouldBe(new NumberRange(96, 5));
        result.Contains.ShouldBe(new NumberRange(91, 5));
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_partition_lt_ct()
    {
        // |81 -- 93 | <- source
        //  Length 13
        //     |91 --- 95| <- DataMapping 
        
        // LessThan: | 81 -- 90 |
        // Contains: | 91 -- 93 |
        //             Length 3

        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(81, 13);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBe(new NumberRange(81, 10));
        result.Contains.ShouldBe(new NumberRange(91, 3));
        result.GreaterThan.ShouldBe(null);
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_partition_ct_gt()
    {
        //        |93 -- 100 | <- source
        //  Length 13
        //     |91 --- 95| <- DataMapping 
        // LessThan: null
        // Contains: | 93 -- 95 |
        // GreaterThan: | 96 -- 100 |
        // 
        //             Length 3

        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(93, 8);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBeNull();
        result.Contains.ShouldBe(new NumberRange(93, 3));
        result.GreaterThan.ShouldBe(new NumberRange(96, 5));
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_partition_lt()
    {
        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(50, 10);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBe(range);
        result.Contains.ShouldBeNull();
        result.GreaterThan.ShouldBeNull();
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_partition_gt()
    {
        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(500, 500);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBeNull();
        result.Contains.ShouldBeNull();
        result.GreaterThan.ShouldBe(range);
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_partition_ct()
    {
        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(92, 3);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBeNull();
        result.Contains.ShouldBe(range);
        result.GreaterThan.ShouldBeNull();
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_partition_ct_exact()
    {
        // Arrange, Act, Assert
        //Arrange
        NumberRange range = new NumberRange(91, 5);
        DataMapping mapping = new DataMapping(0, 91, 5);

        // Act
        PartitionResult result = mapping.Partition(range);

        // Assert 
        result.LessThan.ShouldBeNull();
        result.Contains.ShouldBe(range);
        result.GreaterThan.ShouldBeNull();
        
        // DataMapping mapping = new DataMapping(81, sourceStart, length);
    }

    [Fact]
    public void test_mapping()
    {
        DataMapping mapping = new DataMapping(71, 31, 20);
        NumberRange range = new NumberRange(1, 10);

        PartitionResult result = mapping.Partition(range);

        result.LessThan.ShouldBe(range);
        result.Contains.ShouldBeNull();
        result.GreaterThan.ShouldBeNull();

        // {DataMapping { DestStart = 71, SourceStart = 31, Length = 20 }}
        // {NumberRange (Start: 1, Length: 10)}
        // {PartitionResult { LessThan = NumberRange (Start: 1, Length: 30), 
        // Contains = null, 
        // GreaterThan = null }}

    }

    [Fact]
    public void test_soil_bug()
    {
        DataMapping mapping = new DataMapping(52, 50, 48);
        NumberRange range = new NumberRange(79, 14);

        PartitionResult result = mapping.Partition(range);

        result.LessThan.ShouldBeNull();
        result.Contains.ShouldBe(new NumberRange(79, 14));
        result.GreaterThan.ShouldBeNull();
        
        // {DMS (seed-to-soil map:): 
        // DataMapping { DestStart = 50, SourceStart = 98, Length = 2 }
        // DataMapping { DestStart = 52, SourceStart = 50, Length = 48 }}
        // {NumberRange (Start: 79, Length: 14)}
    }
}