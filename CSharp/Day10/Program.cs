namespace Day10
{
    internal partial class Program
    {
        static void Main()
        {
            var lines = File.ReadAllLines("input.txt");
            var (animal, graph) = ParseInput(lines);
            Puzzle1(animal, graph);
            Puzzle2(animal, graph);
        }

        private static void Puzzle1(Node animal, Dictionary<Pos, Node> graph)
        {
            var pipe = new HashSet<Pos>
            {
                animal.Pos
            };
            var check = new HashSet<Pos>(animal.Neighbours.Select(x => x.Pos));

            while (check.Any())
            {
                var c= check.First();
                var node = graph[c];
                foreach (var n in node.Neighbours)
                {
                    if (!pipe.Contains(n.Pos))
                    {
                        check.Add(n.Pos);
                    }
                }
                pipe.Add(c);
                check.Remove(c);
            }
            
            Console.WriteLine($"Puzzle 1 - path length to the point farthest from the starting position: {pipe.Count/2}");

        }

        private static void Puzzle2(Node animal, Dictionary<Pos, Node> graph)
        {
            var pipe = new HashSet<Pos>
            {
                animal.Pos
            };
            var check = new HashSet<Pos>(animal.Neighbours.Select(x => x.Pos));

            while (check.Any())
            {
                var c = check.First();
                var node = graph[c];
                foreach (var n in node.Neighbours)
                {
                    if (!pipe.Contains(n.Pos))
                    {
                        check.Add(n.Pos);
                    }
                }
                pipe.Add(c);
                check.Remove(c);
            }

            var map = new bool[280, 280];
            foreach (var pos in pipe)
            {
                var node = graph[pos];
                map[pos.X * 2, pos.Y * 2] = true;
                foreach (var dir in node.Directions)
                {
                    var next = node.Pos.Go(dir);
                    map[(pos.X * 2 + next.X * 2) / 2, (pos.Y * 2 + next.Y * 2) / 2] = true;
                }
            }

            // paint outer

            var outerHiRes = new HashSet<Pos>();
            var current = new Pos(0, 0);
            var check2 = new HashSet<Pos>() { current };
            while (check2.Any())
            {
                var c = check2.First();
                for (int d = 0; d < 4; d++)
                {
                    var next = c.Go(d);
                    if (next.X < 0 || next.Y < 0 || next.X == 280 || next.Y == 280)
                    {
                        continue;
                    }

                    if (map[next.X, next.Y])
                    {
                        continue;
                    }
                    if (outerHiRes.Contains(next))
                    {
                        continue;
                    }
                    if (check2.Contains(next))
                    {
                        continue;
                    }
                    check2.Add(next);
                }
                outerHiRes.Add(c);
                check2.Remove(c);
            }

            // Remove odd coordinates
            var outer = outerHiRes
                .Where(pos => (pos.X & 0x01 | pos.Y & 0x01) == 0)
                .Select(p => new Pos(p.X >> 1, p.Y >> 1))
                .ToList();

            var total = 140 * 140;

            Console.WriteLine($"Puzzle 2 - Tiles enclosed by the pipe loop: {total - pipe.Count - outer.Count}");

            var cl = Console.ForegroundColor;
            for (var y = 0; y < 140; y++)
            {
                for (var x = 0; x < 140; x++)
                {
                    var pos = new Pos(x, y);
                    if (pipe.Contains(pos))
                    {
                        if (pos.Equals(animal.Pos))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("(\u2588)");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(RenderPipe(graph[pos].Type));
                        }
                    }
                    else if (outer.Contains(pos))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" . ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\u2588\u2588\u2588");
                    }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = cl;
        }

        private static string RenderPipe(char symbol)
        {
            switch (symbol)
            {
                case '-':
                    return "───";
                case '|':
                    return " │ ";
                case 'F':
                    return " ┌─";
                case '7':
                    return "─┐ ";
                case 'J':
                    return "─┘ ";
                case 'L':
                    return " └─";
            }
            return "   ";
        }


        private static (Node animal, Dictionary<Pos, Node> graph) ParseInput(string[] lines)
        {
            // Add pipes
            var result = new Dictionary<Pos,Node>();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (lines[y][x] == '.' || lines[y][x] == 'S')
                    {
                        continue;
                    }
                    var node = new Node()
                    {
                        Pos = new Pos(x, y),
                        Type = lines[y][x]
                    };
                    result[node.Pos] = node;
                }
            }

            // Add animal
            var animal = new Node()
            {
                Pos = new Pos(32, 25),
                Type = '-',
                Neighbours = new List<Node>()
            };
            result[animal.Pos] = animal;

            // Fill Directions
            foreach (var pair in result)
            {
                var node = pair.Value;
                var directions = new List<int>();
                switch (pair.Value.Type)
                {
                    case '|':
                        directions.Add(0);
                        directions.Add(2);
                        break;
                    case '-':
                        directions.Add(1);
                        directions.Add(3);
                        break;
                    case 'L':
                        directions.Add(0);
                        directions.Add(1);
                        break;
                    case 'J':
                        directions.Add(0);
                        directions.Add(3);
                        break;
                    case '7':
                        directions.Add(3);
                        directions.Add(2);
                        break;
                    case 'F':
                        directions.Add(1);
                        directions.Add(2);
                        break;
                }

                node.Directions = directions;

            }

            // Connect pipes
            foreach (var pair in result)
            {
                var node = pair.Value;
                foreach (var d in node.Directions)
                {
                    var newPos = node.Pos.Go(d);
                    if (result.ContainsKey(newPos) && result[newPos].Directions.Contains((d + 2) % 4))
                    {
                        node.Neighbours.Add(result[newPos]);
                    }
                }
            }

            return (animal, result);
        }
    }
}
