using System.Linq;
using System.Text.RegularExpressions;
using Day05;
using Tools;

var numberRegex = new Regex("(\\d+)");
var lines = File.ReadAllLines("data.txt").ToList();

var a = new IntervalCollection(new List<Interval>() { new Interval(0, 10) });
var b = new IntervalCollection(new List<Interval> { new Interval(10, 10) });

var intersection = a.GetIntersection(b);

var answerA = PartA();
var answerB = PartB();
return;

long PartA()
{
    var seedIntervals = numberRegex.Matches(lines[0]).Select(x => long.Parse(x.Value)).Select(x => new Interval(x, x)).ToList();

    var actions = new List<IntervalCollectionAction>();
    var mapLines = new List<MapLine>();
    foreach (var line in lines.Slice(1, lines.Count - 1))
    {
        if (line.Contains(':'))
        {
            if (mapLines.Any()) actions.Add(new IntervalCollectionAction(mapLines));

            mapLines = new List<MapLine>();
        }
        else if (line.Any())
        {
            var numbers = numberRegex.Matches(line);

            var destinationRangeStart = long.Parse(numbers[0].Value);
            var sourceRangeStart = long.Parse(numbers[1].Value);
            var rangeLength = long.Parse(numbers[2].Value);

            mapLines.Add(new MapLine(destinationRangeStart, sourceRangeStart, rangeLength));
        }
    }

    if (mapLines.Any()) actions.Add(new IntervalCollectionAction(mapLines));
    mapLines = new List<MapLine>();

    var intervalCollection = new IntervalCollection(seedIntervals);

    var paths = numberRegex.Matches(lines[0]).Select(x => long.Parse(x.Value)).Select(x => new Interval(x, x)).Select(x => new List<long> { x.A }).ToList();

    foreach (var action in actions)
    {
        intervalCollection = action.Apply(intervalCollection);

        for (int i = 0; i < intervalCollection.Length; i++)
        {
            paths[i].Add(intervalCollection.Intervals[i].A);
        }
    }

    var lowest = intervalCollection.Intervals.OrderBy(x => x.A).First().A;
    return lowest;
}

long PartB()
{
    var seedNumbers = numberRegex.Matches(lines[0]).Select(x => long.Parse(x.Value)).ToList();

    var seedIntervals = new List<Interval>();
    for (var i = 0; i < seedNumbers.Count / 2; i++)
    {
        var intervalStart = seedNumbers[2 * i];
        var intervalWidth = seedNumbers[2 * i + 1] - 1;

        seedIntervals.Add(new Interval(intervalStart, intervalStart + intervalWidth));
    }

    var actions = new List<IntervalCollectionAction>();
    var mapLines = new List<MapLine>();
    foreach (var line in lines.Slice(1, lines.Count - 1))
    {
        if (line.Contains(':'))
        {
            if (mapLines.Any()) actions.Add(new IntervalCollectionAction(mapLines));

            mapLines = new List<MapLine>();
        }
        else if (line.Any())
        {
            var numbers = numberRegex.Matches(line);

            var destinationRangeStart = long.Parse(numbers[0].Value);
            var sourceRangeStart = long.Parse(numbers[1].Value);
            var rangeLength = long.Parse(numbers[2].Value);

            mapLines.Add(new MapLine(destinationRangeStart, sourceRangeStart, rangeLength));
        }
    }

    if (mapLines.Any()) actions.Add(new IntervalCollectionAction(mapLines));
    mapLines = new List<MapLine>();

    var intervalCollection = new IntervalCollection(seedIntervals);

    foreach (var action in actions)
    {
        intervalCollection = action.Apply(intervalCollection);
    }

    var lowest = intervalCollection.Intervals.OrderBy(x => x.A).First().A;
    return lowest;
}