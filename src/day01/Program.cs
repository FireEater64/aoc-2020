using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

var expenses = File.ReadAllLines("input.txt").Select(int.Parse).ToList();
var lookup = new HashSet<int>(expenses);

const int target = 2020;

var entry1 = expenses.First(x => lookup.Contains(target - x));
var entry2 = target - entry1;

Console.WriteLine($"Part 1: {entry1 * entry2}");

// Got lazy - brute force for part 2
var result = expenses
    .Cartesian(expenses, expenses, (x, y, z) => new [] {x, y, z})
    .First(triple => triple.Sum() == target)
    .Aggregate(1, (x, y) => x * y);

Console.WriteLine($"Part 2: {result}");