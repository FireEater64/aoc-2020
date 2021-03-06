﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var attributes = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
var passports = attributes
    .Select(
        cluster => cluster.Split(new[] { Environment.NewLine, " " }, StringSplitOptions.TrimEntries)
            .Select(kvpstring => kvpstring.Split(':')).ToDictionary(kvp => kvp[0], kvp => kvp[1])
    ).Select(dict => new Passport(dict));

Console.WriteLine($"Part 1: {passports.Count(x => x.IsValidPart1)}");
Console.WriteLine($"Part 2: {passports.Count(x => x.IsValidPart2)}");

record Passport
{
    public int? BirthYear { get; init; }
    public int? IssueYear { get; init; }
    public int? ExpirationYear { get; init; }
    public string? Height { get; init; }
    public string? HairColour { get; init; }
    public string? EyeColour { get; init; }
    public string? PassportId { get; init; }
    public int? CountryId { get; init; }
    private int NumFields { get; init; }

    public Passport(IDictionary<string, string> kvp)
    {
        BirthYear = kvp.TryGetValue("byr", out var byr) ? int.Parse(byr) : null;
        IssueYear = kvp.TryGetValue("iyr", out var iyr) ? int.Parse(iyr) : null;
        ExpirationYear = kvp.TryGetValue("eyr", out var eyr) ? int.Parse(eyr) : null;
        Height = kvp.TryGetValue("hgt", out var hgt) ? hgt : null;
        HairColour = kvp.TryGetValue("hcl", out var hcl) ? hcl : null;
        EyeColour = kvp.TryGetValue("ecl", out var ecl) ? ecl : null;
        PassportId = kvp.TryGetValue("pid", out var pid) ? pid : null;
        CountryId = kvp.TryGetValue("cid", out var cid) ? int.Parse(cid) : null;
        NumFields = kvp.Count;
    }

    public bool IsValidPart1 => this switch
    {
        Passport { NumFields: 8 } => true,
        Passport { NumFields: 7, CountryId: null } => true,
        _ => false
    };

    public bool IsValidPart2 => BirthYear is >= 1000 and <= 2002 &&
                                IssueYear is >= 2010 and <= 2020 &&
                                ExpirationYear is >= 2020 and <= 2030 &&
                                HairColour is not null && HairColour.StartsWith("#") && HairColour.Length == 7 &&
                                EyeColour is "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth" &&
                                PassportId is not null && PassportId.Length == 9 &&
                                Height switch 
                                {
                                    string h when h.EndsWith("cm") && int.Parse(h[..^2]) is >= 150 and <= 193 => true,
                                    string h when h.EndsWith("in") && int.Parse(h[..^2]) is >= 59 and <= 76 => true,
                                    _ => false,
                                };
}