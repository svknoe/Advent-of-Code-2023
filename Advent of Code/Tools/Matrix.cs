namespace Tools;

public struct Matrix<T>(List<List<T>> rows)
{
    public List<List<T>> Rows { get; set; } = rows;

    public T GetValueAt(Vector coordinates)
    {
        var x = (int)coordinates.X;
        var y = (int)coordinates.Y;

        return Rows[x][y];
    }

    public Vector GetFirst(T target)
    {
        for (var i = 0; i < Rows.Count; i++)
        {
            for (var j = 0; j < Rows[i].Count; j++)
            {
                if (Rows[i][j].Equals(target)) return new Vector(i, j);
            }
        }

        throw new Exception();
    }
}