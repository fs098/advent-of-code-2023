namespace AdventOfCode;

public static class Day06
{
    public static int Day => 6;
    public static string Part1(string filename)
    {
        List<Race> races = ParseInput(File.ReadAllLines(filename));
        int marginOfError = 1;
        foreach (Race race in races)
        {
            marginOfError *= race.NumberOfWaysToWin();
        }
        return marginOfError.ToString();
    }

    public static string Part2(string filename)
    {
        Race race = ParseBigRace(File.ReadAllLines(filename));
        return race.NumberOfWaysToWin().ToString();
    }

    public static List<Race> ParseInput(string[] lines)
    {
        List<Race> result = [];
        string[] times = Utils.RemoveExtraWhitespaces(lines[0]).Split(": ")[1].Split(' ');
        string[] distances = Utils.RemoveExtraWhitespaces(lines[1]).Split(": ")[1].Split(' ');
        for (int i = 0; i < times.Length; i++)
        {
            result.Add(new Race(long.Parse(times[i]), long.Parse(distances[i])));
        }
        return result;
    }

    public static Race ParseBigRace(string[] lines)
    {
        string time = Utils.RemoveExtraWhitespaces(lines[0]).Split(": ")[1].Replace(" ", "");
        string distances = Utils.RemoveExtraWhitespaces(lines[1]).Split(": ")[1].Replace(" ", "");
        return new Race(long.Parse(time), long.Parse(distances));
    }
}

public record Race(long Time, long Distance)
{
    public int NumberOfWaysToWin()
    {   
        int result = 0;
        for (long holdDuration = 0; holdDuration <= Time; holdDuration++)
        {
            long timeLeft = Time - holdDuration;
            long dist = timeLeft * holdDuration;
            if (dist > Distance) result++;
        }
        return result;
    }
}