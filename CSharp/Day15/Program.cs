namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0];
            Puzzle1(input);
            Puzzle2(input);
        }

        public class Lens(string Label, int FocalLength)
        {
            public string Label { get; init; } = Label;
            public int FocalLength { get; set; } = FocalLength;
        }

        private static void Puzzle1(string input)
        {
            var steps = input.Split(",");
            var result = 0;

            foreach (var step in steps)
            {
                result += Hash(step);
            }
            Console.WriteLine($"Puzzle 1: {result}");
        }

        private static void Puzzle2(string input)
        {
            var boxes = new List<List<Lens>>();
            for (var i = 0; i < 256; i++)
            {
                boxes.Add(new List<Lens>());
            }

            var steps = input.Split(",");
            foreach (var step in steps)
            {
                if (step.Contains("-"))
                {
                    var label = step.Substring(0, step.Length - 1);
                    var boxIndex = Hash(label);
                    boxes[boxIndex].RemoveAll(b => b.Label == label);
                }
                else
                {
                    var parts = step.Split("=");
                    var label = parts[0];
                    var focalLength = int.Parse(parts[1]);
                    var boxIndex = Hash(label);
                    var lens = boxes[boxIndex].FirstOrDefault(l => l.Label == label);
                    if (lens == null)
                    {
                        lens = new Lens(label, focalLength);
                        boxes[boxIndex].Add(lens);
                    }
                    else
                    {
                        lens.FocalLength = focalLength;
                    }
                }
            }

            var result = 0L;
            for (var i = 0; i < 256; i++)
            {
                for (var j = 0; j < boxes[i].Count; j++)
                {
                    result += (i + 1) * (j + 1) * boxes[i][j].FocalLength;
                }
            }
            Console.WriteLine($"Puzzle 2: {result}");
        }


        private static int Hash(string step)
        {
            var result = 0;
            foreach (var ch in step)
            {
                result += (int)ch;
                result *= 17;
                result %= 256;
            }
            return result;
        }
    }
}
