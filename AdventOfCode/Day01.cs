namespace AdventOfCode;

public static class Day01
{
    public static int Day => 1;
    public static string Part1(string filename)
    {
        string[] content = File.ReadAllLines(filename);
        return SumDigits(content).ToString();
    }

    public static string Part2(string filename)
    {
        string[] content = File.ReadAllLines(filename);
        for (int i = 0; i < content.Length; i++)
        {
            content[i] = StringToDigits(content[i]);
        }
        return SumDigits(content).ToString();
    }

    private static int SumDigits(string[] content)
    {
        int sum = 0;
        foreach (string line in content)
        {
            List<char> digits = GetInts(line);
            sum += (digits.First() - '0') * 10;
            sum += digits.Last() - '0';
        }
        return sum;
    }

    private static List<char> GetInts(string line)
    {
        List<char> result = [];
        foreach (char letter in line)
        {
            if (char.IsDigit(letter)) result.Add(letter);
        }
        return result;
    }

    private static string StringToDigits(string line)
    {
        int currentNumber = 1;
        string[] spelledDigits = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        foreach(string sd in spelledDigits)
        {
            // "one" => "o1e"; "oneight" => "o1e8t"
            line = line.Replace(sd, $"{sd.First()}{currentNumber}{sd.Last()}");
            currentNumber++;
        } 
        return line;
    }
}
