namespace Day07
{
    internal partial class Program
    {

        static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            var hands = input.Select(line => new Hand(line, false)).ToList();
            Console.WriteLine($"Puzzle 1: {CalculateWinnings(hands)}");

            hands = hands = input.Select(line => new Hand(line, true)).ToList();
            Console.WriteLine($"Puzzle 2: {CalculateWinnings(hands)}");
        }

        private static long CalculateWinnings(List<Hand> hands)
        {

            long result = 0;

            hands.Sort(Comparison);
            for (int i = 0; i < hands.Count; i++)
            {
                var rank = i + 1;
                result += hands[i].Bid * rank;
            }

            return result;
        }

        private static int Comparison(Hand first, Hand second)
        {
            if (first.Strength != second.Strength)
            {
                var s1 = (int)first.Strength;
                var s2 = (int)second.Strength;

                return s1.CompareTo(s2);
            }
            else
            {
                return string.Compare(first.Cards, second.Cards);
            }
        }
    }
}
