using System.Numerics;

namespace Day03
{
    public class Symbol
    {
        public char Value { get; set; }
        public Vector2 Coordinates { get; set; }


        public Symbol(string symbolChar, Vector2 startCoordinates)
        {
            Value = char.Parse(symbolChar);
            Coordinates = startCoordinates;
        }
    }

    public class Gear
    {
        public List<Number> Neighbors { get; set; } = new();
        public Vector2 Coordinates { get; set; }

        public Gear(Vector2 coordinates)
        {
            Coordinates = coordinates;
        }

        public int GetValue()
        {
            if (Neighbors.Count != 2) return 0;

            var value = 1;
            Neighbors.ForEach(n => value *= n.Value);

            return value;
        }
    }
}
