// Does a HarmonyX prefix's write to __args reach the original method, compared with a ref parameter?
using System;
using System.Runtime.CompilerServices;
using HarmonyLib;

public static class Target
{
    public static int Received;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void ArgsTarget(int value) => Received = value;
    [MethodImpl(MethodImplOptions.NoInlining)] public static void RefTarget(int value) => Received = value;
}

public static class Prefixes
{
    public static int Seen;
    public static void ArgsPrefix(object[] __args) { Seen = (int)__args[0]; __args[0] = 7; }
    public static void RefPrefix(ref int value) { Seen = value; value = 9; }
}

public static class Program
{
    public static int Main()
    {
        var harmony = new Harmony("kb.probe.prefixargs");
        Console.WriteLine("HarmonyX " + typeof(Harmony).Assembly.GetName().Version + " on CLR " + Environment.Version);
        try
        {
            harmony.Patch(typeof(Target).GetMethod(nameof(Target.ArgsTarget)), prefix: new HarmonyMethod(typeof(Prefixes), nameof(Prefixes.ArgsPrefix)));
            harmony.Patch(typeof(Target).GetMethod(nameof(Target.RefTarget)), prefix: new HarmonyMethod(typeof(Prefixes), nameof(Prefixes.RefPrefix)));
            Target.ArgsTarget(1);
            Console.WriteLine("__args prefix: saw " + Prefixes.Seen + ", wrote 7, original received " + Target.Received);
            Target.RefTarget(1);
            Console.WriteLine("ref prefix: saw " + Prefixes.Seen + ", wrote 9, original received " + Target.Received);
        }
        finally { harmony.UnpatchSelf(); }
        return 0;
    }
}
