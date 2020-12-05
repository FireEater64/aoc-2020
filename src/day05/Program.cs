using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var seatIds = File.ReadAllLines("input.txt").Select(CalculateSeatId);
var maxSeat = seatIds.Max();

Console.WriteLine($"Part 1: {maxSeat}");

var lookup = new HashSet<int>(seatIds);
var mine = Enumerable.Range(0, maxSeat)
    .Where(id => !lookup.Contains(id))
    .Single(id => lookup.Contains(id + 1) &&
                  lookup.Contains(id - 1));

Console.WriteLine($"Part 2: {mine}");

static int CalculateSeatId(string s) =>
    Convert.ToInt32(s
        .Replace('F', '0')
        .Replace('B', '1')
        .Replace('L', '0')
        .Replace('R', '1'), 2);