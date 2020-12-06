using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var passengerGroups = File.ReadAllText("input.txt")
    .Split($"{Environment.NewLine}{Environment.NewLine}")
    .Select(str => str.Split(Environment.NewLine).Select(line => line.ToHashSet()));

var unions = passengerGroups.Select(group => UnionAll(group)).Sum(set => set.Count);
var intersections = passengerGroups.Select(group => IntersectAll(group)).Sum(set => set.Count);

Console.WriteLine($"Part 1: {unions}");
Console.WriteLine($"Part 2: {intersections}");

HashSet<T> UnionAll<T>(IEnumerable<HashSet<T>> sets) => sets.Any() ? sets.Aggregate(new HashSet<T>(sets.First()), (result, next) => { result.UnionWith(next); return result; }) : new HashSet<T>();
HashSet<T> IntersectAll<T>(IEnumerable<HashSet<T>> sets) => sets.Any() ? sets.Aggregate(new HashSet<T>(sets.First()), (result, next) => { result.IntersectWith(next); return result; }) : new HashSet<T>();