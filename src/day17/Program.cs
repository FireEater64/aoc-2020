using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

var input = File.ReadAllLines("input.txt");

var width = input.First().Length;
var height = input.Length;
var iterations = 6;

var universe = new HashSet<Point3D>();
var multiverse = new HashSet<Point4D>();

for (int y = 0; y < height; y++)
for (int x = 0; x < width; x++)
{
    if (input[y][x] == '#')
    {
        universe.Add(new Point3D(x, y, 0));
        multiverse.Add(new Point4D(x, y, 0, 0));
    }
}

var lower = new Point3D(-width - iterations, -height - iterations, -iterations);
var upper = new Point3D(width + iterations, height + iterations, iterations);

for (int i = 0; i < iterations; i++)
{
    var newUniverse = new HashSet<Point3D>();

    foreach (var point in Point3D.Between(lower, upper))
    {
        var numActiveNeighbours = point.GetNeighbours().Count(neighbour => universe.Contains(neighbour));
        var active = universe.Contains(point);

        var insert = point switch
        {
            Point3D when active && numActiveNeighbours is not (2 or 3) => false,
            Point3D when !active && numActiveNeighbours is 3 => true,
            _ => active
        };

        if (insert) newUniverse.Add(point);
    }

    universe = newUniverse;
}

Console.WriteLine($"Part 1: {universe.Count()}");

var start = new Point4D(-width - iterations, -height - iterations, -iterations, -iterations);
var end = new Point4D(width + iterations, height + iterations, iterations, iterations);

for (int i = 0; i < iterations; i++)
{
    var newMultiverse = new HashSet<Point4D>();

    foreach (var point in Point4D.Between(start, end))
    {
        var numActiveNeighbours = point.GetNeighbours().Count(neighbour => multiverse.Contains(neighbour));
        var active = multiverse.Contains(point);

        var insert = point switch
        {
            Point4D when active && numActiveNeighbours is not (2 or 3) => false,
            Point4D when !active && numActiveNeighbours is 3 => true,
            _ => active
        };

        if (insert) newMultiverse.Add(point);
    }

    multiverse = newMultiverse;
}

Console.WriteLine($"Part 2: {multiverse.Count()}");

record Point3D(int X, int Y, int Z)
{
    public Point3D Add(int x, int y, int z) => new Point3D
    (
        X + x,
        Y + y,
        Z + z
    );

    public IEnumerable<Point3D> GetNeighbours()
    {
        var range = Enumerable.Range(-1, 3);
        return range.Cartesian(range, range, (x, y, z) => this.Add(x, y, z))
                    .Where(point => point != this); // Don't include ourselves
    }

    public static IEnumerable<Point3D> Between(Point3D start, Point3D end)
    {
        var xs = Enumerable.Range(start.X, Math.Abs(end.X - start.X) + 1);
        var ys = Enumerable.Range(start.Y, Math.Abs(end.Y - start.Y) + 1);
        var zs = Enumerable.Range(start.Z, Math.Abs(end.Z - start.Z) + 1);
        return xs.Cartesian(ys, zs, (x, y, z) => new Point3D(x, y, z));
    }
}

record Point4D(int X, int Y, int Z, int W)
{
    public Point4D Add(int x, int y, int z, int w) => new Point4D
    (
        X + x,
        Y + y,
        Z + z,
        W + w
    );

    public IEnumerable<Point4D> GetNeighbours()
    {
        var range = Enumerable.Range(-1, 3);
        return range.Cartesian(range, range, range, (x, y, z, w) => this.Add(x, y, z, w))
                    .Where(point => point != this); // Don't include ourselves
    }

    public static IEnumerable<Point4D> Between(Point4D start, Point4D end)
    {
        var xs = Enumerable.Range(start.X, Math.Abs(end.X - start.X) + 1);
        var ys = Enumerable.Range(start.Y, Math.Abs(end.Y - start.Y) + 1);
        var zs = Enumerable.Range(start.Z, Math.Abs(end.Z - start.Z) + 1);
        var ws = Enumerable.Range(start.W, Math.Abs(end.W - start.W) + 1);
        return xs.Cartesian(ys, zs, ws, (x, y, z, w) => new Point4D(x, y, z, w));
    }
}