using System.Text;

namespace AdventOfCode;

public static class Utils
{
    public static string RemoveExtraWhitespaces(string str)
    {
        StringBuilder result = new();
        bool foundWhiteSpace = false;

        int start = 0;
        while(start < str.Length && str[start] == ' ') start++;

        for (int i = start; i < str.Length; i++)
        {
            if (str[i] != ' ')
            {
                if (foundWhiteSpace) result.Append(' ');
                result.Append(str[i]);
                foundWhiteSpace = false;
            }
            else foundWhiteSpace = true;
        }
        return result.ToString();
    }

    // https://en.wikipedia.org/wiki/Euclidean_algorithm#Implementations
    public static long GCD(long a, long b)
    {
        while (b is not 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // https://en.wikipedia.org/wiki/Least_common_multiple#Using_the_greatest_common_divisor
    public static long LCM(long a, long b)  => Math.Abs(a) * (Math.Abs(b) / GCD(a, b));

    public static long LCM(List<int> numbers)
    {
        if (numbers.Count == 0) return 0;
        long result = numbers.First();
        foreach (int number in numbers[1..])
        {
            result = LCM(result, number);
        }
        return result;
    }
}