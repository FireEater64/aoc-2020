using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

var adapters = File.ReadLines("input.txt").Select(long.Parse).ToList();
adapters.Add(0);
adapters.Add(adapters.Max() + 3);
var backpack = adapters.OrderBy(x => x).ToArray();

var deltas = backpack.Pairwise((a, b) => b - a).ToLookup(x => x);
Console.WriteLine($"Part 1: {deltas[1].Count() * deltas[3].Count()}");

Dictionary<long, long> memos = new();
Console.WriteLine($"Part 2: {Combinations(memos, backpack, 0)}");

long Combinations(Dictionary<long, long> memos, long[] backpack, int start)
{
    if (start == backpack.Length - 1)
        return 1;

    if (memos.TryGetValue(start, out var memoed))
        return memoed;

    var count = 0L;
    for (int potential = start + 1; potential < backpack.Length; potential++)
    {
        if (backpack[potential] - backpack[start] <= 3)
            count += Combinations(memos, backpack, potential);
        else
            break;
    }

    memos[start] = count;
    return count;
}