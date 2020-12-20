using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

string SECTION_DELIMITER = $"{Environment.NewLine}{Environment.NewLine}";
const string REPLACE1 = "__REPLACE1__";
const string REPLACE2 = "__REPLACE2__";

var input = File.ReadAllText("input.txt").Split(SECTION_DELIMITER);

var rules = input[0];
var candidates = input[1].Split(Environment.NewLine);
var maxLengthNeeded = candidates.Max(l => l.Length);

var lookup = rules.Split(Environment.NewLine)
                  .Select(line => line.Split(":", StringSplitOptions.TrimEntries))
                  .ToDictionary(kvp => int.Parse(kvp[0]), kvp => kvp[1]);

// Part 1
var regex1 = new Regex($"^{BuildRegexForRule(0)}$");
Console.WriteLine($"Part 1: {candidates.Sum(message => regex1.IsMatch(message) ? 1 : 0)}");

// Part 2
lookup[8] = REPLACE1;
lookup[11] = REPLACE2;
var regex2 = new Regex($"^{BuildRegexForRule(0)}$");
Console.WriteLine($"Part 2: {candidates.Sum(message => regex2.IsMatch(message) ? 1 : 0)}");

string BuildRegexForRule(int id)
{
    var rule = lookup[id];

    switch (rule)
    {
        case string s when s.StartsWith('"'):
            return s.Trim('"');
        case REPLACE1:
            return $"{BuildRegexForRule(42)}+";
        case REPLACE2:
            var r1 = BuildRegexForRule(42);
            var r2 = BuildRegexForRule(31);

            // https://docs.microsoft.com/en-gb/archive/blogs/bclteam/net-regular-expressions-regex-and-balanced-matching-ryan-byington
            return $"(?<count>({r1}))+(?<-count>({r2}))+(?(count)(?!))";
        case string s when s.Contains('|'):
            var or = s.Split('|', StringSplitOptions.TrimEntries);
            var left = string.Concat(or[0].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).Select(BuildRegexForRule));
            var right = string.Concat(or[1].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).Select(BuildRegexForRule));
            return $"({left}|{right})";
        case string s when s.Contains(' '):
            var and = s.Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToList();
            return string.Concat(and.Select(BuildRegexForRule));
        default:
            return BuildRegexForRule(int.Parse(rule));
    }
}