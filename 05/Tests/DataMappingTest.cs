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
}