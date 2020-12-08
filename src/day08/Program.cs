using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

var instructions = File.ReadLines("input.txt").Select(Decode);
var console = new GameConsole(instructions);
var consolePermutations = GetAllPossibleConsoles(console).ToList();

console.WillRunToInfiniteLoop();
Console.WriteLine($"Part 1: {console.Accumulator}");

var terminatingConsole = consolePermutations.First(x => !x.WillRunToInfiniteLoop());
Console.WriteLine($"Part 2: {terminatingConsole.Accumulator}");

IEnumerable<GameConsole> GetAllPossibleConsoles(GameConsole console)
{
    for (int i = 0; i < console.Instructions.Count; i++)
    {
        yield return console;

        var instruction = console.Instructions[i];
        if (instruction.Opcode == "acc")
            continue;

        var jmp = instruction with { Opcode = "jmp" };
        var nop = instruction with { Opcode = "nop" };

        yield return console with
        {
            Instructions = console.Instructions.SetItem(i, jmp)
        };

        yield return console with
        {
            Instructions = console.Instructions.SetItem(i, nop)
        };
    }

    yield break;
}

Instruction Decode(string line)
{
    var parts = line.Split(' ', StringSplitOptions.TrimEntries);
    return new Instruction { Opcode = parts[0], Arg = int.Parse(parts[1]) };
}

record GameConsole
{
    public ImmutableList<Instruction> Instructions { get; init; }
    public int ProgramCounter { get; private set; } = 0;
    public int Accumulator { get; private set; } = 0;

    public GameConsole(IEnumerable<Instruction> instructions)
    {
        Instructions = instructions.ToImmutableList();
    }

    public void Run()
    {
        switch (Instructions[ProgramCounter])
        {
            case Instruction { Opcode: "nop" }:
                ProgramCounter++;
                break;
            case Instruction { Opcode: "acc" } i:
                Accumulator += i.Arg;
                ProgramCounter++;
                break;
            case Instruction { Opcode: "jmp"} i :
                ProgramCounter += i.Arg;
                break;
        }
    }

    public bool WillRunToInfiniteLoop()
    {
        var seen = new HashSet<int>();
        while (true)
        {
            Run();
            if (seen.Contains(ProgramCounter)) return true;
            if (ProgramCounter >= Instructions.Count) return false;
            seen.Add(ProgramCounter);
        }
    }
}

record Instruction
{
    public string Opcode { get; init; }
    public int Arg { get; init; }
}