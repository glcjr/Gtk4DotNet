using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using GtkDotNet.Extensions;

namespace GtkDotNet;

public static class WebKitUriSchemeRequest
{
    public static string GetUri(this WebkitUriSchemeRequestHandle request)
        => _GetUri(request).PtrToString(false) ?? "";

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_finish", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Finish(this WebkitUriSchemeRequestHandle request, InputStreamHandle stream, long length, string content);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_http_headers", CallingConvention = CallingConvention.Cdecl)]
    public extern static SoupMessageHeadersHandle GetHttpHeaders(this WebkitUriSchemeRequestHandle request);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_http_body", CallingConvention = CallingConvention.Cdecl)]
    public extern static InputStreamHandle GetHttpBody(this WebkitUriSchemeRequestHandle request);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_uri_scheme_request_get_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetUri(WebkitUriSchemeRequestHandle request);
}