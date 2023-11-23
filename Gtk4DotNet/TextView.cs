using System;
using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class TextView
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static TextViewHandle New();

    public static TextViewHandle Text(this TextViewHandle textview, string text)
    {
        var buffer = textview.GetBuffer();
        buffer.SetText(text, text.Length);
        return textview;
    }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_get_buffer", CallingConvention = CallingConvention.Cdecl)]
    public extern static TextBufferHandle GetBuffer(this TextViewHandle textView);

    public static TextViewHandle SetEditable(this TextViewHandle textview, bool set)
        => textview.SideEffect(s => s._SetEditable(set));

    public static TextViewHandle SetCursorVisible(this TextViewHandle textview, bool set)
        => textview.SideEffect(s => s._SetCursorVisible(set));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_set_editable", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetEditable(this TextViewHandle textView, bool set);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_set_cursor_visible", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetCursorVisible(this TextViewHandle textView, bool set);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_scroll_to_iter", CallingConvention = CallingConvention.Cdecl)]
    // extern static bool ScrollToIter(this TextViewHandle textView, ref GtkTextIter iter, double withinMargin, bool useAlign, double xAlign, double yAlign);
}

