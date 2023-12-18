using Day12;
// ReSharper disable ParameterTypeCanBeEnumerable.Local

var lines = File.ReadAllLines("data.txt").ToList();

foreach (var line in lines)
{
    var split = line.Split(' ');
    var springString = split[0];
    var damagedGroupSizes = split[1].Split(',').Select(int.Parse).ToList();

    var groups = InitialiseGroups(springString);

    var groupStrings = groups.Select(g => g.Springs.ToList()).ToList();
    FillGroups(groups, damagedGroupSizes);
    for (var i = 0; i < groups.Count; i++) groups[i].Springs = groupStrings[i];

}

return;

List<Group> InitialiseGroups(string springString)
{
    var groups = new List<Group>();
    var group = new Group();
    while (springString.Any())
    {
        if (springString.First() == '.')
        {
            if (group.Springs.Any())
            {
                groups.Add(group);
                group = new Group();
            }

            springString = springString.Remove(0, 1);
        }
        else
        {
            group.Springs.Add(springString.First());
            springString = springString.Remove(0, 1);
        }
    }

    if (group.Springs.Any()) groups.Add(group);

    return groups;
}

void FillGroups(List<Group> groups, List<int> damagedGroupSizes)
{
    var groupQueue = new Queue<Group>(groups);
    var group = groupQueue.Dequeue();

    var size = 0;
    foreach (var t in damagedGroupSizes)
    {
        size = t;
        while (group.Springs.Count < size) group = groupQueue.Dequeue();

        group.DamagedGroupSizes.Add(size);
        group.Springs.RemoveRange(0, Math.Min(size + 1, group.Springs.Count));
    }

    if (groupQueue.Any(g => g.Springs.Count >= size))
    {

    }
}