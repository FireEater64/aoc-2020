using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

const char LBRACKET = '(';
const char RBRACKET = ')';

var homework = File.ReadLines("input.txt");

var results = homework.Select(line => 
{
    var tokens = Tokenise(line).ToArray();
    var exp = ParseExpression(tokens.AsSpan());
    return ExecuteExpression(exp);
});

var result2 = homework.Select(line => 
{
    var tokens = Tokenise(line).ToArray().AsSpan();
    return Evaluate(ref tokens);
});

Console.WriteLine($"Part 1: {results.Sum()}");
Console.WriteLine($"Part 2: {result2.Sum()}");

Expression ParseExpression(Span<string> givenTokens, Expression lhs = null)
{   
    var tokens = givenTokens;
    Expression result = lhs;

    for (int i = 0; i < givenTokens.Length; i++)
    {
        if (result == null)
        {
            if (givenTokens[i] == "(")
            {
                var expressionLength = FindExpressionEnd(tokens);
                var range = 1..expressionLength;
                result = ParseExpression(tokens[range], result);
                i += expressionLength;
            }
            else
            {
                result = Expression.Constant(long.Parse(tokens[i]));
            }
        }
        else
        {
            var op = GetOperand(tokens[i]);
            Expression rhs;
            int consumed = 0;
            if (tokens[i + 1] == "(")
            {
                var expressionLength = FindExpressionEnd(tokens[(i + 1)..]);
                var range = (i + 2)..(i + 1 + expressionLength);
                rhs = ParseExpression(tokens[range]);
                consumed = expressionLength + 1;
            }
            else
            {
                rhs = Expression.Constant(long.Parse(tokens[i + 1]));
                consumed++;
            }
            result = Expression.MakeBinary(op, result, rhs);
            i += consumed;
        }
    }

    return result;
}

ExpressionType GetOperand(string given) => given switch
{
    "+" => ExpressionType.Add,
    "*" => ExpressionType.Multiply,
    _ => throw new Exception($"Unknown operator {given}")
};

long ExecuteExpression(Expression e) => Expression.Lambda<Func<long>>(e).Compile()();

// A different approach for part 2
long Evaluate(ref Span<string> tokens)
{
    var result = 0L;
    var operation = ExpressionType.Add;
    var seen = new Stack<long>();
    while (!tokens.IsEmpty)
    {
        var token = tokens[0];

        switch (token)
        {
            case "(":
                tokens = tokens.Slice(1);
                var nest = Evaluate(ref tokens);
                result = operation switch 
                {
                    ExpressionType.Add => result + nest,
                    ExpressionType.Multiply => result * nest,
                    _ => throw new Exception($"Unsupported operator {operation}")
                };
                continue;
            case ")":
                tokens = tokens.Slice(1);
                while (seen.TryPop(out var popped))
                    result *= popped;
                return result;
            case "*":
                seen.Push(result);
                result = 0L;
                operation = ExpressionType.Add;
                break;
            case "+":
                operation = ExpressionType.Add;
                break;
            default:
                var operand = int.Parse(token);
                result = operation switch
                {
                    ExpressionType.Add => result + operand,
                    ExpressionType.Multiply => result * operand,
                    _ => throw new Exception($"Unsupported operator {operation}")
                };
                break;
        }
        tokens = tokens.Slice(1);
    }


    while (seen.TryPop(out var popped))
        result *= popped;
    return result;
}

IEnumerable<string> Tokenise(string expression)
{
    var parts = expression.Split(' ');
    foreach (var token in parts)
    {
        if (token.StartsWith(LBRACKET))
        {
            var num = token.Count(c => c == LBRACKET);
            for (int i = 0; i < num; i++)
                yield return LBRACKET.ToString();
            yield return token[num..];
        }
        else if (token.EndsWith(RBRACKET))
        {
            var num = token.Count(c => c == RBRACKET);
            yield return token[..^num];
            for (int i = 0; i < num; i++)
                yield return RBRACKET.ToString();
        }
        else
        {
            yield return token;
        }
    }
}

int FindExpressionEnd(ReadOnlySpan<string> given)
{
    var count = 1;
    var index = 1;
    while (count != 0)
    {
        count += given[index] switch
        {
            "(" => 1,
            ")" => -1,
            _ => 0
        };

        index++;
    }

    return index - 1;
}