using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

string TILE_SEP = $"{Environment.NewLine}{Environment.NewLine}";

var input = File.ReadAllText("input.txt").Split(TILE_SEP, StringSplitOptions.TrimEntries);

var tiles = input.Select(t => new Tile(t)).ToList();

// Look for the tiles that have two other tiles with matching edges
var corners = tiles.Where(t => 
{
    var numMatching = tiles.Where(other => other != t).Count(other => t.NumberOfMatchingSides(other) >= 1);
    return numMatching == 2;
});

Console.WriteLine($"Part 1: {corners.Aggregate(1L, (acc, next) => acc * next.Id)}");

record Tile
{
    public int Id { get; init; }
    public HashSet<string> Edges { get; init; }

    public Tile(string input)
    {
        var lines = input.Split(Environment.NewLine);
        Id = int.Parse(lines[0].Split(' ')[1].Trim(':'));

        // Strip off the edges
        var body = lines.Skip(1).ToList();
        Edges = new HashSet<string>();
        Edges.Add(body.First());
        Edges.Add(string.Concat(body.Select(l => l.First())));
        Edges.Add(body.Last());
        Edges.Add(string.Concat(body.Select(l => l.Last())));

        // If we flip the piece, the edges are reversed
        Edges = Edges.Union(Edges.Select(x => string.Concat(x.Reverse()))).ToHashSet();
    }

    public int NumberOfMatchingSides(Tile other)
    {
        return Edges.Intersect(other.Edges).Count();
    }
}