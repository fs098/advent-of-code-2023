namespace AdventOfCode;

public static class Day05
{
    public static int Day => 5;
    public static string Part1(string filename)
    {
        Almanac almanac = Almanac.AlmanacFromInput(File.ReadAllLines(filename));
        return almanac.SolvePart1().ToString();
    }   

    public static string Part2(string filename)
    {
        Almanac almanac = Almanac.AlmanacFromInput(File.ReadAllLines(filename));
        return almanac.SolvePart2().ToString();
    }
}

public class Almanac
{
    private readonly List<long> _seedsToPlant;
    private readonly IntervalMap _seedsInDestinationRanges = new ();
    private readonly IntervalMap _seedToSoil = new ();
    private readonly IntervalMap _soilToFertilizer = new ();
    private readonly IntervalMap _fertilizerToWater = new ();
    private readonly IntervalMap _waterToLight = new ();
    private readonly IntervalMap _lightToTemperature = new ();
    private readonly IntervalMap _temperatureToHumidity = new ();
    private readonly IntervalMap _humidityToLocation = new ();

    private Almanac(List<long> seedsToPlant) { _seedsToPlant = seedsToPlant; }

    public static Almanac AlmanacFromInput(string[] lines)
    {
        string[] seedsFromLine = lines[0].Split(": ")[1].Split(' ');
        List<long> seeds = new (seedsFromLine.Length);
        foreach (string seed in seedsFromLine)
        {
            seeds.Add(long.Parse(seed));
        }
        Almanac almanac = new (seeds);

        for (int i = 0; i < seeds.Count; i += 2)
        {
            almanac._seedsInDestinationRanges.Add(seeds[i], 0, seeds[i+1]);
        }

        Action<long, long, long> currentUpdateFunc = almanac._seedToSoil.Add;
        foreach (string line in lines[3..])
        {
            if (line == "") continue;
            else if (line.StartsWith("soil-to-fertilizer"))
            {
                currentUpdateFunc = almanac._soilToFertilizer.Add;
                continue;
            }
            else if (line.StartsWith("fertilizer-to-water"))
            {
                currentUpdateFunc = almanac._fertilizerToWater.Add;
                continue;
            }
            else if (line.StartsWith("water-to-light"))
            {
                currentUpdateFunc = almanac._waterToLight.Add;
                continue;
            }
            else if (line.StartsWith("light-to-temperature"))
            {
                currentUpdateFunc = almanac._lightToTemperature.Add;
                continue;
            }
            else if (line.StartsWith("temperature-to-humidity"))
            {
                currentUpdateFunc = almanac._temperatureToHumidity.Add;
                continue;
            }
            else if (line.StartsWith("humidity-to-location"))
            {
                currentUpdateFunc = almanac._humidityToLocation.Add;
                continue;
            }

            string[] values = line.Split(' ');
            long dst = long.Parse(values[0]);
            long src = long.Parse(values[1]);
            long rangeLength = long.Parse(values[2]);
            currentUpdateFunc(dst, src, rangeLength);
        }
        return almanac;
    }

    public long SolvePart1()
    {   
        long min = long.MaxValue;
        foreach (long seed in _seedsToPlant)
        {
            long location = GetLocationFromSeed(seed);
            if (location < min) min = location;
        }
        return min;
    }

    public long SolvePart2()
    {
        for (long location = 0; location < long.MaxValue; location++)
        {
            long seed = GetSeedFromLocation(location);
            if (_seedsInDestinationRanges.ContainsDestination(seed)) return location;
        }
        return -1;
    }

    private long GetLocationFromSeed(long seed)
    {
        long soil = _seedToSoil.SearchBySource(seed);
        long fertilizer = _soilToFertilizer.SearchBySource(soil);
        long water = _fertilizerToWater.SearchBySource(fertilizer);
        long light = _waterToLight.SearchBySource(water);
        long temperature = _lightToTemperature.SearchBySource(light);
        long humidity = _temperatureToHumidity.SearchBySource(temperature);
        long location = _humidityToLocation.SearchBySource(humidity);
        return location;
    }

    private long GetSeedFromLocation(long location)
    {
        long humidity = _humidityToLocation.SearchByDestination(location);
        long temperature = _temperatureToHumidity.SearchByDestination(humidity);
        long light = _lightToTemperature.SearchByDestination(temperature);
        long water = _waterToLight.SearchByDestination(light);
        long fertilizer = _fertilizerToWater.SearchByDestination(water);
        long soil = _soilToFertilizer.SearchByDestination(fertilizer);
        long seed = _seedToSoil.SearchByDestination(soil);
        return seed;
    }
}

public class IntervalMap()
{
    private readonly List<Interval> _intervals = [];
    private bool _destinationSorted = true;
    private bool _sourceSorted = true;

    public void Add(Interval interval)
    {
        _intervals.Add(interval);
        _destinationSorted = false;
        _sourceSorted = false;
    }

    public void Add(long dst, long src, long range) => Add(new Interval(dst, src, range));

    public long SearchBySource(long src)
    {
        if (!_sourceSorted) SortBySource();

        IntervalSourceComparer isc = new();
        int index = _intervals.BinarySearch(new Interval(0, src, 0), isc);
        if (index >= 0) return _intervals[index].GetDestinationFromSource(src);
        return src;
    }

    public long SearchByDestination(long dst)
    {
        if (!_destinationSorted) SortByDestination();

        int index = DestinationBinarySearch(dst);
        if (index >= 0) return _intervals[index].GetSourceFromDestination(dst);
        return dst;
    }

    public bool ContainsDestination(long dst)
    {
        if (!_destinationSorted) SortByDestination();

        int index = DestinationBinarySearch(dst);
        if (index >= 0) return true;
        return false;
    }

    private int DestinationBinarySearch(long dst)
    {
        IntervalDestinationComparer idc = new();
        return _intervals.BinarySearch(new Interval(dst, 0, 0), idc);
    }

    public void SortBySource()
    {
        if (_sourceSorted) return;

        _intervals.Sort((x, y) => x.SourceStart.CompareTo(y.SourceStart));
        _destinationSorted = false;
        _sourceSorted = true;
    }

    public void SortByDestination()
    {
        if (_destinationSorted) return;

        _intervals.Sort((x, y) => x.DestinationStart.CompareTo(y.DestinationStart));
        _destinationSorted = true;
        _sourceSorted = false;
    }
}

public readonly struct Interval(long dst, long src, long range)
{
    public long SourceStart { get; } = src;
    public long SourceEnd { get => SourceStart + Range -1; }
    public long DestinationStart { get; } = dst;
    public long DestinationEnd { get => DestinationStart + Range -1; }
    public long Range { get; } = range;

    public bool IsInSourceInterval(long value) => SourceStart <= value && value <= SourceEnd;

    public bool IsInDestinationInterval(long value) => DestinationStart <= value && value <= DestinationEnd;

    public long GetDestinationFromSource(long value) => DestinationStart + value - SourceStart;

    public long GetSourceFromDestination(long value) => SourceStart + value - DestinationStart;
}

public class IntervalSourceComparer : IComparer<Interval>
{
    public int Compare(Interval x, Interval y)
    {
        if (x.IsInSourceInterval(y.SourceStart)) return 0;
        return x.SourceStart.CompareTo(y.SourceStart);
    }
}

public class IntervalDestinationComparer : IComparer<Interval>
{
    public int Compare(Interval x, Interval y)
    {
        if (x.IsInDestinationInterval(y.DestinationStart)) return 0;
        return x.DestinationStart.CompareTo(y.DestinationStart);
    }
}
