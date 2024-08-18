using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ContentProvider
{
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_content_provider_new_typed", CallingConvention = CallingConvention.Cdecl)]
    public extern static ContentProviderHandle NewString(GType type, string text);
}

