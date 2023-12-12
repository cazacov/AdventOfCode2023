using System.ComponentModel;
using System.Text;

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

            return Check(springs.ToArray(), data.ToArray(), strIndex, datIndex, false, false);
        }

        private static long Check(char[] springs, int[] data, int strIndex, int datIndex, bool hashExpected, bool dotExpected)
        {

            if (strIndex == springs.Length && datIndex < data.Length)
            {
                return 0;
            }

            if (strIndex == springs.Length && datIndex == data.Length)
            {
                if (hashExpected )
                {
                    return 0;
                }
                else
                    return 1;
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

            if (strIndex == springs.Length && datIndex < data.Length - 1)
            {
                return 0;
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

            if (springs[strIndex] == '#' && dotExpected)
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
                    res = Check(springs, data, strIndex, datIndex + 1, false, true);
                }
                else
                {
                    res = Check(springs, data, strIndex, datIndex, true, false);
                }
                data[datIndex] += hashCount;
                return res;
            }

            // ?

            if (springs[strIndex] == '?')
            {
                var result = 0L;
                if (!hashExpected)
                {
                    springs[strIndex] = '.';
                    result += Check(springs, data, strIndex, datIndex, hashExpected, dotExpected);
                }

                if (!dotExpected)
                {
                    springs[strIndex] = '#';
                    result += Check(springs, data, strIndex, datIndex, hashExpected, dotExpected);
                }

                springs[strIndex] = '?';
                return result;
            }

            throw new Exception("OMG");
        }

        private static long CountArrangements2(string line)
        {
            long res = 0;

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

            var groups = new List<string[]>();
            var i = 0;
            var str = "";
            var grLen = 0;

            while (i < springs.Length)
            {
                if (springs[i] == '.' || springs[i] == '#')
                {
                    str += springs[i];
                    grLen++;
                    i++;
                    continue;
                }

                if (grLen > 0)
                {
                    groups.Add(new string[1] {str});
                    str = String.Empty;
                    grLen = 0;
                }

                var len = 0;
                while (i < springs.Length && springs[i] == '?') 
                {
                    len++;
                    i++;
                }
                groups.Add(UnknownVarinats(len));
            }

            if (grLen > 0)
            {
                groups.Add(new string[1] { str });
            }

            int[] indices = new int[groups.Count];

            StringBuilder buildy = new StringBuilder();

            bool stop = false;
            do
            {
                buildy.Clear();

                if (indices[0] == 5 && indices[2] == 5 && indices[4] == 5 && indices[6] == 5 && indices[8] == 5)
                {
                    Console.WriteLine("!");
                }

                for (int groupIndex  = 0; groupIndex < groups.Count; groupIndex++)
                {
                    buildy.Append(groups[groupIndex][indices[groupIndex]]);
                }

                if (MatchesPattern2(buildy.ToString(), data))
                {
                    res++;
                }

                // Next index
                var j = groups.Count - 1;
                while (j >= 0)
                {
                    if (indices[j] == groups[j].Length - 1)
                    {
                        indices[j] = 0;
                        if (j == 0)
                        {
                            stop = true;
                            break;
                        }
                    }
                    else
                    {
                        indices[j] += 1;
                        break;
                    }
                    j--;
                }
            } while (!stop);

            return res;
        }

        static Dictionary<int, string[]> cache = new Dictionary<int, string[]>();
        private static string[] UnknownVarinats(int len)
        {
            if (cache.ContainsKey(len))
            {
                return cache[len];
            }

            List<string> variants = new List<string>();

            for (int i = 0; i < (1 << len); i++)
            {
                var variant = "";
                for (int j = 0; j < len; j++)
                {
                    variant += (i & (1 << j)) == 0 ? '.' : '#';
                }
                variants.Add(Simplify(variant));
            }

            variants = variants.Distinct().ToList();
            cache[len] = variants.ToArray();

            return cache[len];
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

        private static bool MatchesPattern2(string springs, List<int> data)
        {
            List<int> starts = new List<int>();
            List<int> ends = new List<int>();

            var n = springs.Length;

            for (int i = 0; i < n; i++)
            {
                if (springs[i] == '#'
                    && (i == 0 || (i > 0 && springs[i - 1] == '.'))
                   )
                    starts.Add(i);

                if (springs[i] == '.' && (i > 0 && springs[i - 1] == '#'))
                {
                    ends.Add(i);
                }
            }

            if (springs[n - 1] == '#')
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
