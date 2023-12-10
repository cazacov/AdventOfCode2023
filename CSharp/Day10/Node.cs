namespace Day10;

internal partial class Program
{
    public class Node()
    {
        public Pos Pos;
        public char Type;
        public List<Node> Neighbours = new List<Node>();
        public List<int> Directions = new List<int>();
    }
}