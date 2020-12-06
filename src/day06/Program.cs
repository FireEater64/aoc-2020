using System;
using System.IO;
using System.Linq;

var groups = File.ReadAllText("input.txt")
    .Split($"{Environment.NewLine}{Environment.NewLine}")
    .Select(str => str.Replace(Environment.NewLine, "")
        .GroupBy(ch => ch)
        .Select(group => new { GroupSize = str.Count(c => c == '\n') + 1, Answered = group }
    ));

var result = groups.Sum(group => group.Count(c => c.Answered.Count() > 0));
var result2 = groups.Sum(group => group.Count(c => c.Answered.Count() == c.GroupSize));

Console.WriteLine($"Part 1: {result}");
Console.WriteLine($"Part 2: {result2}");