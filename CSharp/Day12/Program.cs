using System.Runtime.InteropServices.JavaScript;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day12
{
    internal class Program
    {


        static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            //Puzzle1(lines);
            Puzzle2(lines);
        }

        private static void Puzzle2(string[] lines)
        {
            var res = 0L;
            foreach (var line in lines)
            {
                Console.Write(line + "\t");
                var matches = CountArrangements3(line);
                Console.WriteLine(matches);
                res += matches;
            }
            Console.WriteLine(res);
        }

        private static long CountArrangements3(string line)
        {
            var parts = line.Split(' ');
            var springs = parts[0];
            var d = parts[1].Split(",").Select(int.Parse).ToList();

            springs = Simplify(springs + "?" + springs + "?" + springs + "?" + springs + "?" + springs);

            var data = new List<int>();
            data.AddRange(d);
            data.AddRange(d);
            data.AddRange(d);
            data.AddRange(d);
            data.AddRange(d); 

            var strIndex = 0;
            var datIndex = 0;

            return DoCheck(springs.ToArray(), data.ToArray(), strIndex, datIndex, false, false);
        }

        private static long DoCheck(char[] springs, int[] data, int strIndex, int datIndex, bool b, bool b1)
        {
            var exits = new int[data.Length];

            var sum = 0;
            for (var j = data.Length - 2; j >= 0; j--)
            {
                sum += data[j+1] + 1;
                exits[j] = springs.Length - sum + 1;
            }
            exits[data.Length-1] = springs.Length;

            return Check(ref springs, ref data, strIndex, datIndex, false, false, ref exits);
        }

        private static long Check(ref char[] springs, ref int[] data, int strIndex, int datIndex, bool hashExpected,
            bool dotExpected, ref int[] exits)
        {
            if (strIndex == springs.Length)
            {
                if (datIndex == data.Length)
                {
                    if (hashExpected)
                    {
                        return 0;
                    }
                    else
                        return 1;
                }
                else
                {
                    return 0;
                }
            }

            if (datIndex < data.Length && strIndex > exits[datIndex])
            {
                return 0;
            }


            if (hashExpected && springs[strIndex] == '.')
            {
                return 0;
            }

            if (dotExpected && springs[strIndex] == '#')
            {
                return 0;
            }

            // skip dots
            while (strIndex < springs.Length && springs[strIndex] == '.')
            {
                strIndex++;
                dotExpected = false;
            }

            if (strIndex == springs.Length)
            {
                if (datIndex == data.Length)
                {
                    return 1;
                }
                else return 0;
            }

            if (springs[strIndex] == '#' && datIndex >= data.Length)
            {
                return 0;
            }

            if (springs[strIndex] == '#')
            {
                var hashCount = 0;
                while (strIndex < springs.Length && springs[strIndex] == '#')
                {
                    strIndex++;
                    hashCount++;
                }

                if (hashCount > data[datIndex])
                {
                    return 0;
                }

                data[datIndex] -= hashCount;
                var res = 0L;
                if (data[datIndex] == 0)
                {
                    res = Check(ref springs, ref data, strIndex, datIndex + 1, false, true, ref exits);
                }
                else
                {
                    res = Check(ref springs, ref data, strIndex, datIndex, true, false, ref exits);
                }

                data[datIndex] += hashCount;
                return res;
            }

            // ?

            var result = 0L;
            if (!hashExpected)
            {
                springs[strIndex] = '.';
                result += Check(ref springs, ref data, strIndex, datIndex, hashExpected, dotExpected, ref exits);
            }

            if (!dotExpected)
            {
                springs[strIndex] = '#';
                result += Check(ref springs, ref data, strIndex, datIndex, hashExpected, dotExpected, ref exits);
            }

            springs[strIndex] = '?';
            return result;
        }

        private static string Simplify(string variant)
        {
            var newVar = variant.Replace("..", ".");
            if (newVar != variant)
            {
                return Simplify(newVar);
            }
            else
            {
                return variant;
            }
        }


        private static void Puzzle1(string[] lines)
        {
            var res = 0L;
            foreach (var line in lines)
            {
                res += CountArrangements(line);
            }
            Console.WriteLine(res);
        }

        private static long CountArrangements(string line)
        {
            var parts = line.Split(' ');
            var springs = parts[0];
            var data = parts[1].Split(",").Select(int.Parse).ToList();

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
                    && (i == 0 || (i > 0 && springs[i-1]))
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
    }
}
