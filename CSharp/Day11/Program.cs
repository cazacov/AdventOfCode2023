namespace Day11
{
    internal class Program
    {
        public class Galaxy(int X, int Y)
        {
            public int X { get; set; } = X;
            public int Y { get; set; } = Y;
        }

        static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            
            // Puzzle 1
            var galaxies = FindGalaxies(lines);
            ExpandUniverse(galaxies, 2);
            Console.WriteLine($"Puzzle 1 - Sum of distances after Universe expansion: {CountDistances(galaxies)}");

            // Puzzle 2
            var galaxies2 = FindGalaxies(lines);
            ExpandUniverse(galaxies2, 1000000);
            Console.WriteLine($"Puzzle 2 - Sum of distances after Universe expansion: {CountDistances(galaxies2)}");
        }

        private static List<Galaxy> FindGalaxies(string[] lines)
        {
            var galaxies = new List<Galaxy>();
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        galaxies.Add(new Galaxy(x, y));
                    }
                }
            }
            return galaxies;
        }

        private static void ExpandUniverse(List<Galaxy> galaxies, int scale)
        {
            var maxX = galaxies.Max(g => g.X);
            var maxY = galaxies.Max(g => g.Y);

            // Expand Y
            var emptyRows = new List<int>();
            for (var y = 0; y < maxY; y++)
            {
                if (galaxies.All(g => g.Y != y))
                {
                    emptyRows.Add(y);
                }
            }
            foreach (var galaxy in galaxies)
            {
                galaxy.Y += emptyRows.Count(er => er < galaxy.Y) * (scale - 1);
            }

            // Expand X
            var emptyColumns = new List<int>();
            for (var x = 0; x < maxX; x++)
            {
                if (galaxies.All(g => g.X != x))
                {
                    emptyColumns.Add(x);
                }
            }
            foreach (var galaxy in galaxies)
            {
                galaxy.X += emptyColumns.Count(ec => ec < galaxy.X) * (scale - 1);
            }
        }

        private static long CountDistances(List<Galaxy> galaxies)
        {
            var s = 0L;
            foreach (var first in galaxies)
            {
                foreach (var second in galaxies)
                {
                    // Manhattan distance
                    s += Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y);
                }
            }
            // We counted every pair twice
            return s / 2; 
        }
    }
}
