namespace AdventOfCode;

public static class Day08
{
    public static int Day => 8;
    public static string Part1(string filename)
    {
        Network network = Network.FromFile(filename);
        return network.SolvePart1().ToString();
    }

    public static string Part2(string filename)
    {
        Network network = Network.FromFile(filename);
        return network.SolvePart2().ToString();
    }
}

public class Network
{
    private readonly Instructions _instructions;
    private readonly Dictionary<string, NetworkNode> _network;
    private readonly List<string> _currentNodes;

    private Network(Instructions instructions, Dictionary<string, NetworkNode> network, List<string> currentNodes)
    {
        _instructions = instructions;
        _network = network;
        _currentNodes = currentNodes;
    }

    public static Network FromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        Instructions instructions = new (lines[0]);

        Dictionary<string, NetworkNode> network = [];
        List<string> currentNodes = [];
        foreach (string line in lines[2..])
        {
            string[] keyAndValue = line.Split(" = ");
            string value = keyAndValue[1].Replace("(", "").Replace(")", "");
            string[] leftAndRight = value.Split(", ");

            string key = keyAndValue[0];
            if (key.Last() is 'A') currentNodes.Add(key);

            NetworkNode node = new (leftAndRight[0], leftAndRight[1]);
            network[key] = node;
        }
        return new (instructions, network, currentNodes);
    }

    public int SolvePart1() => StepsUntilCondition("AAA", x => x == "ZZZ");

    public long SolvePart2()
    {
        List<int> loopDurations = new (_currentNodes.Count);
        foreach (string node in _currentNodes)
        {
            int loopDuration = StepsUntilCondition(node, x => x.Last() is 'Z');
            loopDurations.Add(loopDuration);
        }
        return Utils.LCM(loopDurations);
    }

    private int StepsUntilCondition(string from, Func<string, bool> condition)
    {
        int steps = 0;
        string currentNode = from;
        while (!condition(currentNode))
        {
            currentNode = _instructions.Next() switch
            {
                Instruction.MoveLeft => _network[currentNode].Left,
                _ => _network[currentNode].Right,
            };
            steps++;
        }
        _instructions.Reset();
        return steps;
    }
}

public record NetworkNode(string Left, string Right);

public class Instructions
{
    private readonly Instruction[] _instructions;
    private int _current;

    public Instructions(string line)
    {
        Instruction[] instructions = new Instruction[line.Length];
        for (int i = 0; i < line.Length; i++)
        {
            instructions[i] = line[i] switch
            {
                'L' => Instruction.MoveLeft,
                _ => Instruction.MoveRight,
            };
        }
        _instructions = instructions;
        _current = 0;
    }

    public Instruction Next()
    {        
        Instruction result = _instructions[_current];
        _current++;
        if (_current == _instructions.Length)
        {
            _current = 0;
        }
        return result;
    }

    public void Reset() => _current = 0;
}

public enum Instruction { MoveLeft, MoveRight }