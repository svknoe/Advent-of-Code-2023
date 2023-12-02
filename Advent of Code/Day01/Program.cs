var digitDictionary = new Dictionary<string, int> { {"zero", 0}, {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9} };

var lines = File.ReadAllLines("data.txt").ToList();
var digitLists = lines.Select(ToDigitList).Where(Enumerable.Any).ToList();
var concatenatedDigits = digitLists.Select(x => x.First() + x.Last()).ToList();
var numbers = concatenatedDigits.Select(int.Parse).ToList();
var sum = numbers.Sum();

return sum;

List<string> ToDigitList(string line)
{
    var digitList = new List<string>();

    for (var index = 0; index < line.Length; index++)
    {
        if (AsDigit(line, index) is string digit) digitList.Add(digit);
    }

    return digitList;
}

string? AsDigit(string line, int index)
{
    var character = line[index];

    for (var i = 0; i < 10; i++)
    {
        if (character.ToString() == i.ToString()) return i.ToString();
    }

    foreach (var pair in digitDictionary)
    {
        if (line.Length >= index + pair.Value && line.Substring(index, pair.Key.Length) == pair.Key) return pair.Value.ToString();
    }

    return null;
}