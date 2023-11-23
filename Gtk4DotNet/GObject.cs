using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class GObject
{
    public static void SetString(this ObjectHandle obj, string name, string? value)
        => SetString(obj, name, value ?? "", IntPtr.Zero);
    public static string? GetString(this ObjectHandle obj, string name)
    {
        GetString(obj, name, out var value, IntPtr.Zero);
        var result = Marshal.PtrToStringUTF8(value);
        value.Free();
        return result;
    }

    public static THandle AddWeakRef<THandle>(this THandle obj, Action dispose)
        where THandle : ObjectHandle, new()
    {
        // TODO delegates.add
        // TODO free delegate
        void onDispose(IntPtr _, IntPtr __)  => dispose();
        return obj.SideEffect(o => o.AddWeakRef(Marshal.GetFunctionPointerForDelegate((TwoPointerDelegate)onDispose), IntPtr.Zero));
        return obj;
    }

    public static void SetBool(this ObjectHandle obj, string name, bool value)
        => obj.SetBool(name, value, IntPtr.Zero);
    public static bool GetBool(this ObjectHandle obj, string name)
    {
        GetBool(obj, name, out var value, IntPtr.Zero);
        return value;
    }

    [DllImport(Libs.LibGtk, EntryPoint="g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(IntPtr obj);

    [DllImport(Libs.LibGtk, EntryPoint="g_free", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Free(this IntPtr obj);

    [DllImport(Libs.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetString(this ObjectHandle obj, string name, string value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint="g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetString(this ObjectHandle obj, string name, out IntPtr value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBool(this ObjectHandle GtkHandle, string name, bool value, IntPtr end);

    [DllImport(Libs.LibGtk, EntryPoint="g_object_get", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetBool(this ObjectHandle GtkHandle, string name, out bool value, IntPtr end);

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_ref", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr Ref(this IntPtr obj);

    // /// <summary>
    // /// Increase the reference count of object, and possibly remove the [floating][floating-ref] reference, 
    // /// if object has a floating reference. 
    // /// In other words, if the object is floating, then this call “assumes ownership” of the floating reference, 
    // /// converting it to a normal reference by clearing the floating flag while leaving the reference count unchanged. If the object is not floating, then this call adds a new normal reference increasing the reference count by one.
    // /// </summary>
    // /// <param name="obj"></param>
    // /// <returns></returns>
    // [DllImport(Libs.LibGtk, EntryPoint="g_object_ref_sink", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr RefSink(this IntPtr obj);
    
    // [DllImport(Libs.LibGtk, EntryPoint="g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Unref(this IntPtr obj);

    // [DllImport(Libs.LibGtk, EntryPoint="g_clear_object", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Clear(this IntPtr obj);

    // public static void AddWeakRef(this IntPtr obj, FinalizerDelegate finalizer) => AddWeakRef(obj, finalizer, IntPtr.Zero);

    // public static void SetInt(this IntPtr obj, string name, int value)
    //     => SetInt(obj, name, value, IntPtr.Zero);
    // public static int GetInt(this IntPtr obj, string name)
    // {
    //     GetInt(obj, name, out var value, IntPtr.Zero);
    //     return value;
    // }

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_bind_property", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void BindProperty(this IntPtr source, string sourceProperty, IntPtr target, string targetProperty, BindingFlags bindingFlags);

    // [DllImport(Libs.LibGtk, EntryPoint="g_type_from_name", CallingConvention = CallingConvention.Cdecl)]
    // public extern static GType TypeFromName(string objectName);

    // [DllImport(Libs.LibGtk, EntryPoint="g_type_name", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr TypeName(this GType type);

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_set", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetInt(IntPtr obj, string name, int value, IntPtr end);

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_get", CallingConvention = CallingConvention.Cdecl)]
    // extern static bool GetInt(IntPtr obj, string name, out int value, IntPtr end);

    // [DllImport(Libs.LibGtk, EntryPoint="g_object_new", CallingConvention = CallingConvention.Cdecl)]
    // internal extern static IntPtr New(long type, IntPtr zero);

    /// <summary>
    /// Adds a weak reference callback to an object. Weak references are used for notification when an object is disposed. They are called “weak references” 
    /// because they allow you to safely hold a pointer to an object without calling g_object_ref() (g_object_ref() adds a strong reference, that is, 
    /// forces the object to stay alive).
    /// Note that the weak references created by this method are not thread-safe: they cannot safely be used in one thread if the object’s last g_object_unref() might happen in another thread. Use GWeakRef if thread-safety is required.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="finalizer"></param>
    /// <param name="zero"></param>
    [DllImport(Libs.LibGtk, EntryPoint="g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWeakRef(this ObjectHandle obj, IntPtr finalizer, IntPtr zero);
}


