using System.Text.RegularExpressions;

var numberRegex = new Regex("(-?\\d+)");
var lines = File.ReadAllLines("data.txt").ToList();

var answerA = PartA();
var answerB = PartB();
return;

long PartA()
{
    long sum = 0;

    foreach (var line in lines)
    {
        var numbers = numberRegex.Matches(line).Select(x => long.Parse(x.Value)).ToList();
        var matrix = new List<List<long>> { numbers };

        var previousRow = numbers;

        while (previousRow.Any(x => x != 0))
        {
            var nextRow = new long[previousRow.Count - 1].ToList();

            for (var i = 0; i < previousRow.Count - 1; i++)
            {
                nextRow[i] = previousRow[i + 1] - previousRow[i];
            }

            matrix.Add(nextRow);
            previousRow = nextRow;
        }

        foreach (var row in matrix)
        {
            row.Add(0);
        }

        for (var i = matrix.Count - 2; i >= 0; i--)
        {
            matrix[i][^1] = matrix[i + 1][^1] + matrix[i][^2];
        }

        var value = matrix[0][^1];

        if (value > 1e9) { }

        sum += value;
    }

    return sum;
}

long PartB()
{
    long sum = 0;

    foreach (var line in lines)
    {
        var numbers = numberRegex.Matches(line).Select(x => long.Parse(x.Value)).ToList();
        var matrix = new List<List<long>> { numbers };

        var previousRow = numbers;

        while (previousRow.Any(x => x != 0))
        {
            var nextRow = new long[previousRow.Count - 1].ToList();

            for (var i = 0; i < previousRow.Count - 1; i++)
            {
                nextRow[i] = previousRow[i + 1] - previousRow[i];
            }

            matrix.Add(nextRow);
            previousRow = nextRow;
        }

        foreach (var row in matrix)
        {
            row.Insert(0, 0);
        }

        for (var i = matrix.Count - 2; i >= 0; i--)
        {
            matrix[i][0] = -matrix[i + 1][0] + matrix[i][1];
        }

        var value = matrix[0][0];

        if (value > 1e9) { }

        sum += value;
    }

    return sum;
}