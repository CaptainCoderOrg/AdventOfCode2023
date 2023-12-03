// See https://aka.ms/new-console-template for more information

using System.Text;

string[] lines = File.ReadAllLines(args[0]);

Dictionary<Position, int> numbers = BuildNumbers();
// (0, 0) => 467
// (0, 5) => 1146
Dictionary<Position, NumberData> numberData = BuildNumberData();
// (0, 0) => 467
// (0, 1) => 467
// (0, 2) => 467
Dictionary<Position, char> symbols;

Part1();
Part2();

void Part1()
{
    symbols = BuildSymbolsPart1();
    int sum = 0;
    foreach ((Position position, int number) in numbers)
    {
        IEnumerable<Position> numPosition = GetNumberPositions(position);
        if(numPosition.Any(IsAdjacentToSymbol))
        {
            sum += number;
        }
    }
    Console.WriteLine($"Part 1: {sum}");
}

void Part2()
{
    HashSet<Position> gearMap = GearRatios.BuildGearMap(lines);
    int sum = 0;
    foreach (Position position in gearMap)
    {
        List<int> numbers = GetAdjacentNumbers(position).ToList();
        if (numbers.Count != 2){ continue; }
        sum += numbers[0] * numbers[1];
    }
    Console.WriteLine($"Part 2: {sum}");
}

IEnumerable<int> GetAdjacentNumbers(Position pos)
{
    Position[] toCheck =  {
        new Position(pos.Row - 1, pos.Col -1), new Position(pos.Row - 1, pos.Col), new Position(pos.Row - 1, pos.Col  + 1),  
        new Position(pos.Row    , pos.Col -1),                                     new Position(pos.Row    , pos.Col  + 1),  
        new Position(pos.Row + 1, pos.Col -1), new Position(pos.Row + 1, pos.Col), new Position(pos.Row + 1, pos.Col  + 1),  
    };
    HashSet<NumberData> dataSet = new HashSet<NumberData>();
    foreach (Position position in toCheck)
    {
        if (numberData.TryGetValue(position, out NumberData data))
        {
            dataSet.Add(data);
        }
    }
    return dataSet.Select(data => data.Number);
}


bool IsAdjacentToSymbol(Position pos)
{
    Position[] ToCheck =  {
        new Position(pos.Row - 1, pos.Col -1), new Position(pos.Row - 1, pos.Col), new Position(pos.Row - 1, pos.Col  + 1),  
        new Position(pos.Row    , pos.Col -1),                                     new Position(pos.Row    , pos.Col  + 1),  
        new Position(pos.Row + 1, pos.Col -1), new Position(pos.Row + 1, pos.Col), new Position(pos.Row + 1, pos.Col  + 1),  
    };
    foreach (Position position in ToCheck)
    {
        if (symbols.ContainsKey(position)) { return true; }
    }
    return false;
}

IEnumerable<Position> GetNumberPositions(Position pos)
{
    if (!numbers.ContainsKey(pos)) { return new Position[0]; }
    int lengthOfNumber = numbers[pos].ToString().Length;
    return Enumerable.Range(pos.Col, lengthOfNumber).Select(col => new Position(pos.Row, col));
}

Dictionary<Position, char> BuildSymbolsPart1()
{
    Dictionary<Position, char> symbols = new ();
    for (int row = 0; row < lines.Length; row++)
    {
        string line = lines[row];
        for (int col = 0; col < line.Length; col++)
        {
            char ch = line[col];
            if (char.IsDigit(ch) || ch == '.')
            {
                continue;
            }
            symbols[new Position(row, col)] = ch;
        }
    }
    return symbols;
}


Dictionary<Position, int> BuildNumbers()
{
    Dictionary<Position, int> numbers = new ();
    for (int row = 0; row < lines.Length; row++)
    {
        string line = lines[row];
        for (int col = 0; col < line.Length; col++)
        {
            char ch = line[col];
            if (char.IsDigit(ch))
            {
                Position pos = new (row, col);
                StringBuilder number = new ();
                while (col < line.Length && char.IsDigit(line[col]))
                {
                    number.Append(line[col]);
                    col++;
                }
                numbers[pos] = int.Parse(number.ToString());
            }
        }
    }
    return numbers;
}

Dictionary<Position, NumberData> BuildNumberData()
{
    Dictionary<Position, NumberData> numbers = new ();
    for (int row = 0; row < lines.Length; row++)
    {
        string line = lines[row];
        for (int col = 0; col < line.Length; col++)
        {
            char ch = line[col];
            if (char.IsDigit(ch))
            {
                Position initialPosition = new (row, col);
                HashSet<Position> positions = new HashSet<Position>();
                
                StringBuilder numberBuilder = new ();
                while (col < line.Length && char.IsDigit(line[col]))
                {
                    Position pos = new (row, col);
                    positions.Add(pos);
                    numberBuilder.Append(line[col]);
                    col++;
                }

                int number = int.Parse(numberBuilder.ToString());
                NumberData data = new NumberData(number, initialPosition);
                foreach (Position p in positions)
                {
                    numbers[p] = data;
                }
            }
        }
    }
    return numbers;
}



public record struct Position(int Row, int Col);
public record struct NumberData(int Number, Position Pos);