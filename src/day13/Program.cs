using System;
using System.IO;
using System.Linq;
using MoreLinq;

var lines = File.ReadLines("input.txt").ToList();

// Part 1
var t = long.Parse(lines[0]);
var departureTimes = lines[1].Split(',').Where(x => x != "x").Select(long.Parse).ToArray();
var bestBus = departureTimes.MinBy(x => TimeUntilBusDeparts(t, x)).First();
Console.WriteLine($"Part 1: {bestBus * TimeUntilBusDeparts(t, bestBus)}");

var constraints = lines[1].Split(',')
                          .Select((n, i) => new { freq = n, i = i })
                          .Where(x => x.freq != "x")
                          .Select(x => TimeUntilBusDeparts(x.i, long.Parse(x.freq)))
                          .ToArray();

Console.WriteLine($"Part 2: {ChineseRemainderTheorem(departureTimes, constraints)}");

static long TimeUntilBusDeparts(long t, long freq) => Mod(freq - t, freq);

// C# has a remainder operator, not a mod operator :(
static long Mod(long a, long b) => (Math.Abs(a * b) + a) % b;

// Butchered from https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
static long ChineseRemainderTheorem(long[] freqs, long[] constraints)
{
    long prod = freqs.Aggregate(1L, (i, j) => i * j);
    long p;
    long sm = 0;
    for (long i = 0; i < freqs.Length; i++)
    {
        p = prod / freqs[i];
        sm += constraints[i] * ModularMultiplicativeInverse(p, freqs[i]) * p;
    }
    return sm % prod;
}

static long ModularMultiplicativeInverse(long a, long mod)
{
    long b = a % mod;
    for (long x = 1; x < mod; x++)
    {
        if ((b * x) % mod == 1)
        {
            return x;
        }
    }
    return 1;
}