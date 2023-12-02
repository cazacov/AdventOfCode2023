namespace Day02
{
    internal class Program
    {
        public record Game(int Id, List<Dictionary<string, int>> Sets);

        static void Main()
        {
            var input = File.ReadAllLines("input.txt").ToList();
            var games = ParseGames(input);

            Puzzle1(games);
            Puzzle2(games);
        }

        private static void Puzzle1(IEnumerable<Game> games)
        {
            var maxSet = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

            var result = games
                .Where(game => game
                    .Sets
                    .All(set => CanBePlayed(set, maxSet)))
                .Sum(game => game.Id);

            Console.WriteLine($"Puzzle 1: {result}");
        }

        private static bool CanBePlayed(Dictionary<string, int> set, Dictionary<string, int> maxSet)
        {
            return set.Keys.All(color => set[color] <= maxSet[color]);
        }

        private static void Puzzle2(IEnumerable<Game> games)
        {
            var result = games
                .Sum(game => Power(game.Sets));

            Console.WriteLine($"Puzzle 2: {result}");
        }

        private static long Power(List<Dictionary<string, int>> set)
        {
            var dict = new Dictionary<string, int>()
            {
                { "red", 0 },
                { "blue", 0 },
                { "green", 0 }
            };
            foreach (var game in set)
            {
                foreach (var key in game.Keys)
                {
                    if (game[key] > dict[key])
                    {
                        dict[key] = game[key];  
                    }
                }
            }
            
            var res = 1L;
            foreach (var key in dict.Keys)
            {
                res *= dict[key];
            }
            return res;
        }

        private static List<Game> ParseGames(List<string> input)
        {
            var result = new List<Game>();
            var lineNo = 1;
            foreach (var game in input)
            {
                var pos = game.IndexOf(":");
                var inp = game.Substring(pos + 1);

                var sets = inp.Split(';');
                var record = new Game(lineNo, new List<Dictionary<string, int>>());

                foreach (var set in sets)
                {
                    var items = set.Split(',');
                    var dict = new Dictionary<string, int>();   
                    foreach (var item in items)
                    {
                        var parts = item.Trim().Split(' ');
                        var color = parts[1];
                        var count = int.Parse(parts[0]);
                        dict[color] = count;
                    }
                    record.Sets.Add(dict);
                }
                result.Add(record);
                lineNo++;
            }
            return result;
        }
    }
}
