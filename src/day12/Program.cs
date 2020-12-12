using System;
using System.Drawing;
using System.IO;
using System.Linq;

var instructions = File.ReadLines("input.txt").Select(x => new Instuction(x)).ToList();

// Part 1
var location = new Point(0, 0);
var direction = new Size(1, 0);

foreach (var instruction in instructions)
{
    location = instruction switch
    {
        Instuction { Direction: Direction.NORTH } i => location + new Size(0, 1) * i.Units,
        Instuction { Direction: Direction.SOUTH } i => location + new Size(0, -1) * i.Units,
        Instuction { Direction: Direction.EAST }  i => location + new Size(1, 0) * i.Units,
        Instuction { Direction: Direction.WEST }  i => location + new Size(-1, 0) * i.Units,
        Instuction { Direction: Direction.FORWARD } i => location += direction * i.Units,
        _ => location
    };

    direction = instruction switch
    {
        Instuction { Direction: Direction.LEFT }  i => Left(direction, i.Units / 90),
        Instuction { Direction: Direction.RIGHT }  i => Right(direction, i.Units / 90),
        _ => direction
    };
}

Console.WriteLine($"Part 1: {Math.Abs(location.X) + Math.Abs(location.Y)}");

// Part 2
location = new Point(0, 0);
var waypoint = new Size(10, 1);

foreach (var instruction in instructions)
{
    waypoint = instruction switch
    {
        Instuction { Direction: Direction.NORTH } i => waypoint + new Size(0, 1) * i.Units,
        Instuction { Direction: Direction.SOUTH } i => waypoint + new Size(0, -1) * i.Units,
        Instuction { Direction: Direction.EAST }  i => waypoint + new Size(1, 0) * i.Units,
        Instuction { Direction: Direction.WEST }  i => waypoint + new Size(-1, 0) * i.Units,
        Instuction { Direction: Direction.LEFT }  i => RotateAround(waypoint, i.Units),
        Instuction { Direction: Direction.RIGHT }  i => RotateAround(waypoint, 360 - i.Units),
        _ => waypoint
    };

    location = instruction switch
    {
        Instuction { Direction: Direction.FORWARD }  i => location + waypoint * i.Units,
        _ => location
    };
}

Console.WriteLine($"Part 2: {Math.Abs(location.X) + Math.Abs(location.Y)}");

Size RotateAround(Size s, int degrees)
{
    var rads = degrees * (Math.PI / 180);
    var cos = Math.Cos(rads);
    var sin = Math.Sin(rads);

    return new Size
    (
        Convert.ToInt32(
            cos * (s.Width) -
            sin * (s.Height)
        ),
        Convert.ToInt32(
            sin * (s.Width) +
            cos * (s.Height)
        )
    );
}

Size Right(Size s, int steps)
{
    var result = s;
    for (int i = 0; i < steps; i++)
        result = RightStep(result);
    return result;
}

Size Left(Size s, int steps) => Right(s, 4 - steps);

Size RightStep(Size s)
{
    return s switch
    {
        Size {Width: 1, Height: 0}  => new Size(0, -1),
        Size {Width: 0, Height: -1} => new Size(-1, 0),
        Size {Width: -1, Height: 0}  => new Size(0, 1),
        Size {Width: 0, Height: 1}  => new Size(1, 0),
        _ => throw new Exception($"Invalid point: {s}"),
    };
}

record Instuction
{
    public Instuction(string s)
    {
        Direction = s[0] switch
        {
            'N' => Direction.NORTH,
            'S' => Direction.SOUTH,
            'E' => Direction.EAST,
            'W' => Direction.WEST,
            'F' => Direction.FORWARD,
            'L' => Direction.LEFT,
            'R' => Direction.RIGHT,
            _ => throw new Exception("Wut")
        };

        Units = int.Parse(s[1..]);
    }

    public Direction Direction { get; init; }
    public int Units { get; init; }
}

enum Direction
{
    NORTH,
    SOUTH,
    EAST,
    WEST,
    FORWARD,
    LEFT,
    RIGHT
}