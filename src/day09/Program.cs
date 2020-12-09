using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

var input = File.ReadLines("input.txt").Select(long.Parse);

var buffers = input.Window(26); // Sliding window

var result1 = buffers.First(x => !ValidSumTwo(x.Take(25), x[25])).Last();
Console.WriteLine($"Part 1: {result1}");

var result2 = SubarraySum(input.ToArray(), result1);
Console.WriteLine($"Part 2: {result2}");

bool ValidSumTwo(IEnumerable<long> input, long target) 
{
    var search = new HashSet<long>(input);
    return input.Any(x => search.Contains(target - x));
}

long SubarraySum(long[] input, long target)
{
    for (int i = 0; i < input.Length; i++)
    for (int j = i + 2; j < input.Length; j++)
    {
        var range = input[i..j];
        if (range.Sum() == target)
            return range.Min() + range.Max();
    }

    return -1L;
}