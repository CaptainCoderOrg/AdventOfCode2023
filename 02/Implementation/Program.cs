// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var games = File.ReadAllLines(args[0]).Select(Game.Parse).ToArray();
Part1(games);
Part2(games);

void Part1(Game[] games)
{
    int result = games.Where(IsValidGame).Select(game => game.Id).Sum();
    Console.WriteLine($"Part 1: {result}");
}

void Part2(Game[] games)
{
    int result = games.Select(game => MaxSet(game.Sets)).Select(set => set.Red * set.Green * set.Blue).Sum();
    Console.WriteLine($"Part 2: {result}");
}

DiceSet MaxSet(IEnumerable<DiceSet> sets)
{
    int red = 0;
    int green = 0;
    int blue = 0;
    foreach (DiceSet set in sets)
    {
        red = Math.Max(set.Red, red);
        green = Math.Max(set.Green, green);
        blue = Math.Max(set.Blue, blue);
    }
    return new DiceSet(red, green, blue);
}

bool IsValidGame(Game game) => game.Sets.All(IsValidSet);

bool IsValidSet(DiceSet set) => set.Red <= 12 && set.Green <= 13 && set.Blue <= 14;


record Game(int Id, List<DiceSet> Sets)
{
    public static Game Parse(string line)
    {
        var parts = line.Split(":", StringSplitOptions.TrimEntries);
        Regex gameRegex = new Regex(@"Game (?<id>\d+)", RegexOptions.IgnoreCase);
        Match gameMatch = gameRegex.Match(parts[0]);
        Group id = gameMatch.Groups["id"];
        
        return new Game(int.Parse(id.Value), DiceSet.ParseSets(parts[1]));
    }
}
record DiceSet(int Red, int Green, int Blue)
{
    public static List<DiceSet> ParseSets(string line)
    {
        var rawSets = line.Split(";", StringSplitOptions.TrimEntries);
        return rawSets.Select(ParseSet).ToList();
    }

    public static DiceSet ParseSet(string line)
    {
        // 3 green, 4 blue, 1 red
        Dictionary<string, int> colors = new ()
        {
            {"red", 0},
            {"green", 0},
            {"blue", 0},
        };
        string[] parts = line.Split(",", StringSplitOptions.TrimEntries);
        Regex setRegex = new Regex(@"(?<number>\d+) (?<color>red|green|blue)", RegexOptions.IgnoreCase);
        foreach (string part in parts)
        {
            Match gameMatch = setRegex.Match(part);
            Group number = gameMatch.Groups["number"];
            Group color = gameMatch.Groups["color"];
            colors[color.Value] = int.Parse(number.Value);
        }
        return new DiceSet(colors["red"], colors["green"], colors["blue"]);
    }
}