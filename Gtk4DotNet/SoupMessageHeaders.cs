using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SoupMessageHeaders
{
    public static IEnumerable<MessageHeader> Get(this SoupMessageHeadersHandle headers)
    {
        List<MessageHeader> headerList = new();
        headers.Foreach((h, v) => headerList.Add(new(h, v)));
        return headerList;
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "soup_message_headers_foreach", CallingConvention = CallingConvention.Cdecl)]
    extern static void Foreach(this SoupMessageHeadersHandle headers, SoupMessageHeadersDelegate foreachHeader);
}

public record MessageHeader(string Key, string Value);
