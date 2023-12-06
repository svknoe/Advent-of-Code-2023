using System.Text.RegularExpressions;

var numberRegex = new Regex("(\\d+)");
var lines = File.ReadAllLines("data.txt").ToList();

var seeds = numberRegex.Matches(lines[0]).Select(x => int.Parse(x.Value)).ToList();