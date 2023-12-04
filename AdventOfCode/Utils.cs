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
}