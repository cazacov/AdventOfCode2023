namespace Day12
{
    internal class Program
    {
        public record Record(string Springs, List<int> Data);

        public class Descriptor(int lineNo, string lineSource, string line, List<int> data)
        {
            public int lineNo { get; set; } = lineNo;
            public string lineSource { get; set; } = lineSource;
            public string line { get; set; } = line;
            public List<int> Data { get; set; } = data;
            public Int128 matched { get; set; }
        }

        static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            var records = ParseInput(lines);

            Puzzle1(records);
            Puzzle2(records);
        }

        private static List<Record> ParseInput(string[] lines)
        {
            var result = new List<Record>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var springs = parts[0];
                var data = parts[1].Split(",").Select(int.Parse).ToList();
                result.Add(new Record(springs, data));
            }
            return result;
        }

        private static void Puzzle1(List<Record> records)
        {
            var res = 0L;
            foreach (var record in records)
            {
                res += CountArrangements(record);
            }
            Console.WriteLine($"Puzzle 1: {res} ");
        }

        private static long CountArrangements(Record record)
        {
            var springs = record.Springs;
            var data = new List<int>(record.Data);

            var unknowns = new List<int>();
            for (int i = 0; i < springs.Length; i++)
            {
                if (springs[i] == '?')
                {
                    unknowns.Add(i);
                }
            }

            long matches = 0;

            for (int mask = 0; mask < 1 << unknowns.Count; mask++)
            {
                var sp = new bool[springs.Length];
                var unkPtr = 0;
                for (int i = 0; i < springs.Length; i++)
                {
                    if (springs[i] == '#')
                    {
                        sp[i] = false;
                    }
                    else if (springs[i] == '.')
                    {
                        sp[i] = true;
                    }
                    else
                    {
                        sp[i] = (mask & (1 << unkPtr)) != 0;
                        unkPtr++;
                    }
                }

                if (MatchesPattern(sp, data))
                {
                    matches++;
                }
            }
            return matches;
        }

        private static bool MatchesPattern(bool[] springs, List<int> data)
        {
            List<int> starts = new List<int>();
            List<int> ends = new List<int>();

            var n = springs.Length;

            for (int i = 0; i < n; i++)
            {
                if (!springs[i]
                    && (i == 0 || (i > 0 && springs[i - 1]))
                    )
                    starts.Add(i);

                if (springs[i] && (i > 0 && !springs[i - 1]))
                {
                    ends.Add(i);
                }
            }

            if (!springs[n - 1])
            {
                ends.Add(n);
            }


            if (data.Count != starts.Count || data.Count != ends.Count)
            {
                return false;
            }

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] != ends[i] - starts[i])
                {
                    return false;
                }
            }
            return true;
        }



        private static void Puzzle2(List<Record> records)
        {
            var descriptors = new List<Descriptor>();

            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                var springs = record.Springs + "?" + record.Springs + "?" + record.Springs + "?" + record.Springs +
                              "?" + record.Springs;
                var data = new List<int>();
                data.AddRange(record.Data);
                data.AddRange(record.Data);
                data.AddRange(record.Data);
                data.AddRange(record.Data);
                data.AddRange(record.Data);

                descriptors.Add(new Descriptor(i,  record.Springs, springs, data));
            }

            Int128 count = 0;
            Parallel.ForEach(descriptors, item =>
                {
                    CountArrangements4(item);
                    Console.WriteLine($"{count, 3} - {item.lineNo, 3} - {item.lineSource, -35} {item.matched,15}");
                    count++;
                }
            );

            var res = Int128.Zero;
            foreach (var item in descriptors)
            {
                res += item.matched;
            }
            Console.WriteLine($"Puzzle 2: {res}");
        }

        public record Span(string Pattern, Int128 Cardinality);
        
        private static Int128 CountArrangements4(Descriptor descriptor)
        {
            var springs = descriptor.line;
            var data = descriptor.Data;

            if (descriptor.line.All(c => c == '?'))
            {
                descriptor.matched = Magic(descriptor.line.Length, descriptor.Data);
                return descriptor.matched;
            }

            var spans = new List<List<Span>>();

            string current = String.Empty;
            int strIndex = 0;
            while (strIndex < springs.Length)
            {
                if (springs[strIndex] != '?')
                {
                    current += springs[strIndex];
                    strIndex++;
                    continue;
                }

                if (current != String.Empty)
                {
                    spans.Add(new List<Span>() {new Span(current, 1)});
                    current = String.Empty;
                }

                var unknownLen = 0;
                var unknownStart = strIndex;
                while (strIndex < springs.Length && springs[strIndex] == '?')
                {
                    unknownLen++;
                    strIndex++;
                }

                char prevChar = '.';
                char nextChar = '.';
                if (unknownStart > 0)
                {
                    prevChar = springs[unknownStart - 1];
                }

                if (unknownStart + unknownLen < springs.Length)
                {
                    nextChar = springs[unknownStart + unknownLen];
                }
                spans.Add(UnknownSpans(unknownLen, prevChar, nextChar));
            }

            if (current != String.Empty)
            {
                spans.Add(new List<Span>() { new Span(current, 1) });
            }

            int spanIndex = 0;
            int datIndex = 0;
            var result = CheckPlus(spans, data, spanIndex, datIndex, false, false);
            descriptor.matched = result;
            return result;
        }

        private static Int128 Magic(int lineLength, List<int> data)
        {
            var d = data.Sum() - data.Count();
            var n = lineLength - d;
            var k = data.Count();

            n -= k - 1;

            var res = Int128.One;
            for (int i = 0; i < n - k; i++)
            {
                res *= n - i;
            }

            res /= Factorial(n - k);
            return res;
        }

        private static Int128 Factorial(int n)
        {
            var res = Int128.One;

            for (int i = 2; i <= n; i++)
            {
                res *= i;
            }
            return res;
        }


        private static Int128 CheckPlus(List<List<Span>> spans, List<int> data, int spanIndex, int datIndex,
            bool startWithHash, bool startWithDot)
        {
            if (spanIndex == spans.Count)
            {
                if (datIndex != data.Count)
                {
                    return 0;
                }
                return 1;
            }


            Int128 result = 0;
            
            foreach (var span in spans[spanIndex])
            {
                if (startWithHash && !span.Pattern.StartsWith('#'))
                {
                    continue;
                }

                if (startWithDot && !span.Pattern.StartsWith('.'))
                {
                    continue;
                }

                var pattern = span.Pattern;

                var ranges = PatternRanges(pattern);
                if (ranges.Count == 0)
                {
                    result += span.Cardinality * CheckPlus(spans, data, spanIndex + 1, datIndex, false, false);
                }
                else
                {
                    var head = ranges.Count - 1;
                    
                    if (datIndex + head >= data.Count)
                    {
                        continue;
                    }
                    for (int r = 0; r < head; r++)
                    {
                        if (datIndex + r == data.Count)
                        {
                            goto next;
                        }

                        if (ranges[r] != data[datIndex + r])
                        {
                            goto next;
                        }
                    }

                    if (ranges.Last() > data[datIndex + head])
                    {
                        continue;
                    }

                    if (ranges.Last() == data[datIndex + head])
                    {
                        bool swd = pattern.EndsWith('#');
                        
                        result += span.Cardinality * CheckPlus(spans, data, spanIndex + 1, datIndex + head + 1, false, swd);
                    }
                    else if (pattern.EndsWith('#'))
                    {
                        data[datIndex + head] -= ranges.Last();
                        result += span.Cardinality * CheckPlus(spans, data, spanIndex + 1, datIndex + head, true, false);
                        data[datIndex + head] += ranges.Last();
                    }
                }
                next: ;
            }
            return result;
        }

        private static List<Span> UnknownSpans(int unknownLen, char previous, char next)
        {
            var variants = new List<string>();

            if (unknownLen > 30)
            {
                Console.WriteLine("OMFG");
            }

            for (long mask = 0; mask < 1L << unknownLen; mask++)
            {
                var str = "" + previous;
                for (int bit = 0; bit < unknownLen; bit++)
                {
                    if ((mask & (1L << bit)) == 0)
                    {
                        str += '.';
                    }
                    else
                    {
                        str += "#";
                    }
                }
                str += next;
                variants.Add(str);
            }

            var grouped = variants.GroupBy(v => String.Join('-',PatternRanges(v)));

            return grouped
                .Select(g =>
                {
                    var str = g.First();
                    str = str.Substring(1, str.Length - 2);
                    return new Span(str, g.Count());
                })
                .ToList();
        }

        private static List<int> PatternRanges(string str)
        {
            var ranges = new List<int>();

            var len = 0;
            for (var i = 0; i < str.Length; i++) 
            {
                if (str[i] == '#')
                {
                    len++;
                    continue;
                }

                if (len > 0)
                {
                    ranges.Add(len);
                    len = 0;
                }
            }

            if (len > 0)
            {
                ranges.Add(len);
            }

            return ranges;
        }
    }
}
