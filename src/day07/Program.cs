using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadLines("input.txt");

var lookup = lines.Select(l => GetContainedIn(l)).ToDictionary(r => r.Description, r => r.Contains);

var universe = lookup.SelectMany(x => lookup.Where(l => l.Value.Any(c => c.Description.Contains("shiny gold"))).Select(s => s.Key)).ToHashSet();

while (true)
{
    var newUniverse = universe.Union(universe.SelectMany(x => lookup.Where(l => l.Value.Any(v => v.Description.Contains(x))).Select(s => s.Key))).ToHashSet();
    int oldCount = universe.Count, newCount = newUniverse.Count;
    universe = newUniverse;
    if (oldCount == newCount)
        break;
}

Console.WriteLine($"Part 1: {universe.Count}");
Console.WriteLine($"Part 2: {CountContents("shiny gold", lookup)}");

int CountContents(string name, Dictionary<string, HashSet<Contents>> lookup)
{
    var contents = lookup.TryGetValue(name, out var content);

    var count = content.Any() ? content.Sum(b => b.Count + (b.Count * CountContents(b.Description, lookup))) : 0;
    return count;
}

(string Description, HashSet<Contents> Contains) GetContainedIn(string line)
{
    var split = line.IndexOf("contain");
    var description = line[..(split - 6)];

    if (line.Contains("other")) return (description, new HashSet<Contents>());

    var contains = line[(split + 8)..]
        .Trim('.')
        .Split(',', StringSplitOptions.TrimEntries);

    var result = contains.Select(x => 
    {
        var i = x.IndexOf(" bag");
        return new Contents { Description = x[2..i], Count = int.Parse(x[0].ToString()) };
    }).ToHashSet();
    return (description, result);
}

record Contents
{
    public string Description { get; init; }
    public int Count { get; init; }
}