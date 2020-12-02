using System;
using System.IO;
using System.Linq;

var passwords = File.ReadAllLines("input.txt").Select(ParseLine).ToList();

Console.WriteLine($"Part 1: {passwords.Count(x => IsValidRule1(x.start, x.end, x.target, x.password))}");
Console.WriteLine($"Part 2: {passwords.Count(x => IsValidRule2(x.start, x.end, x.target, x.password))}");

bool IsValidRule1(int min, int max, char target, string password)
{
    var occurances = password.Count(c => c == target);

    return min <= occurances && occurances <= max;
}

bool IsValidRule2(int i1, int i2, char target, string password)
{
    var pos1 = password[i1 - 1];
    var pos2 = password[i2 - 1];

    return pos1 == target ^ pos2 == target;
}

(int start, int end, char target, string password) ParseLine(string line)
{
    var parts = line.Split(" ");
    var range = parts[0];
    var target = parts[1][0];
    var password = parts[2];

    var rangeStart = int.Parse(range.Split("-")[0]);
    var rangeEnd = int.Parse(range.Split("-")[1]);

    return (rangeStart, rangeEnd, target, password);
}