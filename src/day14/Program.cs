using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

var lines = File.ReadLines("input.txt");

Index MASK_START = "mask = ".Length;
Index ADDR_START = "mem[".Length;
Index ADDR_END = ^1;

var memory1 = new Dictionary<int, long>();
var memory2 = new Dictionary<long, long>();
Mask mask = null;

foreach (var line in lines)
{
    if (line.StartsWith("mask"))
        mask = new Mask(line[MASK_START..]);

    if (line.StartsWith("mem"))
    {
        var parts = line.Split(' ', StringSplitOptions.TrimEntries);
        var address = int.Parse(parts[0][ADDR_START..ADDR_END]);
        var value = long.Parse(parts[2]);

        memory1[address] = mask.ApplyTo(value);

        foreach (var addr in mask.GetPermutations(address))
            memory2[addr] = value;
    }
}

Console.WriteLine($"Part 1: {memory1.Values.Sum()}");
Console.WriteLine($"Part 2: {memory2.Values.Sum()}");

class Mask
{
    private string mask { get; init; }

    public Mask(string given)
    {
        mask = given;
    }

    public long ApplyTo(long given)
    {
        var bits = new BitArray(BitConverter.GetBytes(given));

        // Could be done with some clever masking
        for (int i = 0; i < mask.Length; i++)
        {
            // Mask is "backwards"
            bits[i] = mask[mask.Length - 1 - i] switch
            {
                '0' => false,
                '1' => true,
                'X' => bits[i],
                _ => throw new Exception($"Invalid mask: {mask}")
            };
        }

        byte[] raw = new byte[sizeof(long)];
        bits.CopyTo(raw, 0);
        return BitConverter.ToInt64(raw);
    }

    public IEnumerable<long> GetPermutations(long given)
    {
        var number = new StringBuilder(Convert.ToString(given, 2).PadLeft(36, '0'));
        for (int i = 0; i < number.Length; i++)
        {
            number[i] = mask[i] switch
            {
                '0' => number[i],
                '1' => '1',
                'X' => 'X',
                _ => throw new Exception($"Invalid mask: {mask}")
            };
        }

        return GetMaskPermutations(number, 0).Select(x => Convert.ToInt64(x, 2));
    }

    private IEnumerable<string> GetMaskPermutations(StringBuilder s, int start)
    {
        var sb = new StringBuilder(s.ToString());
        List<string> results = new List<string>();

        for (int i = start; i < sb.Length; i++)
        {
            if (sb[i] == 'X')
            {
                sb[i] = '0';
                results.AddRange(GetMaskPermutations(sb, i));
                sb[i] = '1';
                results.AddRange(GetMaskPermutations(sb, i));
                return results;
            }
            else if (i == sb.Length - 1)
                return new List<string> { sb.ToString() };
        }

        return null; // Should never get here
    }
}