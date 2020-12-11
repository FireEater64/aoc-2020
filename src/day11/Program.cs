using System;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt");
SeatState[,] status = LoadSeats(lines);

bool changed1, changed2;
SeatState[,] part1 = status, part2 = status;
do
{
    (part1, changed1) = Tick(part1, (s, x, y) => CountAdjacent(s, x, y), 4);
    (part2, changed2) = Tick(part2, (s, x, y) => CountLineOfSight(s, x, y), 5);
}
while (changed1 || changed2);

Console.WriteLine($"Part 1: {CountOccupied(part1)}");
Console.WriteLine($"Part 2: {CountOccupied(part2)}");

static (SeatState[,], bool) Tick(SeatState[,] given, Func<SeatState[,], int, int, int> seatCounter, int tolerance)
{
    var result = new SeatState[given.GetLength(0), given.GetLength(1)];
    var changed = false;

    for (int i = 0; i < given.GetLength(0); i++)
    for (int j = 0; j < given.GetLength(1); j++)
    {
        result[i, j] = given[i, j] switch
        {
            SeatState.FLOOR => SeatState.FLOOR,
            SeatState.EMPTY when seatCounter(given, i, j) is 0 => SeatState.OCCUPIED,
            SeatState.OCCUPIED when seatCounter(given, i, j) >= tolerance => SeatState.EMPTY,
            SeatState.EMPTY => SeatState.EMPTY,
            SeatState.OCCUPIED => SeatState.OCCUPIED,
            _ => throw new Exception("Wut")
        };

        changed |= result[i, j] != (given[i, j]);
    }

    return (result, changed);
}

static int CountAdjacent(SeatState[,] given, int x, int y)
{
    var count = 0;
    if (IsSeat(given, x - 1, y, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x - 1, y - 1, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x - 1, y + 1, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x, y + 1, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x, y - 1, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x + 1, y, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x + 1, y - 1, SeatState.OCCUPIED)) count++;
    if (IsSeat(given, x + 1, y + 1, SeatState.OCCUPIED)) count++;
    return count;
}

static int CountLineOfSight(SeatState[,] given, int x, int y)
{
    var count = 0;
    if (CanSeeOccupiedSeat(given, x, y, 0, 1)) count++;
    if (CanSeeOccupiedSeat(given, x, y, 0, -1)) count++;
    if (CanSeeOccupiedSeat(given, x, y, 1, -1)) count++;
    if (CanSeeOccupiedSeat(given, x, y, 1, 0)) count++;
    if (CanSeeOccupiedSeat(given, x, y, 1, 1)) count++;
    if (CanSeeOccupiedSeat(given, x, y, -1, -1)) count++;
    if (CanSeeOccupiedSeat(given, x, y, -1, 0)) count++;
    if (CanSeeOccupiedSeat(given, x, y, -1, 1)) count++;
    return count;
}

static bool CanSeeOccupiedSeat(SeatState[,] given, int x, int y, int xstep, int ystep)
{
    while (x >= 0 && x < given.GetLength(0) &&
           y >= 0 && y < given.GetLength(1))
    {
        if (IsSeat(given, x + xstep, y + ystep, SeatState.OCCUPIED)) return true;
        if (IsSeat(given, x + xstep, y + ystep, SeatState.EMPTY)) return false;
        x += xstep;
        y += ystep;
    }

    return false;
}

static bool IsSeat(SeatState[,] given, int x, int y, SeatState status)
{
    if (x < 0 || x >= given.GetLength(0))
        return false;
    if (y < 0 || y >= given.GetLength(1))
        return false;
    
    return given[x, y] == status;
}

static SeatState[,] LoadSeats(string[] given)
{
    var result = new SeatState[given.First().Length, given.Length];
    for (int i = 0; i < given.Length; i++)
    for (int j = 0; j < given.First().Length; j++)
    {
        result[j, i] = given[i][j] switch
        {
            '.' => SeatState.FLOOR,
            '#' => SeatState.OCCUPIED,
            'L' => SeatState.EMPTY,
            _ => throw new Exception("Unknown char")
        };
    }

    return result;
}

static int CountOccupied(SeatState[,] given)
{
    var count = 0;
    for (int i = 0; i < given.GetLength(0); i++)
    for (int j = 0; j < given.GetLength(1); j++)
        if (IsSeat(given, i, j, SeatState.OCCUPIED)) count++;
    return count;
}

enum SeatState
{
    FLOOR,
    EMPTY,
    OCCUPIED
}