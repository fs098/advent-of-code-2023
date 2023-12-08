using System.Diagnostics;
using static AdventOfCode.Day08;

int day = Day;

Stopwatch sw = new();
sw.Start();

Console.WriteLine($"Part 1 Example : {Part1($"./example-inputs/day{day}.txt")}");
Console.WriteLine($"Part 1 Solution: {Part1($"./inputs/day{day}.txt")}");

sw.Stop();
// Console.WriteLine($"in: {sw.Elapsed:mm\\:ss\\.ff}");
Console.WriteLine($"in: {sw.ElapsedMilliseconds}ms");

sw.Restart();

Console.WriteLine($"\nPart 2 Example : {Part2($"./example-inputs/day{day}.txt")}");
Console.WriteLine($"Part 2 Solution: {Part2($"./inputs/day{day}.txt")}");

sw.Stop();
// Console.WriteLine($"in: {sw.Elapsed:mm\\:ss\\.ff}");
Console.WriteLine($"in: {sw.ElapsedMilliseconds}ms");