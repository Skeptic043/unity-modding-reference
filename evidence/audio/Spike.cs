using System;
using System.Runtime.InteropServices;
using System.Threading;

// Media Foundation spike: manual vtable calls through Marshal.GetDelegateForFunctionPointer.
static class Spike
{
    [DllImport("mfplat.dll", ExactSpelling = true)] static extern int MFStartup(int version, int flags);
    [DllImport("mfplat.dll", ExactSpelling = true)] static extern int MFShutdown();
    [DllImport("mfplat.dll", ExactSpelling = true)] static extern int MFCreateMediaType(out IntPtr type);
    [DllImport("mfplat.dll", ExactSpelling = true)] static extern int MFTEnumEx(Guid category, int flags, IntPtr input, IntPtr output, out IntPtr activates, out int count);
    [DllImport("mfreadwrite.dll", ExactSpelling = true, CharSet = CharSet.Unicode)] static extern int MFCreateSourceReaderFromURL(string url, IntPtr attributes, out IntPtr reader);
    [DllImport("ole32.dll", ExactSpelling = true)] static extern int CoInitializeEx(IntPtr reserved, int flags);
    [DllImport("ole32.dll", ExactSpelling = true)] static extern void CoUninitialize();
    [DllImport("ole32.dll", ExactSpelling = true)] static extern void CoTaskMemFree(IntPtr p);
    [DllImport("ole32.dll", ExactSpelling = true)] static extern int PropVariantClear(IntPtr p);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)] static extern IntPtr GetModuleHandle(string name);
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)] static extern IntPtr GetProcAddress(IntPtr module, string name);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int Release(IntPtr self);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int SetStreamSelection(IntPtr self, int stream, int selected);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetMediaType(IntPtr self, int stream, int index, out IntPtr type);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetCurrentMediaType(IntPtr self, int stream, out IntPtr type);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int SetCurrentMediaType(IntPtr self, int stream, IntPtr reserved, IntPtr type);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int SetCurrentPosition(IntPtr self, ref Guid format, IntPtr variant);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int ReadSample(IntPtr self, int stream, int control, out int actual, out int flags, out long time, out IntPtr sample);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetPresentationAttribute(IntPtr self, int stream, ref Guid key, IntPtr variant);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetUInt32(IntPtr self, ref Guid key, out int value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetGuid(IntPtr self, ref Guid key, out Guid value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int SetUInt32(IntPtr self, ref Guid key, int value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int SetGuid(IntPtr self, ref Guid key, ref Guid value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetLong(IntPtr self, out long value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int GetPointer(IntPtr self, out IntPtr value);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int Lock(IntPtr self, out IntPtr data, out int max, out int current);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)] delegate int Unlock(IntPtr self);

    static T Method<T>(IntPtr self, int slot) where T : class
    {
        IntPtr table = Marshal.ReadIntPtr(self);
        return (T)(object)Marshal.GetDelegateForFunctionPointer(Marshal.ReadIntPtr(table, slot * IntPtr.Size), typeof(T));
    }
    static void Free(ref IntPtr p) { if (p != IntPtr.Zero) Method<Release>(p, 2)(p); p = IntPtr.Zero; }
    static void Ok(int hr, string what) { if (hr < 0) throw new Exception(what + " failed 0x" + hr.ToString("X8")); }

    static Guid MajorType = new Guid("48eba18e-f8c9-4687-bf11-0a74c9f96a8f");
    static Guid SubType = new Guid("f7e34c9a-42e8-4714-b74b-cb29d72c35e5");
    static Guid Audio = new Guid("73647561-0000-0010-8000-00AA00389B71");
    static Guid Float = new Guid("00000003-0000-0010-8000-00AA00389B71");
    static Guid Aac = new Guid("00001610-0000-0010-8000-00AA00389B71");
    static Guid Alac = new Guid("63616c61-0000-0010-8000-00aa00389b71");
    static Guid Channels = new Guid("37e48bf5-645e-4c5b-89de-ada9e29b696a");
    static Guid Rate = new Guid("5faeeae7-0290-4c31-9e8a-c534f68d9dba");
    static Guid ChannelMask = new Guid("55fb5765-644a-4caf-8479-938983bb1588");
    static Guid Duration = new Guid("6c990d33-bb8e-477a-8598-0d5d96fcd88a");
    static Guid DecoderCategory = new Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7");
    const int FirstAudio = unchecked((int)0xFFFFFFFD), AllStreams = unchecked((int)0xFFFFFFFE), MediaSource = unchecked((int)0xFFFFFFFF);

    static int Main(string[] args)
    {
        Console.WriteLine("Runtime " + (Type.GetType("Mono.Runtime") != null ? "Mono" : ".NET") + " " + Environment.Version + " x64=" + Environment.Is64BitProcess +
            " apartment=" + Thread.CurrentThread.GetApartmentState());
        IntPtr ntdll = GetModuleHandle("ntdll.dll");
        Console.WriteLine("wine_get_version=" + (GetProcAddress(ntdll, "wine_get_version") != IntPtr.Zero));
        int co = CoInitializeEx(IntPtr.Zero, 0);
        Console.WriteLine("CoInitializeEx(MTA) main=0x" + co.ToString("X8"));
        Ok(MFStartup(0x20070, 1), "MFStartup");
        try
        {
            Console.WriteLine("AAC decoders=" + CountDecoders(Aac) + " ALAC decoders=" + CountDecoders(Alac));
            foreach (string path in args)
            {
                try { Decode(path); }
                catch (Exception ex) { Console.WriteLine(path + " ERROR " + ex.Message); }
            }
            // Cross-thread: open on one pool thread, read on another.
            if (args.Length > 0)
            {
                IntPtr reader = IntPtr.Zero;
                var t1 = new Thread(() => { int c = CoInitializeEx(IntPtr.Zero, 0); Ok(MFCreateSourceReaderFromURL(args[0], IntPtr.Zero, out reader), "open"); Configure(reader); if (c >= 0) CoUninitialize(); });
                t1.Start(); t1.Join();
                long total = 0;
                var t2 = new Thread(() => { int c = CoInitializeEx(IntPtr.Zero, 0); total = ReadAll(reader, null); Free(ref reader); if (c >= 0) CoUninitialize(); });
                t2.Start(); t2.Join();
                Console.WriteLine("cross-thread frames=" + total);
            }
        }
        finally { MFShutdown(); if (co >= 0) CoUninitialize(); }
        return 0;
    }

    static int CountDecoders(Guid subtype)
    {
        IntPtr info = Marshal.AllocHGlobal(32);
        try
        {
            Marshal.StructureToPtr(Audio, info, false);
            Marshal.StructureToPtr(subtype, info + 16, false);
            IntPtr list; int count;
            // MFT_ENUM_FLAG_SYNCMFT | ASYNCMFT | LOCALMFT | SORTANDFILTER
            Ok(MFTEnumEx(DecoderCategory, 0x1 | 0x2 | 0x20 | 0x40, info, IntPtr.Zero, out list, out count), "MFTEnumEx");
            for (int i = 0; i < count; i++) { IntPtr a = Marshal.ReadIntPtr(list, i * IntPtr.Size); Free(ref a); }
            if (list != IntPtr.Zero) CoTaskMemFree(list);
            return count;
        }
        finally { Marshal.FreeHGlobal(info); }
    }

    static void Configure(IntPtr reader)
    {
        Ok(Method<SetStreamSelection>(reader, 4)(reader, AllStreams, 0), "deselect");
        Ok(Method<SetStreamSelection>(reader, 4)(reader, FirstAudio, 1), "select");
        IntPtr type;
        Ok(MFCreateMediaType(out type), "MFCreateMediaType");
        try
        {
            Ok(Method<SetGuid>(type, 24)(type, ref MajorType, ref Audio), "major");
            Ok(Method<SetGuid>(type, 24)(type, ref SubType, ref Float), "sub");
            Ok(Method<SetCurrentMediaType>(reader, 7)(reader, FirstAudio, IntPtr.Zero, type), "SetCurrentMediaType");
        }
        finally { Free(ref type); }
    }

    static void Decode(string path)
    {
        IntPtr reader;
        Ok(MFCreateSourceReaderFromURL(path, IntPtr.Zero, out reader), "MFCreateSourceReaderFromURL");
        try
        {
            IntPtr native;
            Ok(Method<GetMediaType>(reader, 5)(reader, FirstAudio, 0, out native), "GetNativeMediaType");
            Guid sub; int nc, nr;
            Method<GetGuid>(native, 10)(native, ref SubType, out sub);
            Method<GetUInt32>(native, 7)(native, ref Channels, out nc);
            Method<GetUInt32>(native, 7)(native, ref Rate, out nr);
            Free(ref native);
            Configure(reader);
            IntPtr current;
            Ok(Method<GetCurrentMediaType>(reader, 6)(reader, FirstAudio, out current), "GetCurrentMediaType");
            int channels, rate, mask = 0;
            Method<GetUInt32>(current, 7)(current, ref Channels, out channels);
            Method<GetUInt32>(current, 7)(current, ref Rate, out rate);
            Method<GetUInt32>(current, 7)(current, ref ChannelMask, out mask);
            Free(ref current);
            IntPtr variant = Marshal.AllocHGlobal(32);
            for (int i = 0; i < 32; i++) Marshal.WriteByte(variant, i, 0);
            Ok(Method<GetPresentationAttribute>(reader, 12)(reader, MediaSource, ref Duration, variant), "duration");
            short vt = Marshal.ReadInt16(variant);
            long duration = Marshal.ReadInt64(variant, 8);
            PropVariantClear(variant);
            long frames = (long)Math.Round(duration * (double)rate / 1e7);
            Console.WriteLine(path + " native=" + sub + " nativeCh=" + nc + " nativeRate=" + nr + " outCh=" + channels + " rate=" + rate +
                " mask=0x" + mask.ToString("X") + " vt=" + vt + " duration100ns=" + duration + " frames=" + frames);
            var first = new System.Collections.Generic.List<string>();
            long total = ReadAll(reader, first);
            Console.WriteLine("  decoded frames=" + total + " first timestamps: " + string.Join(", ", first.ToArray()));
            // Seek to 1.5 s and report the first timestamp returned.
            foreach (double seconds in new[] { 1.5, 0.3, 2.9, 0.0 })
            {
                for (int i = 0; i < 32; i++) Marshal.WriteByte(variant, i, 0);
                Marshal.WriteInt16(variant, 20); // VT_I8
                Marshal.WriteInt64(variant, 8, (long)(seconds * 1e7));
                Guid none = Guid.Empty;
                Ok(Method<SetCurrentPosition>(reader, 8)(reader, ref none, variant), "SetCurrentPosition");
                int actual, flags; long time; IntPtr sample;
                Ok(Method<ReadSample>(reader, 9)(reader, FirstAudio, 0, out actual, out flags, out time, out sample), "ReadSample");
                long sampleFrames = 0;
                if (sample != IntPtr.Zero) { sampleFrames = SampleBytes(sample) / 4 / channels; Free(ref sample); }
                Console.WriteLine("  seek " + seconds + "s -> first time=" + time + " frame=" + Math.Round(time * (double)rate / 1e7) +
                    " targetFrame=" + seconds * rate + " flags=0x" + flags.ToString("X") + " frames=" + sampleFrames);
            }
            Marshal.FreeHGlobal(variant);
        }
        finally { Free(ref reader); }
    }

    static long SampleBytes(IntPtr sample)
    {
        IntPtr buffer;
        Ok(Method<GetPointer>(sample, 41)(sample, out buffer), "ConvertToContiguousBuffer");
        try
        {
            IntPtr data; int max, current;
            Ok(Method<Lock>(buffer, 3)(buffer, out data, out max, out current), "Lock");
            Method<Unlock>(buffer, 4)(buffer);
            return current;
        }
        finally { Free(ref buffer); }
    }

    static long ReadAll(IntPtr reader, System.Collections.Generic.List<string> times)
    {
        IntPtr current;
        Ok(Method<GetCurrentMediaType>(reader, 6)(reader, FirstAudio, out current), "GetCurrentMediaType");
        int channels, rate;
        Method<GetUInt32>(current, 7)(current, ref Channels, out channels);
        Method<GetUInt32>(current, 7)(current, ref Rate, out rate);
        Free(ref current);
        long total = 0;
        while (true)
        {
            int actual, flags; long time; IntPtr sample;
            Ok(Method<ReadSample>(reader, 9)(reader, FirstAudio, 0, out actual, out flags, out time, out sample), "ReadSample");
            if (sample != IntPtr.Zero)
            {
                long frames = SampleBytes(sample) / 4 / channels;
                if (times != null && times.Count < 4) times.Add(time + "(" + Math.Round(time * (double)rate / 1e7) + "+" + frames + ")");
                total += frames;
                Free(ref sample);
            }
            if ((flags & 2) != 0) break;
            if ((flags & 1) != 0) throw new Exception("stream error");
        }
        return total;
    }
}
