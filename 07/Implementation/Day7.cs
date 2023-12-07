using System.Globalization;
using System.Security.Cryptography;

namespace Implementation;

public static class Day7
{

    public static long Part2(string input)
    {
        Hand[] hands = [.. input.Split("\n").Select(s => s.Replace('J', '*')).Select(Hand.Parse)];
        return CalculateWinnings(hands);
    }

    public static long Part1(string input)
    {
        Hand[] hands = [.. input.Split("\n").Select(Hand.Parse)];
        return CalculateWinnings(hands);
    }

    public static long CalculateWinnings(Hand[] hands)
    {
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
        var remaining = cards.Where(c => c.Rank > 1);
        int count = remaining.Count();
        if (count == 5) { return CalculateSingleHandType(cards); }
        // Get permutations of wild cards
        int wildCards = 5 - count;
        // [2, 2], [3, 3], [4, 4]
        IEnumerable<Card[]> substitutions = Card.All.Select(c => Enumerable.Repeat(c, wildCards).ToArray());
        HandType best = HandType.HighCard;
        foreach (Card[] substitution in substitutions)
        {
            Card[] p = [ .. remaining, .. substitution];
            HandType current = CalculateSingleHandType(p);
            best = (HandType) Math.Max((int)best, (int)current);
        }
        return best;
    }

    private static HandType CalculateSingleHandType(Card[] cards)
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

    public override string ToString()
    {
        return $"Hand: {string.Join("", Cards.Select(c => c.Symbol))}";
    }
}

public record Card(int Rank, char Symbol)
{
    public static Card[] All { get; } = new Card[]
    {
        new Card(2, '2'),
        new Card(3, '3'),
        new Card(4, '4'),
        new Card(5, '5'),
        new Card(6, '6'),
        new Card(7, '7'),
        new Card(8, '8'),
        new Card(9, '9'),
        new Card(10, 'T'),
        // new Card(11, 'J'),
        new Card(12, 'Q'),
        new Card(13, 'K'),
        new Card(14, 'A'),
    };

    public static Card Parse(char ch)
    {
        int rank = ch switch
        {
            '*' => 1,
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
        return new Card(rank, ch);
    }
}
