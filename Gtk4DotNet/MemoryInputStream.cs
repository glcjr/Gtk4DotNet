using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class MemoryInputStream
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MemoryInputStreamHandle New();

    public static MemoryInputStreamHandle New(byte[] data)
        => New(GBytes.New(data));

    [DllImport(Libs.LibGio, EntryPoint = "g_memory_input_stream_add_bytes", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddBytes(this MemoryInputStreamHandle stream, BytesHandle bytes);

    [DllImport(Libs.LibGtk, EntryPoint = "g_memory_input_stream_new_from_bytes", CallingConvention = CallingConvention.Cdecl)]
    extern static MemoryInputStreamHandle New(BytesHandle bytes);
}
