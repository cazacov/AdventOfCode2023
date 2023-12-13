namespace Day13
{
    internal class Program
    {
        public record Lava(bool[,] Map, int MaxX, int MaxY, int Nr);

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var input = ParseInput(lines);

            Puzzle1(input);
            Puzzle2(input);
        }

        private static void Puzzle1(List<Lava> input)
        {
            long result = 0;


            foreach (var map in input)
            {
                var x = FindVerticalMirror(map).FirstOrDefault();
                var y = FindHorizontalMirror(map).FirstOrDefault();
                result += x + y * 100;

            }
            Console.WriteLine($"Puzzle 1: {result}");
        }


        private static void Puzzle2(List<Lava> input)
        {
            long result = 0;

            foreach (var map in input)
            {
                var mirX = FindVerticalMirror(map);
                var mirY = FindHorizontalMirror(map);
                bool smudgeFound = false;
                for (var y = 0; y < map.MaxY && !smudgeFound; y++)
                {
                    for (var x = 0; x < map.MaxX && !smudgeFound; x++)
                    {
                        map.Map[y, x] = !map.Map[y, x];

                        var newMirX = FindVerticalMirror(map);
                        var newMirY = FindHorizontalMirror(map);

                        var newX = newMirX.Where(mir => !mirX.Contains(mir)).ToList();
                        var newY = newMirY.Where(mir => !mirY.Contains(mir)).ToList();

                        foreach (var nx in newX)
                        {
                            result += nx;
                            smudgeFound = true;
                            break;
                        }

                        foreach (var ny in newY)
                        {
                            result += ny * 100;
                            smudgeFound = true;
                            break;
                        }
                        map.Map[y, x] = !map.Map[y, x];
                    }
                }
            }
            Console.WriteLine($"Puzzle 2: {result}");
        }



        private static List<int> FindVerticalMirror(Lava map)
        {
            var result = new List<int>();


            for (var x = 1; x < map.MaxX; x++)
            {
                var found = true;
                var dx = Math.Min(x, map.MaxX - x);
                for (var xx = x - dx; xx < x; xx++)
                {
                    var rx = x + (x - xx) - 1;
                    if (rx >= map.MaxX)
                    {
                        continue;
                    }
                    for (var y = 0; y < map.MaxY; y++)
                    {
                        if (map.Map[y, xx] != map.Map[y, rx])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (!found)
                    {
                        break;
                    }
                }
                if (found)
                {
                    result.Add(x);
                }
            }
            return result;
        }

        private static List<int> FindHorizontalMirror(Lava map)
        {
            var result = new List<int>();

            for (var y = 1; y < map.MaxY; y++)
            {
                var found = true;
                for (var yy = 0; yy < y; yy++)
                {
                    var ry = y + (y - yy) - 1;
                    if (ry >= map.MaxY)
                    {
                        continue;
                    }
                    for (var x = 0; x < map.MaxX; x++)
                    {
                        if (map.Map[yy, x] != map.Map[ry, x])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (!found)
                    {
                        break;
                    }
                }

                if (found)
                {
                    result.Add(y);
                }
            }
            return result;
        }

        private static List<Lava> ParseInput(string[] lines)
        {
            var result = new List<Lava>();
            var inp = new List<string>();
            var nr = 0;
            foreach (var line in lines)
            {
                if (line.Trim() == String.Empty)
                {
                    ProcessLava(inp, result, nr++);
                    inp.Clear();
                }
                else
                {
                    inp.Add(line);
                }
            }

            if (inp.Count > 0)
            {
                ProcessLava(inp, result, nr);
            }
            return result;
        }

        private static void ProcessLava(List<string> inp, List<Lava> result, int nr)
        {
            var maxy = inp.Count;
            var maxx = inp[0].Length;

            var map = new bool[maxy, maxx];

            for (var y = 0; y < maxy; y++)
            {
                for (var x = 0; x < maxx; x++)
                {
                    if (inp[y][x] == '#')
                    {
                        map[y, x] = true;
                    }
                }
            }
            result.Add(new Lava(map, maxx, maxy, nr));
        }
    }
}
