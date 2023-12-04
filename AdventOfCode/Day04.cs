namespace AdventOfCode;

public static class Day04
{
    public static int Day => 4;
    public static string Part1(string filename)
    {
        Scratchcards scratchcards = new (ParseInput(File.ReadAllLines(filename)));
        return scratchcards.SumPoints().ToString();
    }

    public static string Part2(string filename)
    {
        Scratchcards scratchcards = new (ParseInput(File.ReadAllLines(filename)));
        return scratchcards.SumCopies().ToString();
    }

    private static List<Scratchcard> ParseInput(string[] lines)
    {
        List<Scratchcard> result = new (lines.Length);
        foreach(string line in lines) result.Add(ParseLine(line));
        return result;
    }

    private static Scratchcard ParseLine(string line)
    {
        // "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"
        line = Utils.RemoveExtraWhitespaces(line);
        string[] idAndNumbers = line.Split(": ");

        // "Card 1"
        string strId = idAndNumbers[0].Split(' ')[1];
        int id = int.Parse(strId);

        // "41 48 83 86 17 | 83 86  6 31 17  9 48 53"
        string[] numbers = idAndNumbers[1].Split(" | ");
        string[] strWinningNumbers = numbers[0].Split(' ');
        string[] strMyNumbers = numbers[1].Split(' ');

        HashSet<int> winningNumbers = [];
        foreach (string number in strWinningNumbers)
        {
            winningNumbers.Add(int.Parse(number));
        }

        List<int> myNumbers = [];
        foreach (string number in strMyNumbers)
        {
            myNumbers.Add(int.Parse(number));
        }

        return new(id, winningNumbers, myNumbers);
    }
}

public class Scratchcards(List<Scratchcard> scratchcards)
{
    private readonly List<Scratchcard> _scratchcards = scratchcards;
    private readonly Dictionary<int, int> _numberOfCopies = [];
    private bool _numberOfCopiesInitialized = false;

    public int SumPoints()
    {
        int sum = 0;
        foreach (Scratchcard scratchcard in _scratchcards)
        {
            sum += scratchcard.PointsWorth();
        }
        return sum;
    }

    public int SumCopies()
    {
        InitNumberOfCopies();
        int sum = 0;
        foreach (KeyValuePair<int, int> entry in _numberOfCopies)
        {
            sum += entry.Value;
        }
        return sum;
    }

    private void InitNumberOfCopies()
    {
        if (_numberOfCopiesInitialized) return;

        for (int i = 1; i <= _scratchcards.Last().Id; i++) _numberOfCopies[i] = 1;

        foreach (Scratchcard scratchcard in _scratchcards)
        {
            int copies = _numberOfCopies[scratchcard.Id];

            int start = scratchcard.Id + 1;
            int end = scratchcard.Id + scratchcard.MatchingNumbers();
            for (int i = start; i <= end; i++)
            {   
                _numberOfCopies[i] += copies;
            }
        }
        _numberOfCopiesInitialized = true;
    }
}

public class Scratchcard(int id, HashSet<int> winningNumbers, List<int> myNumbers)
{
    public int Id { get; } = id;
    private readonly List<int> _myNumbers = myNumbers;
    private readonly HashSet<int> _winningNumbers = winningNumbers;

    public int PointsWorth()
    {
        int result = 0;
        foreach (int number in _myNumbers)
        {
            if (_winningNumbers.Contains(number))
            {
                if (result == 0) result++;
                else result *= 2;
            }
        }
        return result;
    }

    public int MatchingNumbers()
    {
        int result = 0;
        foreach (int number in _myNumbers)
        {
            if (_winningNumbers.Contains(number)) result++;
        }
        return result;
    }
}
