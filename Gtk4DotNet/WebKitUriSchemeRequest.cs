using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using GtkDotNet.Extensions;

namespace GtkDotNet;

public static class WebKitUriSchemeRequest
{
    public static string GetUri(IntPtr context)
        => _GetUri(context).PtrToString(false) ?? "";

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetUri(IntPtr context);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_finish", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Finish(IntPtr rek, InputStreamHandle html, long length, string content);
}