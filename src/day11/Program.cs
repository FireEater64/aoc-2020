using System;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt");
SeatState[,] status = LoadSeats(lines);

bool changed = false;
do
{
    (status, changed) = Tick(status);
}
while (changed);

Console.WriteLine($"Part 1: {CountOccupied(status)}");

changed = false;
status = LoadSeats(lines);
do
{
    (status, changed) = Tick2(status);
}
while (changed);

Console.WriteLine($"Part 2: {CountOccupied(status)}");

(SeatState[,], bool) Tick(SeatState[,] given)
{
    var result = new SeatState[given.GetLength(0), given.GetLength(1)];
    var changed = false;

    for (int i = 0; i < given.GetLength(0); i++)
    for (int j = 0; j < given.GetLength(1); j++)
    {
        result[i, j] = given[i, j] switch
        {
            SeatState.FLOOR => SeatState.FLOOR,
            SeatState.EMPTY when CountAdjacent(given, i, j) is 0 => SeatState.OCCUPIED,
            SeatState.OCCUPIED when CountAdjacent(given, i, j) is >= 4 => SeatState.EMPTY,
            SeatState.EMPTY => SeatState.EMPTY,
            SeatState.OCCUPIED => SeatState.OCCUPIED,
            _ => throw new Exception("Wut")
        };

        changed |= result[i, j] != (given[i, j]);
    }

    return (result, changed);
}

(SeatState[,], bool) Tick2(SeatState[,] given)
{
    var result = new SeatState[given.GetLength(0), given.GetLength(1)];
    var changed = false;

    for (int i = 0; i < given.GetLength(0); i++)
    for (int j = 0; j < given.GetLength(1); j++)
    {
        result[i, j] = given[i, j] switch
        {
            SeatState.FLOOR => SeatState.FLOOR,
            SeatState.EMPTY when CountLineOfSight(given, i, j) is 0 => SeatState.OCCUPIED,
            SeatState.OCCUPIED when CountLineOfSight(given, i, j) is >= 5 => SeatState.EMPTY,
            SeatState.EMPTY => SeatState.EMPTY,
            SeatState.OCCUPIED => SeatState.OCCUPIED,
            _ => throw new Exception("Wut")
        };

        changed |= result[i, j] != (given[i, j]);
    }

    return (result, changed);
}

int CountAdjacent(SeatState[,] given, int x, int y)
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

int CountLineOfSight(SeatState[,] given, int x, int y)
{
    var count = 0;
    if (CanSeeSeat(given, x, y, 0, 1)) count++;
    if (CanSeeSeat(given, x, y, 0, -1)) count++;
    if (CanSeeSeat(given, x, y, 1, -1)) count++;
    if (CanSeeSeat(given, x, y, 1, 0)) count++;
    if (CanSeeSeat(given, x, y, 1, 1)) count++;
    if (CanSeeSeat(given, x, y, -1, -1)) count++;
    if (CanSeeSeat(given, x, y, -1, 0)) count++;
    if (CanSeeSeat(given, x, y, -1, 1)) count++;
    return count;
}

bool CanSeeSeat(SeatState[,] given, int x, int y, int xstep, int ystep)
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

bool IsSeat(SeatState[,] given, int x, int y, SeatState status)
{
    if (x < 0 || x >= given.GetLength(0))
        return false;
    if (y < 0 || y >= given.GetLength(1))
        return false;
    
    return given[x, y] == status;
}

SeatState[,] LoadSeats(string[] given)
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

int CountOccupied(SeatState[,] given)
{
    var count = 0;
    for (int i = 0; i < status.GetLength(0); i++)
    for (int j = 0; j < status.GetLength(1); j++)
        if (IsSeat(status, i, j, SeatState.OCCUPIED)) count++;
    return count;
}

enum SeatState
{
    FLOOR,
    EMPTY,
    OCCUPIED
}