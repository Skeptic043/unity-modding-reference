using System;
using System.Runtime.InteropServices;
using System.Threading;

// ComImport comparison: the same reads through runtime callable wrappers.
[ComImport, Guid("70ae66f2-c809-4e4f-8915-bdcb406b7993"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IMFSourceReader
{
    [PreserveSig] int GetStreamSelection(int stream, out int selected);
    [PreserveSig] int SetStreamSelection(int stream, int selected);
    [PreserveSig] int GetNativeMediaType(int stream, int index, out IntPtr type);
    [PreserveSig] int GetCurrentMediaType(int stream, out IntPtr type);
    [PreserveSig] int SetCurrentMediaType(int stream, IntPtr reserved, IntPtr type);
    [PreserveSig] int SetCurrentPosition([In] ref Guid format, IntPtr variant);
    [PreserveSig] int ReadSample(int stream, int control, out int actual, out int flags, out long time, out IMFSample sample);
}

[ComImport, Guid("c40a00f2-b93a-4d80-ae8c-5a1c634f58e4"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IMFSample
{
    // IMFAttributes slots 3..32 are unused placeholders here.
    void a3(); void a4(); void a5(); void a6(); void a7(); void a8(); void a9(); void a10(); void a11(); void a12();
    void a13(); void a14(); void a15(); void a16(); void a17(); void a18(); void a19(); void a20(); void a21(); void a22();
    void a23(); void a24(); void a25(); void a26(); void a27(); void a28(); void a29(); void a30(); void a31(); void a32();
    [PreserveSig] int GetSampleFlags(out int flags);
    [PreserveSig] int SetSampleFlags(int flags);
    [PreserveSig] int GetSampleTime(out long time);
    [PreserveSig] int SetSampleTime(long time);
    [PreserveSig] int GetSampleDuration(out long duration);
    [PreserveSig] int SetSampleDuration(long duration);
    [PreserveSig] int GetBufferCount(out int count);
    [PreserveSig] int GetBufferByIndex(int index, out IntPtr buffer);
    [PreserveSig] int ConvertToContiguousBuffer(out IMFMediaBuffer buffer);
}

[ComImport, Guid("045FA593-8799-42b8-BC8D-8968C6453507"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IMFMediaBuffer
{
    [PreserveSig] int Lock(out IntPtr data, out int max, out int current);
    [PreserveSig] int Unlock();
}

static class ComSpike
{
    [DllImport("mfplat.dll", ExactSpelling = true)] static extern int MFStartup(int version, int flags);
    [DllImport("mfplat.dll", ExactSpelling = true)] static extern int MFShutdown();
    [DllImport("mfreadwrite.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    static extern int MFCreateSourceReaderFromURL(string url, IntPtr attributes, out IMFSourceReader reader);

    static int Main(string[] args)
    {
        Console.WriteLine("ComImport runtime " + (Type.GetType("Mono.Runtime") != null ? "Mono" : ".NET") + " x64=" + Environment.Is64BitProcess);
        Console.WriteLine("MFStartup=0x" + MFStartup(0x20070, 1).ToString("X8"));
        try
        {
            IMFSourceReader reader;
            int hr = MFCreateSourceReaderFromURL(args[0], IntPtr.Zero, out reader);
            Console.WriteLine("open=0x" + hr.ToString("X8") + " reader=" + (reader != null));
            long total = 0, bytes = 0;
            int samples = 0;
            while (true)
            {
                int actual, flags; long time; IMFSample sample;
                hr = reader.ReadSample(unchecked((int)0xFFFFFFFD), 0, out actual, out flags, out time, out sample);
                if (hr < 0) { Console.WriteLine("ReadSample=0x" + hr.ToString("X8")); break; }
                if (sample != null)
                {
                    IMFMediaBuffer buffer;
                    hr = sample.ConvertToContiguousBuffer(out buffer);
                    IntPtr data; int max, current;
                    buffer.Lock(out data, out max, out current);
                    buffer.Unlock();
                    bytes += current;
                    samples++;
                    Marshal.ReleaseComObject(buffer);
                    Marshal.ReleaseComObject(sample);
                }
                if ((flags & 2) != 0) break;
            }
            total = bytes;
            Console.WriteLine("ComImport read samples=" + samples + " bytes=" + total + " (native output subtype left unconfigured)");
            Console.WriteLine("ReleaseComObject(reader) remaining=" + Marshal.ReleaseComObject(reader));
        }
        catch (Exception ex) { Console.WriteLine("ComImport ERROR " + ex.GetType().Name + ": " + ex.Message); }
        finally { MFShutdown(); }
        return 0;
    }
}
