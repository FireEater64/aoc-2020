using System;
using System.Drawing;
using System.IO;
using System.Linq;

var instructions = File.ReadLines("input.txt").Select(x => new Instuction(x)).ToList();

// Part 1
var location = new Point(0, 0);
var compass = 90;

foreach (var instruction in instructions)
{
    location = instruction switch
    {
        Instuction { Direction: Direction.NORTH or 
                                Direction.SOUTH or 
                                Direction.EAST  or 
                                Direction.WEST } i    => location + GetStepForDirection(i.Direction) * i.Units,

        Instuction { Direction: Direction.FORWARD } i => location += GetStepForCompass(compass) * i.Units,
        _ => location
    };

    compass = instruction switch
    {
        Instuction { Direction: Direction.LEFT }  i => (compass - i.Units) < 0 ? 360 - Math.Abs(compass - i.Units) : (compass - i.Units), // % is not modulo
        Instuction { Direction: Direction.RIGHT } i => (compass + i.Units) % 360,
        _ => compass
    };
}

Console.WriteLine($"Part 1: {Math.Abs(location.X) + Math.Abs(location.Y)}");

// Part 2
location = new Point(0, 0);
var waypoint = new Point(10, 1);

foreach (var instruction in instructions)
{
    waypoint = instruction switch
    {
        Instuction { Direction: Direction.NORTH or 
                                Direction.SOUTH or 
                                Direction.EAST  or 
                                Direction.WEST }  i  => waypoint + GetStepForDirection(i.Direction) * i.Units,

        Instuction { Direction: Direction.LEFT }  i => RotateWaypoint(waypoint, i.Units),
        Instuction { Direction: Direction.RIGHT } i => RotateWaypoint(waypoint, 360 - i.Units),
        _ => waypoint
    };

    location = instruction switch
    {
        Instuction { Direction: Direction.FORWARD }  i => location + (new Size(waypoint) * i.Units),
        _ => location
    };
}

Console.WriteLine($"Part 2: {Math.Abs(location.X) + Math.Abs(location.Y)}");

Point RotateWaypoint(Point s, int degrees)
{
    var rads = degrees * (Math.PI / 180);
    var cos = Math.Cos(rads);
    var sin = Math.Sin(rads);

    return new Point
    {
        X = Convert.ToInt32(cos * (s.X) - sin * (s.Y)),
        Y = Convert.ToInt32(sin * (s.X) + cos * (s.Y))
    };
}

Size GetStepForCompass(int compass) => compass switch
{
    0   => new Size( 0,  1),
    90  => new Size( 1,  0),
    180 => new Size( 0, -1),
    270 => new Size(-1,  0),
    _   => throw new Exception($"Invalid compass: {compass}")
};

Size GetStepForDirection(Direction direction) => direction switch
{
    Direction.NORTH => GetStepForCompass(0),
    Direction.EAST  => GetStepForCompass(90),
    Direction.SOUTH => GetStepForCompass(180),
    Direction.WEST  => GetStepForCompass(270),
    _ => throw new Exception($"Invalid direction: {direction}")
};

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