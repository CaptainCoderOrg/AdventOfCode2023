public static class GearRatios
{
    public static HashSet<Position> BuildGearMap(string[] lines)
    {
        HashSet<Position> gears = new ();
        for (int row = 0; row < lines.Length; row++)
        {
            string line = lines[row];
            for (int col = 0; col < line.Length; col++)
            {
                char ch = line[col];
                if (ch == '*')
                {
                    gears.Add(new Position(row, col));
                }            
            }
        }
        return gears;
    }
}