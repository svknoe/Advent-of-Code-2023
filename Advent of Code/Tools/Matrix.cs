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

public static class MatrixExtensions
{
    public static Matrix<T> GetTransposed<T>(this Matrix<T> matrix)
    {
        var transposed = new Matrix<T>(new());

        for (var index = 0; index < matrix.Rows[0].Count; index++)
        {
            transposed.Rows.Add(matrix.Rows.Select(x => default(T)).ToList() as List<T>);
        }

        for (var i = 0; i < matrix.Rows.Count; i++)
        {
            for (var j = 0; j < matrix.Rows[0].Count; j++)
            {
                transposed.Rows[j][i] = matrix.Rows[i][j];
            }
        }

        return transposed;
    }
}