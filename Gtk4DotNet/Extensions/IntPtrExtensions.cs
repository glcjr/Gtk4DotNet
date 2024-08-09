using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace GtkDotNet.Extensions;

static class IntPtrExtensions
{
    // TODO free if it has to be freed
    public static string? PtrToString(this IntPtr obj, bool free)
    {
        var val = Marshal.PtrToStringUTF8(obj);
        if (free)
            obj.Free();
        return val ?? "";
    }

    public static THandle With<THandle>(this THandle handle, Action<THandle> action)
        where THandle : SafeHandle
        => handle.SideEffect(action);

    public static THandle If<THandle>(this THandle handle, Predicate<THandle> predicate, Action<THandle> action)
        where THandle : SafeHandle
        => handle.SideEffectIf(predicate(handle), action);

    public static THandle If<THandle>(this THandle handle, bool predicate, Action<THandle> action)
        where THandle : SafeHandle
        => handle.SideEffectIf(predicate, action);

    public static THandle Choose<THandle>(this THandle handle, Predicate<THandle> predicate, Action<THandle> trueAction, Action<THandle> falseAction)
        where THandle : SafeHandle
        => handle.SideEffectChoose(predicate(handle), trueAction, falseAction);

    public static THandle Choose<THandle>(this THandle handle, bool predicate, Action<THandle> trueAction, Action<THandle> falseAction)
        where THandle : SafeHandle
        => handle.SideEffectChoose(predicate, trueAction, falseAction);
}
