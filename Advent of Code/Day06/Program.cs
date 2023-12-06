using System.Text.RegularExpressions;

var numberRegex = new Regex("(\\d+)");
var lines = File.ReadAllLines("data.txt").ToList();

var answerA = PartA();
var answerB = PartB();

return;

int PartA()
{
    var durations = numberRegex.Matches(lines[0]).Select(x => x.Value).ToList();
    var distances = numberRegex.Matches(lines[1]).Select(x => x.Value).ToList();

    var product = 1;

    for (var i = 0; i < durations.Count; i++)
    {
        var duration = double.Parse(durations[i]);
        var targetDistance = double.Parse(distances[i]);

        var minSpeed = (int)Math.Ceiling(targetDistance / duration);
        while (minSpeed * (duration - minSpeed) < targetDistance) minSpeed += 1;

        var maxTime = duration;
        while (maxTime * (duration - maxTime) < targetDistance) maxTime -= 1;

        var combinations = (int)maxTime - minSpeed + 1;
        product *= combinations;
    }

    return product;
}

int PartB()
{
    var duration = double.Parse(numberRegex.Matches(lines[0]).Select(x => x.Value).Aggregate((x,y) => x + y));
    var targetDistance = double.Parse(numberRegex.Matches(lines[1]).Select(x => x.Value).Aggregate((x,y) => x + y));

    var minSpeed = (int)Math.Ceiling(targetDistance / duration);
    while (minSpeed * (duration - minSpeed) < targetDistance) minSpeed += 1;

    var maxTime = duration;
    while (maxTime * (duration - maxTime) < targetDistance) maxTime -= 1;

    var combinations = (int)maxTime - minSpeed + 1;
    return combinations;
}