using System;
using System.Collections.Generic;
using System.Linq;

var input = new int[] { 0, 13, 16, 17, 1, 10, 6 };
var sequence = PlayGame(input);
Console.WriteLine($"Part 1: {sequence.Skip(2020 - 1).First()}");
Console.WriteLine($"Part 1: {sequence.Skip(30000000 - 1).First()}");

IEnumerable<int> PlayGame(int[] start)
{
    var seenIndex = new Dictionary<int, int>();
    var t = 1;
    int prev, next;

    for (int i = 0; i < start.Length - 1; i++)
    {
        next = start[i];
        seenIndex[next] = t;
        t++;
        prev = next;
        yield return next;
    }

    prev = start[^1];
    t++;
    yield return prev;

    while (true)
    {
        if (seenIndex.TryGetValue(prev, out var lastT))
            next = t - 1 - lastT;
        else
            next = 0;

        seenIndex[prev] = t - 1;
        prev = next;
        yield return next;

        t++;
    }
        
}