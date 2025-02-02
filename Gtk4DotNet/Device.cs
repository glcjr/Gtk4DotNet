using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Device
{
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_device_get_source", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetSource(this DeviceHandle device);
}

