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

static int CalculateSeatId(string s)
{
    var chars = s.AsSpan();
    var row = CalculateRow(chars[0..7], 0, 127);
    var column = CalculateColumn(chars[7..10], 0, 7);
    return (row * 8) + column;
}

static int CalculateRow(ReadOnlySpan<char> instructions, int front, int back)
{
    if (front == back) return front;

    var instruction = instructions[0];
    var nextInstructions = instructions[1..];

    return instruction switch
    {
        'F' => CalculateRow(nextInstructions, front, CalculateMidpoint(front, back, MidpointRounding.ToZero)),
        'B' => CalculateRow(nextInstructions, CalculateMidpoint(front, back, MidpointRounding.AwayFromZero), back),
        _ => throw new Exception($"Invalid instruction: {instruction}")
    };
}

static int CalculateColumn(ReadOnlySpan<char> instructions, int left, int right)
{
    if (left == right) return left;

    var instruction = instructions[0];
    var nextInstructions = instructions[1..];

    return instruction switch
    {
        'R' => CalculateColumn(nextInstructions, CalculateMidpoint(left, right, MidpointRounding.AwayFromZero), right),
        'L' => CalculateColumn(nextInstructions, left, CalculateMidpoint(left, right, MidpointRounding.ToZero)),
        _ => throw new Exception($"Invalid instruction: {instruction}")
    };
}

static int CalculateMidpoint(int lower, int upper, MidpointRounding rounding) => (int) Math.Round((lower + upper) / 2m, 0, rounding);