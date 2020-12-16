using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt");

var sections = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.TrimEntries);

var rules = sections[0].Split(Environment.NewLine);
var mine = sections[1].Split(Environment.NewLine).Skip(1).Single().Split(',').Select(int.Parse).ToList();
var nearby = sections[2].Split(Environment.NewLine).Skip(1).Select(line => line.Split(',').Select(int.Parse));

// Map rule name to valid ranges
Dictionary<string, HashSet<int>> ruleRanges = rules .Select(line => line.Split(new string[] { ":", " or " }, StringSplitOptions.TrimEntries))
                        .ToDictionary(x => x[0], // Rulename
                                      x => x[1..].SelectMany(range => // Expand ranges to single HashSet<int>
                                      {
                                          var minmax = range.Split('-').Select(int.Parse).ToArray();
                                          return Enumerable.Range(minmax[0], minmax[1] - minmax[0] + 1);
                                      }).ToHashSet());

// Union all the hashsets
var validNumbers = ruleRanges.SelectMany(range => range.Value).ToHashSet();

var invalid = nearby.SelectMany(line => line).Where(x => !validNumbers.Contains(x)).Sum();

Console.WriteLine($"Part 1: {invalid}");

var ruleNames = ruleRanges.Keys;
List<List<int>> validTickets = nearby.Where(line => line.All(x => validNumbers.Contains(x)))
                                     .Select(x => x.ToList()).ToList();

var mapping = new Dictionary<string, int>();
while (ruleNames.Any(name => !mapping.ContainsKey(name))) // Whilst there are any unmapped column names
{
    foreach (var name in ruleNames.Where(name => !mapping.ContainsKey(name)))
    {
        // Get the number of columns that could contain rule
        var unmappedIndices = Enumerable.Range(0, ruleNames.Count)
                                        .Where(x => !mapping.Values.Contains(x));

        var matchingNames = unmappedIndices.Where(index => validTickets.All(ticket => ruleRanges[name].Contains(ticket[index])));

        // If there's only one column - we've found it :)
        if (matchingNames.Count() == 1)
            mapping.Add(name, matchingNames.Single());
    }
}

var result = mapping.Where(x => x.Key.StartsWith("departure"))
                    .Select(mapping => mapping.Value)
                    .Aggregate(1L, (total, index) => total * mine[index]);

Console.WriteLine($"Part 2: {result}");
