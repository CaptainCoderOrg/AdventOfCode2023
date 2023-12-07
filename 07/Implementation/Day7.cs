using System.Security.Cryptography;

namespace Implementation;

public static class Day7
{
    public static long Part1(string input)
    {
        Hand[] hands = [.. input.Split("\n").Select(Hand.Parse)];
        // TODO: Sort Hands
        Hand[] sorted = [..
            hands.OrderBy(hand => hand.HandType)
                 .ThenBy(hand => hand.Cards[0].Rank)
                 .ThenBy(hand => hand.Cards[1].Rank)
                 .ThenBy(hand => hand.Cards[2].Rank)
                 .ThenBy(hand => hand.Cards[3].Rank)
                 .ThenBy(hand => hand.Cards[4].Rank)
        ];
        long result = 0;
        for (long ix = 0; ix < sorted.Length; ix++)
        {
            Hand hand = sorted[ix];
            result += hand.Bid * (ix + 1);
        }
        return result;
    }

    private static int HandStrength(Hand hand)
    {
        throw new NotImplementedException();
    }
}

public enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6,
}

public record Hand(Card[] Cards, long Bid)
{
    public HandType HandType { get; } = CalculateHandType(Cards);
    public static Hand Parse(string input)
    {
        string[] parts = input.Split(new char[0], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Card[] cards = [.. parts[0].Select(Card.Parse)];
        long bid = long.Parse(parts[1]);
        return new Hand(cards, bid);
    }

    public static HandType CalculateHandType(Card[] cards)
    {
        int[] counts = [..
            cards
                .GroupBy(c => c)
                .Select(g => g.Count())
                .OrderDescending()
        ];
        return counts switch {
            [5] => HandType.FiveOfAKind,
            [4, 1] => HandType.FourOfAKind,
            [3, 2] => HandType.FullHouse,
            [3, 1, 1] => HandType.ThreeOfAKind,
            [2, 2, 1] => HandType.TwoPair,
            [2, 1, 1, 1] => HandType.OnePair,
            _ => HandType.HighCard,
        };
    }
}

public record Card(int Rank)
{
    public static Card Parse(char ch)
    {
        int rank = ch switch
        {
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => throw new InvalidOperationException($"Invalid card rank: '{ch}'")
        };
        return new Card(rank);
    }
}
