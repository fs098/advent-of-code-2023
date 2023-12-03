namespace AdventOfCode;

public static class Day02
{
    public static int Day => 2;
    public static string Part1(string filename)
    {
        int idCount = 0;
        List<Game> games = GetInput(filename);
        foreach (Game game in games)
        {
            if (game.IsPossible()) idCount += game.Id;
        }
        return idCount.ToString();
    }

    public static string Part2(string filename)
    {   
        int result = 0;
        List<Game> games = GetInput(filename);
        foreach (Game game in games)
        {
            result += game.Power;
        }
        return result.ToString();
    }

    private static List<Game> GetInput(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        List<Game> result = new(lines.Length);
        foreach (string line in lines)
        {
            result.Add(MakeGame(line));
        }
        return result;
    }

    private static Game MakeGame(string line)
    {
        // Example line: "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green";
        string[] numberAndSets = line.Split(':');

        string number = numberAndSets[0].Split(' ')[1].Trim();
        int id = int.Parse(number);

        string[] allSets = numberAndSets[1].Trim().Split(';');
        List<GameSet> sets = new(allSets.Length);
        foreach (string rawSet in allSets)
        {
            sets.Add(MakeGameSet(rawSet.Trim()));
        }
        return new Game(id, sets);
    }

    private static GameSet MakeGameSet(string set)
    {
        // Example set: "3 blue, 4 red"
        int red = 0;
        int blue = 0;
        int green = 0;
        foreach (string numberColorPair in set.Split(", "))
        {
            int number = int.Parse(numberColorPair.Split(' ')[0]);
            if (numberColorPair.Contains("red")) red = number;
            if (numberColorPair.Contains("blue")) blue = number;
            if (numberColorPair.Contains("green")) green = number;
        }
        return new GameSet(red, blue, green);
    }
}

public class Game
{
    public int Id { get; }
    public int MinRedNecessary { get; } = 0;
    public int MinBlueNecessary { get; } = 0;
    public int MinGreenNecessary { get; } = 0;
    private readonly List<GameSet> _sets;

    public Game(int id, List<GameSet> sets)
    {
        Id = id;
        _sets = sets;
        foreach (GameSet set in _sets)
        {
            if (set.Red > MinRedNecessary) MinRedNecessary = set.Red;
            if (set.Blue > MinBlueNecessary) MinBlueNecessary = set.Blue;
            if (set.Green > MinGreenNecessary) MinGreenNecessary = set.Green;
        }
    }

    public bool IsPossible()
    {
        foreach (GameSet set in _sets)
        {
            if (!set.IsPossible()) return false;
        }
        return true;    
    }

    public int Power { get => MinRedNecessary * MinBlueNecessary * MinGreenNecessary; }
}

public class GameSet(int red, int blue, int green)
{
    private static readonly int MaxRed = 12;
    private static readonly int MaxBlue = 14;
    private static readonly int MaxGreen = 13;
    public int Red { get; } = red;
    public int Blue { get; } = blue;
    public int Green { get; } = green;

    public bool IsPossible() =>  Red <= MaxRed && Blue <= MaxBlue && Green <= MaxGreen;
}
