using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var instructions = File.ReadLines("input.txt").Select(Decode);
var console = new GameConsole(instructions);

console.WillRunToInfiniteLoop();

Console.WriteLine($"Part 1: {console.Accumulator}");
var terminatingConsole = GetAllPossibleConsoles(new GameConsole(instructions)).ToList().First(x => !x.WillRunToInfiniteLoop());
Console.WriteLine($"Part 2: {terminatingConsole.Accumulator}");

IEnumerable<GameConsole> GetAllPossibleConsoles(GameConsole console)
{
    for (int i = 0; i < console.Instructions.Count; i++)
    {
        yield return console;
        var withJmp = console with
        {
            Instructions = new List<Instruction>(console.Instructions)
        };
        withJmp.Instructions[i] = withJmp.Instructions[i] with
        {
            Opcode = withJmp.Instructions[i].Opcode == "nop" ? "jmp" : withJmp.Instructions[i].Opcode
        };
        yield return withJmp;
        var withNop = console with
        {
            Instructions = new List<Instruction>(console.Instructions)
        };
        withNop.Instructions[i] = withNop.Instructions[i] with
        {
            Opcode = withNop.Instructions[i].Opcode == "jmp" ? "nop" : withNop.Instructions[i].Opcode
        };
        yield return withNop;
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
    public List<Instruction> Instructions { get; init; }
    public int ProgramCounter { get; private set; } = 0;
    public int Accumulator { get; private set; } = 0;

    public GameConsole(IEnumerable<Instruction> instructions)
    {
        Instructions = new List<Instruction>(instructions);
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