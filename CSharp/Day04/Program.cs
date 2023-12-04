namespace Day04
{
    internal class Program
    {
        public record Card(int Id, List<int> Winning, List<int> Mine);

        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var cards = ParseInput(input);

            Puzzle1(cards);
            Puzzle2(cards);
        }

        private static void Puzzle1(List<Card> cards)
        {
            var points = 0;
            foreach (var card in cards)
            {
                var match = card.Mine.Count(x => card.Winning.Contains(x));
                if (match != 0)
                {
                    points += 1 << (match - 1);
                }
            }
            Console.WriteLine($"Puzzle 1 - total points: {points}");
        }

        private static void Puzzle2(List<Card> cards)
        {
            var copies = new Dictionary<int, int>();
            foreach (var card in cards)
            {
                copies.Add(card.Id, 1);
            }

            foreach (var card in cards)
            {
                var winningCount = card.Mine.Count(x => card.Winning.Contains(x));
                for (var j = 1; j <= winningCount; j++)
                {
                    copies[card.Id + j] += copies[card.Id];
                }
            }

            Console.WriteLine($"Puzzle 2 - total scratchcards: {copies.Values.Sum()}");
        }

        private static List<Card> ParseInput(string[] input)
        {
            var result = new List<Card>();

            var lineNo = 1;
            foreach (var line in input)
            {
                var parts = line.Split(":|".ToCharArray());

                var winning = parts[1]
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                var my = parts[2]
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                result.Add(new Card(lineNo, winning, my));
                lineNo++;   
            }
            return result;
        }
    }
}
