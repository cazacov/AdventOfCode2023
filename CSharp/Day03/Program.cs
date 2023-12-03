using System.Text.RegularExpressions;
using static Day03.Program;

namespace Day03
{
    internal class Program
    {
        public record Number(long N, int X1, int X2, int Y);
        public record Symbol(char S, int X, int Y);
        public record Gear(Symbol Symbol, List<Number> Numbers);

        static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            var (numbers, symbols) = ParseInput(lines);

            Puzzle1(numbers, symbols);
            Puzzle2(numbers, symbols);
        }

        private static void Puzzle1(IEnumerable<Number> numbers, IEnumerable<Symbol> symbols)
        {

            var partNumbers = numbers
                .Where(num =>
                    symbols.Any(s => IsAdjacent(num, s))
                )
                .Sum(num => num.N);
            
            Console.WriteLine($"Puzzle 1 - Sum of part numbers: {partNumbers}");
        }

        private static void Puzzle2(IEnumerable<Number> numbers, IEnumerable<Symbol> symbols)
        {
            var gears = symbols
                .Select(symbol => 
                    new Gear(
                        symbol, 
                        numbers.Where(n => IsAdjacent(n, symbol)).ToList()
                    )
                )
                .Where(gear => gear.Symbol.S == '*')        // Sacrifice performance for readability
                .Where(gear => gear.Numbers.Count == 2);

            var gearRatios = gears
                .Sum(g => g.Numbers[0].N * g.Numbers[1].N);

            Console.WriteLine($"Puzzle 2 - Sum of gear ratios: {gearRatios}");
        }

        private static bool IsAdjacent(Number number, Symbol symbol)
        {
            return
                symbol.Y >= number.Y - 1 
                && symbol.Y <= number.Y + 1
                && symbol.X >= number.X1 - 1 
                && symbol.X <= number.X2 + 1;
        }

        static ValueTuple<List<Number>, List<Symbol>> ParseInput(string[] lines)
        {
            var numbers = new List<Number>();
            var symbols = new List<Symbol>();


            var regexNumber = new Regex(@"(\d+)");
            var regexSymbol = new Regex(@"([^\d|\.]+)");

            int lineNo = 0;
            foreach (var line in lines)
            {
                var numberMatches = regexNumber.Matches(line);
                foreach (Match match in numberMatches)
                {
                    if (match.Success)
                    {
                        var group = match.Groups[1];
                        numbers.Add(new Number(int.Parse(group.Value), group.Index, group.Index + group.Length - 1, lineNo));
                    }
                }

                var symbolMatches = regexSymbol.Matches(line);
                foreach (Match match in symbolMatches)
                {
                    if (!match.Success) continue;

                    var group = match.Groups[1];
                    symbols.Add(new Symbol(group.Value[0], group.Index,  lineNo));
                }
                lineNo++;
            }

            return (numbers, symbols);
        }
    }
}
