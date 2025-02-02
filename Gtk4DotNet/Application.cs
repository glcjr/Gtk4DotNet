﻿using System.Reflection;
using System.Runtime.InteropServices;
using CsTools.Functional;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Application
{
    public static ApplicationHandle New(string id, int flags = 0)
        => _New(id, 0)
                .SideEffect(_ => Gtk.Init());

    public static ApplicationHandle NewAdwaita(string id, int flags = 0)
        => _NewAdw(id, 0)
                .SideEffect(_ => Gtk.Init());

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle NewWindow(this ApplicationHandle app);

    [DllImport(Libs.LibAdw, EntryPoint="adw_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle NewAdwaitaWindow(this ApplicationHandle app);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_add_window", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddWindow(this ApplicationHandle app, WindowHandle window);

    public static int Run(this ApplicationHandle app, int c, IntPtr a)
    {
        var result = app._Run(c, a);
        app.Dispose();
        return result;
    }

    [DllImport(Libs.LibGtk, EntryPoint="g_application_quit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Quit(this ApplicationHandle app);

    [DllImport(Libs.LibGtk, EntryPoint="g_application_run", CallingConvention = CallingConvention.Cdecl)]
    extern static int _Run(this ApplicationHandle app, int c, IntPtr a);

    public static ApplicationHandle OnActivate(this ApplicationHandle app, Action<ApplicationHandle> activate)
        => app.SideEffect(a => Gtk.SignalConnect<OnePointerDelegate>(a, "activate", (IntPtr _) => activate(app)));

    public static bool RegisterResources()
    {
        var assembly = Assembly.GetEntryAssembly();
        var resources = assembly?.GetManifestResourceNames();
        var legacyName = $"{assembly?.GetName().Name}.resources.gresource";
        var actualName = "app.gresource";
        var resourceName = resources?.Contains(legacyName) == true
            ? legacyName
            : resources?.Contains(actualName) == true
            ? actualName
            : null;
        if (resourceName == null)
            return false;
        var stream = assembly?.GetManifestResourceStream(resourceName);
        var memIntPtr = Marshal.AllocHGlobal((int)(stream?.Length ?? 0));
        unsafe 
        {
            var memBytePtr = (byte*)memIntPtr.ToPointer();
            var writeStream = new UnmanagedMemoryStream(memBytePtr, stream?.Length ?? 0, stream?.Length ?? 0, FileAccess.Write);
            stream?.CopyTo(writeStream);
        }
        using var gbytes = GBytes.New(memIntPtr, stream?.Length ?? 0);
        Marshal.FreeHGlobal(memIntPtr);
        using var res = Resource.NewFromData(gbytes);
        Resource.Register(res); 
        return true;
    }

    public static ApplicationHandle AddActions(this ApplicationHandle app, IEnumerable<GtkAction> actions)
    {
        var gtkActions = actions.OfType<GtkAction>();
        foreach (var action in gtkActions)
        {
            if (action.Action != null)
            {
                GtkDelegates.Add(action.Action);
                // TODO DO actions have to be freed?
                var simpleAction = NewAction(action.Name, null);
                action.action = simpleAction;
                Gtk.SignalConnectAction(simpleAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), IntPtr.Zero, 0);
                AddAction(app, simpleAction);                    
            }
            else 
            {
                GtkDelegates.Add(action.StateChanged);
                var state = action.StateParameterType == "s" 
                    ? NewString(action.State as string ?? "")
                    : NewBool((bool?)action.State == true ? -1 : 0);
                var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
                action.action = simpleAction;
                Gtk.SignalConnectAction(simpleAction, "change-state", Marshal.GetFunctionPointerForDelegate(action.StateChanged), IntPtr.Zero, 0);
                AddAction(app, simpleAction);
            }
        }

        var accelEntries = 
            actions
            .Where(n => n.Accelerator != null)
            .Select(n => new { Name = "app." + n.Name, n.Accelerator});  
        foreach (var accelEntry in accelEntries)
            SetAccelsForAction(app, accelEntry.Name, [accelEntry.Accelerator, null]);
        return app;
    }

    [DllImport(Libs.LibAdw, EntryPoint="adw_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationHandle _NewAdw(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationHandle _New(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_set_accels_for_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetAccelsForAction(ApplicationHandle app, string action, [In] string?[] accels);

    [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewStatefulAction(string action, string? p, nint state);

    [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr NewBool(int value);

    // [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_int32", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr NewInt(int value);

    [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr NewString(string value);

    [DllImport(Libs.LibGtk, EntryPoint="g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(ApplicationHandle app, IntPtr action);
    
    // [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_set_enabled", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void EnableAction(IntPtr action, int enabled);
}
