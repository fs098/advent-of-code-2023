namespace AdventOfCode;

public static class Day09
{
    public static int Day => 9;
    public static string Part1(string filename) => ResultFromFile(filename, x => x.SolvePart1());

    public static string Part2(string filename) => ResultFromFile(filename, x => x.SolvePart2());

    private static string ResultFromFile(string filename, Func<OASISHistory, int> solver)
    {
        string[] lines = File.ReadAllLines(filename);
        int result = 0;
        foreach (string line in lines)
        {
            result += solver(OASISHistory.FromLine(line));
        }
        return result.ToString();
    }
}

public class OASISHistory
{
    private readonly List<int> _lastNumbers;
    private readonly List<int> _firstNumbers;

    private OASISHistory(List<int> numberList)
    {
        _lastNumbers = [];
        _firstNumbers = [];
        while (!IsOver(numberList)) numberList = UpdateHistory(numberList);
    }

    public static OASISHistory FromLine(string line)
    {
        string[] numbers = line.Split(' ');
        List<int> numberList = new (numbers.Length);
        foreach (string number in numbers)
        {
            numberList.Add(int.Parse(number));
        }
        return new OASISHistory(numberList);
    }

    public int SolvePart1() => _lastNumbers.Sum();

    public int SolvePart2() => ExtrapolatePreviousValue(_firstNumbers);

    private List<int> UpdateHistory(List<int> numberList)
    {
        _lastNumbers.Add(numberList.Last());
        _firstNumbers.Add(numberList.First());

        List<int> newList = new(numberList.Count - 1);
        for (int i = 0; i < numberList.Count - 1; i++)
        {
            newList.Add(numberList[i + 1] - numberList[i]);
        }
        return newList;
    }

    private static int ExtrapolatePreviousValue(List<int> nums)
    {
        for (int i = nums.Count-1; i > 0; i--)
        {
            nums[i-1] -= nums[i];
        }
        return nums.First();
    }

    private static bool IsOver(List<int> numberList)
    {
        foreach (int number in numberList)
        {
            if (number is not 0) return false;
        }
        return true;
    }
}