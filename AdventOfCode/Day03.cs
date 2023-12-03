using System.Text;

namespace AdventOfCode;

public static class Day03
{
    public static int Day => 3;
    public static string Part1(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        Schematic schematic = new (lines);
        return schematic.PartNumberSum.ToString();
    }

    public static string Part2(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        Schematic schematic = new (lines);
        return schematic.GearRatioSum.ToString();
    }
}

public class Schematic
{
    private readonly char[][] _matrix;
    public int GearRatioSum { get; private set; } = 0;
    public int PartNumberSum { get; private set; } = 0;
    private static readonly HashSet<char> _symbols = new ("#$%&*+-/=@");

    public Schematic(string[] strings)
    {
        char[][] m = new char[strings.Length][];
        for (int i = 0; i < strings.Length; i++)
        {
            m[i] = strings[i].ToCharArray();
        }
        _matrix = m;
        Init();
    }

    private void Init()
    {
        for (int row = 0; row < _matrix.Length; row++)
        {
            for (int col = 0; col < _matrix[row].Length; col++)
            {
                if (IsSymbol(row, col)) VisitAllAdjacent(row, col);
            }
        }
    }

    private void VisitAllAdjacent(int row, int col)
    {
        int gearCount = 0;
        int partNumberSum = 0;
        int partNumberProduct  = 1;
        Visit(row-1, col-1, ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row-1, col  , ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row-1, col+1, ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row  , col+1, ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row+1, col+1, ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row+1, col  , ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row+1, col-1, ref partNumberSum, ref partNumberProduct, ref gearCount);
        Visit(row  , col-1, ref partNumberSum, ref partNumberProduct, ref gearCount);

        PartNumberSum += partNumberSum;
        if (gearCount == 2) GearRatioSum += partNumberProduct;
    }

    private void Visit(int row, int col, ref int partNumberSum, ref int partNumberProduct, ref int gearCount)
    {
        if (row < 0 || row >= _matrix.Length)      return;
        if (col < 0 || col >= _matrix[row].Length) return;
        if (!char.IsDigit(_matrix[row][col]))      return;

        string strPartNumber = GetPartNumberAt(row, col);
        if (strPartNumber == "") return;

        int partNumber = int.Parse(strPartNumber);
        partNumberProduct *= partNumber;
        partNumberSum += partNumber;
        gearCount++;
    }

    private string GetPartNumberAt(int row, int col)
    {
        int start = col;
        while (start > 0 && char.IsDigit(_matrix[row][start-1])) start--;

        StringBuilder sb = new();
        for (int i = start; i < _matrix[row].Length; i++)
        {
            if (!char.IsDigit(_matrix[row][i])) break;
            sb.Append(_matrix[row][i]);
            _matrix[row][i] = '.';
        }
        return sb.ToString();
    }

    public bool IsSymbol(int row, int col) => _symbols.Contains(_matrix[row][col]);
}