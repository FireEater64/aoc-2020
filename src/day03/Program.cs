using System;
using System.IO;
using System.Linq;

var trees = ParseTrees(File.ReadAllLines("input.txt"));
Console.WriteLine($"Part 1: {CountTrees(trees, 3, 1)}");

var moves = new (int x, int y)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
var part2 = moves
    .Select(move => CountTrees(trees, move.x, move.y))
    .Aggregate((curr, x) => curr * x);

Console.WriteLine($"Part 2: {part2}");

static long CountTrees(bool[,] forest, int xstep, int ystep)
{
    int x = 0, count = 0;
    var height = forest.GetLength(0);
    var width = forest.GetLength(1);

    for (int y = 0; y < height ; y += ystep)
    {
        if (forest[y, x]) count++;
        x = (x + xstep) % width;
    }

    return count;
}

static bool[,] ParseTrees(string[] lines)
{
    int height = lines.Length, width = lines[0].Length;
    var trees = new bool[height, width];

    for (int i = 0; i < height; i++)
    for (int j = 0; j < width; j++)
        trees[i, j] = (lines[i][j].Equals('#'));

    return trees;
}