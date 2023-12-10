namespace Day10;

internal partial class Program
{
    public class Pos()
    {
        public Pos(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        protected bool Equals(Pos other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pos)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public int X;
        public int Y;

        public Pos Go(int d)
        {
            var dx = new int[] { 0, 1, 0, -1 };
            var dy = new int[] { -1, 0, 1, 0 };

            return new Pos(X + dx[d], Y + dy[d]);
        }
    }
}