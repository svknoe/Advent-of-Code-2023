using Day07;

var lines = File.ReadAllLines("data.txt").ToList();
var hands = lines.Select(line => line.Split(' ')).Select(x => new Hand(x.First().ToList(), int.Parse(x.Last()))).ToList();

var answerA = PartA();
var answerB = PartB();
return;

int PartA()
{
    var orderedHands = hands.OrderByDescending(hand => hand, new HandComparer(false)).ToList();

    var totalWinnings = 0;
    for (var i = 0; i < orderedHands.Count; i++)
    {
        var bid = orderedHands[i].Bid;
        var rank = (orderedHands.Count - i);

        totalWinnings += bid * rank;
    }

    return totalWinnings;
}

int PartB()
{
    var orderedHands = hands.OrderByDescending(hand => hand, new HandComparer(true)).ToList();

    var totalWinnings = 0;
    for (var i = 0; i < orderedHands.Count; i++)
    {
        var bid = orderedHands[i].Bid;
        var rank = (orderedHands.Count - i);

        totalWinnings += bid * rank;
    }

    return totalWinnings;
}