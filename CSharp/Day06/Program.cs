namespace Day06
{
    internal class Program
    {
        public record Race(long Time, long Distance);

        static void Main()
        {
            /*
            var races1 = new List<Race>()
            {
                new Race(7, 9),
                new Race(15, 40),
                new Race(30, 200),
            };
            */
            
            var races1 = new List<Race>()
            {
                new(52, 426),
                new(94, 1374),
                new(75, 1279),
                new(94, 1216)
            };
            Console.WriteLine($"Puzzle 1: {races1.Aggregate<Race, long>(1, (current, race) => current * WaysToWin(race))}");

            
            var races2 = new List<Race>()
            {
                new(52947594L, 426137412791216L),
            };
            Console.WriteLine($"Puzzle 2: {races2.Aggregate<Race, long>(1, (current, race) => current * WaysToWin(race))}");
        }
        
        private static long WaysToWin(Race race)
        {
            const double epsilon = 1E-10;
            var minTime = Math.Ceiling((race.Time - Math.Sqrt(race.Time * race.Time - 4 * race.Distance)) / 2 + epsilon);
            var maxTime = Math.Floor((race.Time + Math.Sqrt(race.Time * race.Time - 4 * race.Distance)) / 2 - epsilon);

            return (long)(maxTime - minTime + 1);
        }
    }
}
