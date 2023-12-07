using System.Text;

namespace AdventOfCode;

public static class Day07
{
    public static int Day => 7;
    public static string Part1(string filename)
    {
        CamelCards camelCards = CamelCards.FromFile(filename);
        return camelCards.TotalWinnings().ToString();
    }

    public static string Part2(string filename)
    {
        CamelCards camelCards = CamelCards.FromFileWithJokers(filename);
        return camelCards.TotalWinnings().ToString();
    }
}

public class CamelCards
{
    private readonly List<Hand> _handList;

    private CamelCards(List<Hand> handList) => _handList = handList;

    public static CamelCards FromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);

        List<Hand> handList = new(lines.Length);
        foreach (string line in lines)
        {
            string[] handAndBid = line.Split(' ');
            string hand = handAndBid[0];
            int bid = int.Parse(handAndBid[1]);
            handList.Add(new Hand(hand, bid));
        }
        return new CamelCards(handList);
    }

    public static CamelCards FromFileWithJokers(string filename)
    {
        CamelCards result = FromFile(filename);
        result._handList.ForEach(h => h.ActivateJokers());
        return result;
    }

    public int TotalWinnings()
    {
        _handList.Sort();

        int result = 0;
        int rank = 1;

        foreach (Hand hand in _handList)
        {
            result += rank * hand.Bid;
            rank++;
        }
        return result;
    }
}

public class Hand : IComparable<Hand>
{
    private readonly int[] _hand;
    private HandType _type;
    private HandType _typeWithJokers;
    private bool _countJokers;
    public int Bid { get; }

    public Hand(string hand, int bid)
    {
        _hand = GetHand(hand);
        GetTypes(hand);
        _countJokers = false;
        Bid = bid;
    }

    public void ActivateJokers() => _countJokers = true;

    public int CompareTo(Hand? other)
    {
        if (other == null) return 1;

        if (_countJokers) return CompareToWithJokers(other);

        if (_type  != other._type)
            return _type.CompareTo(other._type);
        
        for (int i = 0; i < 5; i++)
        {
            int result = _hand[i].CompareTo(other._hand[i]);
            if (result != 0) return result;
        }
        return 0;
    }

    private int CompareToWithJokers(Hand other)
    {
        if (_typeWithJokers  != other._typeWithJokers)
            return _typeWithJokers.CompareTo(other._typeWithJokers);
        
        for (int i = 0; i < 5; i++)
        {
            int card = _hand[i];
            int otherCard = other._hand[i];
            // Joker == 11
            if (card == 11) card = 1;
            if (otherCard == 11) otherCard = 1;

            int result = card.CompareTo(otherCard);
            if (result != 0) return result;
        }
        return 0;
    }

    private static int[] GetHand(string hand)
    {
        int[] result = new int[5];
        for (int i = 0; i < 5; i++)
        {
            int card = hand[i] switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => int.Parse(hand[i].ToString()),
            };
            result[i] = card;
        }
        return result;
    }

    private void GetTypes(string hand)
    {
        Dictionary<char, int> cardCount = [];
        foreach (char card in hand)
        {
            if (cardCount.ContainsKey(card)) cardCount[card]++;
            else cardCount[card] = 1;
        }

        _type = HandTypeFromString(OrderedLabelString(cardCount));
        _typeWithJokers = HandTypeFromString(JokerLabelString(cardCount));
    }

    private static string JokerLabelString(Dictionary<char, int> cardCount)
    {
        if (cardCount.TryGetValue('J', out int jokers))
        {
            if (jokers == 5) return OrderedLabelString(cardCount);

            cardCount.Remove('J');
            char max = cardCount.First().Key;
            foreach (KeyValuePair<char, int> cc in cardCount)
            {
                if (cc.Value > cardCount[max]) max = cc.Key;
            }
            cardCount[max] += jokers;
        }
        return OrderedLabelString(cardCount);
    }

    private static string OrderedLabelString(Dictionary<char, int> cardCount)
    {
        StringBuilder cardCounts = new();
        foreach (KeyValuePair<char, int> cc in cardCount) cardCounts.Append(cc.Value);

        char[] labelArr = cardCounts.ToString().ToCharArray();
        Array.Sort(labelArr);
        return new string(labelArr);
    }

    private static HandType HandTypeFromString(string str) => str switch 
    {
        "5" => HandType.FiveOfAKind,
        "14" => HandType.FourOfAKind,
        "23" => HandType.FullHouse,
        "113" => HandType.ThreeOfAKind,
        "122" => HandType.TwoPair,
        "1112" => HandType.OnePair,
        _ => HandType.HighCard,
    };
}

public enum HandType { HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind }