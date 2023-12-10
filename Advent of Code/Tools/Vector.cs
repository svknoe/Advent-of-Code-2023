namespace Tools;

public struct Vector(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;

    public long ManhattanNorm() => Math.Abs(X) + Math.Abs(Y);

    public static Vector operator +(Vector left, Vector right)
    {
        return new Vector(left.X + right.X, left.Y + right.Y);
    }

    public static Vector operator -(Vector left, Vector right)
    {
        return new Vector(left.X - right.X, left.Y - right.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}