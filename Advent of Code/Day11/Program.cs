using Tools;

var lines = File.ReadAllLines("data.txt").ToList();
var charRows = lines.Select(x => x.ToCharArray().ToList()).ToList();

var answerA = PartA();
var answerB = PartB();
return;

long PartA()
{
    var matrix = new Matrix<char>(charRows);
    
    PadMatrixRows();
    matrix = matrix.GetTransposed();
    PadMatrixRows();
    matrix = matrix.GetTransposed();

    var galaxies = new List<Vector>();
    for (var i = 0; i < matrix.Rows.Count; i++)
    {
        for (var j = 0; j < matrix.Rows[i].Count; j++)
        {
            if (matrix.Rows[i][j] == '#') galaxies.Add(new Vector(i, j));
        }
    }

    long manhattanSum = 0;
    for (var i = 0; i < galaxies.Count - 1; i++)
    {
        for (var j = i + 1; j < galaxies.Count; j++)
        {
            manhattanSum += (galaxies[j] - galaxies[i]).ManhattanNorm();
        }
    }

    return manhattanSum;

    void PadMatrixRows()
    {
        for (var i = matrix.Rows.Count - 1; i >= 0; i--)
        {
            var row = matrix.Rows[i];
            if (row.All(x => x == '.')) matrix.Rows.Insert(i + 1, row);
        }
    }
}

long PartB()
{
    var matrix = new Matrix<char>(charRows);
    
    var galaxyRows = new List<List<Vector?>>();
    for (var i = 0; i < matrix.Rows.Count; i++)
    {
        var galaxyRow = new List<Vector?>();

        for (var j = 0; j < matrix.Rows[i].Count; j++)
        {
            if (matrix.Rows[i][j] == '#') galaxyRow.Add(new Vector(i, j));
            else galaxyRow.Add(null);
        }

        galaxyRows.Add(galaxyRow);
    }

    var vectorMatrix = new Matrix<Vector?>(galaxyRows);
    
    SeparateGalaxies(false);
    matrix = matrix.GetTransposed();
    vectorMatrix = vectorMatrix.GetTransposed();
    SeparateGalaxies(true);
    vectorMatrix = vectorMatrix.GetTransposed();

    var galaxies = vectorMatrix.Rows.SelectMany(x => x).OfType<Vector>().ToList();

    long manhattanSum = 0;
    for (var i = 0; i < galaxies.Count - 1; i++)
    {
        for (var j = i + 1; j < galaxies.Count; j++)
        {
            manhattanSum += (galaxies[j] - galaxies[i]).ManhattanNorm();
        }
    }

    return manhattanSum;

    void SeparateGalaxies(bool isTransposed)
    {
        var offsetFactor = (long)1e6;

        for (var i = 0; i < matrix.Rows.Count; i++)
        {
            if (matrix.Rows[i].Any(x => x != '.')) continue;

            for (var j = i + 1; j < vectorMatrix.Rows.Count; j++)
            {
                var vectorRow = matrix.Rows[j];

                for (var k = 0; k < vectorRow.Count; k++)
                {
                    if (vectorMatrix.Rows[j][k] is not Vector galaxy) continue;

                    var offset = isTransposed ? new Vector(0, offsetFactor - 1) : new Vector(offsetFactor - 1, 0);
                    vectorMatrix.Rows[j][k] = galaxy + offset;
                }
            }
        }
    }
}

