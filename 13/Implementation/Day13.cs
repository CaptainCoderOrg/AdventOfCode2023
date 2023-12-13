using System.Text;

public class Day13
{

    public static long Part1(string[] input)
    {
        IEnumerable<Grid> rawGrids = 
                string.Join(Environment.NewLine, input)
                .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(g => new Grid(g));
        
        int rows = rawGrids.Select(g => g.FindMirrorRow()).Sum();
        int cols = rawGrids.Select(g => g.FindMirrorColumn()).Sum();

        return rows*100 + cols;
    }

    public static long Part2(string[] input)
    {
        IEnumerable<Grid> rawGrids = 
                string.Join(Environment.NewLine, input)
                .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(g => new Grid(g));
        
        int rows = rawGrids.Select(g => g.FindSmudgedRow()).Sum();
        int cols = rawGrids.Select(g => g.FindSmudgedCol()).Sum();

        return rows*100 + cols;
    }
}

public record Grid(string RawGrid)
{
    public string[] Rows { get; } = RawGrid.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    public static int HammingDistance(string first, string second) => 
        first.Zip(second).Count(pair => pair.First != pair.Second);

    public int FindSmudgedRow()
    {
        for (int row = 0; row < Rows.Length; row++)
        {
            (Grid top, Grid bottom) = MirrorRows(row);
            if (HammingDistance(top.RawGrid, bottom.RawGrid) == 1) { return row + 1; }
        }
        return 0;
    }

    public int FindSmudgedCol() => Transpose().FindSmudgedRow();

    public int FindMirrorRow()
    {
        for (int row = 0; row < Rows.Length - 1; row++)
        {
            (Grid top, Grid bottom) = MirrorRows(row);
            if (top.RawGrid == bottom.RawGrid) { return row + 1; }
        }
        return 0;
    }

    public int FindMirrorColumn() => Transpose().FindMirrorRow();

    public Grid Transpose()
    {
        List<string> newRows = new();
        for (int col = 0; col < Rows[0].Length; col++)
        {
            StringBuilder builder = new();
            for (int row = 0; row < Rows.Length; row++)
            {
                builder.Append(Rows[row][col]);
            }
            newRows.Add(builder.ToString());
            builder.Clear();
        }
        return new Grid(string.Join(Environment.NewLine, newRows));
    }

    public (Grid Left, Grid Right) MirrorColumns(int col)
    {
        Grid transposed = Transpose();
        (Grid Top, Grid Bottom) = transposed.MirrorRows(col);
        return (Top.Transpose(), Bottom.Transpose());
    }


    public (Grid Top, Grid Bottom) MirrorRows(int row)
    {
        string[] top = Rows[..(row + 1)];
        string[] bottom = Rows[(row + 1)..];
        top = [.. top.TakeLast(bottom.Length)];
        bottom = [.. bottom.Take(top.Length).Reverse()];
        Grid topGrid = new Grid(string.Join(Environment.NewLine, top));
        Grid bottomGrid = new Grid(string.Join(Environment.NewLine, bottom));
        return (topGrid, bottomGrid);
    }


}