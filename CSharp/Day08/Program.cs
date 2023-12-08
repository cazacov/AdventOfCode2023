using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Day08
{
    internal class Program
    {
        public record Node(string Left, string Right);

        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var (Path, nodes) = ParseInput(input);

            //Puzzle1(Path, nodes);
            Puzzle2(Path, nodes);
        }

        private static void Puzzle2(string path, Dictionary<string, Node> nodes)
        {
            var currentNodes = nodes.Keys.Where(k => k[2] == 'A').ToArray();

            var nn = currentNodes.Length;
            long pathLength = path.Length;

            long[] starts = new long[currentNodes.Length];
            List<long>[] loops = new List<long>[currentNodes.Length];

            for (int i = 0; i < nn; i++)
            {
                var n = 0L;
                var current = currentNodes[i];

                var known = new Dictionary<string, long>();
                var key = current + (n % pathLength);

                while (true)
                {
                    FindNext(ref current, ref n, path, nodes);
                    key = current + (n % pathLength);
                    if (known.ContainsKey(key))
                    {
                        starts[i] = known[key];

                        var pairs = known.Where(pair => pair.Value > starts[i]).Select(x => x.Value).OrderBy(x => x).ToList();
                        var c = starts[i];
                        var loop = new List<long>();
                        foreach (var p in pairs)
                        {
                            loop.Add(p - c);
                            c = p;
                        }
                        loops[i] = loop;
                        break;
                    }
                    known[key] = n;
                }
            }

            var res = starts[0];
            foreach (var s in starts.Skip(1))
            {
                res = determineLCM(res, s);
            }

            Console.WriteLine(res);


        }

        public static long determineLCM(long a, long b)
        {
            long num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                long mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }
            return num1 * num2;
        }

        private static void FindNext(ref string current, ref long n, string path, Dictionary<string, Node> nodes)
        {
            do
            {
                var dir = path[(int)(n % path.Length)];
                if (dir == 'L')
                {
                    current = nodes[current].Left;
                }
                else
                {
                    current = nodes[current].Right;
                }

                n++;
            } while (current[2] != 'Z');
        }

        private static void Puzzle1(string path, Dictionary<string, Node> nodes)
        {
            long n = 0;
            var current = "AAA";

            while (current != "ZZZ")
            {
                var dir = path[(int)(n % path.Length)];
                if (dir == 'L')
                {
                    current = nodes[current].Left;
                }
                else
                {
                    current = nodes[current].Right;
                }
                n++;
            }
            Console.WriteLine(n);
        }

        private static (string Path, Dictionary<String, Node> nodes) ParseInput(string[] input)
        {
            var path = input[0];
            var nodes = new Dictionary<String, Node>();

            int i = 2;
            while (i < input.Length)
            {
                var line = input[i];
                var key = line.Substring(0, 3);
                var left = line.Substring(7, 3);
                var right = line.Substring(12, 3);

                nodes[key] = new Node(left, right);
                i++;
            }
            return (path, nodes);
        }
    }
}
