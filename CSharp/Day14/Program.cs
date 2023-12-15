using System.Text;

namespace Day14
{
    internal class Program
    {
        enum State
        {
            Empty,
            Round,
            Cube
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var map = ParseInput(lines);
            Puzzle1(map);

            map = ParseInput(lines);
            Puzzle2(map);
        }

        private static void Puzzle1(State[,] map)
        {
            TiltNorth(map);
            Console.WriteLine($"Puzzle 1: {LoadOnNorth(map)}");
        }

        private static void Puzzle2(State[,] map)
        {

            var hash = new Dictionary<string, int>();
            string str = string.Empty;
            int n = 0;
            do
            {
                TiltNorth(map);
                TiltWest(map);
                TiltSouth(map);
                TiltEast(map);
                n++;
                str = MapToString(map);
                if (hash.ContainsKey(str))
                {
                    break;
                }
                hash[str] = n;
            } while (true);

            var loop = n - hash[str];
            var rest = (1000000000L - n - 1) % loop;

            for (int i = 0; i <= rest; i++)
            {
                TiltNorth(map);
                TiltWest(map);
                TiltSouth(map);
                TiltEast(map);
            }

            Console.WriteLine($"Puzzle 2: {LoadOnNorth(map)}");
        }

        private static string MapToString(State[,] map)
        {
            var maxY = map.GetLength(0);
            var maxX = map.GetLength(1);

            var result = new StringBuilder();

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    switch (map[y, x])
                    {
                        case State.Empty:
                            result.Append(".");
                            break;
                        case State.Round:
                            result.Append("O");
                            break;
                        case State.Cube:
                            result.Append("#");
                            break;
                    }
                }
            }
            return result.ToString();
        }

        private static int LoadOnNorth(State[,] map)
        {
            var maxY = map.GetLength(0);
            var maxX = map.GetLength(1);

            int result = 0;
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (map[y, x] == State.Round)
                    {
                        result += maxY - y;
                    }
                }
            }
            return result;
        }

        private static void TiltNorth(State[,] map)
        {
            for (var y = 1; y < map.GetLength(0); y++)
            {
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == State.Round)
                    {
                        var j = y;
                        while (j > 0)
                        {
                            if (map[j-1, x] == State.Empty)
                            {
                                map[j - 1, x] = State.Round;
                                map[j, x] = State.Empty;
                            }
                            else
                            {
                                break;
                            }
                            j--;
                        }
                        
                    }
                }
            }
        }

        private static void TiltSouth(State[,] map)
        {
            var maxY = map.GetLength(0) - 2;
            var maxX = map.GetLength(1);
            for (var y = maxY; y >=0; y--)
            {
                for (var x = 0; x < maxX; x++)
                {
                    if (map[y, x] == State.Round)
                    {
                        var j = y;
                        while (j <= maxY)
                        {
                            if (map[j + 1, x] == State.Empty)
                            {
                                map[j + 1, x] = State.Round;
                                map[j, x] = State.Empty;
                            }
                            else
                            {
                                break;
                            }
                            j++;
                        }

                    }
                }
            }
        }

        private static void TiltWest(State[,] map)
        {
            var maxX = map.GetLength(1);
            var maxY = map.GetLength(0);
            for (var x = 1; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    if (map[y, x] == State.Round)
                    {
                        var j = x;
                        while (j > 0)
                        {
                            if (map[y, j - 1] == State.Empty)
                            {
                                map[y, j - 1] = State.Round;
                                map[y, j] = State.Empty;
                            }
                            else
                            {
                                break;
                            }
                            j--;
                        }

                    }
                }
            }
        }

        private static void TiltEast(State[,] map)
        {
            int maxX = map.GetLength(1) - 2;
            var maxY = map.GetLength(0);
            for (var x = maxX; x >= 0; x--)
            {
                for (var y = 0; y < maxY; y++)
                {
                    if (map[y, x] == State.Round)
                    {
                        var j = x;
                        while (j <= maxX)
                        {
                            if (map[y, j + 1] == State.Empty)
                            {
                                map[y, j + 1] = State.Round;
                                map[y, j] = State.Empty;
                            }
                            else
                            {
                                break;
                            }
                            j++;
                        }

                    }
                }
            }
        }

        private static State [,] ParseInput(string[] lines)
        {
            var maxX = lines.First().Length;
            var maxY = lines.Length;

            var result = new State[maxX, maxX];

            for (int y = 0; y < maxY; y++)
            {
                var line = lines[y];
                for (int x = 0; x < maxX; x++)
                {
                    State cell = State.Empty;
                    switch (line[x])
                    {
                        case '.':
                            cell = State.Empty;
                            break;
                        case 'O':
                            cell = State.Round;
                            break;
                        case '#':
                            cell = State.Cube;
                            break;
                    }
                    result[y, x] = cell;
                }
            }
            return result;
        }
    }
}
