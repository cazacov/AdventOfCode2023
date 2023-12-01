using static System.Char;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt").ToList();
            
            Puzzle1(input);
            Puzzle2(input);
        }

        private static void Puzzle1(List<string> input)
        {
            var sum = input
                .Select(line => line.First(IsDigit).ToString() + line.Last(IsDigit))
                .Select(int.Parse)
                .Sum();
            Console.WriteLine($"Puzzle 1: {sum}");
        }

        private static void Puzzle2(List<string> input)
        {
            var values = input.Select(CalibrationValue);
            var sum = values
                .Select(int.Parse)
                .Sum();
            Console.WriteLine($"Puzzle 2: {sum}");
        }

        private static string CalibrationValue(string line)
        {
            var digits = new Dictionary<string, char>()
            {
                { "one", '1' },
                { "two", '2' },
                { "three", '3' },
                { "four", '4' },
                { "five", '5' },
                { "six", '6' },
                { "seven", '7' },
                { "eight", '8' },
                { "nine", '9' }
            };

            var first = ' ';
            for (var i = 0; i < line.Length; i++)
            {
                if (IsDigit(line[i]))
                {
                    first = line[i];
                    break;
                }
                foreach (var replacement in digits)
                {
                    if (line.IndexOf(replacement.Key) == i)
                    {
                        first = replacement.Value; 
                        goto find_last;
                    }
                }
            }

        find_last:
            var last = ' ';
            for (var i = line.Length - 1; i >= 0; i--)
            {
                if (IsDigit(line[i]))
                {
                    last = line[i];
                    break;
                }
                foreach (var replacement in digits)
                {
                    if (line.LastIndexOf(replacement.Key) == i)
                    {
                        last = replacement.Value;
                        goto done;
                    }
                }
            }

        done:
            return $"{first}{last}";
        }
    }
}
