using System.Drawing;
using System.Text.RegularExpressions;

var colors = new[] { "red", "green", "blue" };
var regexDictionary = colors.ToDictionary(c => c, c => new Regex($"(?<= )(\\d+)(?= {c})"));
var lines = File.ReadAllLines("data.txt").ToList();

var answerA = PartA();
var answerB = PartB();
return;

int PartA()
{
    var gameRegex = new Regex("(?<=Game )(.*)(?=:)");
    var limitDictionary = new Dictionary<string, int> { { "red", 12 }, { "green", 13 }, { "blue", 14 } };

    return lines.Sum(line => colors.Any(color => HasTooManyCubes(line, color)) ? 0 : int.Parse(gameRegex.Match(line).Value));

    bool HasTooManyCubes(string line, string color) { return regexDictionary[color].Matches(line).Any(match => int.Parse(match.Value) > limitDictionary[color]); }
}

int PartB()
{
    var powerSum = 0;

    foreach (var line in lines)
    {
        var linePower = 1;

        foreach (var color in colors)
        {
            var matches = regexDictionary[color].Matches(line).Select(x => int.Parse(x.Value)).ToList();
            var maxColorIncidence = matches.Any() ? matches.Max() : 0;
            linePower *= maxColorIncidence;
        }

        powerSum += linePower;
    }

    return powerSum;
}