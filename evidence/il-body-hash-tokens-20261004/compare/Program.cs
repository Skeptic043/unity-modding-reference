using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;

// Compares Probe.Target.Check in two builds: raw IL bytes versus opcodes with resolved operands.
internal static class Program
{
    private static readonly Dictionary<short, OpCode> Codes = typeof(OpCodes)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(f => (OpCode)f.GetValue(null)).ToDictionary(o => o.Value);

    private static int Main(string[] args)
    {
        var a = Describe(args[0]);
        var b = Describe(args[1]);
        Console.WriteLine("Raw IL equal:        " + a.Raw.SequenceEqual(b.Raw));
        Console.WriteLine("Raw SHA256 A:        " + Hash(a.Raw));
        Console.WriteLine("Raw SHA256 B:        " + Hash(b.Raw));
        Console.WriteLine("Normalized equal:    " + a.Normalized.SequenceEqual(b.Normalized));
        for (int i = 0; i < Math.Max(a.Normalized.Count, b.Normalized.Count); i++)
            Console.WriteLine($"  {Get(a.Tokens, i),-12} {Get(b.Tokens, i),-12} {Get(a.Normalized, i)}");
        return 0;
    }

    private static string Get(List<string> list, int i) => i < list.Count ? list[i] : "";
    private static string Hash(byte[] bytes)
    {
        using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
    }

    private static (byte[] Raw, List<string> Normalized, List<string> Tokens) Describe(string path)
    {
        var assembly = Assembly.LoadFile(path);
        var method = assembly.GetType("Probe.Target").GetMethod("Check");
        var il = method.GetMethodBody().GetILAsByteArray();
        var module = method.Module;
        var normalized = new List<string>();
        var tokens = new List<string>();
        for (int pos = 0; pos < il.Length;)
        {
            short value = il[pos] == 0xFE ? (short)(0xFE00 | il[pos + 1]) : il[pos];
            var code = Codes[value];
            pos += code.Size;
            string operand = "";
            string token = "";
            switch (code.OperandType)
            {
                case OperandType.InlineField:
                case OperandType.InlineMethod:
                case OperandType.InlineString:
                case OperandType.InlineType:
                case OperandType.InlineTok:
                    int t = BitConverter.ToInt32(il, pos);
                    token = "0x" + t.ToString("x8");
                    operand = code.OperandType == OperandType.InlineString ? "\"" + module.ResolveString(t) + "\""
                        : module.ResolveMember(t) is MemberInfo m ? m.DeclaringType?.FullName + "::" + m.Name : "?";
                    pos += 4;
                    break;
                case OperandType.InlineNone: break;
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineI:
                case OperandType.ShortInlineVar: operand = il[pos].ToString(); pos += 1; break;
                case OperandType.InlineVar: operand = BitConverter.ToInt16(il, pos).ToString(); pos += 2; break;
                case OperandType.InlineI8:
                case OperandType.InlineR: operand = BitConverter.ToInt64(il, pos).ToString(); pos += 8; break;
                case OperandType.InlineSwitch: throw new NotSupportedException("switch");
                default: operand = BitConverter.ToInt32(il, pos).ToString(); pos += 4; break;
            }
            normalized.Add((code.Name + " " + operand).Trim());
            tokens.Add(token);
        }
        return (il, normalized, tokens);
    }
}
