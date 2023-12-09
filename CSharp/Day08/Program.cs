namespace Day08
{
    internal class Program
    {
        public record Node(string Left, string Right);

        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var (path, nodes) = ParseInput(input);

            Puzzle1(path, nodes);
            Puzzle2(path, nodes);
        }

        private static void Puzzle1(string path, Dictionary<string, Node> nodes)
        {
            long n = 0;
            var current = "AAA";

            while (current != "ZZZ")
            {
                var dir = path[(int)(n % path.Length)];
                current = dir == 'L' ? nodes[current].Left : nodes[current].Right;
                n++;
            }
            Console.WriteLine(n);
        }

        private static void Puzzle2(string path, Dictionary<string, Node> nodes)
        {
            var currentNodes = nodes.Keys.Where(k => k[2] == 'A').ToArray();

            var nn = currentNodes.Length;
            long pathLength = path.Length;

            var starts = new long[currentNodes.Length];
            var loops = new List<long>[currentNodes.Length];

            for (var i = 0; i < nn; i++)
            {
                var n = 0L;
                var current = currentNodes[i];

                FindNext(ref current, ref n, path, nodes);
                starts[i] = n;
            }

            var res = starts[0];
            foreach (var s in starts.Skip(1))
            {
                res = DetermineLCM(res, s);
            }

            Console.WriteLine(res);
        }

        private static void FindNext(ref string current, ref long n, string path, Dictionary<string, Node> nodes)
        {
            do
            {
                var dir = path[(int)(n % path.Length)];
                current = dir == 'L' ? nodes[current].Left : nodes[current].Right;
                n++;
            } while (current[2] != 'Z');
        }

        /// <summary>
        /// Least common multiple
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long DetermineLCM(long a, long b)
        {
            if (a < b)
            {
                (a, b) = (b, a);
            }

            for (var i = 1; i < b; i++)
            {
                var mult = a * i;
                if (mult % b == 0)
                {
                    return mult;
                }
            }
            return a * b;
        }

        private static (string Path, Dictionary<string, Node> nodes) ParseInput(string[] input)
        {
            var path = input[0];
            var nodes = new Dictionary<String, Node>();

            foreach (var line in input.Skip(2))
            {
                var key = line.Substring(0, 3);
                var left = line.Substring(7, 3);
                var right = line.Substring(12, 3);
                nodes[key] = new Node(left, right);
            }
            return (path, nodes);
        }
    }
}
