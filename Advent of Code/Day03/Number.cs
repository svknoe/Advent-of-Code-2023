using System.Drawing;
using System.Numerics;

namespace Day03
{
    public class Number
    {
        public const double tolerance = 1e-3;

        public int Value { get; set; }
        public bool Active { get; set; }
        public List<Vector2> Coordinates { get; set; }


        public Number(string numberString, Vector2 startCoordinate)
        {
            Value = int.Parse(numberString);

            var coordinates = new List<Vector2> { startCoordinate };
            for (var i = 1; i < numberString.Length; i++)
            {
                coordinates.Add(startCoordinate + new Vector2(i,0));
            }

            Coordinates = coordinates;
        }

        public bool IsNeighborTo(Vector2 point)
        {
            foreach (var coordinate in Coordinates)
            {
                var difference = point - coordinate;
                if (Math.Abs(Math.Abs(difference.X)) <= 1 && Math.Abs(Math.Abs(difference.Y)) <= 1) return true;
            }

            return false;
        }
    }
}
