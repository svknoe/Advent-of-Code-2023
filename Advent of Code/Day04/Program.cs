using System.Text.RegularExpressions;

var lines = File.ReadAllLines("data.txt").ToList();
var digitRegex = new Regex("(\\d+)");

var answerA = PartA();
var answerB = PartB();
return;

int PartA()
{
    var sum = 0;

    foreach (var line in lines)
    {
        var numbers = digitRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList();
        var winningNumbers = numbers.GetRange(1, 10);
        var ticketNumbers = numbers.GetRange(11, 25);

        var matches = ticketNumbers.Where(x => winningNumbers.Contains(x));
        sum += (int)Math.Pow(2, matches.Count() - 1);
    }

    return sum;
}

int PartB()
{
    var cards = new List<Card>();
    lines.ForEach(x => cards.Add(new Card()));

    for (var i = 0; i < lines.Count; i++)
    {
        var line = lines[i];
        var numbers = digitRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList();
        var winningNumbers = numbers.GetRange(1, 10);
        var ticketNumbers = numbers.GetRange(11, 25);

        cards[i].Matches = ticketNumbers.Count(x => winningNumbers.Contains(x));
    }

    for (var i = 0; i < cards.Count; i++)
    {
        for (var j = 1; j < cards[i].Matches + 1; j++)
        {
            cards[i + j].Copies += cards[i].Copies;
        }
    }

    var sum = cards.Sum(x => x.Copies);
    return sum;
}

public class Card
{
    public int Matches { get; set; }
    public int Copies { get; set; } = 1;
}