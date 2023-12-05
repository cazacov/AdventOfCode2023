namespace Day05
{
    internal class Program
    {

        public class Map
        {
            public Map(long destinationStart, long sourceStart, long rangeLength)
            {
                this.DestinationStart = destinationStart;
                this.SourceStart = sourceStart;
                this.RangeLength = rangeLength;
                this.Offset = destinationStart - sourceStart;
            }

            public long DestinationStart { get; init; }
            public long SourceStart { get; init; }
            public long RangeLength { get; init; }
            public long Offset { get; init; }

            public bool IsInStartRange(long n)
            {
                return n >= SourceStart 
                       && n < SourceStart + RangeLength;
            }
        }

        public record MapSet(string FromCategory, string ToCategory, List<Map> Maps);
        public record Range(long StartFrom, long Length);


        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var (seeds, maps) = ParseInput(input);

            Puzzle1(seeds, maps);
            Puzzle2(seeds, maps);
        }

        private static void Puzzle1(List<long> seeds, List<MapSet> maps)
        {
            var locations = new List<long>();

            foreach (var seed in seeds)
            {
                locations.Add(FindSeedLocation(seed, maps));
            }
            Console.WriteLine(locations.Min());
        }

        private static long FindSeedLocation(long seed, List<MapSet> maps)
        {
            var mapIndex = 0;
            var value = seed;

            do
            {
                foreach (var map in maps[mapIndex].Maps)
                {
                    if (value >= map.SourceStart && value <= map.SourceStart + map.RangeLength)
                    {
                        value = value - map.SourceStart + map.DestinationStart;
                        break;
                    }
                }
                mapIndex++;
            } while (mapIndex < maps.Count);
            return value;
        }

        private static void Puzzle2(List<long> seeds, List<MapSet> mapSets)
        {
            var minLocation = long.MaxValue;
            for (var i = 0; i < seeds.Count / 2; i++)
            {
                var seedRange = new Range(seeds[i * 2], seeds[i * 2 + 1]);

                var currentRanges = new List<Range> { seedRange };

                foreach (var mapSet in mapSets)
                {
                    currentRanges = MapRanges(currentRanges, mapSet.Maps);
                }

                var minSeedLocation = currentRanges.Min(x => x.StartFrom);
                if (minSeedLocation < minLocation)
                {
                    minLocation = minSeedLocation;
                }
            }
            Console.WriteLine(minLocation);
        }


        private static List<Range> MapRanges(List<Range> currentRanges, List<Map> maps)
        {
            var result = new List<Range>();

            foreach (var inpRange in currentRanges)
            {
                var inpFrom = inpRange.StartFrom;
                var inpTo = inpRange.StartFrom + inpRange.Length;

                var relevantMaps = maps.Where(map =>
                        map.SourceStart + map.RangeLength > inpFrom && // CHECK
                        map.SourceStart < inpTo)
                    .ToList();

                foreach (var relevantMap in relevantMaps)
                {
                    var from = Math.Max(inpFrom, relevantMap.SourceStart);
                    var to = Math.Min(inpTo, relevantMap.SourceStart + relevantMap.RangeLength);
                    result.Add(new Range(from + relevantMap.Offset, to - from));
                }

                // find gaps
                var checkPoints =
                    relevantMaps.Select(rm => rm.SourceStart + rm.RangeLength)
                        .Union(new List<long>() { inpFrom })
                        .Distinct()
                        .ToList();


                foreach (var checkPoint in checkPoints)
                {
                    if (checkPoint > inpTo)
                    {
                        continue;
                    }

                    if (relevantMaps.Any(m => m.IsInStartRange(checkPoint)))
                    {
                        // There exists some releventMap covering that check point -> Not a gap
                        continue;
                    }

                    // find lowest relevant map after the check point 
                    var nextMap = relevantMaps
                        .Where(m => m.SourceStart > checkPoint)
                        .MinBy(m => m.SourceStart);

                    if (nextMap == null)
                    {
                        result.Add(new Range(checkPoint, inpTo - checkPoint));
                    }
                    else
                    {
                        result.Add(new Range(checkPoint, nextMap.SourceStart - checkPoint));
                    }
                }
            }
            return result;
        }

        

        private static (List<long> seeds, List<MapSet> mapset) ParseInput(string[] input)
        {
            var seeds = input[0].Substring(7).Split(" ").Select(long.Parse).ToList();

            var mapset = new List<MapSet>();

            var lineNr = 2;
            while (lineNr < input.Length)
            {
                // Parse header
                var header = input[lineNr].Substring(0, input[lineNr].IndexOf(" ")).Split("-");
                var fromCategory = header[0];
                var toCategory = header[2];
                var maps = new List<Map>();

                lineNr += 1;
                while (lineNr < input.Length)
                {
                    var line = input[lineNr];
                    if (line == String.Empty)
                    {
                        lineNr += 1;
                        break;
                    }
                    var parts = line.Split(' ');
                    var destinationStart = long.Parse(parts[0]);
                    var sourceStart = long.Parse(parts[1]);
                    var rangeLength = long.Parse(parts[2]);
                    maps.Add(new Map(destinationStart, sourceStart, rangeLength));
                    lineNr += 1;
                }
                mapset.Add(new MapSet(fromCategory, toCategory, maps));
            }
            return (seeds, mapset);
        }
    }
}
