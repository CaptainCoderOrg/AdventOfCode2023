using Shouldly;

namespace Tests;

public class GearRatiosTests
{
    [Fact]
    public void build_gear_map()
    {
        string[] lines = 
        """
        467..114..
        ...*......
        ..35..633.
        ......#...
        617*......
        .....+.58.
        ..592.....
        ......598.
        ...$.*....
        .664.598..
        """.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        HashSet<Position> positions = GearRatios.BuildGearMap(lines);

        positions.Count.ShouldBe(2);

    }
}