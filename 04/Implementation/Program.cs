// See https://aka.ms/new-console-template for more information

// [1, 1, 2, 5, 7, 7, 8]
// { 1, 2, 5, 7, 8 }
// string filename = args[0];
Card[] cards = GetInput().Select(Card.Parse).ToArray();
Part1();
Part2();

string[] GetInput()
{
    // return 
    // """
    // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
    // Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
    // Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
    // Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
    // Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
    // Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
    // """.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
    string filename = args[0];
    return File.ReadAllLines(filename);
}

void Part1()
{
    int sum = 0;
    foreach (Card card in cards)
    {
        int matches = card.Winning.Intersect(card.Have).Count();
        if (matches == 0) { continue; }
        // 2 ^ (matches - 1)
        // 0b0000001 === 1
        // 0b0000010 === 2
        // 0b0000100 === 4
        // 2 ^ (matches - 1)

        sum += 1 << (matches - 1);
    }
    Console.WriteLine($"Part 1: {sum}");
}

void Part2()
{
    int[] cardCount = new int[cards.Length];

    // Do a thing
    for (int ix = 0; ix < cards.Length; ix++)
    {
        cardCount[ix]++;
        Card card = cards[ix];
        int matches = card.Winning.Intersect(card.Have).Count();
        int multiplier = cardCount[ix];
        for (int jx = ix + 1; jx <= ix + matches && jx < cards.Length; jx++)
        {
            cardCount[jx] += multiplier;
        }
    }

    Console.WriteLine($"Part 2: {cardCount.Sum()}");
}  


record Card(HashSet<int> Winning, HashSet<int> Have)
{
    public static Card Parse(string line)
    {
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        //       ^
        string numbers = line.Split(':', StringSplitOptions.TrimEntries)[1];
        // string numbers
        // 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        //                ^
        string[] parts = numbers.Split('|', StringSplitOptions.TrimEntries);
        string winning = parts[0]; // 41 48 83 86 17 
        string have = parts[1]; // 83 86  6 31 17  9 48 53
        return new Card(ParseNumbers(winning), ParseNumbers(have));
    }

    private static HashSet<int> ParseNumbers(string nums)
    {
        return nums
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToHashSet();
    }

    public override string ToString()
    {
        return $"Card - Winning: {string.Join(", ", Winning)}, Have: {string.Join(", ", Have)} ";
    }
}