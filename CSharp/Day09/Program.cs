namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var histories = ParseInput(lines);

            Puzzle12(histories);
        }

        private static void Puzzle12(List<List<int>> histories)
        {
            long res_pre = 0;
            long res_post = 0;

            foreach (var item in histories)
            {
                var derivatives = new List<List<int>> { new(item) };
                var level = 0;
                while(true)
                {
                    var nextDev = new List<int>();
                    for (var i = 0; i < derivatives[level].Count - 1; i++)
                    {
                        nextDev.Add(derivatives[level][i+1]- derivatives[level][i]);
                    }
                    derivatives.Add(nextDev);
                    level++;
                    if (nextDev.All(x => x == 0))
                    {
                        break;
                    }
                }

                var pre = 0;
                var post = 0;

                while (level > 0)
                {
                    pre = derivatives[level - 1].First() - pre;
                    post = derivatives[level - 1].Last() + post;
                    level--;
                }

                res_pre += pre;
                res_post += post;
            }
            Console.WriteLine($"Puzzle 1: {res_post}\nPuzzle2 : {res_pre}");
        }


        private static List<List<int>> ParseInput(IEnumerable<string> lines)
        {
            return lines.Select(line => line
                .Split(' ')
                .Select(int.Parse)
                .ToList()
            ).ToList();
        }
    }
}
