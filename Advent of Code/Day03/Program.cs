using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;
using Day03;

var lines = File.ReadAllLines("data.txt").ToList();

var answerA = PartA();
var answerB = PartB();
return;

int PartA()
{
    var digitRegex = new Regex("(\\d+)");
    var symbolRegex = new Regex(@"([^0-9,\.])");

    var numbers = new List<Number>();
    var symbols = new List<Symbol>();

    for (var i = 0; i < lines.Count; i++)
    {
        var lineNumbers = digitRegex.Matches(lines[i]).Select(m => new Number(m.Value, new Vector2(m.Index, i))).ToList();
        numbers.AddRange(lineNumbers);

        var lineSymbols = symbolRegex.Matches(lines[i]).Where(x => x.Value != ".").Select(m => new Symbol(m.Value, new Vector2(m.Index, i))).ToList();
        symbols.AddRange(lineSymbols);
    }

    foreach (var number in numbers)
    {
        foreach (var symbol in symbols)
        {
            if (number.IsNeighborTo(symbol.Coordinates))
            {
                number.Active = true;
                break;
            }
        }
    }

    var sum = numbers.Where(x => x.Active).Sum(x => x.Value);
    return sum;
}


int PartB()
{
    var digitRegex = new Regex("(\\d+)");
    var gearRegex = new Regex("(\\*)");

    var numbers = new List<Number>();
    var gears = new List<Gear>();

    for (var i = 0; i < lines.Count; i++)
    {
        var lineNumbers = digitRegex.Matches(lines[i]).Select(m => new Number(m.Value, new Vector2(m.Index, i))).ToList();
        numbers.AddRange(lineNumbers);

        var lineGears = gearRegex.Matches(lines[i]).Where(x => x.Value != ".").Select(m => new Gear(new Vector2(m.Index, i))).ToList();
        gears.AddRange(lineGears);
    }

    foreach (var gear in gears)
    {
        foreach (var number in numbers)
        {
            if (number.IsNeighborTo(gear.Coordinates)) gear.Neighbors.Add(number);
        }
    }

    var validGears = gears.Where(gear => gear.Neighbors.Count == 2).ToList();
    var gearRatios = validGears.Select(x => x.GetValue()).ToList();

    var sum = gearRatios.Sum();
    return sum;
}

return;